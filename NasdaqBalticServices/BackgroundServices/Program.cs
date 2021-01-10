using System;
using System.Threading;

namespace BackgroundServices
{
     public class Program
     {
        public void Startup()
        {

             Thread thread = new Thread(FoniniuFunkcijuPaleidimas);
             thread.Start();

        }

        void FoniniuFunkcijuPaleidimas()
        {
            AkcijuFoninesFunkcijos akcijuFoninesFunkcijos = new AkcijuFoninesFunkcijos();
            int PaskutineDienaKaiBuvoAtnaujinta = DateTime.Now.Day;
            while (true)
            {
                DateTime Dabar = DateTime.Now;
                if (ArBirzaDirba())
                {
                    akcijuFoninesFunkcijos.GautiAkcijuSarasa();
                    if (Dabar.Day != PaskutineDienaKaiBuvoAtnaujinta)
                    {
                        akcijuFoninesFunkcijos.IrasytiNaujasAkcijas();
                        akcijuFoninesFunkcijos.PapildomosInformacijosAtnaujinimas();
                        PaskutineDienaKaiBuvoAtnaujinta = DateTime.Now.Day;
                    }
                    akcijuFoninesFunkcijos.IrasytiNaujaFinansineInformacija();
                }
                else
                {
                    Thread.Sleep(3600000);
                }

                Thread.Sleep(60000);
            }
        }

        bool ArBirzaDirba()
        {
            int BirzosDarboPradziaH = 10;
            int BirzosDarboPabaigaH = 16;
            bool arDirba = false;
            DateTime Siandien = DateTime.Now;
            if (Siandien.DayOfWeek != DayOfWeek.Saturday && Siandien.DayOfWeek != DayOfWeek.Sunday)
                if (Siandien.Hour >= BirzosDarboPradziaH &&  Siandien.Hour <= BirzosDarboPabaigaH)
                {
                    arDirba = true;
                }
            return arDirba;
        }
    }
}
