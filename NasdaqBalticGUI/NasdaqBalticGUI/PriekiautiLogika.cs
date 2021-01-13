using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqBalticGUI
{
    public class PriekiautiLogika
    {
        ApiKontroleris api = new ApiKontroleris();
        private string AkcijosUrl;
        int BirzosDarboPradziaH = 10;
        int BirzosDarboPabaigaH = 16;
        public Dictionary<DateTime, double> MenesioPirkimoReiksmes = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, double> MenesioPardavimoReiksmes = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, double> DienosPirkimoReiksmes = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, double> DienosPardavimoReiksmes = new Dictionary<DateTime, double>();
        public PriekiautiLogika()
        {
            AkcijosUrl = api.BaseUrl + "akcijos";
        }
        public List<Tuple<string, string, int>> LentelesStulpeliaiMapping = new List<Tuple<string, string, int>>() // Lenteles column pav -kintamojo name - width
            {
          new Tuple<string, string, int>("Pavadinimas", "Pavadinimas",-1),
          new Tuple<string, string, int>("Pokytis", "FinansineInformacija.PokytisProcentais",-2),
          new Tuple<string, string, int>("+/-", "FinansineInformacija.Pokytis",-2),
          new Tuple<string, string, int>("Pirkimo K.", "FinansineInformacija.PirkimoKaina",-2),
          new Tuple<string, string, int>("Pardavimo K.", "FinansineInformacija.PardavimoKaina",-2),
          new Tuple<string, string, int>("Paskutine K.", "FinansineInformacija.Paskutine_Kaina",-2),
          new Tuple<string, string, int>("Sandoriai", "FinansineInformacija.Sandoriai",-2),
          new Tuple<string, string, int>("Apyvarta", "FinansineInformacija.Apyvarta",-2)
            };
        public List<Akcijos> GautiVisasAkcijas()
        {
            List<Akcijos> VisosAkcijos;

            VisosAkcijos = api.GetApiCallResponseObject<List<Akcijos>>(AkcijosUrl);

            return VisosAkcijos;
        }
        public Vartotojas GautiVartotoja(int id)
        {
            Vartotojas GautasVartotojas;
            string VartotojoUrl = api.BaseUrl + "Vartotojai/" + id.ToString();
            GautasVartotojas = api.GetApiCallResponseObject<Vartotojas>(VartotojoUrl);

            return GautasVartotojas;
        }
        public bool GautiAkcijosStatistika(String AkcijosKodas)
        {
            MenesioPirkimoReiksmes = null;
            MenesioPardavimoReiksmes = null;
            DienosPirkimoReiksmes = null;
            DienosPardavimoReiksmes = null;
            if (!string.IsNullOrEmpty(AkcijosKodas))
            {
                Dictionary<string, Dictionary<DateTime, double>> ApiAtsakymas = api.GetApiCallResponseObject<Dictionary<string, Dictionary<DateTime, double>>>(AkcijosUrl + $"/statistika/{AkcijosKodas}");
                if (ApiAtsakymas != null)
                {
                    ApiAtsakymas.TryGetValue("Menesis-Pirkimas", out MenesioPirkimoReiksmes);
                    ApiAtsakymas.TryGetValue("Menesis-Pardavimas", out MenesioPardavimoReiksmes);
                    ApiAtsakymas.TryGetValue("Diena-Pirkimas", out DienosPirkimoReiksmes);
                    ApiAtsakymas.TryGetValue("Diena-Pardavimas", out DienosPardavimoReiksmes);
                    return true;
                }
            }
            return false;
        }
        public bool ArDirbaAkcijuBirza(List<Akcijos> gautosAkcijos)
        {
            bool arDirbaBirza = false;
            DateTime dabartineData = DateTime.Now;
            if (dabartineData.DayOfWeek != DayOfWeek.Saturday && dabartineData.DayOfWeek != DayOfWeek.Sunday)
            {
                if (dabartineData.Hour >= BirzosDarboPradziaH && dabartineData.Hour <= BirzosDarboPabaigaH)
                {
                    bool arYraTusciu = true;
                    foreach (Akcijos akcijos in gautosAkcijos)
                    {
                        if (akcijos.finansineInformacija.PirkimoKaina != 0)
                        {
                            arYraTusciu = false;
                            break;
                        }
                    }
                    if (!arYraTusciu)
                    {
                        return true;
                    }

                }
            }
            return arDirbaBirza;
        }
    }
}
