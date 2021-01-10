using DALs;
using NUnit.Framework;
using System.Collections.Generic;
using Models;
using System;
using System.Linq;

namespace DALs_Tests
{
    public class VartotojoAkcijaDALTests
    {

        VartotojoAkcijaDAL VADal = new VartotojoAkcijaDAL("Test");
        [Test]
        public void GautiVisus__rezultatas_AkcijuListDaugiauUz0()
        {
            List<VartotojoAkcija> VisosVartotojuAkcijos = VADal.GautiVisus();
            Assert.IsTrue(VisosVartotojuAkcijos != null && VisosVartotojuAkcijos.Count > 0);
        }
        [Test]
        public void GautiVartotojoAkcijas_PirmoVartotojoAkcijosId_rezultatas_PirmaVartotojoAkcijaListeRezultatu()
        {
            List<VartotojoAkcija> VisosVartotojuAkcijos = VADal.GautiVisus();
            VartotojoAkcija VartotojoAkcija = VisosVartotojuAkcijos[0];

            List<VartotojoAkcija> GautaVartotojoAkcijaPagalVartotojoId = VADal.GautiVartotojoAkcijas(VartotojoAkcija.vartotojas.Id);

            Assert.IsTrue(GautaVartotojoAkcijaPagalVartotojoId.Any(x => x.Id == VartotojoAkcija.Id));
        }
        [Test]
        public void Ivesti_PirmoVartotojoAkcijaBespokePirkimoKaina_rezultatas_VartotojoAkcijaDuomenuBazeje()
        {
            List<VartotojoAkcija> VisosVartotojuAkcijos = VADal.GautiVisus();
            VartotojoAkcija VartotojoAkcija = VisosVartotojuAkcijos[0];

            VartotojoAkcija.PirkimoKaina = DateTime.Now.Month * 10000 + DateTime.Now.Day * 1000 + DateTime.Now.Hour * 100 + DateTime.Now.Minute;
            VADal.Ivesti(VartotojoAkcija);

            VisosVartotojuAkcijos = VADal.GautiVartotojoAkcijas(VartotojoAkcija.vartotojas.Id);

            Assert.IsTrue(VisosVartotojuAkcijos.Any(x => x.PirkimoKaina.Equals(VartotojoAkcija.PirkimoKaina) && x.Kiekis.Equals(VartotojoAkcija.Kiekis) && x.vartotojas.Id.Equals(VartotojoAkcija.vartotojas.Id) && x.Kiekis == VartotojoAkcija.Kiekis));
        }
        [Test]
        public void Atnaujinti_PirmoVartotojoAkcijaBespokePirkimoKaina_rezultatas_InformacijaAtnaujinta()
        {
            List<VartotojoAkcija> VisosVartotojuAkcijos = VADal.GautiVisus();
            VartotojoAkcija VartotojoAkcija = VisosVartotojuAkcijos[0];

            VartotojoAkcija.PirkimoKaina = DateTime.Now.Month * 10000 + DateTime.Now.Day * 1000 + DateTime.Now.Hour * 100 + DateTime.Now.Minute;
            VADal.Atnaujinti(VartotojoAkcija);

            VisosVartotojuAkcijos = VADal.GautiVartotojoAkcijas(VartotojoAkcija.vartotojas.Id);

            Assert.IsTrue(VisosVartotojuAkcijos.Any(x => x.PirkimoKaina.Equals(VartotojoAkcija.PirkimoKaina) && x.Kiekis.Equals(VartotojoAkcija.Kiekis) && x.vartotojas.Id.Equals(VartotojoAkcija.vartotojas.Id) && x.Kiekis == VartotojoAkcija.Kiekis && x.Id == VartotojoAkcija.Id));
        }

    }
}