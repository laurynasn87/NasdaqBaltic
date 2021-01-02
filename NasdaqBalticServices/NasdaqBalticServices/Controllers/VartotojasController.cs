using DALs;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NasdaqBalticServices
{
    [Route("api/VartotojasController")]
    [ApiController]
    public class VartotojasController : Controller
    {
        // POST: api/VartotojasController/Create
        [HttpPost]
        public HttpStatusCode Create([FromBody] Dictionary<string, string> keys)
        {
            string Vardas = String.Empty;
            String Slaptazodis = string.Empty;
            if (!string.IsNullOrEmpty(Vardas) && !string.IsNullOrEmpty(Slaptazodis))
            {
                Vartotojas vartotojas = new Vartotojas();
                LoginIrRegistracijosDAL dal = new LoginIrRegistracijosDAL();
                vartotojas.Vardas = Vardas;
                vartotojas.Slaptazodis = Slaptazodis;
                if (dal.Ivesti(vartotojas))
                    return HttpStatusCode.Created;
            }
            return HttpStatusCode.BadRequest;
        }
        // POST: api/VartotojasController/Autentifikuoti 
        [HttpPost]
        public HttpStatusCode Post([FromBody] Dictionary<string, string> keys)
        {
            string Vardas = String.Empty;
            String Slaptazodis = string.Empty;
            if (!string.IsNullOrEmpty(Vardas) && !string.IsNullOrEmpty(Slaptazodis))
            {
                Vartotojas vartotojas = new Vartotojas();
                LoginIrRegistracijosDAL dal = new LoginIrRegistracijosDAL();
                vartotojas.Vardas = Vardas;
                vartotojas.Slaptazodis = Slaptazodis;
                if (dal.ArTeisingiLogin(vartotojas))
                    return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        // GET api/VartotojasController/5
        [HttpGet("{kodas}")]
        public string Get(int kodas)
        {
            return "belenkaip";
        }
    }
}
