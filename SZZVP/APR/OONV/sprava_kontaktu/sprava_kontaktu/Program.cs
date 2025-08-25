using sprava_kontaktu.Models;
using sprava_kontaktu.Patterns.Commands;
using sprava_kontaktu.Patterns.Services;
using sprava_kontaktu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Contact Manager (Console)";

            var storage = new FileStorageService("contacts.json");
            var book = new ContactBook();
            var manager = new CommandManager(capacity: 5);

            // Načtení při startu
            try
            {
                foreach (var c in storage.Load())
                    book.Add(c);
            }
            catch { /* první start bez souboru je OK */ }

            var mainMenu = new ConsoleMenu();

            while (true)
            {
                var choice = mainMenu.Select(
                    "Hlavní menu",
                    new[] { "Kontakty", "Poslední změny", "Odejít" });

                if (choice == 0)
                {
                    ContactsListView.Show(book, manager, storage);
                }
                else if (choice == 1)
                {
                    RecentChangesView.Show(manager);
                }
                else
                {
                    break;
                }
            }

            // Uložení při ukončení
            storage.Save(book.All());
            Console.WriteLine("Uloženo. Stiskni libovolnou klávesu…");
            Console.ReadKey(true);
        }
    }
}
