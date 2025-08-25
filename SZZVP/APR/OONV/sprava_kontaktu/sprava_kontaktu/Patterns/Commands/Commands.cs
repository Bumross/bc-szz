using sprava_kontaktu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu.Patterns.Commands
{
    public class DeleteContactCommand : ICommand
    {
        private readonly ContactBook _book;
        private readonly Guid _id;
        private Contact? _backup;

        public string Description => $"Smazání kontaktu: {_backup?.Name ?? _id.ToString()}";

        public DeleteContactCommand(ContactBook book, Guid id)
        {
            _book = book;
            _id = id;
        }

        public void Execute()
        {
            _backup = _book.Find(_id);
            if (_backup != null)
                _book.Remove(_id);
        }

        public void Unexecute()
        {
            if (_backup != null)
                _book.Add(_backup);
        }
    }


    public class UpdateContactCommand : ICommand
    {
        private readonly ContactBook _book;
        private readonly Contact _new;
        private Contact? _old;

        public string Description => $"Změna kontaktu: {_new.Name}";

        public UpdateContactCommand(ContactBook book, Contact updated)
        {
            _book = book;
            _new = updated;
        }

        public void Execute()
        {
            _old = _book.Find(_new.Id);
            if (_old != null)
                _book.Update(_new);
        }

        public void Unexecute()
        {
            if (_old != null)
                _book.Update(_old);
        }
    }


    public class CloneContactCommand : ICommand
    {
        private readonly ContactBook _book;
        private readonly Guid _sourceId;
        private Contact? _cloned;

        public string Description => _cloned == null
            ? $"Klonování kontaktu (source: {_sourceId})"
            : $"Klonování kontaktu: {_cloned.Name}";

        public CloneContactCommand(ContactBook book, Guid sourceId)
        {
            _book = book;
            _sourceId = sourceId;
        }

        public void Execute()
        {
            var src = _book.Find(_sourceId);
            if (src != null)
            {
                _cloned = src.Clone(); // Prototype
                _cloned.Name += " (copy)";
                _book.Add(_cloned);
            }
        }

        public void Unexecute()
        {
            if (_cloned != null)
                _book.Remove(_cloned.Id);
        }
    }

    public class AddContactCommand : ICommand
    {
        private readonly ContactBook _book;
        private readonly Contact _contact;

        public string Description => $"Vytvoření kontaktu: {_contact.Name}";

        public AddContactCommand(ContactBook book, Contact contact)
        {
            _book = book;
            _contact = contact;
        }

        public void Execute() => _book.Add(_contact);

        public void Unexecute() => _book.Remove(_contact.Id);
    }

}
