using DALs;
using NUnit.Framework;
using System.Collections.Generic;
using Models;
using System;
using System.Linq;

namespace DALs_Tests
{
    public class LoginIrRegistracijosDALTests
    {
        LoginIrRegistracijosDAL RegistracijosLoginDAL = new LoginIrRegistracijosDAL("Test");
        [Test]
        public void Ivesti_Vartotojas_rezultatas_True()
        {
            Vartotojas NaujasVartotojas = new Vartotojas();
            NaujasVartotojas.Vardas = "LaurynasTEST" + DateTime.Now.ToString("MM-dd/HH:mm");
            NaujasVartotojas.Slaptazodis = "Test";

            Assert.IsTrue(RegistracijosLoginDAL.Ivesti(NaujasVartotojas));
        }
        [Test]
        public void ArTeisingiLogin_Vartotojas_rezultatas_True()
        {
            Vartotojas NaujasVartotojas = new Vartotojas();
            NaujasVartotojas.Vardas = "LaurynasTEST" + DateTime.Now.ToString("MM-dd/HH:mm:ss");
            NaujasVartotojas.Slaptazodis = "Test";
            RegistracijosLoginDAL.Ivesti(NaujasVartotojas);

            Vartotojas PrisijungesVartotojas;

            bool SekmingasPrisijungimas = RegistracijosLoginDAL.ArTeisingiLogin(NaujasVartotojas, out PrisijungesVartotojas);

            Assert.IsTrue(SekmingasPrisijungimas && PrisijungesVartotojas != null && PrisijungesVartotojas.Vardas.Equals(NaujasVartotojas.Vardas));
        }
    }
}