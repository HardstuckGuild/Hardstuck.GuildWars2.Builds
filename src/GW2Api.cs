using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hardstuck.GuildWars2.Builds
{
    internal sealed class GW2Api : IDisposable
    {
        #region definitions
        private string _apiKey = string.Empty;
        private const string basePoint = "https://api.guildwars2.com/";
        private readonly HttpClient httpClient = new HttpClient();

        internal string ApiKey
        {
            get => _apiKey;
            set
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", value);
                _apiKey = value;
            }
        }
        internal bool ApiKeySet => !string.IsNullOrWhiteSpace(ApiKey);
        #endregion

        internal GW2Api() { }

        internal GW2Api(string apiKey, bool checkPerms = true)
        {
            ApiKey = apiKey;
            if (checkPerms)
            {
                APIClasses.TokenInfo tokenInfo = Request<APIClasses.TokenInfo>("v2/tokeninfo").Result;
                if (tokenInfo is null)
                {
                    throw new NotEnoughPermissionsException("API key is invalid.", NotEnoughPermissionsReason.Invalid);
                }
                if (!tokenInfo.Permissions.Any(x => x.Equals("characters")))
                {
                    throw new NotEnoughPermissionsException("API key is missing \"characters\" permission.", NotEnoughPermissionsReason.Characters);
                }
                if (!tokenInfo.Permissions.Any(x => x.Equals("builds")))
                {
                    throw new NotEnoughPermissionsException("API key is missing \"builds\" permission.", NotEnoughPermissionsReason.Builds);
                }
            }
        }

        internal void ChangeApiKey(string apiKey, bool checkPerms)
        {
            ApiKey = apiKey;
            if (checkPerms)
            {
                APIClasses.TokenInfo tokenInfo = Request<APIClasses.TokenInfo>("v2/tokeninfo").Result;
                if (tokenInfo is null)
                {
                    throw new NotEnoughPermissionsException("API key is invalid.", NotEnoughPermissionsReason.Invalid);
                }
                if (!tokenInfo.Permissions.Any(x => x.Equals("characters")))
                {
                    throw new NotEnoughPermissionsException("API key is missing \"characters\" permission.", NotEnoughPermissionsReason.Characters);
                }
                if (!tokenInfo.Permissions.Any(x => x.Equals("builds")))
                {
                    throw new NotEnoughPermissionsException("API key is missing \"builds\" permission.", NotEnoughPermissionsReason.Builds);
                }
            }
        }

        internal async Task<T> Request<T>(string endpoint, string query = "")
        {
            using (HttpResponseMessage response = await httpClient.GetAsync($"{basePoint}{endpoint}{((query != string.Empty) ? $"?{query}" : "")}"))
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
