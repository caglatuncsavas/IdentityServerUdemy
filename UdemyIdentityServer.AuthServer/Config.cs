using IdentityServer4.Models;

namespace UdemyIdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
           return new List<ApiResource>
            {
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
    }
}
