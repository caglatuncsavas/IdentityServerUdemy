var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Authority = "https://localhost:7296"; // Token'?n do?rulamas?n? yapaca??m?z IdentityServer'?n adresi
    options.Audience = "resource_api1"; // Token'?n hangi API için oldu?unu belirtiyoruz
});

//Kimli?ini do?rulanm?? bir kullan?c?n?n yetkilendirmesini yapmak için AddAuthorization metodu kullan?l?r.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadProduct", policy =>
    {
        policy.RequireClaim("scope", "api1.read");
    });

    options.AddPolicy("UpdateOrCreate", policy =>
    {
        policy.RequireClaim("scope", new[] { "api1.update", "api1.create" });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
