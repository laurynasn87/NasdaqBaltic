using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqBalticGUI
{
    class LoginAndRegistracija
    {
        string Url = "https://localhost:44355/api/VartotojasController";
        string Vardas;
        string Slaptazdodis;
        ApiKontroleris api = new ApiKontroleris();

        public LoginAndRegistracija(string vardas, string slaptazodis)
        {
            this.Vardas = vardas;
            this.Slaptazdodis = slaptazodis;
        }
        public bool BandytiPrisijungti()
        {
            bool rezultatas = false;
            var reiksmes = new Dictionary<string, string>
            {
                { "Vardas", Vardas },
                { "Slaptazdodis", Slaptazdodis }
            };
            rezultatas = Task.Run(async () => await api.ApiCallAsync(reiksmes, Url + "/Autentifikuoti")).Result;

            return rezultatas;
        }
        public bool BandytiRegistruoti()
        {
            bool rezultatas = false;
            var reiksmes = new Dictionary<string, string>
            {
                { "Vardas", Vardas },
                { "Slaptazdodis", Slaptazdodis }
            };
            rezultatas = Task.Run(async () => await api.ApiCallAsync(reiksmes, Url + "/Create")).Result;

            return rezultatas;
        }
    }
}
