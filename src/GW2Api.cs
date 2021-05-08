using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hardstuck.GuildWars2.Builds
{
    internal class GW2Api : IDisposable
    {
        private string _apiKey;
        internal string ApiKey
        {
            get
            {
                return _apiKey;
            }
            set
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", value);
                _apiKey = value;
            }
        }
        private static readonly string basePoint = "https://api.guildwars2.com/";
        private readonly HttpClient httpClient = new HttpClient();

        internal GW2Api() { }

        internal async Task<dynamic> Request(string endpoint, string query)
        {
            using (HttpResponseMessage response = await httpClient.GetAsync($"{basePoint}{endpoint}?{query}"))
            {
                return JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            }
        }

        internal async Task<T> Request<T>(string endpoint, string query)
        {
            using (HttpResponseMessage response = await httpClient.GetAsync($"{basePoint}{endpoint}?{query}"))
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
