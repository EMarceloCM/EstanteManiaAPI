using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOcelot();

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", opt =>
{
    opt.Authority = "https://localhost:7146/";
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true
    };
});

var app = builder.Build();

await app.UseOcelot();

app.Run();
