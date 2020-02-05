using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImmersiveReaderCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ImmersiveReaderCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly string TenantId;     
        private readonly string ClientId;     
        private readonly string ClientSecret;
        private readonly string Subdomain;

        public HomeController(IConfiguration configuration)
        {
            TenantId = configuration["TenantId"];
            ClientId = configuration["ClientId"];
            ClientSecret = configuration["ClientSecret"];
            Subdomain = configuration["Subdomain"];
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("token")]
        public async Task<JsonResult> Token()
        {
            string authority = $"https://login.windows.net/{TenantId}";
            const string resource = "https://cognitiveservices.azure.com/";

            var authContext = new AuthenticationContext(authority);
            var clientCredential = new ClientCredential(ClientId, ClientSecret);

            var authResult = await authContext.AcquireTokenAsync(resource, clientCredential);

            return new JsonResult(new { token = authResult.AccessToken, subdomain = Subdomain });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}