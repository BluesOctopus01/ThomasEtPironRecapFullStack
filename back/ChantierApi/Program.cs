using ChantierApi.Configuration;
using ChantierApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configuration des services
ServiceConfiguration.Configure(builder);

var app = builder.Build();

// Déclaration des routes
AuthEndpoints.Map(app);
DbCheckEndpoints.Map(app);

// Démarrage
app.Run();
