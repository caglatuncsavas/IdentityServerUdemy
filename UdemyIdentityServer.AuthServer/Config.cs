using IdentityServer4;
using IdentityServer4.Models;

namespace UdemyIdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                // İlgili tokenın bu api için geçerli olup, olmadığını test edebiliriz.Bu API ile birlikte jwt içerisindeki payload kısmında yer alan datalarda dönüyor olacak.
                 new ApiResource("resource_api1") {Scopes= {"api1.read", "api1.write", "api1.update" }, ApiSecrets = new[]{new Secret("secretapi1".Sha256())} },
                 new ApiResource("resource_api2") {Scopes = { "api2.read", "api2.write", "api2.update" }, ApiSecrets = new[]{new Secret("secretapi2".Sha256())}}
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("api1.read", "API1 için okuma izni"),
                new ApiScope("api1.write", "API1 için yazma izni"),
                new ApiScope("api1.update", "API1 için güncelleme izni"),
                new ApiScope("api2.read", "API2 için okuma izni"),
                new ApiScope("api2.write", "API2 için yazma izni"),
                new ApiScope("api2.update", "API2 için güncelleme izni"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "Client1",
                    ClientName = "Client1 app uygulaması",
                    ClientSecrets = new[] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.read"},

                },
                new Client()
                {
                    ClientId = "Client2",
                    ClientName = "Client2 app uygulaması",
                    ClientSecrets = new[] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.read", "api1.update", "api2.write", "api2.update" }
                },
                new Client()
                {
                    ClientId = "Client1-Mvc",
                    RequirePkce = false,
                    ClientName = "Client1 app uygulaması",
                    ClientSecrets = new[] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ "https://localhost:7077/signin-oidc"}, // Tokenın döneceği url, yani token alma işlemini yapan url-Kulanının yönlendirileceği url username ve şifreyi girdikten sonra
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile},
                }
            };
        }
    }
}
