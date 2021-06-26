# SudoEngine
Moteur de jeu 2D en C# avec OpenTK (OpenGL et OpenAL)


## Comment utiliser le .DLL :

1 : Télécharger le .DLL le plus récent dans https://github.com/YohannDR/SudoEngine/releases

2 : Créer un nouveau projet console C# en .NET FrameWork 4.8

3 : De préférence, placer le .DLL dans le dossier `bin/Debug` et/ou le dossier `bin/Release` du projet

4 : Dans l'explorateur de solutions, clic droit sur `Références` -> `Ajouter une référence`

5 : Choisir l'onglet `Parcourir` puis cliquer sur `Parcourir...`

6 : Sélectionner `SudoEngine.DLL` et cliquer sur Ajouter

7 : Le .DLL est désormais bind au projet, il ne reste plus qu'à accéder aux namespaces avec des `using SudoEngine.<>` et à utiliser les classes
