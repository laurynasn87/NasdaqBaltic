using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqBalticGUI
{
   public class ApiKontroleris
    {
        public readonly string BaseUrl = "https://localhost:44319/api/";
        private static readonly HttpClient client = new HttpClient();

        public async Task<bool> PostApiCallAsync(Dictionary<string, string> keys, string ulr, Object Objektas = null)
        {
            var response = new HttpResponseMessage();

            if (keys != null && keys.Count > 0 && Objektas == null)
            {
                var content = new FormUrlEncodedContent(keys);

                 response = await client.PostAsync(ulr, content);
            }
            else if (Objektas != null)
            {

                string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(Objektas);
                StringContent stringContent = new StringContent(serialized,Encoding.UTF8, "application/json");
                response = await client.PostAsync(ulr, stringContent);
            }
            else
            {
                return false;
            }

            var responseString = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;

           }
        public T PostApiCallResponseObject<T>(Dictionary<string, string> keys, string ulr)
        {
            string json = String.Empty;
            try
            {
                json = Task.Run(async () => await PostApiCallObjectAsync(keys, ulr)).Result;
            }
            catch( Exception e)
            {

            }
            if (!String.IsNullOrEmpty(json))
            {
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            else return default(T);
        }
        async Task<string> PostApiCallObjectAsync(Dictionary<string, string> keys, string ulr)
        {
            if (keys != null && keys.Count > 0)
            {
                var content = new FormUrlEncodedContent(keys);

                var response = await client.PostAsync(ulr, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            return String.Empty;
        }
        public T GetApiCallResponseObject<T>(string ulr)
        {
            string json = Task.Run(async () => await GetApiCallObjectAsync(ulr)).Result;
            if (!String.IsNullOrEmpty(json))
            {
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            else return default(T);
        }
        async Task<string> GetApiCallObjectAsync(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var responseString = await client.GetStringAsync(url);
                return responseString;
            }
            return String.Empty;
        }

    }
}
