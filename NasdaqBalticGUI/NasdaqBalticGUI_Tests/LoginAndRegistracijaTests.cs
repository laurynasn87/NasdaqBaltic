using Models;
using NasdaqBalticGUI;
using NUnit.Framework;
using System;

namespace NasdaqBalticGUI_Tests
{
    public class LoginAndRegistracijaTests
    {
        LoginAndRegistracija login;

        [Test]
        public void BandytiPrisijungt_VardasTESTslaptazodisTESTPAS_rezultatas_()
        {
            login = new LoginAndRegistracija("TEST", "TESTPAS");
            Vartotojas DabartinisVartotojas;
            Assert.IsTrue(login.BandytiPrisijungti(out DabartinisVartotojas) && DabartinisVartotojas != null);
        }

        [Test]
        public void BandytiRegistruoti_VardasTESTdataSlaptazodisTESTPAS_rezultatas_()
        {
            login = new LoginAndRegistracija("TEST" + DateTime.Now.ToString("dd/hh:mm:ss"), "TESTPAS");

            Assert.IsTrue(login.BandytiRegistruoti() );
        }
    }
}