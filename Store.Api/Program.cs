using FluentValidation;
using Scalar.AspNetCore;
using Serilog;
using Store.Application;
using Store.Infrastructure;
using Store.Infrastructure.Data.StoreContext;
using Store.Api.Extensions;
using Store.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<StoreContext>("store");

builder.Host.UseSerilog(((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.WithEnvironmentName()));

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi(options =>
{
    options.CreateSchemaReferenceId = jsonTypeInfo =>
        jsonTypeInfo.Type.FullName?.Replace("+", ".");
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
    };
});

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LogUsernameMiddleware>();

app.MapOpenApi();
app.MapEndpoints();
app.MapScalarApiReference("/scalar", options =>
{
    options
        .WithTitle("Store API")
        .WithOpenApiRoutePattern("/openapi/v1.json");
});

app.Run();