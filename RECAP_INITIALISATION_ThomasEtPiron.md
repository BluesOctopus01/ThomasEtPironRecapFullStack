# ThomasEtPiron — Récapitulatif d'initialisation Full Stack

> **Objectif :** documenter, de A à Z, l'initialisation d'un projet **Angular (frontend)** + **C# / ASP.NET Core (backend)**, son exécution en local et son versionnement avec Git / GitHub.  
> **Environnement utilisé :** macOS, terminal Zsh, VS Code.  
> **État du projet :** les deux projets sont créés, fonctionnent séparément et le dépôt Git est publié. Aucune communication front/back ni base de données n'a encore été implémentée dans ce récapitulatif.

**Dépôt GitHub :** https://github.com/BluesOctopus01/ThomasEtPironRecapFullStack

---

## 1. Prérequis

Vérifier les outils disponibles dans le terminal :

```bash
dotnet --version
node --version
npm --version
git --version
```

Pour Angular, nous avons utilisé **Angular CLI** :

```bash
npm install -g @angular/cli
ng version
```

- `dotnet` : outil en ligne de commande du SDK .NET.
- `node` : environnement JavaScript utilisé par les outils Angular.
- `npm` : gestionnaire des dépendances JavaScript.
- `git` : outil de versionnement local.
- `ng` : commande fournie par Angular CLI.

**Versions observées dans les fichiers du projet :** backend ciblant **.NET 10** (`net10.0`), dépendances Angular **22.2.x** et npm déclaré en **11.19.1**. La version exacte de Node.js et celle du SDK .NET installé ne sont pas reproduites ici : les vérifier avec les commandes ci-dessus.

## 2. Les bases du terminal

Commandes utilisées pour éviter de créer des fichiers au mauvais endroit :

| Commande | Rôle |
| --- | --- |
| `pwd` | Afficher le chemin du dossier courant |
| `ls` | Lister les éléments du dossier courant |
| `ls -la` | Lister les éléments avec détails et fichiers cachés |
| `cd dossier` | Entrer dans un sous-dossier |
| `cd ..` | Remonter au dossier parent |
| `cd ~` | Retourner au dossier personnel |
| `mkdir dossier` | Créer un dossier |
| `cat fichier` | Afficher le contenu d'un fichier |
| `touch fichier` | Créer un fichier vide s'il n'existe pas |
| `code .` | Ouvrir le dossier courant dans VS Code (si la commande est installée) |
| `Ctrl + C` | Interrompre un serveur lancé dans le terminal |

**Règle essentielle :** les chemins relatifs sont interprétés **depuis le répertoire courant du terminal**, qui peut différer du dossier affiché par VS Code. Avant une commande de création : `pwd`, puis `ls`.

## 3. Création du répertoire principal

Notre projet a été créé dans le répertoire personnel de projets :

```bash
cd ~/Dev/Projets/perso
pwd
mkdir ThomasEtPiron
cd ThomasEtPiron
ls -la
code .
```

Le `~` représente le dossier personnel sur macOS (dans notre cas `/Users/metensthomas`).

À partir de maintenant, sauf indication contraire, les commandes d'initialisation sont exécutées **à la racine de `ThomasEtPiron`**.

## 4. Initialisation du backend ASP.NET Core

### 4.1. Créer une solution .NET

```bash
dotnet new sln -n ThomasEtPiron
```

- `dotnet new` : crée un élément à partir d'un modèle.
- `sln` : modèle de **solution .NET**, qui sert à regrouper plusieurs projets .NET.
- `-n ThomasEtPiron` : nom de la solution.

Avec le SDK utilisé, un fichier **`ThomasEtPiron.slnx`** a été généré.

Une **solution** regroupe les projets ; elle ne contient pas directement leur code métier.

### 4.2. Créer le projet backend

Toujours **à la racine `ThomasEtPiron`** :

```bash
mkdir back
dotnet new web -n ChantierApi -o back/ChantierApi
```

- `web` : modèle **ASP.NET Core vide**, adapté à une API minimale.
- `-n ChantierApi` : nom du projet C#.
- `-o back/ChantierApi` : dossier de sortie (chemin relatif au dossier courant).

**Erreur rencontrée et corrigée :** lancer cette commande depuis `back/` avec `-o back/ChantierApi` crée par erreur `back/back/ChantierApi`. Toujours vérifier `pwd` avant de créer un projet.

### 4.3. Ajouter le projet à la solution et compiler

```bash
dotnet sln add back/ChantierApi/ChantierApi.csproj
dotnet sln list
dotnet build
```

- `dotnet sln add` : référence le projet C# dans la solution.
- `dotnet sln list` : affiche les projets référencés.
- `dotnet build` : restaure les dépendances nécessaires et compile les projets.

Une compilation réussie confirme que le squelette du backend est valide.

### 4.4. Fichiers backend importants

**`back/ChantierApi/ChantierApi.csproj`** :

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

| Élément | Signification |
| --- | --- |
| `Microsoft.NET.Sdk.Web` | SDK adapté aux projets ASP.NET Core |
| `TargetFramework` | Version du framework .NET ciblée |
| `Nullable` | Active l'analyse des références pouvant être nulles |
| `ImplicitUsings` | Ajoute automatiquement certains imports `using` |

