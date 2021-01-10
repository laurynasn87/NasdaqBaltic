using DALs;
using NUnit.Framework;
using System.Collections.Generic;
using Models;
using System;
using System.Linq;

namespace DALs_Tests
{
    public class VartotojasDALTests
    {
        VartotojasDAL VartotojuDal = new VartotojasDAL("Test");
         [Test]
         public void GautiVisus_0_rezultatas_AkcijuListDaugiauUz0()
         {
            List <Vartotojas> Vartotojai = VartotojuDal.GautiVisus();
             Assert.IsTrue(Vartotojai != null && Vartotojai.Count > 0);
        }
         [Test]
         public void GautiPagalId_PirmasVisuVartotojuElementas_rezultatas_VartotojasSurastas()
         {
            List<Vartotojas> Vartotojai = VartotojuDal.GautiVisus();
            Vartotojas vartotojas = Vartotojai[0];

            Vartotojas GautasVartotojasPagalId = VartotojuDal.GautiPagalId(vartotojas.Id.ToString());

             Assert.AreEqual(vartotojas, GautasVartotojasPagalId);
         }

          [Test]
          public void Atnaujinti_Vartotojas_rezultatas_VartotojasAtnaujintasDuomBazeje()
          {
            List<Vartotojas> Vartotojai = VartotojuDal.GautiVisus();
            Vartotojas vartotojas = Vartotojai[0];

            vartotojas.Vardas = vartotojas.Vardas + DateTime.Now.ToString("dd/HH:mm:ss");

            VartotojuDal.Atnaujinti(vartotojas);
            Vartotojas vartotojasIsDb = VartotojuDal.GautiPagalId(vartotojas.Id.ToString());

            Assert.IsTrue(vartotojas.Vardas.Equals(vartotojasIsDb.Vardas));
          }
        [Test]
        public void AtnaujintiBalansa_Vartotojas_rezultatas_VartotojasAtnaujintasDuomBazeje()
        {
            List<Vartotojas> Vartotojai = VartotojuDal.GautiVisus();
            Vartotojas vartotojas = Vartotojai[0];

            vartotojas.Balansas = DateTime.Now.Day * 1000 + DateTime.Now.Hour * 100 + DateTime.Now.Month;

            VartotojuDal.AtnaujintiBalansa(vartotojas);
            Vartotojas vartotojasIsDb = VartotojuDal.GautiPagalId(vartotojas.Id.ToString());

            Assert.IsTrue(vartotojas.Balansas.Equals(vartotojasIsDb.Balansas));
        }

    }
}