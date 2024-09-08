using Microsoft.EntityFrameworkCore;
using UdemyIdentityServer.AuthServer;
using UdemyIdentityServer.AuthServer.Models;
using UdemyIdentityServer.AuthServer.Repository;
using UdemyIdentityServer.AuthServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomUserRepository, CustomUserRepository>();

builder.Services.AddDbContext<CustomDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"));
});


builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    //.AddTestUsers(Config.GetUsers().ToList())
    .AddDeveloperSigningCredential() // Development için private ve public key olu?turur.
    .AddProfileService<CustomProfileService>(); 

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

app.UseIdentityServer();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
