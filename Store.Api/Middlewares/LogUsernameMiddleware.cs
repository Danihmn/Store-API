using Serilog.Context;

namespace Store.Api.Middlewares;

public class LogUsernameMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        var name = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;

        using (LogContext.PushProperty("name", name))
            await next(httpContext);
    }
}