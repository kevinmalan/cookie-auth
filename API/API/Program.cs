using Core.Contexts;
using Core.Interfaces;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration["Sql:ConnectionString"]), ServiceLifetime.Transient);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddTransient<ICryptographicService, CryptographicService>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddTransient<ITokenService, TokenService>();

// Config
builder.Services.Configure<TokenConfig>(builder.Configuration.GetSection("Token"));
builder.Services.Configure<PasswordConfig>(builder.Configuration.GetSection("Password"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();