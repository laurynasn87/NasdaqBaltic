using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Models
{
   public class FinansineInformacija : ModelBase
    {
        public int Id { get; set; }
        public string AkcijosKodas { get; set; }
        public double Paskutine_Kaina { get; set; }
        public double Pokytis { get; set; }
        public double PokytisProcentais { get; set; }
        public double PirkimoKaina { get; set; }
        public double PardavimoKaina { get; set; }
        public int Sandoriai { get; set; }
        public int Kiekis { get; set; }
        public int Apyvarta { get; set; }
        public DateTime Timestamp { get; set; }

        public FinansineInformacija()
        {
            KintamujuMapping = GetMapping();
        }
        public FinansineInformacija ListToFinansineInformacija(List<Tuple<string, string>> vienasFI)
        {
            FinansineInformacija FI = ListToObject<FinansineInformacija>(vienasFI);
            return FI;
        }
        public List<Tuple <string,string>> PaverstFinansineinformacijaITupleList(List<string> IgnoreList)
        {
            List<Tuple<string, string>> tuples = PaverstObjektaITupleList(this, IgnoreList);
            return tuples;
        }
        private List<Tuple<string, string>> GetMapping()
        {
            List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>()
            {
            new Tuple<string, string>("Id","Id"),
            new Tuple<string, string>("AkcijosKodas","AkcijosKodas"),
            new Tuple<string, string>("PaskKaina","Paskutine_Kaina"),
            new Tuple<string, string>("Pokytis","Pokytis"),
            new Tuple<string, string>("PokytisProcentais","PokytisProcentais"),
            new Tuple<string, string>("PirkimoKaina","PirkimoKaina"),
            new Tuple<string, string>("PardavimoKaina","PardavimoKaina"),
            new Tuple<string, string>("Sandoriai","Sandoriai"),
            new Tuple<string, string>("Kiekis","Kiekis"),
            new Tuple<string, string>("Apyvarta","Apyvarta"),
            new Tuple<string, string>("LaikoZyme","Timestamp")
        };
            return KintamujuMapping;
        }
        public override bool Equals(object o)
        {
            if (!(o is FinansineInformacija)) { return false; }
            return ((FinansineInformacija)o).Id == this.Id && ((FinansineInformacija)o).AkcijosKodas.Equals(this.AkcijosKodas) && ((FinansineInformacija)o).Paskutine_Kaina == this.Paskutine_Kaina && ((FinansineInformacija)o).Pokytis == this.Pokytis &&
                ((FinansineInformacija)o).PokytisProcentais == this.PokytisProcentais && ((FinansineInformacija)o).PirkimoKaina == this.PirkimoKaina && ((FinansineInformacija)o).PardavimoKaina == this.PardavimoKaina &&
                ((FinansineInformacija)o).Sandoriai == this.Sandoriai && ((FinansineInformacija)o).Apyvarta == this.Apyvarta && ((FinansineInformacija)o).Timestamp == this.Timestamp;
        }


        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
