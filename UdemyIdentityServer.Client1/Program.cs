using Microsoft.AspNetCore.Authentication;
using UdemyIdentityServer.Client1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IApiResourceHttpClient, ApiResourceHttpClient>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc"; // OpenId Connect => IdentityServerdan gelen cookie ile haberle?ecek.
})
    .AddCookie("Cookies",options =>
    {
        options.AccessDeniedPath = "/Home/AccessDenied";
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.SignInScheme = "Cookies";
        options.Authority = "https://localhost:7296";
        options.ClientId = "Client1-Mvc";
        options.ClientSecret = "secret";
        options.ResponseType = "code id_token";
        options.GetClaimsFromUserInfoEndpoint = true; // UserInfo endpointten gelen bilgileri almak için; bu ep'e istek yapacak ve oradan gelen cliamleri otomatik bir þekilde ekleyecek.
        options.SaveTokens = true;
        options.Scope.Add("api1.read");
        options.Scope.Add("offline_access");
        options.Scope.Add("CountryAndCity");
        options.Scope.Add("Roles");

        options.ClaimActions.MapUniqueJsonKey("country", "country");
        options.ClaimActions.MapUniqueJsonKey("city", "city");
        options.ClaimActions.MapUniqueJsonKey("role", "role");

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            RoleClaimType = "role"
        };
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
