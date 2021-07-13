using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using SudoEngine.Core;
using SudoEngine.Maths;

namespace SudoEngine.Render
{
    /// <summary>
    /// Classe permettant de gérer des Textures, fourni un ensemble de méthodes et de propriétés qui facilitent la création et l'utilisation
    /// <para>Hérite de <see cref="BaseObject"/> et ne peut pas être héritée</para>
    /// </summary>
    public sealed class Texture : BaseObject
    {
        /// <summary>Liste de toutes les <see cref="Texture"/> actuellement chargées en mémoire</summary>
        public static List<Texture> AllTextures { get; set; } = new List<Texture>();

        /// <summary>Handle de la texture (nécessaire au fonctionnement d'OpenGL)</summary>
        public int Handle { get; private set; }
        /// <summary>Taille de la texture, en pixels</summary>
        public Vector2D Size { get; set; }
        /// <summary>Longueur de la texture en pixels</summary>
        public int Width
        {
            get => (int)Size.X;
            set => Size = new Vector2D(value, Height);
        }
        /// <summary>Hauteur de la texture en pixels</summary>
        public int Height
        {
            get => (int)Size.Y;
            set => Size = new Vector2D(Width, value);
        }
        /// <summary>Les données brutes des pixels de la texture</summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Crée un nouvel objet <see cref="Texture"/> et appele le constructeur de <see cref="BaseObject"/>
        /// </summary>
        /// <param name="name">Le nom interne de l'objet (Texture par défaut)</param>
        public Texture(string name = "Texture") : base(name) => AllTextures.Add(this);

        /// <summary>Supprime la texture</summary>
        public override void Delete()
        {
            AllTextures.Remove(this);
            GL.DeleteTexture(Handle);
            base.Delete();
        }
        /// <summary>Supprime toutes les <see cref="Texture"/></summary>
        public static void DeleteAll() { for (int i = 0; i < AllTextures.Count; i++) if (AllTextures[i]) AllTextures[i].Delete(); }

        /// <summary>Bind la texture</summary>
        public void Bind() => GL.BindTexture(TextureTarget.Texture2D, Handle);

        /// <summary>
        /// Crée une texture à partir d'un fichier (préférablement un PNG)
        /// </summary>
        /// <param name="path">Le chemin vers le fichier de la texture (ajoute "Texture/" devant par défaut)</param>
        public void LoadFromFile(string path)
        {
            if (!File.Exists("Textures/" + path))
            {
                Log.Error($"Le fichier pour la texture n'existe pas : {path}");
                path = "Default.png";
            }

            Bitmap image = new Bitmap("Textures/" + path);
            image.RotateFlip(RotateFlipType.Rotate180FlipX);
            List<byte> pixels = new List<byte>(4 * image.Width * image.Height);

            for (int x = 0; x < image.Height; x++)
            {
                for (int y = 0; y < image.Width; y++)
                {
                    pixels.Add(image.GetPixel(y, x).R);
                    pixels.Add(image.GetPixel(y, x).G);
                    pixels.Add(image.GetPixel(y, x).B);
                    pixels.Add(image.GetPixel(y, x).A);
                }
            }

            Size = new Vector2D(image.Width, image.Height);
            Data = pixels.ToArray();
            Generate();
        }

        /// <summary>
        /// Crée une texture à partir d'un <see cref="Bitmap"/>
        /// </summary>
        /// <param name="image">L'image à convertir en texture</param>
        /// <param name="reverse"><see cref="bool"/> indiquant si l'image doit être flip verticalement</param>
        public void LoadFromBitmap(Bitmap image, bool reverse)
        {
            if (reverse) image.RotateFlip(RotateFlipType.Rotate180FlipX);
            List<byte> pixels = new List<byte>(4 * image.Width * image.Height);

            for (int x = 0; x < image.Height; x++)
            {
                for (int y = 0; y < image.Width; y++)
                {
                    pixels.Add(image.GetPixel(y, x).R);
                    pixels.Add(image.GetPixel(y, x).G);
                    pixels.Add(image.GetPixel(y, x).B);
                    pixels.Add(image.GetPixel(y, x).A);
                }
            }

            Size = new Vector2D(image.Width, image.Height);
            Data = pixels.ToArray();
            Generate();
        }

        /// <summary>
        /// Génère une texture directement à partir de données brutes de pixels
        /// </summary>
        /// <param name="data">Array de <see cref="byte"/> contenant les données RGBA de chaque pixel</param>
        public void Generate(byte[] data)
        {
            Data = data;
            Generate();
        }

        void Generate()
        {
            Handle = GL.GenTexture();
            Bind();

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
        }
    }
}
