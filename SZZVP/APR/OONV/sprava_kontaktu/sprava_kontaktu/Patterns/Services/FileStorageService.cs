using sprava_kontaktu.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;           // <<< změna
using sprava_kontaktu.Models;

namespace sprava_kontaktu.Patterns.Services
{
    public class FileStorageService : IStorageService
    {
        private readonly string _path;
        public FileStorageService(string path) => _path = path;

        public IEnumerable<Contact> Load()
        {
            if (!File.Exists(_path)) return new List<Contact>();
            var json = File.ReadAllText(_path);
            var data = JsonConvert.DeserializeObject<List<Contact>>(json);   // <<< změna
            return data ?? new List<Contact>();
        }

        public void Save(IEnumerable<Contact> contacts)
        {
            var json = JsonConvert.SerializeObject(contacts, Formatting.Indented); // <<< změna
            File.WriteAllText(_path, json);
        }
    }
}
