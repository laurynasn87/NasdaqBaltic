using NUnit.Framework;
using DALs;
using Models;
using System;
using FoniniaiProcesai;

namespace FoniniaiProcesai_Tests
{
    public class Tests
    {
        VersloFonineLogika versloFonineLogika = new VersloFonineLogika();

        [Test]
        public void TikrintiMinimaliaMarža_SukurtaVartotojoAkcijaZemiauMinimaliosMarzos_rezultatas_UzdarytaVartotojoAkcijaZemiauMinMarzos()
        {
            AkcijuDAL akcijuDAL = new AkcijuDAL();
            FinansinesInformacijosDAL finansinesInformacijosDAL = new FinansinesInformacijosDAL();
            VartotojasDAL vartotojasDAL = new VartotojasDAL();
            VartotojoAkcijaDAL vartotojoAkcijaDAL = new VartotojoAkcijaDAL();

            Akcijos akcijos = new Akcijos();
            akcijos.Pavadinimas = "Nauja";
            akcijos.AkcijosKodas = "NJ" + DateTime.Now.ToString("dd/hh:mm:ss");

            akcijuDAL.Ivesti(akcijos);

            FinansineInformacija FI = new FinansineInformacija();
            FI.AkcijosKodas = akcijos.AkcijosKodas;
            FI.Kiekis = 500;
            FI.PirkimoKaina = 20;
            FI.PardavimoKaina = 20;
            finansinesInformacijosDAL.Ivesti(FI);

            VartotojoAkcija vartotojoAkcija = new VartotojoAkcija();
            vartotojoAkcija.Pirkimas = true;
            vartotojoAkcija.vartotojas = vartotojasDAL.GautiVisus()[0];
            vartotojoAkcija.PirkimoKaina = 500;
            vartotojoAkcija.akcija = akcijos;
            vartotojoAkcija.Kiekis = 45;

            vartotojoAkcijaDAL.Ivesti(vartotojoAkcija);

            VartotojoAkcija VartotojoAkcijaIsDuombazes = vartotojoAkcijaDAL.GautiVartotojoAkcijas(vartotojoAkcija.vartotojas.Id).Find(x => x.PirkimoKaina == vartotojoAkcija.PirkimoKaina && x.Kiekis == vartotojoAkcija.Kiekis && x.akcija.AkcijosKodas.Equals(vartotojoAkcija.akcija.AkcijosKodas));
            if (VartotojoAkcijaIsDuombazes == null)
                Assert.Fail();
            else
            {
                versloFonineLogika.TikrintiMinimaliaMarža();
                VartotojoAkcija VartotojoAkciaNaujausia = vartotojoAkcijaDAL.GautiVartotojoAkcijas(vartotojoAkcija.vartotojas.Id).Find(x => x.Id == VartotojoAkcijaIsDuombazes.Id);

                if (VartotojoAkciaNaujausia != null && !string.IsNullOrEmpty(VartotojoAkciaNaujausia.Priezastis) && VartotojoAkciaNaujausia.UzdarymoKaina != 0)
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }
    }
}