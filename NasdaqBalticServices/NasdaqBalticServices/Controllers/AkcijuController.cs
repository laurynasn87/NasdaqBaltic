using Dals;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NasdaqBalticServices
{
    [Route("api/AkcijosController")]
    [ApiController]
    public class AkcijuController : Controller
    {
        // GET: api/AkcijosController
        [HttpGet]
        public IEnumerable<Akcijos> Get()
        {
            AkcijuDAL akcijuDAL = new AkcijuDAL();
            return akcijuDAL.GautiVisus();
        }

        // GET api/AkcijosController/5
        [HttpGet("{kodas}")]
        public Akcijos Get(string kodas)
        {
            if (!String.IsNullOrEmpty(kodas))
            {
                AkcijuDAL akcijuDAL = new AkcijuDAL();
                return akcijuDAL.GautiPagalKoda(kodas);
            }
            return null;
        }
    }
}
