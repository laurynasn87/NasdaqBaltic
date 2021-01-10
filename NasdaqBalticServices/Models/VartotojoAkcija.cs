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

            VartotojoAkcija vartotojoAkcija = (VartotojoAkcija)o;

            if (vartotojoAkcija.Id != this.Id)
                return false;

            if (vartotojoAkcija.akcija != null || this.akcija != null)
            {
                if ((vartotojoAkcija.akcija == null && this.akcija != null) || (vartotojoAkcija.akcija != null && this.akcija == null))
                    return false;
                else
                {
                    if (!vartotojoAkcija.akcija.Equals(akcija))
                        return false;
                }
            }
            if (vartotojoAkcija.vartotojas != null || this.vartotojas != null)
            {
                if ((vartotojoAkcija.vartotojas == null && this.vartotojas != null) || (vartotojoAkcija.vartotojas != null && this.vartotojas == null))
                    return false;
                else
                {
                    if (!vartotojoAkcija.vartotojas.Equals(vartotojas))
                        return false;
                }
            }
            if (vartotojoAkcija.UzdarymoData != null || this.UzdarymoData != null)
            {
                if ((vartotojoAkcija.UzdarymoData == null && this.UzdarymoData != null) || (vartotojoAkcija.UzdarymoData != null && this.UzdarymoData == null))
                    return false;
                else
                {
                    if (!vartotojoAkcija.UzdarymoData.Equals(UzdarymoData))
                        return false;
                }
            }
            if (vartotojoAkcija.AtidarymoData != null || this.AtidarymoData != null)
            {
                if ((vartotojoAkcija.AtidarymoData == null && this.AtidarymoData != null) || (vartotojoAkcija.AtidarymoData != null && this.AtidarymoData == null))
                    return false;
                else
                {
                    if (!vartotojoAkcija.AtidarymoData.Equals(AtidarymoData))
                        return false;
                }
            }
            if (vartotojoAkcija.Pirkimas != this.Pirkimas)
                return false;

            if (vartotojoAkcija.PirkimoKaina != this.PirkimoKaina)
                return false;

            if (vartotojoAkcija.UzdarymoKaina != this.UzdarymoKaina)
                return false;

            if (vartotojoAkcija.Kiekis != this.Kiekis)
                return false;

            if (!object.Equals(vartotojoAkcija.Priezastis, this.Priezastis))
                return false;
            return true;

        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
