using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImmersiveReaderCore.Models;
using Microsoft.Extensions.Configuration;

namespace ImmersiveReaderCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly string SubscriptionKey;
        private readonly string Endpoint;

        public HomeController(IConfiguration configuration)
        {
            SubscriptionKey = configuration["SubscriptionKey"];
            Endpoint = configuration["Endpoint"];
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("token")]
        public async Task<string> Token()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                using (var response = await client.PostAsync(Endpoint, null))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return content;
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}