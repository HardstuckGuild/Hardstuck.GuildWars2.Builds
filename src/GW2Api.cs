﻿using Newtonsoft.Json;
using System;
using System.Linq;
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
                if (!tokenInfo.Permissions.Where(x => x.Equals("characters")).Any())
                {
                    throw new NotEnoughPermissionsException("API key is missing \"characters\" permission.", NotEnoughPermissionsReason.Characters);
                }
                if (!tokenInfo.Permissions.Where(x => x.Equals("builds")).Any())
                {
                    throw new NotEnoughPermissionsException("API key is missing \"builds\" permission.", NotEnoughPermissionsReason.Builds);
                }
            }
        }

        internal async Task<T> Request<T>(string endpoint, string query = "")
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
