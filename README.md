# Comment installer l'API ?

## Clône le projet
- Ouvrir Microsoft Visual Studio 2022
- Cliquer sur **Cloner un dépot**
- Suivre les instructions

## Installer les dépendances
Vérifier que vous avez bien d'installé les packages de la solution dans **Outils > Gestionnaire de package NuGet > Gérer les packages NuGet pour la solution**

## Lancer Docker Desktop
Le projet est sur Docker pour maintenir les mêmes versions entre chaque PC. De ce fait, vous devez toujours avoir Docker Desktop d'ouvert pour lancer la solution.
Cliquez sur le bouton avec la flèche verte "Docker"

## Structure de l'API
- Controllers
- Models
- Services


## Commande pour réaliser les migrations 
1) Faire la migration
 dotnet ef migrations add InitialCreate
2) Migrer la migration
dotnet ef database update