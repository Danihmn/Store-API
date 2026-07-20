using System.Security.Claims;

namespace Store.Api.Extensions.Endpoints;

public static class ClaimTypesExtension
{
    public static Guid Id(this ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var id = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value ?? string.Empty;
            return Guid.Parse(id);
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }

    public static string Name(this ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            return claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value ?? string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static string Email(this ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            return claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value ?? string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static string Role(this ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            return claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value ?? string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}