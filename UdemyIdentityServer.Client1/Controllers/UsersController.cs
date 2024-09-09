using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;

namespace UdemyIdentityServer.Client1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc"); Bu metotla identity server'dan çıkış gerçekleiyordu. Ancak artık identity server'dan çıkış işlemini yapmıyoruz. 

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> GetRefreshToken()
        {
            //Tekrardan signin yapacağız, yeni bir cookie oluşacak.
            HttpClient httpClient = new HttpClient();

            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7296");

            if (disco.IsError)
            {
                //loglama yapılabilir
            };

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();

            refreshTokenRequest.ClientId = _configuration["ClientResourceOwner:ClientId"];
            refreshTokenRequest.ClientSecret = _configuration["ClientResourceOwner: ClientSecret"];
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = disco.TokenEndpoint;

            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                //yönlendirme yapılabilir
            }

            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name = OpenIdConnectParameterNames.IdToken, Value = token.IdentityToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)}
            };

            var authenticationResult = await HttpContext.AuthenticateAsync(); 

            var properties = authenticationResult.Properties;

            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult AdminAction()
        {
            return View();
        }

        [Authorize(Roles = "customer")]
        public IActionResult CustomerAction()
        {
            return View();
        }
    }
}
