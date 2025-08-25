using sprava_kontaktu.Patterns.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu.UI
{
    public static class RecentChangesView
    {
        public static void Show(CommandManager manager)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Poslední změny");
                Console.WriteLine("--------------");
                var list = manager.RecentChanges();
                if (list.Count == 0) Console.WriteLine("(Žádné změny)");
                else
                {
                    for (int i = Math.Max(0, list.Count - 20); i < list.Count; i++)
                        Console.WriteLine($"- {list[i]}");
                }
                Console.WriteLine();
                Console.WriteLine("[Z] odebrat poslední záznam  [Y] vrátit odebraný  [Q] zpět");
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Q) return;
                else if (key.Key == ConsoleKey.Z)
                {
                    manager.PopLastChange(out _);
                }
                else if (key.Key == ConsoleKey.Y)
                {
                    manager.PushBackChange();
                }
            }
        }
    }
}
