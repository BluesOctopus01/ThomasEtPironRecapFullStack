
using ChantierApi.models;

namespace ChantierApi.Endpoints;

public static class AuthEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGroup("/auth")
            .MapIdentityApi<ApplicationUser>()
            .AddEndpointFilter(async (context, next) =>
            {
                var http = context.HttpContext;

                if (http.Request.Path == "/auth/register")
                {
                    if (http.User.Identity?.IsAuthenticated != true)
                        return Results.Unauthorized();

                    if (!http.User.IsInRole("Administrateur"))
                        return Results.Forbid();
                }

                return await next(context);
            });
    }
}
