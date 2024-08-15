using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyIdentityServer.API1.Models;

namespace UdemyIdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : Controller
    {
        [Authorize(Policy = "ReadProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>(){
            new Product { Id = 1, Name = "Kalem", Price = 100, Stock = 10 },
            new Product { Id = 2, Name = "Silgi", Price = 200, Stock = 20 },
            new Product { Id = 3, Name = "Defter", Price = 300, Stock = 30 },
            new Product { Id = 4, Name = "Kitap", Price = 400, Stock = 40 }};

            return Ok(productList);
        }

        [Authorize(Policy = "UpdateOrCreate")]
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"id'si{id}olan product güncellenmiştir.");
        }

        [Authorize(Policy = "UpdateOrCreate")]
        [HttpGet]
        public IActionResult CreateProduct(Product product)
        {
            return Ok(product);
        }
    }
}
