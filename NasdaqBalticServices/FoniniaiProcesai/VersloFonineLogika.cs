using Models;
using System;
using System.Collections.Generic;
using System.Text;
using DALs;

namespace FoniniaiProcesai
{
    public class VersloFonineLogika
    {
        const int MarzaUzdarant = -70;

        public VersloFonineLogika()
        {
        }
       public void TikrintiMinimaliaMarža()
        {
            List<VartotojoAkcija> AkcijuUzklausos = new List<VartotojoAkcija>();
            VartotojoAkcijaDAL vartotojoAkcijaDAL = new VartotojoAkcijaDAL();
            VartotojasDAL vartotojasDAL = new VartotojasDAL();
            AkcijuDAL akcijuDal = new AkcijuDAL();
            AkcijuUzklausos = vartotojoAkcijaDAL.GautiVisus();

            foreach(VartotojoAkcija uzklausa in AkcijuUzklausos)
            {
                if (uzklausa.UzdarymoKaina != 0 || !String.IsNullOrEmpty(uzklausa.Priezastis))
                    continue;

                uzklausa.akcija = akcijuDal.GautiPagalKoda(uzklausa.akcija.AkcijosKodas);
                double SumaPerkant = uzklausa.PirkimoKaina * uzklausa.Kiekis;
                double SumaDabar = 0;
                double pokytis = 0;
                if (uzklausa.Pirkimas)
                {
                    SumaDabar = uzklausa.akcija.finansineInformacija.PirkimoKaina * uzklausa.Kiekis;
                    pokytis = ((SumaDabar - SumaPerkant) / Math.Abs(SumaPerkant)) * 100;
                }
                else
                {
                    SumaDabar = uzklausa.akcija.finansineInformacija.PardavimoKaina * uzklausa.Kiekis;
                    pokytis = ((SumaPerkant - SumaDabar) / Math.Abs(SumaDabar)) * 100;
                }


                if (pokytis < MarzaUzdarant)
                {
                    uzklausa.Priezastis = "Pasiekta Min. Marža";
                    uzklausa.UzdarymoData = DateTime.Now;
                    if (uzklausa.Pirkimas) uzklausa.UzdarymoKaina = uzklausa.akcija.finansineInformacija.PirkimoKaina;
                    else uzklausa.UzdarymoKaina = uzklausa.akcija.finansineInformacija.PardavimoKaina;
                    bool Atnaujinta = vartotojoAkcijaDAL.Atnaujinti(uzklausa);
                    if (Atnaujinta)
                    {
                        Vartotojas Dabartinis = vartotojasDAL.GautiPagalId(uzklausa.vartotojas.Id.ToString());
                        Dabartinis.Balansas = Dabartinis.Balansas + SumaDabar;
                        vartotojasDAL.Atnaujinti(Dabartinis);

                    }
                    
                }
            }
        }

    }
}
