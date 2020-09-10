using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_discogs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscogsController : ControllerBase
    {
        public DiscogsController(ILogger<DiscogsController> logger)
        {
        }

        [HttpGet]
        public string Get()
        {
            try
            {
                HttpClient Client = new HttpClient
                {
                    BaseAddress = new Uri("https://api.discogs.com/users/ausamerika/collection/folders/0/releases")
                };

                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/json");
                Client.DefaultRequestHeaders.Add("User-Agent", "PetalMD test");


                HttpResponseMessage httpResponse = Client.GetAsync("").Result;

                dynamic obj = JsonConvert.DeserializeObject(httpResponse.Content.ReadAsStringAsync().Result);
                dynamic releases = obj["releases"];

                var rng = new Random();

                return JsonConvert.SerializeObject(Enumerable.Range(1, 5).Select(index => new
                {
                   discog = releases[rng.Next(releases.Count)]
                }));
            }
            catch { return null; }
        }

        [HttpGet("{per_page}")]
        public string Get(int per_page)
        {
            try
            {
                HttpClient Client = new HttpClient
                {
                    BaseAddress = new Uri(string.Format("https://api.discogs.com/users/ausamerika/collection/folders/0/releases?per_page={0}", per_page))
                };

                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/json");
                Client.DefaultRequestHeaders.Add("User-Agent", "PetalMD test");


                HttpResponseMessage httpResponse = Client.GetAsync("").Result;

                return httpResponse.Content.ReadAsStringAsync().Result;
            }
            catch { return null; }
        }
    }
}
