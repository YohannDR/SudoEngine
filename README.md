# SudoEngine
Moteur de jeu 2D en C# avec OpenTK (OpenGL et OpenAL)


## Comment utiliser le moteur :

1 : Télécharger la release la plus récente dans https://github.com/YohannDR/SudoEngine/releases (le .7z ou les 4fichiers indépendamment) ou compiler le projet en Release, en bibliothèque de classes

2 : Créer un nouveau projet console C# en .NET FrameWork 4.8

3 : De préférence, placer les fichiers dans le dossier `bin/Debug` et/ou le dossier `bin/Release` du projet

4 : Dans l'explorateur de solutions, clic droit sur `Références` -> `Ajouter une référence`

5 : Choisir l'onglet `Parcourir` puis cliquer sur `Parcourir...`

6 : Sélectionner `SudoEngine.DLL` et cliquer sur Ajouter

7 : Le .DLL est désormais bind au projet, il faut maintenant installer le package NuGet `OpenTK` pour que le moteur puisse fonctionner : `Projet` -> `Gérer les packages NuGet` -> `Parcourir` -> `OpenTK 3.2.0` et installer

8 : Suivre les instructions disponibles sur cette page pour setup la fenêtre : https://opentk.net/learn/chapter1/1-creating-a-window.html et ajouter les lignes suivantes : `GameObject.Update();` dans la fonction OnUpdateFrame de la fenêtre et `GameObject.Render()` dans la fonction OnRenderFrame de la fenêtre


## Fonctionnalités actuelles

### Shader
- Gestion complète des shaders
- Fonctions pour set les variables uniform

### Texture
- Création d'une texture depuis une image ou une bitmap

### BackGround
- 5 layers différents pour les BackGrounds
- Génération avec une image ou un tileset
- (Suppression d'une tile)

### Audio
- Fonctions pour manipuler OpenAL faciliement
- Création d'un son depuis un .wav
- Posibilité de jouer et de mettre en pause un son

### Log
- 3 log différents : info (en bleu), warning (en jaune) et erreur (en rouge)
- Supporte n'importe quel type de variable
- Log pour les erreurs OpenGL, OpenAL et Alc les plus récentes
- Possibilité de démarrer et d'arrêter une Stopwatch (l'arrêter va automatiquement log le temps passé en Tick et en MilliSecondes)

### BaseObject
- Offre des fonctionnalités de base pour des classes

### GameObject
- Offre du scripting pour des objets
- (Offre la possibilité de créer une hiérarachie de GameObject)

### Maths
- Vecteur de taille 2, 3 et 4 disponibles
- Matrice 4\*4 disponible
- Quaternion disponnible (rotation 3D, probablement jamais utilisé mais à voir)

### Sprite
- Permet en héritant de cette classe, de facilement créer un sprite (comportement, animations...)
- Gestion automatique du Rendering
- Possibilité de faire des animations en changeant d'image sur le vol
- Possibilité de déplacer un sprite


## Fonctionnalités prévues

### Camera
- Une caméra

### Audio
- Possibilité de jouer des .MP3
- Meilleure implémentation d'OpenAL

### Collisions
- Système de collisions
