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

                Thread.Sleep(60000);
            }
        }

        bool ArBirzaDirba()
        {
            int DarboBirzosPradziaH = 10;
            int DarboBirzosPabaigaH = 16;
            bool arDirba = false;
            DateTime Siandien = DateTime.Now;
            if (Siandien.DayOfWeek != DayOfWeek.Saturday && Siandien.DayOfWeek != DayOfWeek.Sunday)
                if (Siandien.Hour >= DarboBirzosPradziaH &&  Siandien.Hour <= DarboBirzosPabaigaH)
                {
                    arDirba = true;
                }
            return arDirba;
        }
    }
}