**`back/ChantierApi/Program.cs`** initial :

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
```

- `CreateBuilder(args)` : prépare configuration, services, journalisation et hébergement.
- `builder.Build()` : construit **l'objet application** ; ne pas confondre avec `dotnet build` qui compile le projet.
- `MapGet("/", ...)` : expose une route HTTP `GET /`.
- `Run()` : démarre le serveur web.

Autres éléments : `appsettings.json` pour la configuration, `Properties/` pour les profils de lancement, `bin/` et `obj/` pour les fichiers générés par la compilation.

## 5. Initialisation du frontend Angular

### 5.1. Créer l'application

**Depuis la racine `ThomasEtPiron`** :

```bash
ng new front
```

Cette commande :

1. Crée le dossier `front/`.
2. Génère les fichiers de l'application Angular.
3. Installe les dépendances npm.
4. Initialise normalement un dépôt Git dans `front/` (nous l'avons ensuite remplacé par un dépôt commun à la racine).

Les questions interactives (styles, routage, SSR, etc.) dépendent de la version de la CLI et des options choisies. Les réponses exactes à ces questions n'ont pas été conservées dans ce récapitulatif.

### 5.2. Fichiers frontend importants

| Fichier / dossier | Rôle |
| --- | --- |
| `front/package.json` | Métadonnées du projet npm, scripts et dépendances |
| `front/package-lock.json` | Versions résolues des dépendances : **à versionner** |
| `front/node_modules/` | Dépendances installées localement : **à ignorer par Git** |
| `front/angular.json` | Configuration de l'outil Angular CLI |
| `front/tsconfig.json` | Configuration TypeScript |
| `front/src/index.html` | Page HTML qui héberge la balise `<app-root>` |
| `front/src/main.ts` | Point d'entrée Angular |
| `front/src/app/app.ts` | Classe du composant racine et décorateur `@Component` |
| `front/src/app/app.html` | Template HTML du composant racine |
| `front/src/app/app.css` | Styles du composant racine |
| `front/src/app/app.config.ts` | Configuration des fournisseurs globaux |
| `front/src/app/app.routes.ts` | Routes de navigation Angular |

**`front/src/main.ts`** généré :

```typescript
import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
```

`bootstrapApplication(App, appConfig)` démarre Angular avec le composant racine `App` et sa configuration.

Dans `index.html`, l'élément `<app-root></app-root>` correspond au `selector: 'app-root'` déclaré dans `app.ts`.

`app.routes.ts` contient initialement :

```typescript
import { Routes } from '@angular/router';
export const routes: Routes = [];
```

Nous avons volontairement vidé `app.html` : une page blanche est donc normale tant que nous ne lui ajoutons pas de contenu.

### 5.3. Les commandes Angular

```bash
cd front
npm start
# Équivalent au script "start": "ng serve" dans package.json
```

Autres commandes utiles, **depuis `front/`** :

| Commande | Action |
| --- | --- |
| `ng serve --open` | Lance le serveur de développement et ouvre le navigateur |
| `ng g c chantier` | Génère un composant `chantier` |
| `ng g s services/auth` | Génère un service Angular |
| `npm run build` | Construit l'application Angular |
| `npm test` | Lance les tests configurés |
| `npm install` | Installe les dépendances du projet |
| `npm ci` | Réinstalle les versions définies par `package-lock.json` |

**Erreur rencontrée :** `This command is not available when running the Angular CLI outside a workspace.` Elle apparaît notamment lorsqu'on exécute `ng g c ...` hors du dossier contenant `angular.json`. Solution : `cd front` puis relancer la commande.

Un terminal occupé par `ng serve` doit rester ouvert ; utiliser un second terminal pour d'autres commandes.

## 6. Arborescence retenue

```text
ThomasEtPiron/
├── .git/                         # Dépôt Git commun (créé ensuite)
├── .gitignore                    # Règles d'exclusion communes
├── ThomasEtPiron.slnx           # Solution .NET
├── back/
│   └── ChantierApi/
│       ├── Program.cs
│       ├── ChantierApi.csproj
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       ├── Properties/
│       ├── bin/                  # Généré, ignoré
│       └── obj/                  # Généré, ignoré
└── front/
    ├── src/
    │   ├── main.ts
    │   ├── index.html
    │   └── app/
    │       ├── app.ts
    │       ├── app.html
    │       ├── app.css
    │       ├── app.config.ts
    │       └── app.routes.ts
    ├── angular.json
    ├── package.json
    ├── package-lock.json
    └── node_modules/             # Généré, ignoré
