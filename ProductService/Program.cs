

using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Data;
using ProductService.Middleware;
using ProductService.Services;

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

app.UseAuthorization();

// Map attribute-routed API controllers.
app.MapControllers();
app.MapGet("/test-exception", () =>
{
    throw new Exception("Test exception for middleware");
});
app.Run();

