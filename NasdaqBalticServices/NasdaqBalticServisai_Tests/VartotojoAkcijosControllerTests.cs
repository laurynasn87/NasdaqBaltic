using DALs;
using Models;
using NasdaqBalticServisai.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace NasdaqBalticServisai_Tests
{
    public class VartotojoAkcijosControllerTests
    {
        VartotojoAkcijosController vartotojuAkcijosController = new VartotojoAkcijosController();
        int TestUserId = 15;
        [Test]
        public void Get_TestUser_rezultatas_GautosVartojoAkcijos()
        {
            IHttpActionResult Response = vartotojuAkcijosController.Get(TestUserId);
            var contentResult = Response as OkNegotiatedContentResult<List<VartotojoAkcija>>;

            Assert.NotNull(contentResult);
        }
        [Test]
        public void Create_TestVartotojoAkcija_rezultatas_SekmingaiSukurtaVartotojoAkcija()
        {
            VartotojoAkcija vartotojoAkcija = new VartotojoAkcija();
            vartotojoAkcija.akcija = new Akcijos("AMG1L");
            vartotojoAkcija.Kiekis = 20;
            vartotojoAkcija.Pirkimas = true;
            vartotojoAkcija.PirkimoKaina = 2.2;
            vartotojoAkcija.vartotojas = new Vartotojas(TestUserId);


            IHttpActionResult Response = vartotojuAkcijosController.Create(vartotojoAkcija);
            var OkRes = Response as OkResult;

            Assert.NotNull(OkRes);
        }
        [Test]
        public void Edit_TestVartotojoAkcija_rezultatas_SekmingaiPakeistaVartotojoAkcijosInformacija()
        {
            VartotojoAkcijaDAL vartotojoAkcijaDAL = new VartotojoAkcijaDAL();
            VartotojoAkcija vartotojoAkcija = vartotojoAkcijaDAL.GautiVartotojoAkcijas(TestUserId)[0];


            vartotojoAkcija.Kiekis = DateTime.Now.Day * 1000 + DateTime.Now.Hour * 100 + DateTime.Now.Minute;

            IHttpActionResult Response = vartotojuAkcijosController.Edit(vartotojoAkcija.Id, vartotojoAkcija);
            var OkRes = Response as OkResult;

            Assert.NotNull(OkRes);
        }
    }
}