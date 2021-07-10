# SudoEngine
Moteur de jeu 2D en C# avec OpenTK (OpenGL et OpenAL)


## Comment utiliser le moteur :

1 : Télécharger le .DLL le plus récent dans https://github.com/YohannDR/SudoEngine/releases ou compiler le projet en tant que bibliothèque de classe

2 : Créer un nouveau projet console C# en .NET FrameWork 4.8

3 : De préférence, placer le .DLL dans le dossier `bin/Debug` et/ou le dossier `bin/Release` du projet

4 : Dans l'explorateur de solutions, clic droit sur `Références` -> `Ajouter une référence`

5 : Choisir l'onglet `Parcourir` puis cliquer sur `Parcourir...`

6 : Sélectionner `SudoEngine.DLL` et cliquer sur Ajouter

7 : Le .DLL est désormais bind au projet, il ne reste plus qu'à accéder aux namespaces avec des `using SudoEngine.<>` et à utiliser les classes


## Fonctionnalités actuelles

### Shader
- Gestion complète des shaders
- Fonction unique pour set une valeur uniform quelque soit le type de la variable

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
- Un log pour l'erreur OpenGL la plus récente

### BaseObject
- Offre des fonctionnalités de base pour des classes

### GameObject
- Offre du scripting pour des objets
- (Offre la possibilité de créer une hiérarachie de GameObject)

### Maths
- Vecteur de taille 2, 3 et 4 disponibles
- Matrice 4\*4 disponible
- Quaternion disponnible (rotation 3D, probablement jamais utilisé mais à voir)


## Fonctionnalités prévues

### Camera
- Une caméra

### Audio
- Possibilité de jouer des .MP3
- Meilleure implémentation d'OpenAL

### GameObject
- Plus de propriétés

### Sprite
- Gestion des sprites
