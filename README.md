# Chantier Manager — Projet full-stack

Projet d'apprentissage construit avec **ASP.NET Core (C#)**, **Angular** et **PostgreSQL**. L'objectif est de développer progressivement une application de gestion de chantiers avec des accès différents selon le rôle de l'utilisateur.

> **État du projet : en développement.** Le backend dispose d'une connexion PostgreSQL, d'Entity Framework Core, d'ASP.NET Core Identity et des premières routes d'authentification et d'administration. L'interface Angular et la gestion métier des chantiers restent à développer.

## Stack technique

| Couche | Technologie | Utilisation |
| --- | --- | --- |
| Frontend | Angular 22, TypeScript | Interface utilisateur (en cours) |
| Backend | C#, ASP.NET Core 10, Minimal API | Routes HTTP et logique applicative |
| ORM | Entity Framework Core 10 | Accès aux données et migrations |
| Base de données | PostgreSQL | Stockage des comptes, rôles et futurs chantiers |
| Fournisseur PostgreSQL | Npgsql | Connexion d'EF Core à PostgreSQL |
| Authentification | ASP.NET Core Identity | Utilisateurs, mots de passe hachés et rôles |

## Fonctionnement prévu

L'application distingue trois rôles :

| Rôle | Fonctionnalités prévues |
| --- | --- |
| **Administrateur** | Créer et gérer les comptes des employés, leur attribuer un rôle |
| **Responsable** | Créer et gérer ses chantiers, y affecter des ouvriers |
| **Ouvrier** | Se connecter et consulter les chantiers auxquels il est affecté |

L'autorisation des actions doit être contrôlée par le **backend**. Les Guards et Signals Angular serviront plus tard à gérer la navigation et l'affichage, mais ne remplaceront pas les vérifications côté API.

## Prérequis

- **.NET SDK 10** : `dotnet --version`
- **Node.js et npm** : `node --version` et `npm --version`
- **PostgreSQL** installé et démarré (en local pour le moment)
- **Git** : `git --version`
- Optionnel : **pgAdmin 4** pour consulter et administrer la base de données

Angular CLI est déjà une dépendance du frontend : il n'est **pas nécessaire** de l'installer globalement pour utiliser `npm start`.

## 1. Récupérer le projet

```bash
git clone https://github.com/BluesOctopus01/ThomasEtPironRecapFullStack.git
cd ThomasEtPironRecapFullStack
```

Si le projet est déjà sur ta machine, ouvre simplement un terminal à sa racine. Les commandes suivantes supposent que tu pars de ce dossier, sauf mention contraire.

## 2. Installer les dépendances

### Backend (.NET / NuGet)

```bash
dotnet restore
```

`dotnet restore` récupère les packages déclarés dans `ChantierApi.csproj`, notamment Identity, EF Core Design et Npgsql.

### Frontend (Angular / npm)

```bash
cd front
npm install
cd ..
```

`npm install` installe les dépendances du fichier `front/package.json` dans `front/node_modules/`.

> **Important :** `dotnet new` et `ng new` servent à créer un **nouveau projet**. Il ne faut pas les relancer pour utiliser ce dépôt déjà existant.

## 3. Préparer PostgreSQL

La configuration locale attend les paramètres suivants :

| Paramètre | Valeur utilisée dans le projet |
| --- | --- |
| Hôte | `127.0.0.1` |
| Port | `5432` |
| Base de données | `chantier_manager` |
| Utilisateur applicatif | `chantier_app` |
| Mot de passe | À définir sur chaque machine |

Dans PostgreSQL (par exemple avec pgAdmin), préparer :

1. Un rôle de connexion `chantier_app` avec un mot de passe local.
2. Une base `chantier_manager`, idéalement avec `chantier_app` comme propriétaire pour ce projet de développement.

Si la base appartient déjà à un autre rôle, `chantier_app` doit au minimum pouvoir s'y connecter et créer des objets dans le schéma `public`, afin de permettre les migrations locales. Ne pas exécuter de commandes de création si ces éléments existent déjà.

### Enregistrer la chaîne de connexion avec User Secrets

Ouvre un terminal **Zsh** dans le dossier du backend :

