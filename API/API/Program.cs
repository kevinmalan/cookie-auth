using DataLayer.EF.Contexts;
using DataLayer.EF.Interfaces;
using DataLayer.EF.Repositories;
using DataLayer.EF.Repositories.UnitOfWork;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:AccessToken:Secret"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidateLifetime = true,
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["cookie-auth-access-token"];
                return Task.CompletedTask;
            }
        };
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