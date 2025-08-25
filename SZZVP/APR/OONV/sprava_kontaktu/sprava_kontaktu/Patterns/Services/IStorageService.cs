using sprava_kontaktu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu.Patterns.Services
{
    public interface IStorageService
    {
        IEnumerable<Contact> Load();
        void Save(IEnumerable<Contact> contacts);
    }
}
