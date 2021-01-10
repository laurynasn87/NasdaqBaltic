using Models;
using NasdaqBalticGUI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NasdaqBalticGUI_Tests
{
    public class AtidarytosIrUzdarytosPozicijosLogikaTests
    {
        AtidarytosIrUzdarytosPozicijosLogika atidarytosIrUzdarytosPozicijosLogika = new AtidarytosIrUzdarytosPozicijosLogika();
        int TestUserId = 15;
        [Test]
        public void GautiAtidarytasIrUzdarytasPozicijas_Vartotojas15_rezultatas_GautosPozicijos()
        {
            List<VartotojoAkcija> Atidarytos;
            List<Dictionary<string,string>> AtidarytuPozicijuCustomLaukai;
            List<VartotojoAkcija> Uzdarytos;
            List<Dictionary<string, string>> UzdarytuPozicijuCustomLaukai;
            
            
            //15 vartotojas yra test
            atidarytosIrUzdarytosPozicijosLogika.GautiAtidarytasIrUzdarytasPozicijas(TestUserId, null, out Atidarytos, out AtidarytuPozicijuCustomLaukai, out Uzdarytos, out UzdarytuPozicijuCustomLaukai);
            
            
            Assert.IsTrue(Atidarytos != null && Atidarytos.Count > 0 && Uzdarytos != null && Uzdarytos.Count > 0);
        }
        [Test]
        public void GautiPelnaNuostoli_Vartotojas15Akcijos_rezultatas_GautasPelnasIrPortfolioVerte()
        {
            PriekiautiLogika priekiautiLogika = new PriekiautiLogika();
            List<VartotojoAkcija> Atidarytos;
            List<Dictionary<string, string>> AtidarytuPozicijuCustomLaukai;
            List<VartotojoAkcija> Uzdarytos;
            List<Dictionary<string, string>> UzdarytuPozicijuCustomLaukai;
            List<Akcijos> akcijos = priekiautiLogika.GautiVisasAkcijas();

            
            atidarytosIrUzdarytosPozicijosLogika.GautiAtidarytasIrUzdarytasPozicijas(TestUserId, akcijos, out Atidarytos, out AtidarytuPozicijuCustomLaukai, out Uzdarytos, out UzdarytuPozicijuCustomLaukai);
            double verte = 0;

            double pelnas = atidarytosIrUzdarytosPozicijosLogika.GautiPelnaNuostoli(Atidarytos, out verte);

            Assert.IsTrue(verte != 0 && pelnas != 0);
        }
        [Test]
        public void GautiVartotojoAkcijasPagalId_Id_rezultatas_SarasasId()
        {
            List<VartotojoAkcija> vartotojoAkcijos = atidarytosIrUzdarytosPozicijosLogika.GautiVartotojoAkcijasPagalId(TestUserId);//15 vartotojas yra test

            Assert.IsTrue(vartotojoAkcijos != null && vartotojoAkcijos.Count > 0);
        }
        [Test]
        public void KurtiUzsakyma_PirmaAkcijaSaraseIrTestVartotojas_rezultatas_SekmingaiSukurtasUzsakymas()
        {
            PriekiautiLogika priekiautiLogika = new PriekiautiLogika();
            Vartotojas vartotojas = new Vartotojas();
            vartotojas.Id = TestUserId;
            List<Akcijos> akcijos = priekiautiLogika.GautiVisasAkcijas();

            Assert.IsTrue(atidarytosIrUzdarytosPozicijosLogika.KurtiUzsakyma(true,akcijos[0], vartotojas,200));
        }
        [Test]
        public void KurtiUzsakyma_PirmaAkcijaSaraseIrTestVartotojas_rezultatas_SukurtasUzsakymasAtsirandaUzsakymuSarase()
        {
            PriekiautiLogika priekiautiLogika = new PriekiautiLogika();
            Vartotojas vartotojas = new Vartotojas();
            List<Akcijos> akcijos = priekiautiLogika.GautiVisasAkcijas();

            vartotojas.Id = TestUserId;
            List<VartotojoAkcija> vartotojoAkcijos = atidarytosIrUzdarytosPozicijosLogika.GautiVartotojoAkcijasPagalId(TestUserId);
            bool SekmingaiSukurta = atidarytosIrUzdarytosPozicijosLogika.KurtiUzsakyma(false, akcijos[0], vartotojas, 200);
            List<VartotojoAkcija> vartotojoAkcijosPoSukurimo = atidarytosIrUzdarytosPozicijosLogika.GautiVartotojoAkcijasPagalId(TestUserId);

            Assert.IsTrue(SekmingaiSukurta && vartotojoAkcijos != null && vartotojoAkcijosPoSukurimo != null && vartotojoAkcijos.Count < vartotojoAkcijosPoSukurimo.Count);

        }
        [Test]
        public void UzdarytiAkcija_NaujausiaAtidarytaVartotojoAkcija_rezultatas_VartotojoAkcijaUzdaryta()
        {
            PriekiautiLogika priekiautiLogika = new PriekiautiLogika();
            Vartotojas vartotojas = new Vartotojas();
            List<Akcijos> akcijos = priekiautiLogika.GautiVisasAkcijas();

            vartotojas.Id = TestUserId;
            List<VartotojoAkcija> vartotojoAkcijos = atidarytosIrUzdarytosPozicijosLogika.GautiVartotojoAkcijasPagalId(TestUserId).Where(x=> String.IsNullOrEmpty(x.Priezastis)).ToList();
            bool SekmingaiUzdaryta = atidarytosIrUzdarytosPozicijosLogika.UzdarytiAkcija(vartotojoAkcijos[vartotojoAkcijos.Count-1],vartotojas,akcijos);
            List<VartotojoAkcija> vartotojoAkcijosPoUzdarymo = atidarytosIrUzdarytosPozicijosLogika.GautiVartotojoAkcijasPagalId(TestUserId);
            VartotojoAkcija vartotojoAkcijaUzdaryta = vartotojoAkcijosPoUzdarymo.Find(x => x.Id == vartotojoAkcijos[vartotojoAkcijos.Count - 1].Id);


            Assert.IsTrue(SekmingaiUzdaryta && vartotojoAkcijaUzdaryta != null && !String.IsNullOrEmpty(vartotojoAkcijaUzdaryta.Priezastis));

        }
    }
}