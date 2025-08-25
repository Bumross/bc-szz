using System;

namespace sprava_kontaktu.UI
{
    public class ConsoleMenu
    {
        public int Select(string title, string[] options)
        {
            int index = 0;
            ConsoleKey key;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(title);
                Console.WriteLine(new string('-', title.Length));
                for (int i = 0; i < options.Length; i++)
                {
                    var prefix = (i == index) ? ">> " : "   ";
                    Console.WriteLine($"{prefix}{options[i]}");
                }

                var info = Console.ReadKey(true);
                key = info.Key;

                if (key == ConsoleKey.UpArrow)
                    index = (index - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.DownArrow)
                    index = (index + 1) % options.Length;
                else if (key == ConsoleKey.Enter)
                    return index;
            }
        }

        // Stránkovaný list: šipky pro pohyb, Enter pro výběr
        public int ListNavigate(string title, string[] rows, int pageSize = 10)
        {
            if (rows.Length == 0) return -1;

            int selected = 0;
            while (true)
            {
                Console.Clear();
                int page = selected / pageSize;
                int start = page * pageSize;
                int end = Math.Min(start + pageSize, rows.Length);

                Console.WriteLine(title);
                Console.WriteLine($"(Stránka {page + 1}/{(rows.Length + pageSize - 1) / pageSize})");
                Console.WriteLine(new string('-', title.Length));

                for (int i = start; i < end; i++)
                {
                    var prefix = (i == selected) ? ">> " : "   ";
                    Console.WriteLine($"{prefix}{rows[i]}");
                }

                Console.WriteLine();
                Console.WriteLine("[↑/↓] pohyb  [Enter] vybrat  [Q] zpět");
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow) selected = (selected - 1 + rows.Length) % rows.Length;
                else if (key.Key == ConsoleKey.DownArrow) selected = (selected + 1) % rows.Length;
                else if (key.Key == ConsoleKey.Enter) return selected;
                else if (key.Key == ConsoleKey.Q) return -1;
            }
        }

        public enum ListAction { Select, Back, Load, Save, Add }

        public struct ListResult
        {
            public ListAction Action;
            public int Index;   // platí jen pro Select
        }
    }
}
