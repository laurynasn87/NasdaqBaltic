using DALs;
using NUnit.Framework;
using System.Collections.Generic;
using Models;
using System;
using System.Linq;

namespace DALs_Tests
{
    public class FinansinesInformacijosDALTests
    {
        FinansinesInformacijosDAL FIDal = new FinansinesInformacijosDAL();
        [Test]
        public void GautiVisus__rezultatas_GautiVisiFinansinesInformacijosIrasai()
        {
            List<FinansineInformacija> VisosFiN = FIDal.GautiVisus();

            Assert.IsTrue(VisosFiN != null && VisosFiN.Count > 0);
        }
        [Test]
        public void GautiPagalId_PirmasVisuFinansiniuInformacijuElementas_rezultatas_FinansineInformacijaSurasta()
        {
            List<FinansineInformacija> VisosFiN = FIDal.GautiVisus();
            FinansineInformacija akcijosFiN = VisosFiN[0];

            FinansineInformacija GautaAkcijaPagalId = FIDal.GautiPagalId(akcijosFiN.Id.ToString());

            Assert.AreEqual(akcijosFiN, GautaAkcijaPagalId);
        }
        [Test]
        public void GautiPagalKodaNaujausia_NaujausiasVisuFinansiniuInformacijuElementas_rezultatas_FinansineInformacijaSurasta()
        {
            List<FinansineInformacija> VisosFiN = FIDal.GautiVisus();
            VisosFiN = VisosFiN.OrderByDescending(o => o.Id).ToList();
            FinansineInformacija akcijosFiN = VisosFiN[0];

            FinansineInformacija GautaAkcijaPagalId = FIDal.GautiPagalKodaNaujausia(akcijosFiN.AkcijosKodas);

            Assert.AreEqual(akcijosFiN, GautaAkcijaPagalId);
        }

        [Test]
        public void Ivesti_NaujausiaFinansineInformacija_rezultatas_FinansineInformacijaVisuFiNSarase()
        {
            List<FinansineInformacija> VisosFiN = FIDal.GautiVisus();
            VisosFiN = VisosFiN.OrderByDescending(o => o.Id).ToList();
            FinansineInformacija akcijosFiN = VisosFiN[0];
            akcijosFiN.PardavimoKaina = 0;
            akcijosFiN.PirkimoKaina = 0;
            akcijosFiN.Paskutine_Kaina = 0;

            FinansinesInformacijosDAL TestFiNDal = new FinansinesInformacijosDAL("Test");
            TestFiNDal.Ivesti(akcijosFiN);

            List<FinansineInformacija> VisosTestFiN = TestFiNDal.GautiVisus();

            Assert.IsTrue(VisosTestFiN.Find(x => x.AkcijosKodas.Equals(akcijosFiN.AkcijosKodas) && x.Kiekis.Equals(akcijosFiN.Kiekis) && x.PardavimoKaina.Equals(akcijosFiN.PardavimoKaina) && x.PirkimoKaina.Equals(akcijosFiN.PirkimoKaina) && x.Paskutine_Kaina.Equals(akcijosFiN.Paskutine_Kaina)) != null);
        }


        [Test]
        public void Atnaujinti_NaujausiaFinansineInformacija_rezultatas_AtnaujintiFinansineInformacijaVisuFinansiniuInformacijuSarase()
        {
            FinansinesInformacijosDAL TestFiNDal = new FinansinesInformacijosDAL("Test");
            List<FinansineInformacija> VisosFiN = TestFiNDal.GautiVisus();
            VisosFiN = VisosFiN.OrderByDescending(o => o.Id).ToList();
            FinansineInformacija akcijosFiN = VisosFiN[0];
            akcijosFiN.PardavimoKaina = 666;
            akcijosFiN.PirkimoKaina = 666;
            akcijosFiN.Paskutine_Kaina = 666;


            TestFiNDal.Ivesti(akcijosFiN);
            akcijosFiN.PardavimoKaina = 333;
            akcijosFiN.PirkimoKaina = 333;
            akcijosFiN.Paskutine_Kaina = 333;
            akcijosFiN.Id = akcijosFiN.Id + 1;
            TestFiNDal.Atnaujinti(akcijosFiN);


            List<FinansineInformacija> VisosTestFiN = TestFiNDal.GautiVisus();
            Assert.IsTrue(VisosTestFiN.Any(x => x.Id == akcijosFiN.Id && akcijosFiN.PardavimoKaina == x.PardavimoKaina && akcijosFiN.PirkimoKaina == x.PirkimoKaina && akcijosFiN.Paskutine_Kaina == x.Paskutine_Kaina));
        }
        [Test]
        public void Istrinti_FinansineInformacija_rezultatas_FinansinesInformacijosNeberaVisuFISarase()
        {
            FinansinesInformacijosDAL TestFiNDal = new FinansinesInformacijosDAL("Test");
            List<FinansineInformacija> VisosFiN = FIDal.GautiVisus();
            FinansineInformacija akcijosFiN = VisosFiN[0];
            akcijosFiN.PardavimoKaina = 777;
            akcijosFiN.PirkimoKaina = 777;
            akcijosFiN.Paskutine_Kaina = 777;

            TestFiNDal.Ivesti(akcijosFiN);
            List<FinansineInformacija> VisosTestFiN = TestFiNDal.GautiVisus();
            FinansineInformacija NaujafinansineInformacija = VisosTestFiN.Find(x => x.PardavimoKaina == akcijosFiN.PardavimoKaina && x.PirkimoKaina == akcijosFiN.PirkimoKaina && x.Paskutine_Kaina == akcijosFiN.Paskutine_Kaina);
            if (NaujafinansineInformacija != null)
                TestFiNDal.Istrinti(NaujafinansineInformacija);

            VisosTestFiN = TestFiNDal.GautiVisus();

            Assert.IsTrue(!VisosTestFiN.Any(x => x.Id == NaujafinansineInformacija.Id && NaujafinansineInformacija.PardavimoKaina == x.PardavimoKaina && NaujafinansineInformacija.PirkimoKaina == x.PirkimoKaina && NaujafinansineInformacija.Paskutine_Kaina == x.Paskutine_Kaina) && NaujafinansineInformacija != null);

        }
        [Test]
        public void GautiStatistikas_NaujausiaFinansineInformacija_rezultatas_GrazinamosTrysStatistikos()
        {
            FinansinesInformacijosDAL TestFiNDal = new FinansinesInformacijosDAL();

            Dictionary<string, Dictionary<DateTime, double>>  Statistika = FIDal.GautiStatistikas(TestFiNDal.GautiVisus()[0].AkcijosKodas);


            Assert.IsTrue(Statistika != null && Statistika.Count > 0);

        }
    }
}