using System;
using System.Threading;

namespace FoniniaiProcesai
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(FoniniuFunkcijuPaleidimas);
               thread.Start();
        }

       static void FoniniuFunkcijuPaleidimas()
        {
            InformacijosGavimoFunkcijos akcijuFoninesFunkcijos = new InformacijosGavimoFunkcijos();
            VersloFonineLogika versloFonineLogika = new VersloFonineLogika();
            int PaskutineDienaKaiBuvoAtnaujinta = DateTime.Now.Day;
            while (true)
            {
                DateTime Dabar = DateTime.Now;
                if (ArBirzaDirba())
                {
                    if (Dabar.Day != PaskutineDienaKaiBuvoAtnaujinta)
                    {
                        akcijuFoninesFunkcijos.NuskaitytiInformacija(true, true,true);
                        PaskutineDienaKaiBuvoAtnaujinta = DateTime.Now.Day;
                    }
                    else
                    {
                        akcijuFoninesFunkcijos.NuskaitytiInformacija(false, true);
                        versloFonineLogika.TikrintiMinimaliaMarža();
                    }

                }
                else
                {
                    Thread.Sleep(10800000);
                }

                Thread.Sleep(300000);
            }
        }

      static  bool  ArBirzaDirba()
        {
            int BirzosDarboPradziaH = 10;
            int BirzosDarboPabaigaH = 16;
            bool arDirba = false;
            DateTime Siandien = DateTime.Now;
            if (Siandien.DayOfWeek != DayOfWeek.Saturday && Siandien.DayOfWeek != DayOfWeek.Sunday)
                if (Siandien.Hour >= BirzosDarboPradziaH && Siandien.Hour <= BirzosDarboPabaigaH)
                {
                    arDirba = true;
                }
            return arDirba;
        }
    }
}
