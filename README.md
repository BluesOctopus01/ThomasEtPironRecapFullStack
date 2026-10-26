# ThomasEtPiron — Projet Full Stack

Projet d'apprentissage composé d'une API **ASP.NET Core (C#)**, d'un frontend **Angular** et d'une base de données **PostgreSQL**.

## Prérequis

Installer sur sa machine :

- **.NET SDK 10** (`dotnet --version`)
- **Node.js et npm** (`node --version` et `npm --version`)
- **PostgreSQL** (serveur local démarré)

> Angular CLI n'a pas besoin d'être installé globalement : les commandes du projet utilisent la version présente dans `node_modules`.

## Récupérer le projet

```bash
git clone https://github.com/BluesOctopus01/ThomasEtPironRecapFullStack.git
cd ThomasEtPironRecapFullStack
```

Si le projet est déjà sur ta machine, ouvre simplement un terminal à sa racine.

## Installer les dépendances

Depuis la racine du projet :

### Backend (.NET)

```bash
dotnet restore
```

Cette commande télécharge les packages NuGet déclarés dans le projet C#, notamment le fournisseur PostgreSQL d'Entity Framework Core.

### Frontend (Angular)

```bash
cd front
npm install
cd ..
```

Cette commande télécharge les dépendances JavaScript du frontend dans `front/node_modules/`.

> `dotnet restore` et `npm install` sont à effectuer après un clonage ou lorsque les dépendances changent. Il n'est pas nécessaire de recréer les projets avec `dotnet new` ou `ng new`.

## Configurer la base de données (développement local)

Le backend utilise PostgreSQL avec :

- **Base de données** : `chantier_manager`
- **Utilisateur applicatif** : `chantier_app`
- **Hôte** : `127.0.0.1`
- **Port** : `5432`

La base et l'utilisateur doivent déjà exister dans PostgreSQL, avec les permissions nécessaires. Le serveur PostgreSQL doit être démarré.

### Enregistrer la chaîne de connexion

Dans un terminal **Zsh** sur macOS :

```bash
cd back/ChantierApi

# À faire uniquement si le projet ne possède pas encore de UserSecretsId
dotnet user-secrets init

# Saisie du mot de passe sans l'afficher dans le terminal
read -s "PG_PASS?Mot de passe PostgreSQL : "
echo

# Enregistrement local de la chaîne de connexion
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=127.0.0.1;Port=5432;Database=chantier_manager;Username=chantier_app;Password=$PG_PASS"

unset PG_PASS
cd ../..
```

**Important :** les *User Secrets* ne sont pas enregistrés dans Git. Chaque personne qui clone le projet doit configurer sa propre chaîne de connexion. Ne jamais committer un mot de passe.

> Si `UserSecretsId` existe déjà dans `back/ChantierApi/ChantierApi.csproj`, la commande `dotnet user-secrets init` est inutile. En environnement `Development`, ASP.NET Core charge automatiquement les User Secrets.

## Lancer le projet

Ouvrir **deux terminaux**, tous les deux depuis la racine du projet.

### Terminal 1 — Backend C#

```bash
dotnet run --project back/ChantierApi
```

L'API affiche son adresse dans le terminal, par exemple :

```text
Now listening on: http://localhost:5154
```

Ouvrir l'adresse affichée pour vérifier que l'API répond. Si la route de test est toujours présente, `http://localhost:5154/db-check` permet de vérifier la connexion à PostgreSQL (en adaptant le port à celui affiché).

### Terminal 2 — Frontend Angular

```bash
cd front
npm start
```

Ouvrir **http://localhost:4200** (ou l'adresse indiquée par Angular dans le terminal).

> Le frontend est encore en cours de développement : une page vide peut être normale à ce stade.

Pour arrêter chaque serveur : **Ctrl + C** dans son terminal.

## Vérifier la compilation

Depuis la racine :

```bash
dotnet build
```

Pour le frontend :

```bash
cd front
npm run build
```

## Structure du projet

```text
ThomasEtPironRecapFullStack/
├── back/
│   └── ChantierApi/
│       ├── Data/
│       │   └── AppDbContext.cs
│       ├── Program.cs
│       └── ChantierApi.csproj
├── front/
│   ├── src/
│   └── package.json
└── ThomasEtPiron.slnx
```

## Résumé des commandes du quotidien

Depuis la racine du projet :

| Action | Commande |
| --- | --- |
| Installer les dépendances C# | `dotnet restore` |
| Installer les dépendances Angular | `cd front && npm install` |
| Démarrer le backend | `dotnet run --project back/ChantierApi` |
| Démarrer le frontend | `cd front && npm start` |
| Compiler le backend | `dotnet build` |
| Compiler le frontend | `cd front && npm run build` |
