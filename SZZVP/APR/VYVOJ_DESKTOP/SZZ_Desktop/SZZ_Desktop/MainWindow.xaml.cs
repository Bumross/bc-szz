using FractalTreeWPF.Models;
using FractalTreeWPF.Services;
using Microsoft.Win32;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System;
using System.Windows.Controls;

namespace FractalTreeWPF
{
    public partial class MainWindow : Window
    {
        private TreeParameters parameters;
        private CancellationTokenSource cts;
        private ManualResetEventSlim pauseEvent = new(true);

        public MainWindow()
        {
            InitializeComponent();

            StartBtn.Click += StartBtn_Click;
            PauseBtn.Click += (s, e) => pauseEvent.Reset();
            ResumeBtn.Click += (s, e) => pauseEvent.Set();
            StopBtn.Click += (s, e) => cts?.Cancel();
            ExportBtn.Click += ExportBtn_Click;
            ImportBtn.Click += ImportBtn_Click;
            ColorBtn.Click += ColorBtn_Click;
        }

        private void ColorBtn_Click(object sender, RoutedEventArgs e)
        {
            // jednoduchý dialog s přednastavenými barvami
            var dlg = new Window
            {
                Title = "Vyber barvu",
                Width = 300,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            var stack = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(10) };

            var colors = new Dictionary<string, string>
    {
        {"Černá", "#000000"},
        {"Červená", "#FF0000"},
        {"Zelená", "#00FF00"},
        {"Modrá", "#0000FF"},
        {"Fialová", "#800080"},
        {"Oranžová", "#FFA500"}
    };

            foreach (var kv in colors)
            {
                var btn = new Button
                {
                    Content = kv.Key,
                    Tag = kv.Value,
                    Margin = new Thickness(2)
                };
                btn.Click += (s, e2) =>
                {
                    parameters ??= new TreeParameters();
                    parameters.ColorHex = (string)((Button)s).Tag;

                    ColorPreview.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(parameters.ColorHex));
                    dlg.Close();
                };
                stack.Children.Add(btn);
            }

            dlg.Content = stack;
            dlg.ShowDialog();
        }


        private async void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            TreeCanvas.Children.Clear();

            parameters = new TreeParameters
            {
                Iterations = int.Parse(IterationsBox.Text),
                Angle = double.Parse(AngleBox.Text),
                Scale = double.Parse(ScaleBox.Text),
                ColorHex = parameters?.ColorHex ?? "#000000"
            };

            var color = (Color)ColorConverter.ConvertFromString(parameters.ColorHex);
            var drawer = new FractalTreeDrawer(TreeCanvas, color);


            cts = new CancellationTokenSource();
            var token = cts.Token;

            await Task.Run(() =>
            {
                DrawTree(drawer, token);
            });
        }


        private void DrawTree(FractalTreeDrawer drawer, CancellationToken token)
        {
            double startX = TreeCanvas.ActualWidth / 2;
            double startY = TreeCanvas.ActualHeight - 10;
            double length = 150;
            double startThickness = 10;

            Action<double, double, double, double, int, double> drawRecursive = null;
            drawRecursive = (x, y, len, angle, depth, thickness) =>
            {
                if (depth == 0 || token.IsCancellationRequested) return;

                pauseEvent.Wait(token);

                double x2 = x + len * Math.Cos(angle * Math.PI / 180);
                double y2 = y - len * Math.Sin(angle * Math.PI / 180);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    drawer.DrawLine(x, y, x2, y2, thickness);
                });

                Thread.Sleep(30);

                double nextThickness = Math.Max(1, thickness * parameters.Scale);

                drawRecursive(x2, y2, len * parameters.Scale, angle - parameters.Angle, depth - 1, nextThickness);
                drawRecursive(x2, y2, len * parameters.Scale, angle + parameters.Angle, depth - 1, nextThickness);
            };

            // tady je klíč: místo -90 dáme 90
            drawRecursive(startX, startY, length, 90, parameters.Iterations, startThickness);
        }



        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "JSON files|*.json" };
            if (dlg.ShowDialog() == true)
            {
                IOManager.Save(dlg.FileName, parameters);
            }
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "JSON files|*.json" };
            if (dlg.ShowDialog() == true)
            {
                parameters = IOManager.Load(dlg.FileName);

                IterationsBox.Text = parameters.Iterations.ToString();
                AngleBox.Text = parameters.Angle.ToString();
                ScaleBox.Text = parameters.Scale.ToString();

                // najdi barvu v ComboBoxu
                ColorPreview.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(parameters.ColorHex));

            }
        }

    }
}
