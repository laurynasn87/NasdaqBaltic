using Models;
using NasdaqBalticGUI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NasdaqBalticGUI_Tests
{
    public class ApiKontrolerisTests
    {
        ApiKontroleris api = new ApiKontroleris();

        [Test]
        public void GetApiCallResponseObject_GautiPagalKodaAkcija_rezultatas_AkcijaGauta()
        {
            Akcijos akcija = api.GetApiCallResponseObject<Akcijos>("https://localhost:44319/api/akcijos?kodas=AMG1L");
            Assert.NotNull(akcija);
        }
        [Test]
        public void PostApiCallAsync_SukurtiUseri_rezultatas_SekmingaiSukurtas()
        {
            var reiksmes = new Dictionary<string, string>
            {
                { "Vardas", "ApiCallTest" + DateTime.Now.ToString("dd/hh:mm: ss") },
                { "Slaptazodis", "TEEEST" }
            };
            bool response = Task.Run(async () => await api.PostApiCallAsync(reiksmes, "https://localhost:44319/api/Vartotojai/Create")).Result;

            Assert.IsTrue(response);
        }
        [Test]
        public void PostApiCallResponseObject_Autentifikuoti_rezultatas_GrazinaPrisjungusiVartotoja()
        {
            Vartotojas dabartinisNaudotojas;

            var reiksmes = new Dictionary<string, string>
            {
                { "Vardas", "TEST" },
                { "Slaptazodis", "TESTPAS" }
            };
            dabartinisNaudotojas = api.PostApiCallResponseObject<Vartotojas>(reiksmes, "https://localhost:44319/api/Vartotojai/Autentifikuoti");

            Assert.NotNull(dabartinisNaudotojas);
        }
    }
}