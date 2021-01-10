using Models;
using NasdaqBalticServisai.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace NasdaqBalticServisai_Tests
{
    public class VartotojaiControllerTests
    {
        int TestUserId = 15;
        VartotojaiController vartotojaiController = new VartotojaiController();
        [Test]
        public void Get_TestUserId_rezultatas_GautasTestVartotojas()
        {
            IHttpActionResult Response = vartotojaiController.Get(TestUserId);
            var contentResult = Response as OkNegotiatedContentResult<Vartotojas>;

            Assert.NotNull(contentResult);
        }
        [Test]
        public void Autentifikuoti_TestUser_rezultatas_TrueIrGrazintasPrisijungesVartotojas()
        {
            Vartotojas Testvartotojas = new Vartotojas();
            Testvartotojas.Vardas = "TEST";
            Testvartotojas.Slaptazodis = "TESTPAS";
            IHttpActionResult Response = vartotojaiController.Autentifikuoti(Testvartotojas);
            var contentResult = Response as OkNegotiatedContentResult<Vartotojas>;

            Assert.NotNull(contentResult);
        }
        [Test]
        public void Create_TestUser_rezultatas_OkResult()
        {
            Vartotojas Testvartotojas = new Vartotojas();
            Testvartotojas.Vardas = "TEST" + DateTime.Now.ToString("dd/hh:mm:ss");
            Testvartotojas.Slaptazodis = "TESTPAS";
            IHttpActionResult Response = vartotojaiController.Create(Testvartotojas);
            var OkRes = Response as OkResult;

            Assert.NotNull(OkRes);
        }
    }
}