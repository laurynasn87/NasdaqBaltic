using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Models
{
   public class PapildomaInformacija : ModelBase
    {
        public int Id { get; set; }
        public string AkcijosKodas { get; set; }
        public string Apie { get; set; }
        public string AtaskaitosURL { get; set; }
        public double Didziausia_Kaina { get; set; }
        public double Maziausia_Kaina { get; set; }
        public double Vidutine_Kaina { get; set; }
        public double Atidarymo_Kaina { get; set; }
        public string Vadyba { get; set; }
        public string Kontaktai { get; set; }
        public string Kalba { get; set; }

        public PapildomaInformacija()
        {
            KintamujuMapping = GetMapping();
        }
        public PapildomaInformacija ListToPapildomaInformacija(List<Tuple<string, string>> pI)
        {
            PapildomaInformacija PapInfo = ListToObject<PapildomaInformacija>(pI);
            return PapInfo;
        }
        public List<Tuple<string, string>> PaverstPapildomaInformacijaITupleList(List<string> IgnoravimoSar = null)
        {
            List<Tuple<string, string>> tuples = PaverstObjektaITupleList(this, IgnoravimoSar);
            return tuples;
        }
        private List<Tuple<string, string>> GetMapping()
        {
            List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>()
            {
            new Tuple<string, string>("Id","Id"),
            new Tuple<string, string>("AkcijosKodas","AkcijosKodas"),
            new Tuple<string, string>("Apie","Apie"),
            new Tuple<string, string>("AtaskaitosURL","AtaskaitosURL"),
            new Tuple<string, string>("DidKaina","Didziausia_Kaina"),
            new Tuple<string, string>("MinKaina","Maziausia_Kaina"),
            new Tuple<string, string>("VidKaina","Vidutine_Kaina"),
            new Tuple<string, string>("AtidarymoKaina","Atidarymo_Kaina"),
            new Tuple<string, string>("Vadyba","Vadyba"),
            new Tuple<string, string>("Kontaktai","Kontaktai"),
            new Tuple<string, string>("Kalba","Kalba")
            };
            return KintamujuMapping;
        }
        public override bool Equals(object o)
        {
            if (!(o is PapildomaInformacija)) { return false; }

            PapildomaInformacija papildomaInformacija = (PapildomaInformacija)o;
            if (papildomaInformacija.Id != this.Id)
                return false;
            if (!object.Equals(papildomaInformacija.AkcijosKodas, this.AkcijosKodas))
                return false;
            if (!object.Equals(papildomaInformacija.Apie, this.Apie))
                return false;
            if (!object.Equals(papildomaInformacija.AtaskaitosURL, this.AtaskaitosURL))
                return false;
            if (!object.Equals(papildomaInformacija.Vadyba, this.Vadyba))
                return false;
            if (!object.Equals(papildomaInformacija.Kontaktai, this.Kontaktai))
                return false;
            if (!object.Equals(papildomaInformacija.Kalba, this.Kalba))
                return false;
            if (papildomaInformacija.Didziausia_Kaina != this.Didziausia_Kaina)
                return false;
            if (papildomaInformacija.Maziausia_Kaina != this.Maziausia_Kaina)
                return false;
            if (papildomaInformacija.Vidutine_Kaina != this.Vidutine_Kaina)
                return false;
            if (papildomaInformacija.Atidarymo_Kaina != this.Atidarymo_Kaina)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
