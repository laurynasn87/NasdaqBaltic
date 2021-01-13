using DALs;
using FoniniaiProcesai;
using Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoniniaiProcesai_Tests
{
    public class InformacijosGavimoFunkcijosTests
    {
        InformacijosGavimoFunkcijos foniniaiProcesai = new InformacijosGavimoFunkcijos();

        [Test]
        public void GautiAkcijuSarasa__rezultatas_GautasNuskaitytasAkcijuSarasas()
        {
            List<Akcijos> NaujosAkcijos = foniniaiProcesai.GautiAkcijuSarasa();

            Assert.IsTrue(NaujosAkcijos != null && NaujosAkcijos.Count > 0);
        }
        [Test]
        public void GautiPapildomaInformacija__rezultatas_GautaNuskaitytaPapildomaInformacija()
        {

            List<PapildomaInformacija> NaujaPapildomaInformacija = foniniaiProcesai.GautiPapildomaInformacija();

            Assert.IsTrue(NaujaPapildomaInformacija != null && NaujaPapildomaInformacija.Count > 0);
        }
        [Test]
        public void IrasytiNaujasAkcijas__rezultatas_AtnaujintosAkcijos()
        {
            List<Akcijos> NaujosAkcijos = foniniaiProcesai.GautiAkcijuSarasa();
            AkcijuDAL akcijuDAL = new AkcijuDAL();
            List<Akcijos> NuklonuotosAkcijos = NaujosAkcijos.ToArray().ToList();

            Akcijos akcijos = new Akcijos(NuklonuotosAkcijos[0].AkcijosKodas);

            akcijos.Pavadinimas = "TestChange" + DateTime.Now.ToString("MM/dd/HH:mm:ss");
            akcijuDAL.Atnaujinti(akcijos);

            foniniaiProcesai.IrasytiNaujasAkcijas(NaujosAkcijos);

            Akcijos NaujaAkcija = akcijuDAL.GautiPagalKoda(akcijos.AkcijosKodas);

            Assert.AreNotEqual(akcijos.Pavadinimas,NaujaAkcija.Pavadinimas);
        }
        [Test]
        public void IrasytiNaujaFinansineInformacija__rezultatas_PridetaNaujaFinansineInformacija()
        {
            List<Akcijos> NaujosAkcijos = foniniaiProcesai.GautiAkcijuSarasa();
            List<FinansineInformacija> DBAkcijosPries = new FinansinesInformacijosDAL().GautiVisus();

            NaujosAkcijos[0].finansineInformacija.Apyvarta = 1;
            NaujosAkcijos[0].finansineInformacija.PardavimoKaina = 1;
            NaujosAkcijos[0].finansineInformacija.PirkimoKaina = 1;
            NaujosAkcijos[0].finansineInformacija.Kiekis = 1;
            NaujosAkcijos[0].finansineInformacija.Sandoriai = 1;
            foniniaiProcesai.IrasytiNaujaFinansineInformacija(NaujosAkcijos);

            List<FinansineInformacija> DBAkcijosPo = new FinansinesInformacijosDAL().GautiVisus();

            Assert.IsTrue(DBAkcijosPo.Count > DBAkcijosPries.Count);
        }
        [Test]
        public void IrasytiNaujaPapildomaInformacija__rezultatas_AtnaujintaPapildomaInformacija()
        {
            List<PapildomaInformacija> NaujaPapildomaInformacija = foniniaiProcesai.GautiPapildomaInformacija();
            PapildomosInformacijosDAL papildomosInformacijosDAL = new PapildomosInformacijosDAL();
            PapildomaInformacija papildomaInformacija = new PapildomaInformacija();

            papildomaInformacija.AkcijosKodas = NaujaPapildomaInformacija[0].AkcijosKodas;
            papildomaInformacija.Maziausia_Kaina = 12;
            papildomaInformacija.Didziausia_Kaina = 181818;
            papildomaInformacija.Vidutine_Kaina = 9998;
            papildomosInformacijosDAL.Atnaujinti(papildomaInformacija);

            foniniaiProcesai.PapildomosInformacijosAtnaujinimas(NaujaPapildomaInformacija);

            PapildomaInformacija AtnaujintaPapildomaInfo = papildomosInformacijosDAL.GautiPagalKoda(papildomaInformacija.AkcijosKodas);

            Assert.IsTrue(AtnaujintaPapildomaInfo != null && AtnaujintaPapildomaInfo.Maziausia_Kaina != papildomaInformacija.Maziausia_Kaina && AtnaujintaPapildomaInfo.Vidutine_Kaina != papildomaInformacija.Vidutine_Kaina && AtnaujintaPapildomaInfo.Didziausia_Kaina != papildomaInformacija.Didziausia_Kaina);
        }
        [Test]
        public void GautiHTML_google_rezultatas_GautasHTML()
        {
           string html = foniniaiProcesai.GautiHTML("https://nasdaqbaltic.com");

            Assert.IsTrue(!string.IsNullOrEmpty(html));
        }
    }
}