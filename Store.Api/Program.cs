using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Store.Api.Endpoints;
using Store.Application;
using Store.Infrastructure;
using Store.Infrastructure.Data.StoreContext;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSecretKey"]
                                     ?? throw new InvalidOperationException("JWT key is not configured"));

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
builder.Services.AddAuthorization(options => { options.AddPolicy("admin", policy => policy.RequireRole("admin")); });

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

app.MapProductEndpoints();
app.MapCustomerEndpoints();
app.MapAddressEndpoints();
app.MapStoreEndpoints();
app.MapOrderEndpoints();
app.MapOrderProductEndpoints();
app.MapUserEndpoints();

app.MapScalarApiReference("/scalar", options =>
{
    options
        .WithTitle("Store API")
        .WithOpenApiRoutePattern("/openapi/v1.json");
});

app.Run();