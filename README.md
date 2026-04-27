# Puzzle VR

**Auteur :** Léandre Kanmegne & Bady Pascal
**Cours :** Environnements Immersifs — Cégep de Victoriaville  
**Session :** Hiver 2026

## Description

Jeu de puzzle en réalité virtuelle pour Meta Quest 2 développé avec Unity 6.3. Le joueur doit assembler une maquette en récupérant des pièces dispersées dans l'environnement et en les plaçant dans les bons emplacements. Chaque pièce a une forme distincte (cube, cylindre, sphère) qui correspond à un socket spécifique sur la maquette.

Le jeu fonctionne en mode endurance : à chaque tour complété, le joueur gagne du temps bonus pour continuer. La partie se termine lorsque le timer atteint zéro. Le score correspond au nombre de tours complétés.

---

## Versions et packages

| Outil                     | Version                         |
| ------------------------- | ------------------------------- |
| Unity                     | 6000.3.5f1                      |
| XR Interaction Toolkit    | 3.3.1                           |
| XR Plugin Management      | 4.5.4                           |
| XR Core Utilities         | 2.5.3                           |
| Input System              | 1.17.0                          |
| Universal Render Pipeline | 17.3.0                          |
| Plateforme cible          | Meta Quest 2 (Android, API 29+) |
| Architecture              | ARM64, IL2CPP                   |

**Asset Store**

- Free Pop Sound Effects Pack 1.0 — sons d'interaction (apparition des cibles, impact)

## Fonctionnalités implémentées

- Grab cinématique des pièces via XR Interaction Toolkit
- Détection automatique du placement correct via XR Socket Interactor
- Filtrage par Interaction Layers (chaque socket n'accepte que sa forme)
- Spawn aléatoire des pièces à différents points à chaque tour
- Boucle de jeu en mode endurance : menu → tours successifs → fin → rejouer
- Système de bonus de temps à chaque tour complété
- Score basé sur le nombre de tours complétés
- Visuels fantômes sur la maquette pour guider le placement
- UI en World Space (aucun Canvas en Screen Space)
- Hiérarchie de feedback haptique : grab d'une pièce, tour complété, fin de partie
- Sons spatiaux au grab et au placement des pièces

---

## Structure des scripts

| Script                  | Responsabilité                                                       |
| ----------------------- | -------------------------------------------------------------------- |
| `GestionnaireJeu.cs`    | États de jeu, timer, score, gestion des panneaux UI, logique de tour |
| `GestionnaireSpawn.cs`  | Création des pièces aux points de spawn aléatoires                   |
| `GestionnairePuzzle.cs` | Détection du placement correct via les événements des sockets        |
| `FeedbackPiece.cs`      | Retour haptique et audio lors du grab d'une pièce                    |
| `VisuelSocket.cs`       | Gère l'affichage des fantômes guides sur la maquette                 |

La communication entre `GestionnairePuzzle` et `GestionnaireJeu` se fait via un singleton (`GestionnaireJeu.instance`). La détection du placement utilise les événements `selectEntered` du XR Interaction Toolkit, ce qui évite les vérifications manuelles de tag — la validation de la bonne forme est entièrement déléguée aux Interaction Layer Masks configurés sur chaque socket.

---

## Défis rencontrés et solutions

**UI en World Space** — La configuration du Canvas en mode World Space avec le Tracked Device Graphic Raycaster et le XR UI Input Module sur l'EventSystem n'était pas évidente. La solution a été de remplacer le Graphic Raycaster standard par le Tracked Device Graphic Raycaster et de substituer le Standalone Input Module par le XR UI Input Module pour permettre l'interaction via les contrôleurs.

**Interaction XR Toolkit** — Comprendre la distinction entre le Near-Far Interactor (géré automatiquement par le toolkit) et les raycast manuels via Physics.Raycast a demandé de l'exploration. Dans ce projet, le grab du marteau passe entièrement par le toolkit tandis que la détection des coups repose sur la collision physique directe.

**Déploiement sur casque** — Le premier build IL2CPP pour Android ARM64 prend considérablement de temps. L'utilisation du cache entre les builds suivants et la validation systématique sur le casque avant la remise finale ont permis d'éviter les surprises de dernière minute.

**XRbaseController - Obsolete** - L'utilisation du component XRbaseController renvoie un warning CS0618: 'XRBaseController' is obsolete: 'XRBaseController has been deprecated dans la version 3.0.0 du XR interaction Toolkit. Elle a été officiellement remplacé par XRBaseInputInteractor
Source : https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.0/manual/upgrade-guide-3.0.html

---

## Utilisation de l'IA

L'IA (Claude, Anthropic) a été utilisée comme outil d'apprentissage et d'assistance au développement, conformément au niveau d'utilisation autorisé dans le cadre du cours. Concrètement :

- Aide à la compréhension des concepts théoriques (raycasting, quaternions, XR Toolkit, events C#)
- Suggestions de structure de code et révision des scripts
- Débogage et identification d'erreurs de compilation
- Guidance sur la configuration Unity (Canvas World Space, XR UI Input Module)

Toutes les décisions d'implémentation, la configuration de la scène Unity et la compréhension du code sont de ma responsabilité.

---

## Sources

- Cégep de Victoriaville. _Travail pratique — Whack-a-Mole VR_. Environnements Immersifs, 2026.
- Cégep de Victoriaville. _Exercice 4 — Tri spatial VR : Grab & Socket_. Environnements Immersifs, 2026.
- Cégep de Victoriaville. _Exercice 4.1 — Feedback VR : haptiques et audio spatial_. Environnements Immersifs, 2026.
- Cégep de Victoriaville. _Exercice 4.2 — UI VR et GameManager_. Environnements Immersifs, 2026.
- Cégep de Victoriaville. _Configuration VR dans Unity_. Environnements Immersifs, 2026.
- Unity Technologies. _XR Interaction Toolkit Documentation_. docs.unity3d.com, 2024.
