using FractalTreeWPF.Models;
using System.IO;
using System.Text.Json;

namespace FractalTreeWPF.Services
{
    public static class IOManager
    {
        public static void Save(string path, TreeParameters parameters)
        {
            string json = JsonSerializer.Serialize(parameters, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static TreeParameters Load(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<TreeParameters>(json);
        }
    }
}