```bash
cd back/ChantierApi
```

Le projet possède déjà un `UserSecretsId` dans son `.csproj` : **ne relance pas `dotnet user-secrets init`**.

Saisis le mot de passe PostgreSQL sans l'afficher :

```bash
read -s "PG_PASS?Mot de passe PostgreSQL : "
echo
```

Enregistre la chaîne de connexion :

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=127.0.0.1;Port=5432;Database=chantier_manager;Username=chantier_app;Password=$PG_PASS"
unset PG_PASS
```

ASP.NET Core chargera cette valeur automatiquement en environnement `Development`.

**Sécurité :** les User Secrets sont conservés localement, en dehors du dépôt Git. Ils ne sont pas chiffrés : ne les utilise pas comme gestionnaire de secrets de production et ne publie jamais de mot de passe ou de jeton d'accès.

## 4. Créer les tables avec les migrations EF Core

Le dépôt contient les migrations du backend. Sur une nouvelle base, il faut **appliquer les migrations existantes**, pas en générer une nouvelle.

### Installer l'outil de migration (une fois par machine)

```bash
dotnet tool install --global dotnet-ef --version 10.0.3
```

Si l'outil est déjà installé, cette étape est inutile. Vérification :

```bash
dotnet ef --version
```

### Appliquer les migrations

Depuis `back/ChantierApi` :

```bash
dotnet ef database update
```

Cette commande met à jour `chantier_manager` en appliquant uniquement les migrations manquantes. Elle crée notamment les tables Identity : `AspNetUsers`, `AspNetRoles` et `AspNetUserRoles`.

**Lorsqu'un modèle EF Core change plus tard**, le développeur crée une nouvelle migration et la versionne dans Git :

```bash
dotnet ef migrations add NomDeLaMigration
```

Puis il l'applique localement avec `dotnet ef database update`. En production, l'application des migrations sera à automatiser dans un processus de déploiement distinct, et non au démarrage de l'API.

## 5. Configurer le premier administrateur (développement local)

Au démarrage, `RoleSeeder` vérifie la présence des rôles `Administrateur`, `Responsable` et `Ouvrier`.

`AdminSeeder` initialise le premier compte administrateur **uniquement en environnement `Development`**, en utilisant les User Secrets suivants. Depuis `back/ChantierApi` :

```bash
dotnet user-secrets set "BootstrapAdmin:Email" "admin@chantier.test"

read -s "ADMIN_PASS?Mot de passe administrateur : "
echo
dotnet user-secrets set "BootstrapAdmin:Password" "$ADMIN_PASS"
unset ADMIN_PASS
```

Choisir un mot de passe de test solide (majuscules, minuscules, chiffres, caractère spécial). Identity applique ses propres règles de validation.

Lors du premier `dotnet run`, le code :

- crée le compte s'il n'existe pas encore ;
- lui attribue le rôle `Administrateur` s'il ne l'a pas ;
- évite de recréer le compte à chaque démarrage.

> Le *bootstrap* du premier administrateur est une facilité de développement local, **pas un mécanisme de provisionnement prêt pour la production**. Changer le secret après la création ne modifie pas automatiquement le mot de passe du compte existant.

## 6. Lancer le projet

Ouvrir **deux terminaux**.

### Terminal 1 — API ASP.NET Core

Depuis la racine du dépôt :

```bash
cd back/ChantierApi
dotnet run
```

L'API affiche son adresse d'écoute, par exemple :

```text
Now listening on: http://localhost:5154
```

Avec ce port, la route de diagnostic est accessible à l'adresse :

```text
http://localhost:5154/db-check
```

Elle permet de vérifier la connexion à PostgreSQL. Adapter le port à celui indiqué par le terminal.

### Terminal 2 — Angular

Depuis la racine du dépôt :

```bash
cd front
npm start
```

Le serveur Angular est normalement accessible sur `http://localhost:4200` (vérifier l'adresse affichée dans le terminal).

**Le frontend n'est pas encore relié aux fonctionnalités d'authentification.** Une interface vide ou incomplète est donc normale à ce stade.

Pour arrêter chaque serveur : `Ctrl + C`.

