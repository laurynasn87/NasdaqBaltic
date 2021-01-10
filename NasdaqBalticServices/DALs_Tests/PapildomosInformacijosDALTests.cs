using DALs;
using NUnit.Framework;
using System.Collections.Generic;
using Models;
using System;
using System.Linq;

namespace DALs_Tests
{
    public class PapildomosInformacijosDALTests
    {
        
        PapildomosInformacijosDAL PIDal = new PapildomosInformacijosDAL();
        [Test]
        public void GautiVisus_0_rezultatas_AkcijuListDaugiauUz0()
        {
           List <PapildomaInformacija> VisosPiN = PIDal.GautiVisus();
            Assert.IsTrue(VisosPiN != null && VisosPiN.Count > 0);
        }
        [Test]
        public void GautiPagalAkcijosKoda_1VisuPIElementas_rezultatas_PapildomaInformacijaSurasta()
        {
            List<PapildomaInformacija> VisosPiN = PIDal.GautiVisus();
            PapildomaInformacija PI = VisosPiN[0];

            PapildomaInformacija GautasPIPagalKoda = PIDal.GautiPagalKoda(PI.AkcijosKodas);
            Assert.AreEqual(PI, GautasPIPagalKoda);
        }
        [Test]
        public void GautiPagalId_1VisuPIElementas_rezultatas_PapildomaInformacijaSurasta()
        {
            List<PapildomaInformacija> VisosPiN = PIDal.GautiVisus();
            PapildomaInformacija PI = VisosPiN[0];

            PapildomaInformacija GautasPIPagalId = PIDal.GautiPagalId(PI.Id.ToString());
            Assert.AreEqual(PI, GautasPIPagalId);
        }
       [Test]
        public void Ivesti_PapildomaInformacija_rezultatas_PapildomaInformacijaVisuAkcijuSarase()
        {
            AkcijuDAL TestakcijuDal = new AkcijuDAL("Test");
            PapildomosInformacijosDAL TestPIDAL = new PapildomosInformacijosDAL("Test");
            List<PapildomaInformacija> VisosPI = PIDal.GautiVisus();
            PapildomaInformacija PI = VisosPI[0];
            PI.Didziausia_Kaina = 0;
            PI.Maziausia_Kaina = 0;
            PI.Vidutine_Kaina = 0;
            string NaujosAkcijosKodas = "Kodas"+ DateTime.Now.ToString("MM/dd-HH/m/ss");
            string NaujosAkcijosPav = "Pavadinimas" + DateTime.Now.ToString("MM/dd-HH/m/ss");


            Akcijos NaujaTestAckija = new Akcijos(NaujosAkcijosKodas, NaujosAkcijosPav);
            TestakcijuDal.Ivesti(NaujaTestAckija);
            PI.AkcijosKodas = NaujosAkcijosKodas;
            TestPIDAL.Ivesti(PI);

            List<PapildomaInformacija> VisosTestAkcijos = TestPIDAL.GautiVisus();

            Assert.IsTrue(VisosTestAkcijos.Find(x=> x.AkcijosKodas.Equals(PI.AkcijosKodas) && x.Didziausia_Kaina == PI.Didziausia_Kaina && x.Maziausia_Kaina == PI.Maziausia_Kaina && x.Vidutine_Kaina == PI.Vidutine_Kaina) != null);
        } 
          [Test]
          public void Atnaujinti_PapildomaInformacija_rezultatas_AtnaujintaInformacijaVisuPISarase()
          {
            PapildomosInformacijosDAL TestPIDAL = new PapildomosInformacijosDAL("Test");
            List<PapildomaInformacija> VisosPI = TestPIDAL.GautiVisus();
            PapildomaInformacija PI = VisosPI[0];
            PI.Didziausia_Kaina = DateTime.Now.Month;
            PI.Maziausia_Kaina = DateTime.Now.Day;
            PI.Vidutine_Kaina = DateTime.Now.Hour * 100 + DateTime.Now.Minute;

            TestPIDAL.Atnaujinti(PI);
            PapildomaInformacija NaujaPI = TestPIDAL.GautiPagalKoda(PI.AkcijosKodas);
            if (NaujaPI != null)
            {
                TestPIDAL.Istrinti(NaujaPI);
            }

            Assert.IsTrue(NaujaPI != null && NaujaPI.AkcijosKodas.Equals(PI.AkcijosKodas) && NaujaPI.Didziausia_Kaina == PI.Didziausia_Kaina && NaujaPI.Maziausia_Kaina == PI.Maziausia_Kaina && NaujaPI.Vidutine_Kaina == PI.Vidutine_Kaina);

        }
        [Test]
          public void Istrinti_PapildomaInformacija_rezultatas_PapildomuInfromacijuSaraseNeberaTosPI()
          {
            AkcijuDAL TestakcijuDal = new AkcijuDAL("Test");
            PapildomosInformacijosDAL TestPIDAL = new PapildomosInformacijosDAL("Test");
            List<PapildomaInformacija> VisosPI = PIDal.GautiVisus();
            PapildomaInformacija PI = VisosPI[0];
            PI.Didziausia_Kaina = 0;
            PI.Maziausia_Kaina = 0;
            PI.Vidutine_Kaina = 0;
            string NaujosAkcijosKodas = "Kodas" + DateTime.Now.ToString("MM/dd-HH/m/ss");
            string NaujosAkcijosPav = "Pavadinimas" + DateTime.Now.ToString("MM/dd-HH/m/ss");


            Akcijos NaujaTestAckija = new Akcijos(NaujosAkcijosKodas, NaujosAkcijosPav);
            TestakcijuDal.Ivesti(NaujaTestAckija);
            PI.AkcijosKodas = NaujosAkcijosKodas;
            TestPIDAL.Ivesti(PI);
            PapildomaInformacija NaujaPI = TestPIDAL.GautiPagalKoda(NaujosAkcijosKodas);
            if (NaujaPI != null)
            {
                TestPIDAL.Istrinti(NaujaPI);
            }
            List<PapildomaInformacija> VisosTestAkcijos = TestPIDAL.GautiVisus();

            Assert.IsTrue(NaujaPI != null && VisosTestAkcijos.Find(x => x.AkcijosKodas.Equals(PI.AkcijosKodas) && x.Didziausia_Kaina == PI.Didziausia_Kaina && x.Maziausia_Kaina == PI.Maziausia_Kaina && x.Vidutine_Kaina == PI.Vidutine_Kaina) == null);

          }
    }
}