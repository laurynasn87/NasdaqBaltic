using NUnit.Framework;
using Database;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Database_Tests
{
    public class SQLCommandsTests
    {
        SQLCommands sql = new SQLCommands("Test");

        [TestCase("akcijos")]
        [TestCase("finansineinformacija")]
        [TestCase("papildomainformacija")]
        [TestCase("users")]
        [TestCase("vartotojuakcijos")]
        public void GetAll_LentelesPav_rezultatas_VisiLentelesElementai(string LentelesPav)
        {
            List<List<Tuple<string, string>>> result = sql.GetAll(LentelesPav);

            Assert.IsTrue(result != null && result.Count > 0);
        }
        [TestCase("akcijos", new string[] { "Pavadinimas" }, new string[] { "Apranga" }, 1)]
        [TestCase("finansineinformacija", new string[] { "AkcijosKodas", "PirkimoKaina" }, new string[] { "SAUNA", "0" }, 2)]
        [TestCase("papildomainformacija", new string[] { "AkcijosKodas", "Kalba" }, new string[] { "SAUNA", "en" }, 1)]
        [TestCase("users", new string[] { "Vardas" }, new string[] { "laurynas1" }, 1)]
        [TestCase("vartotojuakcijos", new string[] { "Pirkimas", "VartotojoId" }, new string[] { "1", "3" }, 2)]
        public void GetByCondition_Kondicija_rezultatas_IsfiltruotiElementai(string TableName, string[] Stulpeliai, string[] Reiksmes, int limit)
        {
            bool Sekmingas = false;
            if (Stulpeliai.Length == Reiksmes.Length && Reiksmes.Length > 0)
            {
                List<Tuple<string, string>> StulpeliaiIrReiksmes = new List<Tuple<string, string>>();
                for (int i = 0; i < Stulpeliai.Length; i++)
                {
                    StulpeliaiIrReiksmes.Add(new Tuple<string, string>(Stulpeliai[i], Reiksmes[i]));

                }
                List<List<Tuple<string, string>>> Rezultatai = sql.GetByCondition(TableName, StulpeliaiIrReiksmes, limit);

                if (Rezultatai != null && Rezultatai.Count > 0 && Rezultatai.Count == limit)
                {
                    bool AtitinkaConditiona = true;
                    foreach (List<Tuple<string, string>> vienasEl in Rezultatai)
                    {

                        for (int i = 0; i < Stulpeliai.Length; i++)
                        {
                            if (!vienasEl.Any(x => x.Item1.Equals(Stulpeliai[i]) && x.Item2.Equals(Reiksmes[i])))
                                AtitinkaConditiona = false;

                        }
                        if (!AtitinkaConditiona)
                            break;
                    }
                    Sekmingas = true;
                }



            }
            Assert.IsTrue(Sekmingas);
        }
        [TestCase("finansineinformacija")]
        [TestCase("papildomainformacija")]
        [TestCase("users")]
        [TestCase("vartotojuakcijos")]
        public void GetByCondition_OrderPagalIdDesc_rezultatas_IsfiltruotiElementaiTinkamuEiliskumu(string TableName)
        {

            List<Tuple<string, string>> StulpeliaiIrReiksmes = new List<Tuple<string, string>>();

            List<List<Tuple<string, string>>> Rezultatai = sql.GetByCondition(TableName, StulpeliaiIrReiksmes,0,"Id",true);

            if (Rezultatai != null && Rezultatai.Count >= 2)
            {
                int PriesTaiBuvesId = 0;
                foreach (List<Tuple<string, string>> vienasEl in Rezultatai)
                {
                   Tuple<string,string> IdTuple = vienasEl.Find(x => x.Item1.Equals("Id"));
                    if (IdTuple != null)
                    {
                        int id = int.Parse(IdTuple.Item2);
                        if (PriesTaiBuvesId == 0 || PriesTaiBuvesId > id)
                            PriesTaiBuvesId = id;
                        else Assert.Fail();
                    }
                   
                }
                Assert.Pass();
            }
            Assert.Fail();
        }
        [TestCase("akcijos", new string[] { "AkcijosKodas","Pavadinimas" }, new string[] { "TEST", "TESTTY" })]
        [TestCase("finansineinformacija", new string[] { "AkcijosKodas", "PaskKaina", "PirkimoKaina" }, new string[] { "SAUNA", "20", "0" })]
        [TestCase("papildomainformacija", new string[] { "AkcijosKodas", "Kalba", "AtidarymoKaina", "DidKaina" }, new string[] { "SAUNA", "en","12","500" })]
        [TestCase("users", new string[] { "Vardas","Slaptazodis" }, new string[] { "laurynas", "Test" })]
        [TestCase("vartotojuakcijos", new string[] { "AkcijosKodas", "VartotojoId" }, new string[] { "APG1L", "3" })]
        public void Insert_LentelesNameIrReiksmes_rezultatas_SekmingaiIvestasIrasas(string TableName, string[] Stulpeliai, string[] Reiksmes)
        {
            if (Stulpeliai.Length == Reiksmes.Length && Reiksmes.Length > 0)
            {
                List<Tuple<string, string>> StulpeliaiIrReiksmes = new List<Tuple<string, string>>();
                for (int i = 0; i < Stulpeliai.Length; i++)
                {
                    if ((TableName.Equals("akcijos") && Stulpeliai[i].Equals("AkcijosKodas")) || Stulpeliai[i].Equals("Vardas"))
                        Reiksmes[i] = Reiksmes[i] + DateTime.Now.ToString("MM/dd/HH:mm:ss");

                    StulpeliaiIrReiksmes.Add(new Tuple<string, string>(Stulpeliai[i], Reiksmes[i]));

                }
              Assert.IsTrue(sql.Insert(StulpeliaiIrReiksmes,TableName));

            }
            else
            Assert.Fail();
        }
        [TestCase("finansineinformacija", new string[] {"PaskKaina", "PirkimoKaina" }, new string[] {"20", "0" }, "AkcijosKodas","SAUNA")]
        [TestCase("papildomainformacija", new string[] { "Kalba", "AtidarymoKaina", "DidKaina" }, new string[] { "en", "12", "500" }, "AkcijosKodas", "SAUNA")]
        [TestCase("users", new string[] {"Balansas" }, new string[] {"50000" }, "Slaptazodis","test")]
        [TestCase("vartotojuakcijos", new string[] {"Kiekis" }, new string[] { "50" },"Priezastis", "Uždaryta Vartojo Noru")]
        public void Update_LentelesNameIrReiksmesIdentifikavimoReiksmes_rezultatas_AtnaujintasIrasasSekmingai(string TableName, string[] Stulpeliai, string[] Reiksmes, string IdentifikavimoStulpelioPav, string IdentifikavimoReiksme)
        {
            
            if (Stulpeliai.Length == Reiksmes.Length && Reiksmes.Length > 0)
            {
                List<Tuple<string, string>> StulpeliaiIrReiksmes = new List<Tuple<string, string>>();
                for (int i = 0; i < Stulpeliai.Length; i++)
                {
                    if ((TableName.Equals("akcijos") && Stulpeliai[i].Equals("AkcijosKodas")) || Stulpeliai[i].Equals("Vardas"))
                        Reiksmes[i] = Reiksmes[i] + DateTime.Now.ToString("MM/dd/HH:mm:ss");

                    StulpeliaiIrReiksmes.Add(new Tuple<string, string>(Stulpeliai[i], Reiksmes[i]));

                }
                Assert.IsTrue(sql.Update(StulpeliaiIrReiksmes,TableName, IdentifikavimoStulpelioPav, IdentifikavimoReiksme));

            }
            else
                Assert.Fail();
        }
    }

}
