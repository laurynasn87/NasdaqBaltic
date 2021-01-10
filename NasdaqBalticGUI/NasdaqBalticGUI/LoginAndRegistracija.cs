using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqBalticGUI
{
   public class LoginAndRegistracija
    {
        string Url = "https://localhost:44319/api/Vartotojai";
        string Vardas;
        string Slaptazodis;
        ApiKontroleris api = new ApiKontroleris();

        public LoginAndRegistracija(string vardas, string slaptazodis)
        {
            this.Vardas = vardas;
            this.Slaptazodis = slaptazodis;
        }
        public bool BandytiPrisijungti(out Vartotojas dabartinisNaudotojas)
        {
            var reiksmes = new Dictionary<string, string>
            {
                { "Vardas", Vardas },
                { "Slaptazodis", Slaptazodis }
            };
           dabartinisNaudotojas = api.PostApiCallResponseObject<Vartotojas>(reiksmes, Url + "/Autentifikuoti");
            return dabartinisNaudotojas != null;
        }
        public bool BandytiRegistruoti()
        {
            bool rezultatas = false;
            var reiksmes = new Dictionary<string, string>
            {
                { "Vardas", Vardas },
                { "Slaptazodis", Slaptazodis }
            };
            try
            {
                rezultatas = Task.Run(async () => await api.PostApiCallAsync(reiksmes, Url + "/Create")).Result;
            }
            catch(Exception e)
            { }
            

            return rezultatas;
        }
    }
}
