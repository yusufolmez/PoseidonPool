using PoseidonPool.Application;
using PoseidonPool.Infrastructure;
using PoseidonPool.Persistance;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .WithOrigins("https://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience         = true,
        ValidateIssuer           = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidAudience            = builder.Configuration["Token:Audience"],
        ValidIssuer              = builder.Configuration["Token:Issuer"],
        IssuerSigningKey         = new SymmetricSecurityKey(
                                      Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        NameClaimType            = ClaimTypes.Name
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Set default culture to en-US to ensure decimal parsing uses dot (19.99) by default.
var supportedCultures = new[] { new CultureInfo("en-US") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    await next();
});


app.MapControllers();

app.Run();
