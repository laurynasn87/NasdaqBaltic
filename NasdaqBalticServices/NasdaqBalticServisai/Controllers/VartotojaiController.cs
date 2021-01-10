using DALs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace NasdaqBalticServisai.Controllers
{
    public class VartotojaiController : ApiController
    {

        // POST: Vartotojai/Create
        public IHttpActionResult Create([FromBody] Vartotojas vartotojas )
        {
            if (!string.IsNullOrEmpty(vartotojas.Vardas) && !string.IsNullOrEmpty(vartotojas.Slaptazodis))
            {
                LoginIrRegistracijosDAL dal = new LoginIrRegistracijosDAL();
                if (dal.Ivesti(vartotojas))
                    return Ok();
            }
            return BadRequest();
        }
        // Get: Vartotojai/id
        public IHttpActionResult Get(int id)
        {
            if (id>0)
            {
                VartotojasDAL dal = new VartotojasDAL();
                Vartotojas gautas = dal.GautiPagalId(id.ToString());
                gautas.Slaptazodis = String.Empty;
                return Ok(gautas);
            }
            return BadRequest();
        }
        // POST: Vartotojai/Autentifikuoti
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Vartotojai/Autentifikuoti")]
        public IHttpActionResult Autentifikuoti([FromBody] Vartotojas vartotojas)
        {
            if (!string.IsNullOrEmpty(vartotojas.Vardas) && !string.IsNullOrEmpty(vartotojas.Slaptazodis))
            {
                Vartotojas GautasVarototjas;
                LoginIrRegistracijosDAL dal = new LoginIrRegistracijosDAL();
                if (dal.ArTeisingiLogin(vartotojas, out GautasVarototjas))
                    return Ok(GautasVarototjas);
            }
            return BadRequest();
        }
    
    }
}
