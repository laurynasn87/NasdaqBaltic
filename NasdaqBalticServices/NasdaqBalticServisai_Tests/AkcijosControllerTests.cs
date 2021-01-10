using Models;
using NasdaqBalticServisai.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace NasdaqBalticServisai_Tests
{
    public class AkcijosControllerTests
    {
        AkcijosController akcijosControlleris = new AkcijosController() ;

        [Test]
        public void GetAll__rezultatas_GautosVisosAkcijos()
        {
            IHttpActionResult Response =  akcijosControlleris.GetAll();
            var contentResult = Response as OkNegotiatedContentResult<List<Akcijos>>;

            Assert.NotNull(contentResult.Content != null && contentResult.Content.Count>0);
        }
        [Test]
        public void Get_AMG1L_rezultatasGautaSpecifineAkcija()
        {
            IHttpActionResult Response = akcijosControlleris.Get("AMG1L");
            var contentResult = Response as OkNegotiatedContentResult<Akcijos>;

            Assert.NotNull(contentResult.Content != null);
        }
    }
}