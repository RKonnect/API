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

## ROUTES

BASE_URL = ``https://localhost:7178/api``

### AUTHENTICATION

### Inscription
**POST** : `{{BASE_URL}}/Auth/register`
```
	"email": "string",
	"password": "string"
```

### Connexion
**POST** : `{{BASE_URL}}/Auth/login`
```
	"email": "string",
	"password": "string"
```

### USER

### Ajout allergie
**POST** : `{{BASE_URL}}/User/addAllergy`
- **id** => id de l'aliment dans la table Food
```
  "id": "string",
```

### Ajout aliment préféré
**POST** : `{{BASE_URL}}/User/addFavorite`
- **id** => id de l'aliment dans la table Food
```
  "id": "string",
```

### Modification
**PUT** : `{{BASE_URL}}/User/update`
- **Role** => 0: User ; 1: Professionnel
- **Gender** => 0: Femme ; 1: Homme ; 2: Autre
```
  "name": "string",
  "surname": "string",
  "pseudo": "string",
  "biography": "string",
  "avatar": "string",
  "gender": 0,
  "role": 0
```

### FOOD

### Ajout aliment
**POST** : `{{BASE_URL}}/Food/add`
```
  "name": "string"
```

### TAG

### Ajout tag
**POST** : `{{BASE_URL}}/Tag/add`
```
  "title": "string",
  "icon": "string"
```