```

## 7. Git : un seul dépôt pour le front et le back

Lors de `ng new front`, Angular avait créé `front/.git`. Nous avons décidé de garder **un seul dépôt Git à la racine** pour versionner toute l'application.

### 7.1. Sauvegarder l'ancien dépôt Angular

Depuis `ThomasEtPiron/` :

```bash
mv front/.git ../ThomasEtPiron-front-git-backup
```

Cela déplace **les métadonnées Git uniquement**, pas le code Angular. La sauvegarde est en dehors du projet.

### 7.2. Initialiser Git à la racine

```bash
git init -b main
```

La racine contient désormais un seul dossier `.git/`. La branche initiale s'appelle `main`.

### 7.3. Le `.gitignore` commun

Un fichier `.gitignore` a été créé à la racine pour exclure les fichiers locaux et générés. Exemple de règles adaptées au projet :

```gitignore
# macOS
.DS_Store

# .NET
**/bin/
**/obj/
.vs/

# Angular / Node
**/node_modules/
**/dist/
**/.angular/

# Secrets locaux
.env
.env.*
!.env.example

# Bases SQLite locales
*.db
*.db-shm
*.db-wal
```

Le fichier `front/.gitignore` généré par Angular peut rester en place : Git cumule les règles des différents `.gitignore`.

**Important :** ne jamais committer de mots de passe, secrets, tokens ou bases locales contenant des données privées. Vérifier aussi les fichiers de configuration avant de les publier.

### 7.4. Premier commit

```bash
git status
git add .
git diff --cached --stat
git commit -m "Initialisation du projet Full Stack Angular et ASP.NET Core"
```

| Commande | Ce qu'elle fait |
| --- | --- |
| `git status` | Affiche les changements en attente |
| `git add .` | Place les changements dans la zone de préparation (*staging*) |
| `git diff --cached --stat` | Résume ce qui va être inclus dans le commit |
| `git commit` | Crée un point dans l'historique **local** |
| `git push` | Envoie les commits au dépôt distant configuré |

### 7.5. Publier sur GitHub

Un dépôt GitHub a été créé :

https://github.com/BluesOctopus01/ThomasEtPironRecapFullStack

Les commandes typiques pour associer le dépôt distant et envoyer `main` sont :

```bash
git remote add origin https://github.com/BluesOctopus01/ThomasEtPironRecapFullStack.git
git push -u origin main
```

**Remarque :** la présence du dépôt GitHub a été signalée dans la conversation. Les commandes exactes de sa première publication n'ont pas été montrées. Si `origin` existe déjà, **ne pas relancer `git remote add origin`** ; vérifier avec :

```bash
git remote -v
```

Pour les prochains changements :

```bash
git status
git add .
git commit -m "Description des modifications"
git push
```

## 8. Démarrer les deux applications localement

Depuis **la racine `ThomasEtPiron`**, ouvrir **deux terminaux**.

**Terminal 1 — Backend :**

```bash
dotnet run --project back/ChantierApi --urls http://localhost:5080
```

Le template initial expose `GET /` et renvoie `Hello World!`. Tester dans le navigateur :

http://localhost:5080/

**Terminal 2 — Frontend :**

```bash
cd front
npm start
# ou : ng serve --open
```

Ouvrir :

http://localhost:4200/

Le frontend et le backend démarrent **séparément** ; ils ne communiquent pas encore entre eux. Ce raccordement viendra lors de l'ajout d'un service Angular et d'appels HTTP à l'API ASP.NET Core.

Pour arrêter chaque serveur : **`Ctrl + C`** dans son terminal.

## 9. Résumé des différences avec Python

| Concept | Python | C# / .NET | Angular |
| --- | --- | --- | --- |
| Langage | Python | C# | TypeScript |
| Framework web | FastAPI / Flask | ASP.NET Core | Angular (frontend) |
| Dépendances | pip / uv | NuGet | npm |
| Configuration principale | `pyproject.toml` | `.csproj` | `package.json` |
| Environnement virtuel | Souvent `.venv/` | Généralement pas de `venv` | `node_modules/` pour les packages npm |
| Point d'entrée | Selon l'application | `Program.cs` | `main.ts` |
| Lancement | p. ex. Uvicorn | `dotnet run` | `ng serve` / `npm start` |

**À retenir :** `node_modules/` est un dossier de packages, **pas** l'équivalent exact d'un environnement virtuel Python. Les dépendances .NET sont déclarées dans les projets et résolues par NuGet.

## 10. Bilan et prochaines étapes

**Ce qui est terminé :**

- [x] Création d'un dossier de projet propre.
- [x] Création d'une solution .NET et du backend ASP.NET Core.
- [x] Ajout du projet C# à la solution et compilation.
- [x] Création du frontend Angular.
- [x] Repérage des fichiers de démarrage et de configuration.
- [x] Démarrage indépendant des deux applications.
- [x] Mise en place d'un dépôt Git unique, premier commit et publication GitHub.

**À développer ensuite (pas encore réalisé ici) :**

- [ ] Première route API métier en C#.
- [ ] Premier appel HTTP Angular → ASP.NET Core.
- [ ] Base SQLite avec Entity Framework Core.
- [ ] Authentification avec ASP.NET Core Identity.
- [ ] Formulaire et pages Angular correspondants.
- [ ] Tests, documentation et améliorations.

---

*Ce document décrit les étapes réalisées et les points compris pendant l'initialisation. Les exemples futurs d'authentification et de base de données ne sont pas inclus comme s'ils étaient déjà implémentés.*
