using DALs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace NasdaqBalticServisai.Controllers
{
    public class VartotojoAkcijosController : ApiController
    {
        // POST: VartotojoAkcijos/Create
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/VartotojoAkcijos/Create")]
        public IHttpActionResult Create([FromBody] VartotojoAkcija vartotojoAkcija)
        {
            if (vartotojoAkcija.akcija != null && !String.IsNullOrEmpty(vartotojoAkcija.akcija.AkcijosKodas) && vartotojoAkcija.vartotojas != null && vartotojoAkcija.vartotojas.Id > 0)
            {
                VartotojasDAL VDal = new DALs.VartotojasDAL();
                VartotojoAkcijaDAL dal = new VartotojoAkcijaDAL();
                if (dal.Ivesti(vartotojoAkcija))
                {
                    Vartotojas vartotojas = VDal.GautiPagalId(vartotojoAkcija.vartotojas.Id.ToString());
                    vartotojas.Balansas = vartotojas.Balansas - (vartotojoAkcija.Kiekis * vartotojoAkcija.PirkimoKaina);
                   if (VDal.AtnaujintiBalansa(vartotojas))
                        return Ok();
                }
                    
            }
            return BadRequest();
        }
        // GET VartotojoAkcijos/11
        public IHttpActionResult Get(int VartotojoId)
        {
            if (VartotojoId != 0)
            {
                VartotojoAkcijaDAL vartotojoAkcijuDAL = new VartotojoAkcijaDAL();
                return Ok(vartotojoAkcijuDAL.GautiVartotojoAkcijas(VartotojoId));
            }
            return BadRequest();
        }
        // POST: VartotojoAkcijos/Edit/5
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/VartotojoAkcijos/Edit/{id}")]
        public IHttpActionResult Edit(int id, [FromBody] VartotojoAkcija vartotojoAkcija)
        {
            if (id != 0)
            {
                VartotojoAkcijaDAL vartotojoAkcijuDAL = new VartotojoAkcijaDAL();
                bool Atnaujinta = vartotojoAkcijuDAL.Atnaujinti(vartotojoAkcija);
                if (Atnaujinta)
                {
                    vartotojoAkcija.vartotojas.Balansas = vartotojoAkcija.vartotojas.Balansas + vartotojoAkcija.UzdarymoKaina * vartotojoAkcija.Kiekis;
                    VartotojasDAL dAL = new VartotojasDAL();
                   if  (dAL.AtnaujintiBalansa(vartotojoAkcija.vartotojas))
                        return Ok();
                }
                return Ok(vartotojoAkcijuDAL.Atnaujinti(vartotojoAkcija));
            }
            return BadRequest();
        }
    }
}
