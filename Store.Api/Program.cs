using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Store.Api.Endpoints;
using Store.Application;
using Store.Infrastructure;
using Store.Infrastructure.Data.StoreContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.MapProductEndpoints();
app.MapCustomerEndpoints();
app.MapAddressEndpoints();
app.MapStoreEndpoints();
app.MapOrderEndpoints();
app.MapOrderProductEndpoints();

app.MapScalarApiReference("/scalar", options =>
{
    options
        .WithTitle("Store API")
        .WithOpenApiRoutePattern("/openapi/v1.json");
});

app.Run();
