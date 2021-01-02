using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
  public class Vartotojas : ModelBase
    {
        public int Id { get; set; }
        public string Vardas { get; set; }
        public string Slaptazodis { get; set; }
        public string Balansas { get; set; }
        public DateTime Created_on { get; set; }
        public Vartotojas()
        {
            KintamujuMapping = GetMapping();
        }
        private List<Tuple<string, string>> GetMapping()
        {
            List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>(){new Tuple<string, string>("Id", "Id"), // DB name - Objekto property name
            new Tuple<string, string>("Vardas", "Vardas"),
            new Tuple<string, string>("Slaptazodis", "Slaptazodis"),
            new Tuple<string, string>("Balansas", "Balansas"),
            new Tuple<string, string>("Created_on", "Created_on")
            };

            return KintamujuMapping;
        }

        public List<Tuple<string, string>> PaverstVartotojaITupleList(List<string> ignoreColumns = null)
        {
            return PaverstObjektaITupleList(this, ignoreColumns);
        }

        public Vartotojas ListToVartotojas(List<Tuple<string, string>> vienasElementas)
        {
            Vartotojas vartotojas = ListToObject<Vartotojas>(vienasElementas);
            return vartotojas;
        }
    }
}
