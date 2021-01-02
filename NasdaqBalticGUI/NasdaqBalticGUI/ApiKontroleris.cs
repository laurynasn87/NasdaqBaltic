using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqBalticGUI
{
   public class ApiKontroleris
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<bool> ApiCallAsync(Dictionary<string, string> keys, string ulr)
        {
            if (keys != null && keys.Count > 0)
            {
                var content = new FormUrlEncodedContent(keys);

                var response = await client.PostAsync(ulr, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;
            }
            return false;
        }
        public T ApiCallResponseObject<T>(Dictionary<string, string> keys, string ulr)
        {
           string json = Task.Run(async () => await ApiCallObjectAsync(keys, ulr)).Result;
            if (!String.IsNullOrEmpty(json))
            {
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            else return default(T);
        }
        async Task<string> ApiCallObjectAsync(Dictionary<string, string> keys, string ulr)
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
    }
}
