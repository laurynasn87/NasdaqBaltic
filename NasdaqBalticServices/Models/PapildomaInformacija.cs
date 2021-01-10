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
            return ((PapildomaInformacija)o).Id == this.Id && ((PapildomaInformacija)o).AkcijosKodas.Equals(this.AkcijosKodas) && ((PapildomaInformacija)o).Apie.Equals(this.Apie) && ((PapildomaInformacija)o).Vadyba.Equals(this.Vadyba) && ((PapildomaInformacija)o).Kontaktai.Equals(this.Kontaktai) &&
                ((PapildomaInformacija)o).AtaskaitosURL.Equals(this.AtaskaitosURL) && ((PapildomaInformacija)o).Kalba.Equals(this.Kalba) && ((PapildomaInformacija)o).Didziausia_Kaina == this.Didziausia_Kaina && ((PapildomaInformacija)o).Maziausia_Kaina == this.Maziausia_Kaina && ((PapildomaInformacija)o).Vidutine_Kaina == this.Vidutine_Kaina &&
                ((PapildomaInformacija)o).Atidarymo_Kaina == this.Atidarymo_Kaina;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
