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
        public bool ArIdentiski(PapildomaInformacija v1, PapildomaInformacija v2)
        {
                foreach (PropertyInfo prop in v1.GetType().GetProperties())
                {
                if (prop.Name.Equals("Id"))
                    continue;

                    var v1Value = prop.GetValue(v1);
                    var v2Value = prop.GetValue(v2);
                    if (v1Value != v2Value)
                    {
                    return false;
                    }

                }
            return true;
        }
        public List<Tuple<string, string>> PaverstPapildomaInformacijaITupleList()
        {
            List<Tuple<string, string>> tuples = PaverstObjektaITupleList(this);
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
            new Tuple<string, string>("Vadyba","Vadyba"),
            new Tuple<string, string>("Kontaktai","Kontaktai"),
            new Tuple<string, string>("Kalba","Kalba")
            };
            return KintamujuMapping;
        }
    }
}