## 7. Routes API actuellement prévues dans le code

| Méthode | Route | Usage |
| --- | --- | --- |
| `GET` | `/db-check` | Vérification de la connexion PostgreSQL (développement) |
| `POST` | `/auth/login` | Connexion avec e-mail et mot de passe (Identity) |
| `POST` | `/auth/refresh` | Renouvellement du jeton d'authentification (Identity) |
| `POST` | `/auth/register` | Route fournie par Identity, **non destinée à l'inscription publique** |
| `POST` | `/admin/utilisateurs` | Création d'un compte `Responsable` ou `Ouvrier` par un `Administrateur` |

La route d'administration attend un JSON de cette forme :

```json
{
  "email": "ouvrier@test.local",
  "password": "MOT_DE_PASSE_DE_TEST_SOLIDE",
  "role": "Ouvrier"
}
```

Les deux valeurs de rôle autorisées lors de la création sont `Responsable` et `Ouvrier`. Le serveur doit vérifier que la requête provient d'un administrateur authentifié ; le frontend seul ne suffit pas à sécuriser cette opération.

> **État de la sécurité :** l'exposition des endpoints Identity, en particulier `/auth/register`, et la protection de `/admin/utilisateurs` doivent être revérifiées par des tests HTTP avant toute mise en ligne. Ne pas déployer cette version comme application publique.

## 8. Structure du dépôt

```text
ThomasEtPironRecapFullStack/
├── back/
│   └── ChantierApi/
│       ├── configurations/       # Configuration des services et initialisation des comptes/rôles
│       ├── data/                 # AppDbContext (Entity Framework Core)
│       ├── endpoints/            # Routes d'authentification, d'administration et de diagnostic
│       ├── models/               # ApplicationUser (Identity)
│       ├── Migrations/           # Historique des migrations EF Core
│       ├── Program.cs            # Point d'entrée de l'API
│       └── ChantierApi.csproj    # Dépendances NuGet et configuration .NET
├── front/
│   ├── src/                      # Application Angular
│   └── package.json             # Dépendances et scripts npm
├── doc/                         # Notes et documentation complémentaires
├── ThomasEtPiron.slnx           # Solution .NET
└── README.md
```

Le backend sépare volontairement le démarrage (`Program.cs`), la configuration des services, les modèles de données et les endpoints HTTP.

## 9. Commandes utiles

| Action | Commande | Dossier |
| --- | --- | --- |
| Installer les dépendances .NET | `dotnet restore` | Racine |
| Compiler le backend | `dotnet build` | Racine |
| Lancer le backend | `dotnet run` | `back/ChantierApi` |
| Voir les packages .NET | `dotnet list package` | `back/ChantierApi` |
| Installer les dépendances Angular | `npm install` | `front` |
| Lancer Angular | `npm start` | `front` |
| Compiler Angular | `npm run build` | `front` |
| Vérifier l'outil EF Core | `dotnet ef --version` | `back/ChantierApi` |
| Appliquer les migrations | `dotnet ef database update` | `back/ChantierApi` |
| Voir les outils .NET globaux | `dotnet tool list --global` | N'importe où |

## 10. Prochaines fonctionnalités

- Compléter le profil utilisateur (prénom, nom, etc.) et adapter les requêtes d'administration.
- Tester la création d'utilisateurs et les restrictions par rôle.
- Ajouter les modèles et endpoints de gestion des chantiers et des affectations.
- Construire la connexion et les vues Angular avec un état d'authentification adapté.
- Prévoir plus tard la conteneurisation Docker et l'automatisation des migrations au déploiement.

## Documentation officielle

- [ASP.NET Core — Documentation](https://learn.microsoft.com/fr-fr/aspnet/core/)
- [ASP.NET Core Identity](https://learn.microsoft.com/fr-fr/aspnet/core/security/authentication/identity)
- [Entity Framework Core — Migrations](https://learn.microsoft.com/fr-fr/ef/core/managing-schemas/migrations/)
- [Npgsql — Fournisseur PostgreSQL pour EF Core](https://www.npgsql.org/efcore/)
- [Angular — Documentation](https://angular.dev/)
