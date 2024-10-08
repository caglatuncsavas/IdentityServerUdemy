﻿using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using UdemyIdentityServer.Client1.Models;
using UdemyIdentityServer.Client1.Services;


namespace UdemyIdentityServer.Client1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResourceHttpClient _apiResourceHttpClient;

        public ProductsController(IConfiguration configuration, IApiResourceHttpClient apiResourceHttpClient)
        {
            _configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = await _apiResourceHttpClient.GetHttpClient();
            
            List<Product> products = new List<Product>();
    
            //https://localhost:7257

            var response = await client.GetAsync("https://localhost:7290/api/products/getproducts");

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
