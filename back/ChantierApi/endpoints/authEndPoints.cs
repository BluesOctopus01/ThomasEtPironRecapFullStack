
using ChantierApi.models;

namespace ChantierApi.Endpoints;

public static class AuthEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGroup("/auth")
           .MapIdentityApi<ApplicationUser>();
    }
}
