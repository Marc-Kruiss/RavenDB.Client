using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDB.Client.Contracts
{
    public class Owner
    {
        public Owner()
        {
            Pets = new List<Pet>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<Pet> Pets { get; set; }

        public override string ToString()
        {
            return
                "Owner's Id: " + Id + "\n" +
                "Owner's name: " + Name + "\n" +
                "Pets:\n\t" +
                string.Join("\n\t", Pets.Select(p => p.ToString()));
        }
    }
}
