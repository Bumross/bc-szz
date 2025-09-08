using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace FractalTreeWPF.Services
{
    public class FractalTreeDrawer
    {
        private readonly Canvas canvas;
        private readonly SolidColorBrush brush;

        public FractalTreeDrawer(Canvas canvas, Color color)
        {
            this.canvas = canvas;
            this.brush = new SolidColorBrush(color);
        }

        public void DrawLine(double x1, double y1, double x2, double y2, double thickness)
        {
            var line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = brush,
                StrokeThickness = thickness
            };

            canvas.Children.Add(line);
        }
    }
}
