using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Store.Application;
using Store.Infrastructure;
using Store.Infrastructure.Data.StoreContext;
using Store.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("seller", policy => policy.RequireRole("seller"));
    options.AddPolicy("purchaser", policy => policy.RequireRole("purchaser"));
    options.AddPolicy("stock_clerk", policy => policy.RequireRole("stock_clerk"));
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi(options =>
{
    options.CreateSchemaReferenceId = jsonTypeInfo =>
        jsonTypeInfo.Type.FullName?.Replace("+", ".");
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.MapEndpoints();
app.MapScalarApiReference("/scalar", options =>
{
    options
        .WithTitle("Store API")
        .WithOpenApiRoutePattern("/openapi/v1.json");
});

app.Run();