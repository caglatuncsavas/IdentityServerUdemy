using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;
using UdemyIdentityServer.Client1.Models;

namespace UdemyIdentityServer.Client1.Controllers
{
    public class LoginController : Controller
    {
        IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);

            if (disco.IsError)
            {
                // hata ve loglama
            }

            var password = new PasswordTokenRequest();

            password.Address = disco.TokenEndpoint;
            password.UserName = loginViewModel.Email;
            password.Password = loginViewModel.Password;
            password.ClientId = _configuration["ClientResourceOwner:ClientId"];
            password.ClientSecret = _configuration["ClientResourceOwner:ClientSecret"];

            var token = await client.RequestPasswordTokenAsync(password);

            if (token.IsError) 
            {

                ModelState.AddModelError("","Email veya şifreniz yanlış");
                // hata ve loglama
            }

            var userInfoRequest = new UserInfoRequest();

            userInfoRequest.Token = token.AccessToken;
            userInfoRequest.Address = disco.UserInfoEndpoint;

            var userInfo = await client.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                   // hata ve loglama
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, "Cookies", "name", "role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name = OpenIdConnectParameterNames.IdToken, Value = token.IdentityToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)}
            });


            HttpContext.SignInAsync("Cookies", claimsPrincipal, authenticationProperties);

            return RedirectToAction("Index","User");
        }
    }
}
