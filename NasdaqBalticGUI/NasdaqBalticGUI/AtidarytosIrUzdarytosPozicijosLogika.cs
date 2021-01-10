using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqBalticGUI
{
  public class AtidarytosIrUzdarytosPozicijosLogika
    {
        ApiKontroleris api = new ApiKontroleris();
        private string AkcijosUrl;
        public AtidarytosIrUzdarytosPozicijosLogika()
        {
            AkcijosUrl = api.BaseUrl + "VartotojoAkcijos";
        }
        public List<Tuple<string, string, int>> AtidarytuAkcijuUzsakymuLentelesStulpeliaiMapping = new List<Tuple<string, string, int>>()
            {
             new Tuple<string, string, int>("Pavadinimas", "Akcija.Pavadinimas",-1),
             new Tuple<string, string, int>("Tipas", "Pirkimas",-2),
             new Tuple<string, string, int>("P&N", "P&N",-2),
             new Tuple<string, string, int>("Pokytis %", "PokytisCustom",-2),
             new Tuple<string, string, int>("Verte", "Verte",-2),
             new Tuple<string, string, int>("Pirkimo Kaina", "PirkimoKaina",-2),
             new Tuple<string, string, int>("Dabartine Kaina", "DabartineKaina",-2),
             new Tuple<string, string, int>("Kiekis", "Kiekis",-2),
             new Tuple<string, string, int>("Atidarymo Data", "AtidarymoData",-2),
            };
        public List<Tuple<string, string, int>> UzdarytuAkcijuUzsakymuLentelesStulpeliaiMapping = new List<Tuple<string, string, int>>()
            {
             new Tuple<string, string, int>("Pavadinimas", "Akcija.Pavadinimas",-1),
             new Tuple<string, string, int>("Tipas", "Pirkimas",-2),
             new Tuple<string, string, int>("P&N", "P&N",-2),
             new Tuple<string, string, int>("Uždarymo Verte", "Verte",-2),
             new Tuple<string, string, int>("Pirkimo Kaina", "PirkimoKaina",-2),
             new Tuple<string, string, int>("Uzdarymo Kaina", "UzdarymoKaina",-2),
             new Tuple<string, string, int>("Aktyvumo Data", "AktyvumoData",-1),
             new Tuple<string, string, int>("Priezastis", "Priezastis",-2)
            };

        public List<VartotojoAkcija> GautiVartotojoAkcijasPagalId(int id)
        {
            List<VartotojoAkcija> VisosVaartotojoAkcijos;

            VisosVaartotojoAkcijos = api.GetApiCallResponseObject<List<VartotojoAkcija>>(AkcijosUrl + "?VartotojoId=" + id);

            return VisosVaartotojoAkcijos;
        }
        public double GautiPelnaNuostoli(List<VartotojoAkcija> vartotojoAtidarytosPozicijos, out double PortfolioVerte)
        {
            double PelnasIrNuostolis = 0;
            PortfolioVerte = 0;
            if (vartotojoAtidarytosPozicijos.Count > 0)
            {

                foreach (VartotojoAkcija akcija in vartotojoAtidarytosPozicijos)
                {
                    if (akcija.Pirkimas)
                    {
                        PelnasIrNuostolis = PelnasIrNuostolis + (akcija.akcija.finansineInformacija.PirkimoKaina * akcija.Kiekis - akcija.PirkimoKaina * akcija.Kiekis);
                        PortfolioVerte = PortfolioVerte + (akcija.akcija.finansineInformacija.PirkimoKaina * akcija.Kiekis);
                    }
                    else
                    {
                        PelnasIrNuostolis = PelnasIrNuostolis + (akcija.PirkimoKaina * akcija.Kiekis - akcija.akcija.finansineInformacija.PardavimoKaina * akcija.Kiekis);
                        PortfolioVerte = PortfolioVerte + (akcija.akcija.finansineInformacija.PardavimoKaina * akcija.Kiekis);
                    }
                }

            }
            return PelnasIrNuostolis;
        }
        public void GautiAtidarytasIrUzdarytasPozicijas(int VarotojoId, List<Akcijos> DabartinesAkcijos, out List<VartotojoAkcija> AtidarytosPozicijos, out List<Dictionary<string, string>> CustomAtidarytuPozicijuLaukai, out List<VartotojoAkcija> UzdarytosPozicijos, out List<Dictionary<string, string>> CustomUzdarytuPozicijuLaukai)
        {
            AtidarytosPozicijos = new List<VartotojoAkcija>();
            CustomAtidarytuPozicijuLaukai = new List<Dictionary<string, string>>();

            UzdarytosPozicijos = new List<VartotojoAkcija>();
            CustomUzdarytuPozicijuLaukai = new List<Dictionary<string, string>>();

            if (VarotojoId != 0)
            {
                List<VartotojoAkcija> Pozicijos = GautiVartotojoAkcijasPagalId(VarotojoId);
                if (Pozicijos != null && Pozicijos.Count > 0)
                {
                    foreach (VartotojoAkcija vartotojo in Pozicijos)
                    {
                        if (vartotojo.akcija != null && !String.IsNullOrEmpty(vartotojo.akcija.AkcijosKodas) && DabartinesAkcijos != null && DabartinesAkcijos.Count > 0)
                        {
                            vartotojo.akcija = DabartinesAkcijos.Find(x => x.AkcijosKodas.Equals(vartotojo.akcija.AkcijosKodas));
                        }

                        if (vartotojo.AtidarymoData == vartotojo.UzdarymoData || vartotojo.AtidarymoData > vartotojo.UzdarymoData)
                        {
                            AtidarytosPozicijos.Add(vartotojo);
                            if (DabartinesAkcijos != null && DabartinesAkcijos.Count > 0)
                            CustomAtidarytuPozicijuLaukai.Add(SkaiciuotiCustomLaukeliusAtidayrtomPozicijom(vartotojo));
                        }
                        else
                        {
                            UzdarytosPozicijos.Add(vartotojo);
                            if (DabartinesAkcijos != null && DabartinesAkcijos.Count > 0)
                                CustomUzdarytuPozicijuLaukai.Add(SkaiciuotiCustomLaukeliusUzdarytoomPozicijom(vartotojo));
                        }

                    }

                }
            }
        }

        Dictionary<String, String> SkaiciuotiCustomLaukeliusAtidayrtomPozicijom(VartotojoAkcija akcija)
        {
            Dictionary<string, string> CustomLaukai = new Dictionary<string, string>();
            double SumaPerkant = akcija.PirkimoKaina * akcija.Kiekis;
            double SumaDabar = 0;
            double pokytis = 0;
            if (akcija.Pirkimas)
            {
                CustomLaukai.Add("DabartineKaina", akcija.akcija.finansineInformacija.PirkimoKaina.ToString());
                CustomLaukai.Add("Verte", (akcija.akcija.finansineInformacija.PirkimoKaina * akcija.Kiekis).ToString());
                CustomLaukai.Add("P&N", (akcija.akcija.finansineInformacija.PirkimoKaina * akcija.Kiekis - akcija.PirkimoKaina * akcija.Kiekis).ToString("0.##") + " €");
                SumaDabar = akcija.akcija.finansineInformacija.PirkimoKaina * akcija.Kiekis;
                pokytis = ((SumaDabar - SumaPerkant) / Math.Abs(SumaPerkant)) * 100;
            }
            else
            {
                CustomLaukai.Add("DabartineKaina", akcija.akcija.finansineInformacija.PardavimoKaina.ToString());
                CustomLaukai.Add("P&N", (akcija.PirkimoKaina * akcija.Kiekis - akcija.akcija.finansineInformacija.PardavimoKaina * akcija.Kiekis).ToString("0.##") + " €");
                CustomLaukai.Add("Verte", (akcija.akcija.finansineInformacija.PardavimoKaina * akcija.Kiekis).ToString());
                SumaDabar = akcija.akcija.finansineInformacija.PardavimoKaina * akcija.Kiekis;
                pokytis = ((SumaPerkant - SumaDabar) / Math.Abs(SumaDabar)) * 100;
            }
            if (pokytis != 0)
                CustomLaukai.Add("PokytisCustom", (pokytis).ToString("#.##"));
            else
                CustomLaukai.Add("PokytisCustom", "0");


            return CustomLaukai;
        }
        Dictionary<String, String> SkaiciuotiCustomLaukeliusUzdarytoomPozicijom(VartotojoAkcija akcija)
        {
            Dictionary<string, string> CustomLaukai = new Dictionary<string, string>();

            CustomLaukai.Add("AktyvumoData", akcija.AtidarymoData + " - " + akcija.UzdarymoData);
            CustomLaukai.Add("Verte", (akcija.UzdarymoKaina * akcija.Kiekis).ToString());
            if (akcija.Pirkimas)
            {
                CustomLaukai.Add("P&N", (akcija.UzdarymoKaina * akcija.Kiekis - akcija.PirkimoKaina * akcija.Kiekis).ToString("0.##") + " €");
            }
            else
            {
                CustomLaukai.Add("P&N", (akcija.PirkimoKaina * akcija.Kiekis - akcija.UzdarymoKaina * akcija.Kiekis).ToString("0.##") + " €");
            }

            return CustomLaukai;
        }

        public bool UzdarytiAkcija(VartotojoAkcija vartotojoAkcija, Vartotojas DabartinisVartotojas, List<Akcijos> dabartinesPirkimoAkcijos)
        {
            string url = AkcijosUrl + "/Edit/" + vartotojoAkcija.Id;
            bool ArSekminga = false;
            if (vartotojoAkcija != null)
            {
                DateTime UzdarymoData = DateTime.Now;
                Akcijos akcija = dabartinesPirkimoAkcijos.Find(x => x.AkcijosKodas.Equals(vartotojoAkcija.akcija.AkcijosKodas));
                if (akcija != null)
                {
                    vartotojoAkcija.akcija = akcija;
                    vartotojoAkcija.vartotojas = DabartinisVartotojas;

                    vartotojoAkcija.Priezastis = "Uždaryta Vartojo Noru";
                    vartotojoAkcija.UzdarymoData = UzdarymoData;
                    if (vartotojoAkcija.Pirkimas)
                        vartotojoAkcija.UzdarymoKaina = akcija.finansineInformacija.PirkimoKaina;
                    else
                        vartotojoAkcija.UzdarymoKaina = akcija.finansineInformacija.PardavimoKaina;

                    ArSekminga = Task.Run(async () => await api.PostApiCallAsync(null, url, vartotojoAkcija)).Result;
                }
            }
            return ArSekminga;
        }
        public bool KurtiUzsakyma(bool Pirkimas, Akcijos pasirinktaAkcija, Vartotojas dabartinisVarotojas, int Kiekis)
        {
            bool ArSekmingas = false;
            String ApiUrl = api.BaseUrl + "VartotojoAkcijos";
            VartotojoAkcija NaujaVartotojoAkcija = new VartotojoAkcija();
            if (pasirinktaAkcija != null && pasirinktaAkcija.finansineInformacija != null && dabartinisVarotojas.Id > 0 && Kiekis > 0)
            {
                NaujaVartotojoAkcija.akcija = pasirinktaAkcija;
                NaujaVartotojoAkcija.vartotojas = dabartinisVarotojas;
                NaujaVartotojoAkcija.Pirkimas = Pirkimas;
                NaujaVartotojoAkcija.Kiekis = Kiekis;
                NaujaVartotojoAkcija.AtidarymoData = DateTime.Now;
                double Kaina = 0;
                if (Pirkimas)
                    Kaina = pasirinktaAkcija.finansineInformacija.PirkimoKaina;
                else
                {
                    Kaina = pasirinktaAkcija.finansineInformacija.PardavimoKaina;
                }

                NaujaVartotojoAkcija.PirkimoKaina = Kaina;
                try
                {
                    ArSekmingas = Task.Run(async () => await api.PostApiCallAsync(null, ApiUrl + "/Create", NaujaVartotojoAkcija)).Result;
                }
                catch (Exception e)
                {
                }


            }


            return ArSekmingas;
        }
    }

}
