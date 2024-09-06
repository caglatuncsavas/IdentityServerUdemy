using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using UdemyIdentityServer.Client1.Models;


namespace UdemyIdentityServer.Client1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private const string CacheKey = "ClientCredentialsToken";
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public ProductsController(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = new List<Product>();

            HttpClient httpClient = new HttpClient();

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
         
            //https://localhost:7257

            httpClient.SetBearerToken(accessToken);

            var response = await httpClient.GetAsync("https://localhost:7290/api/products/getproducts");

            if (response.IsSuccessStatusCode) 
            {
                var content = await response.Content.ReadAsStringAsync();

                products = JsonConvert.DeserializeObject<List<Product>>(content);
            }
            else
            {
                //loglama yapılabilir
            }

            return View(products);
        }
    }
}
