var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Authority = "https://localhost:7296"; // Tokenýn doðrulamasýný yapacaðýmýz IdentityServer'ýn adresi
    options.Audience = "resource_api2"; // Tokenýn hangi API için olduðunu belirtiyoruz
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
