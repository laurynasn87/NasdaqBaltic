using Models;
using NasdaqBalticGUI;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NasdaqBalticGUI_Tests
{
    public class PriekiautiLogikaTests
    {
        PriekiautiLogika PrekiautiLogika = new PriekiautiLogika();
        int TestUserId = 15;
        [Test]
        public void GautiVisasAkcijas__rezultatas_VisosAkcijos()
        {
            List<Akcijos> visosAkcijos = PrekiautiLogika.GautiVisasAkcijas();

            Assert.IsTrue(visosAkcijos != null && visosAkcijos.Count > 0);
        }
        [Test]
        public void GautiVartotoja_TestVartotojas_rezultatas_GautasVartotojas()
        {
           Vartotojas vartotojas = PrekiautiLogika.GautiVartotoja(TestUserId);

            Assert.IsTrue(vartotojas != null && vartotojas.Id > 0);
        }
        [Test]
        public void ArDirbaAkcijuBirza_PakeistiDataIPirmadieni_rezultatas_DirbaTrue()
        {
            bool arDirbaBirza = false;
            DateTime dabartineData = DateTime.Now;
            if (dabartineData.DayOfWeek != DayOfWeek.Saturday && dabartineData.DayOfWeek != DayOfWeek.Sunday)
            {
                if (dabartineData.Hour >= 10 && dabartineData.Hour <= 16)
                {
                    arDirbaBirza = true;
                }
            }
                    ;

            Assert.IsTrue(PrekiautiLogika.ArDirbaAkcijuBirza(new System.Collections.Generic.List<Models.Akcijos>()) == arDirbaBirza);
        }
    }
}