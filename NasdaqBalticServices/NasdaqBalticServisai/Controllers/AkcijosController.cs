using Dals;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace NasdaqBalticServisai.Controllers
{
    public class AkcijosController : ApiController
    {
        // GET: Akcijos
        public IHttpActionResult GetAll()
        {
            AkcijuDAL akcijuDAL = new AkcijuDAL();
            return Ok(akcijuDAL.GautiVisus());
        }

        // GET Akcijos
        public IHttpActionResult Get(string kodas)
        {
            if (!String.IsNullOrEmpty(kodas))
            {
                AkcijuDAL akcijuDAL = new AkcijuDAL();
                return Ok(akcijuDAL.GautiPagalKoda(kodas));
            }
            return BadRequest();
        }
    }
}
