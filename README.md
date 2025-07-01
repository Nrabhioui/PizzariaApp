# PizzeriaApp

PizzeriaApp est une application de gestion de pizzeria développée en C# avec WPF (Windows Presentation Foundation) suivant le modèle MVVM.

## Fonctionnalités principales
- **Gestion des Pizzas** : Ajout, modification, suppression et affichage des pizzas (nom, prix, description, image).
- **Gestion des Clients** : Ajout, modification, suppression et affichage des clients (nom, adresse, téléphone).
- **Gestion des Commandes** : Création et gestion des commandes associées à des clients, ajout/suppression de pizzas dans une commande.
- **Exportation XML** : Possibilité d’exporter une pizza au format XML dans le dossier Documents\PizzeriaApp\Saves.
- **Sauvegarde automatique** : Les données sont sauvegardées en JSON dans le dossier Documents\PizzeriaApp.
- **Paramètres utilisateur** : Personnalisation du dossier de données, du thème et de la langue via une fenêtre dédiée.

## Structure du projet
- `PizzeriaApp.Core` : Définit les classes de données principales (`Pizza`, `Client`, `Commande`).
- `PizzeriaApp.Services` : Gère la sérialisation/désérialisation JSON et XML, ainsi que les paramètres utilisateur.
- `PizzeriaApp` : Projet principal WPF contenant les vues (XAML) et les ViewModels.

## Architecture
- **MVVM** : Séparation claire entre la logique métier (ViewModels), les données (Models) et l’interface utilisateur (Views).
- **INotifyPropertyChanged** : Toutes les classes de données implémentent cette interface pour la mise à jour dynamique de l’UI.
- **RelayCommand** : Gestion des commandes et des interactions utilisateur.

## Utilisation
1. **Lancer l’application**
2. Gérer les pizzas, clients et commandes via les différentes fenêtres.
3. Utiliser le bouton d’exportation pour générer un fichier XML d’une pizza.
4. Modifier les paramètres via la fenêtre dédiée.

## Prérequis
- .NET 6 ou .NET 8 (framework cible du projet)
- Windows 10 ou supérieur

## Dossiers importants
- `Documents\PizzeriaApp` : Contient toutes les données sauvegardées (JSON, XML)
- `Saves` : Sous-dossier pour les exports XML

## Auteurs
- Projet réalisé par Nassim
