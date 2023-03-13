using Domain.Interfaces;
using Domain.Services;
using DataLayer.EF.Contexts;
using DataLayer.EF.Interfaces;
using DataLayer.EF.Repositories;
using DataLayer.EF.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.EntityFrameworkCore;
using Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(o =>
    {
        o.AllowedCertificateTypes = CertificateTypes.All;
    });

builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration["Sql:ConnectionString"]), ServiceLifetime.Transient);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddTransient<ICryptographicService, CryptographicService>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddTransient<ITokenService, TokenService>();

// Repositories
builder.Services.AddTransient<IProfileFlowRepository, ProfileUoWRepository>();
builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();