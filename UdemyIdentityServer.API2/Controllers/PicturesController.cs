using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyIdentityServer.API2.Models;

namespace UdemyIdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetPictures()
        {
            var pictures = new List<Picture>
            {
                new Picture { Id =1, Name = "Picture 1", Url = "http://www.picture1.com" },
                new Picture { Id =2, Name = "Picture 2", Url = "http://www.picture2.com" }
            };

            return Ok(pictures);
        }
    }
}
