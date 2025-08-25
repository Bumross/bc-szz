using sprava_kontaktu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu.Models
{
    public interface IContactIterator
    {
        bool MoveNext();
        void Reset();
        Contact Current { get; }
        int Index { get; }
    }

    public class ContactIterator : IContactIterator
    {
        private readonly List<Contact> _contacts;
        private int _index = -1;

        public ContactIterator(List<Contact> contacts)
        {
            _contacts = contacts;
        }

        public bool MoveNext()
        {
            if (_index + 1 < _contacts.Count)
            {
                _index++;
                return true;
            }
            return false;
        }

        public void Reset() => _index = -1;

        public Contact Current => (_index >= 0 && _index < _contacts.Count)
            ? _contacts[_index]
            : null;

        public int Index => _index;
    }
}
