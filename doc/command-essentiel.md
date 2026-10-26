# Commandes essentielles — Full Stack

À exécuter depuis la racine `ThomasEtPiron/`, sauf indication contraire.

## Back — C# / ASP.NET Core

```bash
dotnet --version
dotnet new sln -n ThomasEtPiron
dotnet new web -n ChantierApi -o back/ChantierApi
dotnet sln add back/ChantierApi/ChantierApi.csproj
dotnet restore
dotnet build
dotnet run --project back/ChantierApi --urls http://localhost:5080
```

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
git remote add origin <URL_DU_DEPOT>
git push -u origin main       # Première publication de main
git push -u origin develop    # Première publication de develop
git push                      # Publications suivantes
git pull                      # Récupérer les changements
git log --oneline -5
```

> `ng new` et `dotnet new` servent à **créer** les projets (une seule fois). Pour travailler au quotidien : `dotnet run` et `npm start` dans deux terminaux.
