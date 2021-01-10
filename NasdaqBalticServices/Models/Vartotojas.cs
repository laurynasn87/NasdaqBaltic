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
        public Double Balansas { get; set; }
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
        public Vartotojas(int Id)
        {
            KintamujuMapping = GetMapping();
            this.Id = Id;
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
        public override bool Equals(object o)
        {
            if (!(o is Vartotojas)) { return false; }

            Vartotojas vartotojas = (Vartotojas)o;

            if (vartotojas.Id != this.Id)
                return false;
            if (!object.Equals(vartotojas.Vardas, this.Vardas))
                return false;
            if (!object.Equals(vartotojas.Slaptazodis, this.Slaptazodis))
                return false;
            if (vartotojas.Balansas != this.Balansas)
                return false;
            if (!vartotojas.Created_on.Equals(this.Created_on))
                return false;

            return true;
        }
      
        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
