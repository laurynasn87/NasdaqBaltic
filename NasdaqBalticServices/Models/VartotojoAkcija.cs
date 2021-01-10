using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class VartotojoAkcija : ModelBase
    {
        public int Id { get; set; }
        public Akcijos akcija { get; set; }
        public Vartotojas vartotojas { get; set; }
        public bool Pirkimas { get; set; }
        public double PirkimoKaina { get; set; }
        public double UzdarymoKaina { get; set; }
        public int Kiekis { get; set; }
        public DateTime UzdarymoData { get; set; }
        public DateTime AtidarymoData { get; set; }
        public string Priezastis { get; set; }



        public VartotojoAkcija()
        {
            KintamujuMapping = GetMapping();
        }
        public VartotojoAkcija(int id)
        {
            KintamujuMapping = GetMapping();
            Id = id;
        }

        public List<Tuple<string, string>> PaverstAkcijaITupleList(List<string> IgnoreList = null)
        {
            return PaverstObjektaITupleList(this, IgnoreList);
        }

        public VartotojoAkcija ListToVartotojoAkcija(List<Tuple<string, string>> vienasElementas)
        {
            VartotojoAkcija akcija = ListToObject<VartotojoAkcija>(vienasElementas);
            return akcija;
        }

        private List<Tuple<string, string>> GetMapping()
        {
            List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>(){
            new Tuple<string, string>("Id", "Id"),
            new Tuple<string, string>("AkcijosKodas", "akcija.AkcijosKodas"),
            new Tuple<string, string>("VartotojoId", "vartotojas.Id"),
            new Tuple<string, string>("Pirkimas", "Pirkimas"),
            new Tuple<string, string>("PirkimoKaina", "PirkimoKaina"),
            new Tuple<string, string>("UzdarymoKaina", "UzdarymoKaina"),
            new Tuple<string, string>("Kiekis", "Kiekis"),
            new Tuple<string, string>("UzdarymoData", "UzdarymoData"),
            new Tuple<string, string>("Priezastis", "Priezastis"),
            new Tuple<string, string>("AtidarymoData", "AtidarymoData")
            };

            return KintamujuMapping;
        }
        public override bool Equals(object o)
        {
            if (!(o is VartotojoAkcija)) { return false; }
            return ((VartotojoAkcija)o).Id == this.Id && ((VartotojoAkcija)o).PirkimoKaina == this.PirkimoKaina && ((VartotojoAkcija)o).UzdarymoKaina == this.UzdarymoKaina && ((VartotojoAkcija)o).Kiekis == this.Kiekis &&
                ((VartotojoAkcija)o).akcija.Equals(this.akcija) && ((VartotojoAkcija)o).vartotojas.Equals(this.vartotojas) && ((VartotojoAkcija)o).UzdarymoData.Equals(this.AtidarymoData) && ((VartotojoAkcija)o).Priezastis.Equals(this.Priezastis) &&
                 ((VartotojoAkcija)o).Pirkimas == this.Pirkimas;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
