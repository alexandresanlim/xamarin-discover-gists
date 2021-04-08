using DiscoverGists.Models;
using DiscoverGists.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscoverGists.Services
{
    public class GitHubService : IGitHubService
    {
        public async Task<List<Gist>> GetUser()
        {
            string requestUrl = "https://api.github.com/gists/public?page=0";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                var content = await client.GetStringAsync(requestUrl);

                var gists = JsonConvert.DeserializeObject<List<Gist>>(content, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                return gists;
            }
        }
    }
}
