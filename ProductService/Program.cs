using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Data;
using ProductService.Middleware;
using ProductService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProductService.Authentication;
using Microsoft.AspNetCore.Identity;
using ProductService.Models;

var builder = WebApplication.CreateBuilder(args);

// Register controllers for our Web API.
builder.Services.AddControllers();
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ProductConnection")));
builder.Services.AddScoped<
    IProductService,
    global::ProductService.Services.ProductService>();

builder.Services.AddProblemDetails();
//using Asp.Versioning;

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader =
            new UrlSegmentApiVersionReader();
    })
    .AddMvc();

// jwt token code 
var jwtSettings = builder.Configuration.GetSection("Jwt");

var key = jwtSettings["Key"]
    ?? throw new InvalidOperationException("JWT Key is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();



builder.Services.AddAuthorization();

builder.Services.AddScoped<JwtTokenService>();

var app = builder.Build();
//app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // We can add Swagger/OpenAPI configuration here later.
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map attribute-routed API controllers.
app.MapControllers();

app.Run();

