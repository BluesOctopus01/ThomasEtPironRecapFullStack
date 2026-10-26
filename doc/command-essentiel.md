# Commandes essentielles — Full Stack

À exécuter depuis la racine `ThomasEtPiron/`, sauf indication contraire.

## Back — C# / ASP.NET Core

```bash
dotnet --version

# Création du projet (une seule fois)
dotnet new sln -n ThomasEtPiron
dotnet new web -n ChantierApi -o back/ChantierApi
dotnet sln add back/ChantierApi/ChantierApi.csproj

# Commandes courantes
dotnet restore
dotnet build
dotnet run --project back/ChantierApi --urls http://localhost:5080
```

## PostgreSQL — User Secrets

Les **User Secrets** permettent de conserver les mots de passe et chaînes de connexion localement, sans les publier sur GitHub.

### 1. Se placer dans le backend

```bash
cd back/ChantierApi
```

### 2. Initialiser les secrets (une seule fois)

```bash
dotnet user-secrets init
```

Cette commande ajoute un identifiant `UserSecretsId` dans le fichier `.csproj`.

Les secrets eux-mêmes sont conservés ailleurs, sur ton Mac.

### 3. Enregistrer la chaîne de connexion PostgreSQL

**D'abord, saisir le mot de passe sans l'afficher (terminal Zsh) :**

```bash
read -s "PG_PASS?Mot de passe PostgreSQL : "
echo
```

`PG_PASS` est une variable temporaire contenant le mot de passe de l'utilisateur PostgreSQL `chantier_app`.

**Ensuite, enregistrer la chaîne de connexion :**

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=127.0.0.1;Port=5432;Database=chantier_manager;Username=chantier_app;Password=$PG_PASS"
```

**Enfin, supprimer la variable temporaire :**

```bash
unset PG_PASS
```

### 4. Vérifier les secrets enregistrés

```bash
dotnet user-secrets list
```

**Attention :** cette commande peut afficher le mot de passe en clair. Ne partage pas sa sortie sans masquer le mot de passe.

### 5. Modifier le mot de passe

Si le mot de passe PostgreSQL change, répéter les commandes de l'étape 3. La commande `set` remplace l'ancienne valeur.

### 6. Utiliser la chaîne de connexion en C#

Dans `Program.cs` :

```csharp
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");
```

`DefaultConnection` doit correspondre au nom enregistré avec `dotnet user-secrets set`.

**À retenir :**

- `dotnet user-secrets init` : une seule fois par projet.
- `dotnet user-secrets set` : pour enregistrer ou modifier un secret.
- `dotnet user-secrets list` : pour vérifier les secrets.
- `GetConnectionString()` : pour récupérer la chaîne dans le code C#.
- Les secrets sont stockés localement, hors du dépôt Git.
- ASP.NET Core les charge automatiquement en environnement `Development`.

## Front — Angular

```bash
npm install -g @angular/cli   # Installer Angular CLI (une fois)
ng new front                  # Depuis la racine
```

```bash
cd front
npm install
npm start                     # ng serve
ng g c chantier               # Générer un composant
ng g s services/auth          # Générer un service
npm run build
cd ..
```

## Git

```bash
git init -b main
git status
git add .
git commit -m "message"
git branch
git switch -c develop
git switch main
git switch develop
git remote -v
git remote add origin URL_DU_DEPOT
git push -u origin main       # Première publication de main
git push -u origin develop    # Première publication de develop
git push                      # Publications suivantes
git pull                      # Récupérer les changements
git log --oneline -5
```

Remplacer `URL_DU_DEPOT` par l'adresse de ton dépôt GitHub.

---

**Important :**

- `ng new` et `dotnet new` servent à **créer** les projets (une seule fois).
- `dotnet user-secrets init` sert à **initialiser** le stockage local des secrets (une seule fois).
- Pour travailler au quotidien, utiliser `dotnet run` et `npm start` dans deux terminaux.
- Ne jamais écrire un vrai mot de passe dans un fichier suivi par Git.
