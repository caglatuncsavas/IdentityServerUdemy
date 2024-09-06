
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace UdemyIdentityServer.Client1.Services
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _client;

        public ApiResourceHttpClient(IHttpContextAccessor httpContextAccessor, HttpClient client)
        {
            _httpContextAccessor = httpContextAccessor;
            _client = client;
        }

        public async Task<HttpClient> GetHttpClient()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            _client.SetBearerToken(accessToken);

           return _client;
        }
    }
}
