using ChantierApi.Configuration;
using ChantierApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configuration des services
ServiceConfiguration.Configure(builder);

var app = builder.Build();

// Initialiser les rôles, puis l'administrateur
await RoleSeeder.SeedAsync(app);
await AdminSeeder.SeedAsync(app);

// Déclaration des routes
AuthEndpoints.Map(app);
DbCheckEndpoints.Map(app);

// Démarrage
app.Run();
