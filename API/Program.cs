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
using System.Reflection;
using System.Text;
using Serilog;
using API.Middleware;
using API.Endpoints;
using Carter;

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
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration["Sql:ConnectionString"]), ServiceLifetime.Transient);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog
Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Information()
       .WriteTo.Console()
       .WriteTo.Seq(builder.Configuration["Seq:Url"])
       .CreateLogger();

builder.Host.UseSerilog();

// Services
builder.Services.AddCarter();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ICryptographicService, CryptographicService>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddTransient<ICookieService, CookieService>();

// Repositories
builder.Services.AddTransient<IProfileFlowRepository, ProfileUoWRepository>();
builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

// Config
builder.Services.Configure<TokenConfig>(builder.Configuration.GetSection("Token"));
builder.Services.Configure<PasswordConfig>(builder.Configuration.GetSection("Password"));
builder.Services.Configure<CookieConfig>(builder.Configuration.GetSection("Cookie"));

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

app.MapCarter();

app.Run();