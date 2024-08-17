var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc"; // OpenId Connect => IdentityServerdan gelen cookie ile haberle?ecek.
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.SignInScheme = "Cookies";
        options.Authority = "https://localhost:7296";
        options.ClientId = "Client1-Mvc";
        options.ClientSecret = "secret";
        options.ResponseType = "code id_token";
        options.GetClaimsFromUserInfoEndpoint = true; // UserInfo endpointten gelen bilgileri almak için; bu ep'e istek yapacak ve oradan gelen cliamleri otomatik bir þekilde ekleyecek.
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
