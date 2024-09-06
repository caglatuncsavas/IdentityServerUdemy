using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Security.Claims;

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
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),// Tokenın içindeki subject ıd =>subıd bilgisini almak için
                new IdentityResources.Profile(), // Kullanıcı hakkında belli claimleri almak için
                new IdentityResource(){ Name = "CountryAndCity", DisplayName = "Country and City", Description = "Kullanıcının ülke ve şehir bilgisi",
                UserClaims = new[]{"country","city" } },
                new IdentityResource(){Name = "Roles", DisplayName = "Roles", Description = "Kullanıcı rolleri", UserClaims = new[]{"role"}}
            };
        }

        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser{SubjectId = "1", Username = "CSavas", Password = "Password12*", Claims = new List<Claim>()
                    {
                        new Claim("given_name", "Çağla"),
                        new Claim("family_name", "Savaş"),
                        new Claim("country", "Türkiye"),
                        new Claim("city", "Ankara"),
                        new Claim("role", "admin")
                    }
                },

                new TestUser{SubjectId = "2", Username = "SSavas", Password = "Password12*", Claims = new List<Claim>()
                    {
                        new Claim("given_name", "Sinem"),
                        new Claim("family_name", "Savaş"),
                        new Claim("country", "Türkiye"),
                        new Claim("city", "İstanbul"),
                        new Claim("role", "customer")
                    }
                }
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
                    RedirectUris = new List<string>{ "https://localhost:7257/signin-oidc"}, // Tokenın döneceği url, yani token alma işlemini yapan url-Kulanının yönlendirileceği url username ve şifreyi girdikten sonra
                    PostLogoutRedirectUris = new List<string>{ "https://localhost:7257/signout-callback-oidc"}, // Kullanıcı logout olduğunda yönlendirileceği url
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile, "api1.read", 
                        IdentityServerConstants.StandardScopes.OfflineAccess, "CountryAndCity", "Roles"},

                    AccessTokenLifetime = 2*60*60,// 2 saatlik saniye cinsinden
                    AllowOfflineAccess = true,// Refresh token alabilmek için
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RequireConsent = false
                },
                 new Client()
                {
                    ClientId = "Client2-Mvc",
                    RequirePkce = false,
                    ClientName = "Client2 app uygulaması",
                    ClientSecrets = new[] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ "https://localhost:7251/signin-oidc"}, // Tokenın döneceği url, yani token alma işlemini yapan url-Kulanının yönlendirileceği url username ve şifreyi girdikten sonra
                    PostLogoutRedirectUris = new List<string>{ "https://localhost:7251/signout-callback-oidc"}, // Kullanıcı logout olduğunda yönlendirileceği url
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile, "api1.read",
                        IdentityServerConstants.StandardScopes.OfflineAccess, "CountryAndCity", "Roles"},

                    AccessTokenLifetime = 2*60*60,// 2 saatlik saniye cinsinden
                    AllowOfflineAccess = true,// Refresh token alabilmek için
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RequireConsent = true
                }
            };
        }
    }
}
