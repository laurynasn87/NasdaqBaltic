using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Models
{
    public class Akcijos : ModelBase
    {
        public string AkcijosKodas { get; set;}

        public string Pavadinimas { get; set;}
        public FinansineInformacija finansineInformacija { get; set; }
        public PapildomaInformacija papildomaInformacija { get; set; }

        public Akcijos()
        {
            KintamujuMapping = GetMapping();
        }
        public Akcijos(string AkcijosKodas, string Pavadinimas)
        {
            this.AkcijosKodas = AkcijosKodas;
            this.Pavadinimas = Pavadinimas;
            KintamujuMapping = GetMapping();
        }
        public Akcijos(string AkcijosKodas)
        {
            this.AkcijosKodas = AkcijosKodas;
            KintamujuMapping = GetMapping();
        }
        public List<Tuple<string, string>> PaverstAkcijaITupleList(List<string> IgnoreList = null)
        {
            return PaverstObjektaITupleList(this, IgnoreList);
        }

        public Akcijos ListToAkcija(List<Tuple<string, string>> vienasElementas)
        {
            Akcijos akcija = ListToObject<Akcijos>(vienasElementas);
            return akcija;
        }

        private List<Tuple<string, string>> GetMapping()
        {
            List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>(){new Tuple<string, string>("AkcijosKodas", "AkcijosKodas"),
            new Tuple<string, string>("Pavadinimas", "Pavadinimas") };

            return KintamujuMapping;
        }
        public override bool Equals(object o)
        {
            if (!(o is Akcijos)) { return false; }

            Akcijos akcija = (Akcijos) o;

            if (!object.Equals(akcija.AkcijosKodas, this.AkcijosKodas))
                return false;

            if (!object.Equals(akcija.Pavadinimas, this.Pavadinimas))
                return false;

            if (akcija.finansineInformacija != null || this.finansineInformacija != null)
            {
                if ((akcija.finansineInformacija == null && this.finansineInformacija != null) || (akcija.finansineInformacija != null && this.finansineInformacija == null))
                    return false;
                else
                {
                    if (!akcija.finansineInformacija.Equals(finansineInformacija))
                        return false;
                }
            }
            if (akcija.papildomaInformacija != null || this.papildomaInformacija != null)
            {
                if ((akcija.papildomaInformacija == null && this.papildomaInformacija != null) || (akcija.papildomaInformacija != null && this.papildomaInformacija == null))
                    return false;
                else
                {
                    if (!akcija.papildomaInformacija.Equals(papildomaInformacija))
                        return false;
                }
            }

            return true;

        }
    }
}
