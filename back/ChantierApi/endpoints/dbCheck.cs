
using ChantierApi.data;
using ChantierApi.models;

namespace ChantierApi.Endpoints;

public static class DbCheckEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/db-check", async (AppDbContext db) =>
        {
            bool connected = await db.Database.CanConnectAsync();
            return connected
                ?"Connexion PostgreSQL réussie !"
                : "Impossible de se connecter à PostgreSQL.";
        });
    }
}
