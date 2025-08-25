using sprava_kontaktu.Models;
using sprava_kontaktu.Patterns.Commands;
using sprava_kontaktu.Patterns.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu.UI
{
    public static class ContactsListView
    {
        public static void Show(ContactBook book, CommandManager manager, IStorageService storage)
        {
            var menu = new ConsoleMenu();

            while (true)
            {
                // Iterator prochází všechny – slouží jako splnění vzoru v rámci navigace
                var iterator = book.GetIterator();
                var lines = book.All().Select(c => $"{c.Name} [{c.Id}]").ToArray();

                Console.Clear();
                Console.WriteLine("Kontakty");
                Console.WriteLine("-------");
                Console.WriteLine("L = Načíst ze souboru | S = Uložit | A = Přidat | Q = Zpět");
                Console.WriteLine();

                if (lines.Length == 0)
                {
                    Console.WriteLine("(Žádné kontakty)");
                }
                else
                {
                    // Použijeme stránkovanou navigaci
                    int selected = menu.ListNavigate("Seznam kontaktů", lines, 10);
                    if (selected >= 0)
                    {
                        var chosen = book.All().ElementAt(selected);
                        ContactDetailView.Show(book, manager, chosen.Id);
                        continue;
                    }
                    else
                    {
                        // „Q“ z ListNavigate => návrat do lokální smyčky a níže přečteme volbu
                    }
                }

                // Přečti volby L/S/A/Q
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Q) break;
                else if (key.Key == ConsoleKey.L)
                {
                    foreach (var c in storage.Load())
                    { /* nahradíme celou knihovnu */
                        // Jednoduše: vyčistíme a nahradíme
                    }
                    // lepší: nahradit celý obsah:
                    var loaded = storage.Load().ToList();
                    // Smaž staré:
                    foreach (var c in book.All().ToList())
                        book.Remove(c.Id);
                    // Přidej nové:
                    foreach (var c in loaded) book.Add(c);

                    manager.Do(new NoopLogCommand("Načtení kontaktů ze souboru"));
                }
                else if (key.Key == ConsoleKey.S)
                {
                    storage.Save(book.All());
                    manager.Do(new NoopLogCommand("Uložení kontaktů do souboru"));
                }
                else if (key.Key == ConsoleKey.A)
                {
                    var created = EditForm(new Contact(), "Vytvořit nový kontakt");
                    if (created != null)
                        manager.Do(new AddContactCommand(book, created));
                }
            }
        }

        // Jednoduchý formulář s pohybem šipkami (položky pod sebou)
        private static Contact? EditForm(Contact original, string title)
        {
            var work = new Contact
            {
                Id = original.Id, // při vytváření má nový v konstruktoru; při editaci zachováme
                Name = original.Name,
                Phone = original.Phone,
                Email = original.Email,
                Skype = original.Skype
            };

            int index = 0;
            string[] labels = { "Name", "Phone", "Email", "Skype" };

            while (true)
            {
                Console.Clear();
                Console.WriteLine(title);
                Console.WriteLine(new string('-', title.Length));
                for (int i = 0; i < labels.Length; i++)
                {
                    string val = labels[i] switch
                    {
                        "Name" => work.Name,
                        "Phone" => work.Phone,
                        "Email" => work.Email,
                        "Skype" => work.Skype,
                        _ => ""
                    };
                    var prefix = (i == index) ? ">> " : "   ";
                    Console.WriteLine($"{prefix}{labels[i]}: {val}");
                }
                Console.WriteLine();
                Console.WriteLine("[Enter] upravit položku  |  [S] uložit  |  [Q] zrušit");

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) index = (index - 1 + labels.Length) % labels.Length;
                else if (key.Key == ConsoleKey.DownArrow) index = (index + 1) % labels.Length;
                else if (key.Key == ConsoleKey.Q) return null;
                else if (key.Key == ConsoleKey.S)
                {
                    // Při tvorbě: zajistíme nové Guid
                    if (original.Id == Guid.Empty)
                        work.Id = Guid.NewGuid();
                    return work;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.Write($"{labels[index]}: ");
                    var input = Console.ReadLine() ?? "";
                    switch (labels[index])
                    {
                        case "Name": work.Name = input; break;
                        case "Phone": work.Phone = input; break;
                        case "Email": work.Email = input; break;
                        case "Skype": work.Skype = input; break;
                    }
                }
            }
        }

        // pomocná třída – log-only command pro L/S
        private class NoopLogCommand : ICommand
        {
            public string Description { get; }
            public NoopLogCommand(string description) => Description = description;
            public void Execute() { }
            public void Unexecute() { }
        }
    }
}
