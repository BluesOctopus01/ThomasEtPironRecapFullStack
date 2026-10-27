using Microsoft.AspNetCore.Identity;
using ChantierApi.data;
using Microsoft.EntityFrameworkCore;
using ChantierApi.models;

var builder = WebApplication.CreateBuilder(args);

// Récupérer notre chaîne de connexion
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connexion PostgreSQL introuvable.");

// Configurer Entity Framework Core avec PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/db-check", async (AppDbContext db) =>
{
    bool connected = await db.Database.CanConnectAsync();

    return connected
        ? "Connexion PostgreSQL réussie !"
        : "Impossible de se connecter à PostgreSQL.";
});

app.Run();
