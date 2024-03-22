# Comment installer l'API ?

## Clône le projet
- Ouvrir Microsoft Visual Studio 2022
- Cliquer sur **Cloner un dépot**
- Suivre les instructions

## Installer les dépendances
Vérifier que vous avez bien d'installé les packages de la solution dans **Outils > Gestionnaire de package NuGet > Gérer les packages NuGet pour la solution**

## appsettings.json
Il faut ajouter le fichier ``appsettings.json`` à la racine du projet .NET
```json
{
  "ConnectionStrings": {
    "WebApiDatabase": "Host=host; Database=database; Username=username; Password=password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## .env
Il faut ajouter un .env dans la racine du projet .NET
```
PORT_BACK=8080:8080
PORT_DB=5432:5432
DB_NAME=dbName
DB_USER=dbUser
DB_PASSWORD=dbPassword 
```

## Lancer Docker Desktop
Le projet est sur Docker pour maintenir les mêmes versions entre chaque PC. De ce fait, vous devez toujours avoir Docker Desktop d'ouvert pour lancer la solution.
Cliquez sur le bouton avec la flèche verte "Docker"
Faite un ``docker compose up -d``
*Si vous êtes amené à recréer les containers, supprimer le dossier db_data*

## Structure de l'API
- Controllers
- Models
- Services


## Commande pour réaliser les migrations 
1) Installer le tool
``dotnet tool install --global dotnet-ef``
2) Faire la migration
``dotnet ef migrations add InitialCreate``
3) Migrer la migration
``dotnet ef database update``
