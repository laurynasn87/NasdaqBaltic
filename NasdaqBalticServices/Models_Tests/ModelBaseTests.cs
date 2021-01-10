using Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Models_Tests
{
    public class ModelBaseTests : ModelBase
    {

        [Test]
        public void SetObjectProperty_AkcijosProperty_rezultatas_PropertyUzdetas()
        {
            Akcijos akcija = new Akcijos();
            string AkcijosKodas = "TEST";
            string Pavadinimas = "Pavad";

            SetObjectProperty("Pavadinimas", Pavadinimas, akcija);
            SetObjectProperty("AkcijosKodas", AkcijosKodas, akcija);

            Assert.IsTrue(akcija.Pavadinimas.Equals(Pavadinimas) && akcija.AkcijosKodas.Equals(AkcijosKodas));
        }
        [Test]
        public void SetObjectProperty_FinansineInformacijaProperty_rezultatas_PropertyUzdetas()
        {
            FinansineInformacija FI = new FinansineInformacija();
            string Kiekis = "20";
            string PirkimoKaina = "1.2";

            SetObjectProperty("Kiekis", Kiekis, FI);
            SetObjectProperty("PirkimoKaina", PirkimoKaina, FI);

            Assert.IsTrue(FI.Kiekis == int.Parse(Kiekis) && FI.PirkimoKaina == double.Parse(PirkimoKaina));
        }
        [Test]
        public void SetObjectProperty_PapildomaInformacijaProperty_rezultatas_PropertyUzdetas()
        {
            PapildomaInformacija PI = new PapildomaInformacija();
            string Apie = "TEST";
            string AtaskaitosURL = "www.google.lt";

            SetObjectProperty("AtaskaitosURL", AtaskaitosURL, PI);
            SetObjectProperty("Apie", Apie, PI);

            Assert.IsTrue(PI.Apie.Equals(Apie) && PI.AtaskaitosURL.Equals(AtaskaitosURL));
        }
        [Test]
        public void PaverstObjektaITupleList_FinansinesInformacijosObjektas_rezultatas_TupleList()
        {
            FinansineInformacija FI = new FinansineInformacija();
            FI.PardavimoKaina = 1.2;
            FI.Paskutine_Kaina = 1;
            FI.PirkimoKaina = 3;
            FI.Sandoriai = 4;
            FI.AkcijosKodas = "TEST";
            KintamujuMapping = SukurtiFinansinesInformacijosMappinga();

            List<Tuple<string, string>> Tuple = PaverstObjektaITupleList(FI);
            Assert.IsTrue(Tuple.Any(x => x.Item2.Equals(FI.PardavimoKaina.ToString())) && Tuple.Any(x => x.Item2.Equals(FI.Paskutine_Kaina.ToString())) && Tuple.Any(x => x.Item2.Equals(FI.PirkimoKaina.ToString())) && Tuple.Any(x => x.Item2.Equals(FI.Sandoriai.ToString())));
        }
        [Test]
        public void PaverstObjektaITupleList_PapildomosInformacijosObjektas_rezultatas_TupleList()
        {
            PapildomaInformacija PI = new PapildomaInformacija();
            PI.AkcijosKodas = "TEST";
            PI.Atidarymo_Kaina = 12;
            PI.Didziausia_Kaina = 1;
            PI.Maziausia_Kaina = 1;
            PI.Vidutine_Kaina = 1;
            KintamujuMapping = SukurtiPapildomosInformacijosMappinga();

            List<Tuple<string, string>> Tuple = PaverstObjektaITupleList(PI);
            Assert.IsTrue(Tuple.Any(x => x.Item2.Equals(PI.AkcijosKodas)) && Tuple.Any(x => x.Item2.Equals(PI.Atidarymo_Kaina.ToString())) && Tuple.Any(x => x.Item2.Equals(PI.Didziausia_Kaina.ToString())) && Tuple.Any(x => x.Item2.Equals(PI.Maziausia_Kaina.ToString())) && Tuple.Any(x => x.Item2.Equals(PI.Vidutine_Kaina.ToString())));
        }
        [TestCase("AkcijosKodas", true, typeof(Akcijos))]
        [TestCase("UzdarymoKaina", true, typeof(VartotojoAkcija))]
        [TestCase("PaskKaina", true, typeof(FinansineInformacija))]
        [TestCase("Paskutine_Kaina", false, typeof(FinansineInformacija))]
        [TestCase("DidKaina", true, typeof(PapildomaInformacija))]
        public void KonvertuotiDbVardaIObjektoVarda_DBvardas_rezultatas_AtitinkimasVardas(String PropertyVardas, bool DBVardasIObjekta, Type tipas)
        {
            if (tipas == typeof(Akcijos))
                KintamujuMapping = SukurtiAkcijuMappinga();
            if (tipas == typeof(VartotojoAkcija))
                KintamujuMapping = SukurtiVartotojuAkcijuMappinga();
            if (tipas == typeof(FinansineInformacija))
                KintamujuMapping = SukurtiFinansinesInformacijosMappinga();
            if (tipas == typeof(PapildomaInformacija))
                KintamujuMapping = SukurtiPapildomosInformacijosMappinga();

            string Konvertuotas = KonvertuotiDbVardaIObjektoVarda(PropertyVardas, DBVardasIObjekta);
            string TiketinasKonvertuotas = String.Empty;

            if (!DBVardasIObjekta)
                TiketinasKonvertuotas = KintamujuMapping.Find(x => x.Item2.Equals(PropertyVardas)).Item1;
            else
                TiketinasKonvertuotas = KintamujuMapping.Find(x => x.Item1.Equals(PropertyVardas)).Item2;

            Assert.AreEqual(TiketinasKonvertuotas, Konvertuotas);


        }
        [Test]
        public void ArIdentiski_AkcijosSkirtingiObjektai_rezultatas_False()
        {
            Akcijos PirmaAkcija = new Akcijos("TEST", "Pavadinimas");
            Akcijos AntraAkcija = new Akcijos("TEST1", "Pavadinimas");

            Assert.IsFalse(ArIdentiski(PirmaAkcija,AntraAkcija));
        }
        [Test]
        public void ArIdentiski_IdentiskosAkcijos_rezultatas_True()
        {
            Akcijos PirmaAkcija = new Akcijos("TEST", "Pavadinimas");
            Akcijos AntraAkcija = new Akcijos("TEST", "Pavadinimas");

            Assert.IsTrue(ArIdentiski(PirmaAkcija, AntraAkcija));
        }

        [TestCase(new string[] {"AkcijosKodas","Pavadinimas"}, new string[] {"TEST","Pavadinimas"}, typeof(Akcijos))]
        [TestCase(new string[] { "AkcijosKodas", "PokytisProcentais", "Pokytis", "Sandoriai" }, new string[] { "TEST", "32","1","20" }, typeof(FinansineInformacija))]
        [TestCase(new string[] { "AkcijosKodas", "AtaskaitosURL", "Vadyba" }, new string[] { "TEST", "WWWW","teeest" }, typeof(PapildomaInformacija))]
        public void ListToObject_List_rezultatas_TeisingaiSukurtasObjektas(String[] PropertyName, String[] Reiksme, Type tipas)
        {
            if (PropertyName.Length == Reiksme.Length && PropertyName.Length > 0)
            {
                List<Tuple<string, string>> ReiksmiuTupleList = new List<Tuple<string, string>>();
                for (int i = 0; i < PropertyName.Length; i++)
                {
                    ReiksmiuTupleList.Add(new Tuple<string, string>(PropertyName[i], Reiksme[i]));
                }

                Object KonvertuotasElementas = new object();
                if (tipas == typeof(Akcijos))
                {
                    KintamujuMapping = SukurtiAkcijuMappinga();
                    KonvertuotasElementas = ListToObject<Akcijos>(ReiksmiuTupleList);
                }
                if (tipas == typeof(VartotojoAkcija))
                {
                    KintamujuMapping = SukurtiVartotojuAkcijuMappinga();
                    KonvertuotasElementas = ListToObject<VartotojoAkcija>(ReiksmiuTupleList);
                }
                if (tipas == typeof(FinansineInformacija))
                {
                    KintamujuMapping = SukurtiFinansinesInformacijosMappinga();
                    KonvertuotasElementas = ListToObject<FinansineInformacija>(ReiksmiuTupleList);
                }
                if (tipas == typeof(PapildomaInformacija))
                {
                    KintamujuMapping = SukurtiPapildomosInformacijosMappinga();
                    KonvertuotasElementas = ListToObject<PapildomaInformacija>(ReiksmiuTupleList);
                }
                bool AtitinkaMappinga = true;
                if (KonvertuotasElementas != null)
                {
                    for (int i = 0; i < PropertyName.Length; i++)
                    {
                        if (!AtitinkaMappinga)
                            break;

                        String DbName = KonvertuotiDbVardaIObjektoVarda(PropertyName[i], false);
                        string RastaReiksme = GetPropValue(KonvertuotasElementas, DbName).ToString();

                        if (string.IsNullOrWhiteSpace(RastaReiksme) || !RastaReiksme.Equals(Reiksme[i]))
                            AtitinkaMappinga = false;

                    }
                }
                else
                    AtitinkaMappinga = false;


                Assert.IsTrue(AtitinkaMappinga);

            }
            else
                Assert.Fail();
        }
        List<Tuple<string, string>> SukurtiAkcijuMappinga()
        {
            List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>(){new Tuple<string, string>("AkcijosKodas", "AkcijosKodas"),
            new Tuple<string, string>("Pavadinimas", "Pavadinimas") };

            return KintamujuMapping;
        }
        List<Tuple<string, string>> SukurtiVartotojuAkcijuMappinga()
        {
            List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>(){
            new Tuple<string, string>("Id", "Id"),
            new Tuple<string, string>("AkcijosKodas", "akcija.AkcijosKodas"),
            new Tuple<string, string>("VartotojoId", "vartotojas.Id"),
            new Tuple<string, string>("Pirkimas", "Pirkimas"),
            new Tuple<string, string>("PirkimoKaina", "PirkimoKaina"),
            new Tuple<string, string>("UzdarymoKaina", "UzdarymoKaina"),
            new Tuple<string, string>("Kiekis", "Kiekis"),
            new Tuple<string, string>("UzdarymoData", "UzdarymoData"),
            new Tuple<string, string>("Priezastis", "Priezastis"),
            new Tuple<string, string>("AtidarymoData", "AtidarymoData")
            };

            return KintamujuMapping;
        }
        List<Tuple<string, string>> SukurtiFinansinesInformacijosMappinga()
        {
            List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>()
            {
            new Tuple<string, string>("Id","Id"),
            new Tuple<string, string>("AkcijosKodas","AkcijosKodas"),
            new Tuple<string, string>("PaskKaina","Paskutine_Kaina"),
            new Tuple<string, string>("Pokytis","Pokytis"),
            new Tuple<string, string>("PokytisProcentais","PokytisProcentais"),
            new Tuple<string, string>("PirkimoKaina","PirkimoKaina"),
            new Tuple<string, string>("PardavimoKaina","PardavimoKaina"),
            new Tuple<string, string>("Sandoriai","Sandoriai"),
            new Tuple<string, string>("Kiekis","Kiekis"),
            new Tuple<string, string>("Apyvarta","Apyvarta"),
            new Tuple<string, string>("LaikoZyme","Timestamp")
        };

            return KintamujuMapping;
        }
        List<Tuple<string, string>> SukurtiPapildomosInformacijosMappinga()
        {
            List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>()
            {
            new Tuple<string, string>("Id","Id"),
            new Tuple<string, string>("AkcijosKodas","AkcijosKodas"),
            new Tuple<string, string>("Apie","Apie"),
            new Tuple<string, string>("AtaskaitosURL","AtaskaitosURL"),
            new Tuple<string, string>("DidKaina","Didziausia_Kaina"),
            new Tuple<string, string>("MinKaina","Maziausia_Kaina"),
            new Tuple<string, string>("VidKaina","Vidutine_Kaina"),
            new Tuple<string, string>("AtidarymoKaina","Atidarymo_Kaina"),
            new Tuple<string, string>("Vadyba","Vadyba"),
            new Tuple<string, string>("Kontaktai","Kontaktai"),
            new Tuple<string, string>("Kalba","Kalba")
            };

            return KintamujuMapping;
        }
        public object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}