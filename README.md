# BDD Salle de Sport

Application console C# permettant de gérer une salle de sport connectée à une base de données MySQL.

Le programme propose actuellement deux espaces principaux dans le code :

- un espace administrateur pour gérer les membres, coachs, cours, inscriptions, spécialités et statistiques ;
- un espace membre pour consulter son profil, modifier ses informations et gérer ses réservations de cours.

La base peut également contenir des comptes fonctionnels pour les coachs et un compte de gérant de salle. Dans l'état actuel du code, le gérant correspond au profil administrateur principal si son compte est enregistré dans la table `Administrateur` avec le rôle attendu. Les comptes coachs doivent être prévus dans le script SQL ou le jeu de données, mais aucun menu de connexion coach séparé n'est encore routé dans `Program.cs`.

## Technologies

- C# / .NET Framework 4.7.2
- Application console Windows
- MySQL
- Connecteur `MySql.Data`
- Gestion des dépendances via `packages.config`
- Projet Visual Studio au format `.csproj`

## Structure du projet

```text
.
|-- BDD-Salle-de-Sport/
|   |-- App.config
|   |-- BDD-Salle-de-Sport.csproj
|   |-- Membre.cs
|   |-- Program.cs
|   |-- Properties/
|   |   `-- AssemblyInfo.cs
|   `-- packages.config
|-- .gitignore
`-- README.md
```

Les dossiers suivants peuvent exister en local, mais ne doivent pas être versionnés :

- `.vs/`
- `packages/`
- `BDD-Salle-de-Sport/bin/`
- `BDD-Salle-de-Sport/obj/`

## Fonctionnalités

### Connexion et inscription

- connexion en tant qu'administrateur, gérant ou membre ;
- présence possible de comptes coachs dans la base de données ;
- inscription d'un nouveau membre ;
- validation d'une inscription par un administrateur ;
- séparation entre administrateur principal, administrateur secondaire et membre.

### Espace administrateur

- gestion des membres : ajout, suppression, modification, recherche et liste ;
- gestion des coachs : ajout, suppression, modification, recherche et liste ;
- gestion des cours : ajout, suppression, modification et affichage ;
- gestion des inscriptions : création, suppression, modification et validation ;
- gestion des spécialités et association des spécialités aux coachs ;
- affichage de statistiques : nombre de membres, réservations, popularité des coachs, occupation des salles et cours sans réservation.

### Espace membre

- affichage du profil ;
- modification des informations personnelles ;
- consultation des cours disponibles ;
- inscription à un cours avec vérification des places restantes ;
- désinscription d'un cours ;
- consultation des prochains cours ;
- consultation de l'historique des cours.

## Prérequis

- Windows avec Visual Studio compatible .NET Framework.
- .NET Framework Developer Pack 4.7.2.
- MySQL Server disponible en local sur le port `3306`.
- NuGet ou la restauration NuGet intégrée à Visual Studio.

## Installation

1. Cloner le dépôt.

```powershell
git clone <url-du-depot>
cd BDD-Salle-de-Sport
```

2. Restaurer les dépendances NuGet si le dossier `packages/` n'est pas présent ou n'est pas à jour.

```powershell
nuget restore .\BDD-Salle-de-Sport\packages.config -PackagesDirectory .\packages
```

3. Ouvrir le projet `BDD-Salle-de-Sport/BDD-Salle-de-Sport.csproj` dans Visual Studio.

4. Compiler le projet en `Debug` ou `Release`.

En ligne de commande, si MSBuild est disponible :

```powershell
msbuild .\BDD-Salle-de-Sport\BDD-Salle-de-Sport.csproj /p:Configuration=Debug
```

5. Lancer l'application depuis Visual Studio ou depuis l'exécutable généré dans :

```text
BDD-Salle-de-Sport/bin/Debug/
```

## Configuration MySQL

Le code utilise actuellement des chaînes de connexion codées en dur dans `Program.cs`.

Base attendue :

```text
GestionSalleSport
```

Connexion initiale utilisée pour vérifier les identifiants applicatifs :

```text
server=localhost;user=root;database=GestionSalleSport;port=3306;password=root
```

Comptes MySQL ensuite utilisés par l'application :

| Rôle applicatif | Utilisateur MySQL | Mot de passe |
| --- | --- | --- |
| Administrateur principal | `admin_principal` | `MotDePasseFort1!` |
| Administrateur secondaire | `admin_app` | `MotDePasseApp2!` |
| Membre | `membre_client` | `Membre` |

Ces comptes MySQL sont des comptes techniques utilisés pour ouvrir les connexions à la base après authentification. Ils ne remplacent pas les comptes fonctionnels stockés dans les tables métier, par exemple les comptes administrateur/gérant, membre et coach.

Comptes fonctionnels attendus côté application :

| Profil | Stockage attendu | Remarque |
| --- | --- | --- |
| Gérant de salle | `Administrateur` | À traiter comme administrateur principal, avec `role = 'Principal'`. |
| Administrateur secondaire | `Administrateur` | Donne accès au menu administrateur secondaire. |
| Membre | `Membre` | Connexion avec `adresse_mail` et `mot_de_passe`; accès seulement si `admis = 1`. |
| Coach | À prévoir dans le schéma SQL ou le jeu de données | Des données coachs existent dans `Coach`; le code actuel gère les coachs côté admin mais ne route pas encore un espace coach dédié. |

Le dépôt ne contient pas encore de script SQL de création de base. La base doit donc être créée avant le lancement de l'application.

Tables utilisées par le code :

- `Administrateur`
- `Membre`
- `Coach`
- `Cours`
- `Salle`
- `Reservations`
- `Specialite`
- `Coach_Specialites`

Colonnes principales attendues par l'application :

- `Administrateur` : `login`, `password`, `role`
- `Membre` : `id_membre`, `nom`, `prenom`, `adresse`, `numero_tel`, `adresse_mail`, `mot_de_passe`, `date_inscription`, `admis`
- `Coach` : `id_coach`, `nom`, `prenom`, `numero_tel`, `adresse_mail`, `formation`
- `Cours` : `id_cours`, `nom`, `horaire`, `duree_minutes`, `niveau_difficulte`, `intensite`, `capacite_cours`, `id_coach`, `id_salle`
- `Salle` : `id_salle`, `nom`
- `Reservations` : `id_reservation`, `id_membre`, `id_cours`
- `Specialite` : `id_spe`, `nom`, `description`
- `Coach_Specialites` : `id_coach`, `id_spe`

## Utilisation

Au démarrage, l'application demande un login et un mot de passe.

- Si les identifiants correspondent à un administrateur, le programme ouvre le menu d'administration.
- Si les identifiants correspondent au gérant, il doit être reconnu comme administrateur principal.
- Si les identifiants correspondent à un membre validé, le programme ouvre le menu membre.
- Les comptes coachs sont utiles dans la base métier, mais le code actuel ne propose pas encore d'espace coach distinct au démarrage.
- Si le membre existe mais n'est pas encore admis, l'accès est refusé jusqu'à validation par un administrateur.
- Si aucun compte ne correspond, l'utilisateur peut réessayer, créer un compte ou quitter.

## Notes de sécurité

Ce projet est un projet pédagogique. Avant une utilisation réelle, plusieurs points doivent être améliorés :

- déplacer les identifiants de base de données hors du code source ;
- remplacer les requêtes SQL construites par concaténation par des requêtes paramétrées ;
- ne pas stocker les mots de passe en clair ;
- ajouter un script SQL versionné pour créer la base, les tables, les utilisateurs et les droits ;
- ajouter des tests automatisés sur les règles métier.

## Dépannage

### Impossible de se connecter à MySQL

Vérifier que :

- MySQL est lancé ;
- le serveur écoute sur `localhost:3306` ;
- la base `GestionSalleSport` existe ;
- les utilisateurs MySQL attendus existent ;
- les mots de passe correspondent à ceux utilisés dans `Program.cs`.

### Références NuGet introuvables

Restaurer les packages :

```powershell
nuget restore .\BDD-Salle-de-Sport\packages.config -PackagesDirectory .\packages
```

Puis recompiler le projet.

### Erreur sur les cours, coachs ou salles

Vérifier que les tables de référence contiennent déjà des données cohérentes, notamment :

- au moins un coach dans `Coach` ;
- au moins une salle dans `Salle` ;
- des clés étrangères valides entre `Cours`, `Coach` et `Salle`.
