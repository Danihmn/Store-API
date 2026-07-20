using Store.Api.Endpoints;

namespace Store.Api.Extensions;

public static class EndpointsExtension
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapProductEndpoints();
        app.MapCustomerEndpoints();
        app.MapAddressEndpoints();
        app.MapStoreEndpoints();
        app.MapOrderEndpoints();
        app.MapOrderProductEndpoints();
        app.MapUserEndpoints();
    }
}