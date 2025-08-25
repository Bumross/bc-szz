using sprava_kontaktu.Models;
using sprava_kontaktu.Patterns.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu.UI
{
    public static class ContactDetailView
    {
        public static void Show(ContactBook book, CommandManager manager, Guid id)
        {
            while (true)
            {
                var c = book.Find(id);
                if (c == null) return;

                Console.Clear();
                Console.WriteLine("Detail kontaktu");
                Console.WriteLine("---------------");
                Console.WriteLine($"Name : {c.Name}");
                Console.WriteLine($"Phone: {c.Phone}");
                Console.WriteLine($"Email: {c.Email}");
                Console.WriteLine($"Skype: {c.Skype}");
                Console.WriteLine();
                Console.WriteLine("[E] editovat  [D] smazat  [C] klonovat  [U] undo  [R] redo  [Q] zpět");
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Q) return;
                else if (key.Key == ConsoleKey.E)
                {
                    var edited = EditForm(c);
                    if (edited != null)
                        manager.Do(new UpdateContactCommand(book, edited));
                }
                else if (key.Key == ConsoleKey.D)
                {
                    manager.Do(new DeleteContactCommand(book, c.Id));
                    return; // po smazání se vrať
                }
                else if (key.Key == ConsoleKey.C)
                {
                    var draft = c.Clone(); // má nové Guid a zkopírovaná pole

                    // Otevři formulář stejně jako u "vytvořit nový", ale s předvyplněnými hodnotami
                    var editedClone = CreateFromTemplateForm(draft, "Klonování kontaktu – upravte údaje");
                    if (editedClone != null)
                    {
                        // Uložení proběhne přes Command (Add) = podporuje undo/redo
                        manager.Do(new AddContactCommand(book, editedClone));
                    }
                }
                else if (key.Key == ConsoleKey.U)
                {
                    manager.Undo();
                }
                else if (key.Key == ConsoleKey.R)
                {
                    manager.Redo();
                }
            }
        }

        private static Contact? EditForm(Contact src)
        {
            var work = new Contact
            {
                Id = src.Id,
                Name = src.Name,
                Phone = src.Phone,
                Email = src.Email,
                Skype = src.Skype
            };

            int index = 0;
            string[] labels = { "Name", "Phone", "Email", "Skype" };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Editace kontaktu");
                Console.WriteLine("----------------");
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
                else if (key.Key == ConsoleKey.S) return work;
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

        private static Contact? CreateFromTemplateForm(Contact template, string title)
        {
            // Pracovní kopie – už má nové Guid (z Clone), jen ji necháme uživateli upravit
            var work = new Contact
            {
                Id = template.Id,        // zachováme, je už nový
                Name = template.Name,
                Phone = template.Phone,
                Email = template.Email,
                Skype = template.Skype
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
                    // případně můžeš vynutit nové Guid ještě tady:
                    if (work.Id == Guid.Empty) work.Id = Guid.NewGuid();
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
    }
}