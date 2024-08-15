var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Authority = "https://localhost:7296"; // Token�n do�rulamas�n� yapaca��m�z IdentityServer'�n adresi
    options.Audience = "resource_api2"; // Token�n hangi API i�in oldu�unu belirtiyoruz
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
