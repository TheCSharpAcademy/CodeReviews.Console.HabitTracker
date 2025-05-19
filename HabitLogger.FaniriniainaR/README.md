# 🧠 HabitLogger

Une application console en C# conçue pour suivre et gérer ses habitudes quotIdiennes. Ce projet a été développé dans un objectif d'apprentissage du langage C#, de la programmation structurée et de l'utilisation de SQLite sans ORM.

---

## 🎯 Objectif du projet

Créer une application fonctionnelle capable de :
- Gérer des enregistrements d'habitudes (type, quantité, Date, commentaire)
- Permettre à l'utilisateur d'ajouter, modifier, supprimer et consulter ses habitudes
- Générer des statistiques simples à partir des données enregistrées
- Offrir une interface en console claire et sans plantage

---

## ⚙️ Fonctionnalités

- **Création automatique** de la base de données SQLite et des tables au démarrage
- **Interface textuelle** avec un menu principal pour naviguer entre les options
- **Opérations CRUD** complètes (Créer, Lire, Mettre à jour, Supprimer)
- **Filtrage par type ou Date** pour explorer les habitudes
- **Résumés statistiques** : total, moyenne, max, etc.
- **ValIdation des entrées** (Dates, quantités, types)
- **Gestion des erreurs** robuste pour éviter les blocages

---

## 🛠️ Technologies utilisées

- Langage : **C#**
- Base de données : **SQLite** (accès via `System.Data.SQLite`)
- Environnement : **Visual Studio 2022**
- Aucune bibliothèque externe utilisée pour l'affichage (tout est fait à la main)

---

## 🚧 Défis techniques

- Apprendre à manipuler **SQLite sans ORM** en C# avec des requêtes SQL pures
- Mettre en place un **menu réactif** et ergonomique en ligne de commande
- Structurer le code de façon modulaire en **séparant les responsabilités**
- Travailler avec le **formatage de Dates** de manière stricte, en utilisant `DateTime.TryParseExact()`
- Afficher les résultats en colonnes lisibles, **sans utiliser de bibliothèques tierces**
- Assurer la stabilité de l’application en gérant toutes les exceptions critiques

---

## 📈 Idées d'amélioration

- Ajouter une **exportation CSV** pour sauvegarder les habitudes hors ligne
- Utiliser **Spectre.Console** pour améliorer l’interface visuelle
- Ajouter un **rappel quotIdien** via console ou notification système
- Créer une **interface graphique simple** (WinForms ou MAUI)
- Ajouter un **système de comptes utilisateurs** pour différencier les données

---

## 🧪 Ce que j'ai appris

- Mettre en place une base de données SQLite manuellement en C#
- Structurer une application avec des **méthodes réutilisables** et une logique claire
- ValIder les saisies utilisateur pour éviter les comportements inattendus
- Utiliser des structures de données simples pour générer des **statistiques** utiles
- Appréhender l’importance de la **gestion d’erreurs** et de la culture (`CultureInfo`) dans le traitement des Dates

---

## 📚 Ressources

- Documentation Microsoft (`System.Data.SQLite`, `DateTime`, `CultureInfo`)
- Forums Stack Overflow pour les cas spécifiques SQLite et parsing
- Chaîne YouTube de Mosh Hamedani pour les bases C#
- Discussions avec des développeurs pour l'organisation du code

---

## 👤 Auteur

Développé par **RAKOTOMAHENINA FanirniainaR** dans le cadre d’un projet d’apprentissage approfondi du C# en console. Ce projet m’a permis de consolIder mes compétences en gestion de base de données, structuration du code et manipulation de données utilisateur.

---

