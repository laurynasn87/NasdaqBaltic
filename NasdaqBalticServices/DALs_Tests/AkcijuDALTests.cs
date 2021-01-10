using DALs;
using NUnit.Framework;
using System.Collections.Generic;
using Models;
using System;

namespace DALs_Tests
{
    public class AkcijuDALTests
    {
        AkcijuDAL akcijuDAL = new AkcijuDAL();
        [Test]
        public void GautiVisus_0_rezultatas_AkcijuListDaugiauUz0()
        {
           List <Akcijos> VisosAkcijos = akcijuDAL.GautiVisus();
            Assert.IsTrue(VisosAkcijos != null && VisosAkcijos.Count > 0);
        }
        [Test]
        public void GautiPagalAkcijosKoda_1VisuAkcijuElementas_rezultatas_AkcijaSurasta()
        {
            List<Akcijos> VisosAkcijos = akcijuDAL.GautiVisus();
            Akcijos akcija = VisosAkcijos[0];

            Akcijos GautaAkcijaPagalKoda = akcijuDAL.GautiPagalKoda(akcija.AkcijosKodas);
            Assert.IsTrue(GautaAkcijaPagalKoda.Equals(akcija));
        }
        [Test]
        public void Ivesti_Akcija_rezultatas_AkcijaVisuAkcijuSarase()
        {
            List<Akcijos> VisosAkcijos = akcijuDAL.GautiVisus();
            Akcijos akcija = VisosAkcijos[0];
            akcija.Pavadinimas = "TEST" + akcija.Pavadinimas;
            akcija.AkcijosKodas = "Test" + DateTime.Now.ToString("MM/dd:HH/m");

            AkcijuDAL TestAkcijuDal = new AkcijuDAL("Test");
            TestAkcijuDal.Ivesti(akcija);

            List<Akcijos> VisosTestAkcijos = TestAkcijuDal.GautiVisus();

            Assert.IsTrue(VisosTestAkcijos.Find(x=> x.AkcijosKodas.Equals(akcija.AkcijosKodas) && x.Pavadinimas.Equals(akcija.Pavadinimas)) != null);
        }
        [Test]
        public void Atnaujinti_Akcija_rezultatas_AtnaujintiAkcijaVisuAkcijuSarase()
        {
            List<Akcijos> VisosAkcijos = akcijuDAL.GautiVisus();
            Akcijos akcija = VisosAkcijos[0];
            akcija.Pavadinimas = "TEST" + akcija.Pavadinimas;
            akcija.AkcijosKodas = DateTime.Now.ToString("MM/dd-HH/m/ss");

            AkcijuDAL TestAkcijuDal = new AkcijuDAL("Test");
            TestAkcijuDal.Ivesti(akcija);
            akcija.Pavadinimas = "TEST" + akcija.Pavadinimas + "2";
            TestAkcijuDal.Atnaujinti(akcija);


            List<Akcijos> VisosTestAkcijos = TestAkcijuDal.GautiVisus();

            Assert.IsTrue(VisosTestAkcijos.Find(x => x.AkcijosKodas.Equals(akcija.AkcijosKodas) && x.Pavadinimas.Equals(akcija.Pavadinimas)) != null);
        }
        [Test]
        public void Istrinti_Akcija_rezultatas_AkcijuSaraseNeberaTosAkcijos()
        {
            List<Akcijos> VisosAkcijos = akcijuDAL.GautiVisus();
            AkcijuDAL TestAkcijuDal = new AkcijuDAL("Test");
            Akcijos akcija = VisosAkcijos[0];


            akcija.Pavadinimas = "TEST" + akcija.Pavadinimas;
            akcija.AkcijosKodas = DateTime.Now.ToString("MM/dd-HH/m/ss");
            TestAkcijuDal.Ivesti(akcija);
            TestAkcijuDal.Istrinti(akcija);

            List<Akcijos> VisosTestAkcijos = TestAkcijuDal.GautiVisus();

            Assert.IsTrue(VisosTestAkcijos.Find(x => x.AkcijosKodas.Equals(akcija.AkcijosKodas) && x.Pavadinimas.Equals(akcija.Pavadinimas)) == null);
        }
    }
}