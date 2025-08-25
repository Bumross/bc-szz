using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu.Models
{
    public class ContactBook
    {
        private readonly List<Contact> _contacts = new();

        public int Count => _contacts.Count;

        public IContactIterator GetIterator() => new ContactIterator(_contacts);

        public IEnumerable<Contact> All() => _contacts.ToList(); // kopie

        public void Add(Contact c) => _contacts.Add(c);

        public bool Remove(Guid id)
        {
            var idx = _contacts.FindIndex(x => x.Id == id);
            if (idx >= 0) { _contacts.RemoveAt(idx); return true; }
            return false;
        }

        public Contact? Find(Guid id) => _contacts.FirstOrDefault(x => x.Id == id);

        public bool Update(Contact c)
        {
            var idx = _contacts.FindIndex(x => x.Id == c.Id);
            if (idx >= 0) { _contacts[idx] = c; return true; }
            return false;
        }

        public IEnumerable<Contact> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return All();
            query = query.Trim().ToLowerInvariant();
            return _contacts.Where(c =>
                (c.Name?.ToLowerInvariant().Contains(query) ?? false) ||
                (c.Email?.ToLowerInvariant().Contains(query) ?? false) ||
                (c.Phone?.ToLowerInvariant().Contains(query) ?? false) ||
                (c.Skype?.ToLowerInvariant().Contains(query) ?? false));
        }
    }
}
