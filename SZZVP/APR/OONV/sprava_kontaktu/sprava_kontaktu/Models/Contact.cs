using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu.Models
{
    public class Contact
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Skype { get; set; } = "";

        // Prototype – hluboká kopie
        public Contact Clone()
        {
            return new Contact
            {
                Id = Guid.NewGuid(), // nový identifikátor pro klon
                Name = this.Name,
                Phone = this.Phone,
                Email = this.Email,
                Skype = this.Skype
            };
        }

        public override string ToString()
            => $"{Name} | {Phone} | {Email} | {Skype}";
    }
}
