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

    }
}
