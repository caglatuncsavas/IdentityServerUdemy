using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using UdemyIdentityServer.Client1.Models;


namespace UdemyIdentityServer.Client1.Controllers
{
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

            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7296"); 

            if(disco.IsError)
            {
                //loglama yapılabilir
            };

            TokenResponse tokenResponse = _memoryCache.Get<TokenResponse>(CacheKey);

            tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                ClientId = _configuration["Client:ClientId"],
                ClientSecret = _configuration["Client:ClientSecret"],
                Address = disco.TokenEndpoint
            });

            //ClientCredentialsTokenRequest clientCredentialsTokenRequest = new ClientCredentialsTokenRequest();

            //clientCredentialsTokenRequest.ClientId = _configuration["Client:ClientId"]!;
            //clientCredentialsTokenRequest.ClientSecret = _configuration["Client:ClientSecret"];
            //clientCredentialsTokenRequest.Address = disco.TokenEndpoint;

            //var token = await httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if(tokenResponse.IsError)
            {
                //loglama yapılabilir
            }

            //https://localhost:7257

            httpClient.SetBearerToken(tokenResponse.AccessToken);

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
