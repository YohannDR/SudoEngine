using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using SudoEngine.Core;
using SudoEngine.Maths;

namespace SudoEngine.Render
{
    public sealed class Texture : BaseObject
    {
        public int Handle { get; private set; }
        public Vector2D Size { get; set; }
        public int Width
        {
            get => (int)Size.X;
            set => Size = new Vector2D(value, Height);
        }

        public int Height
        {
            get => (int)Size.Y;
            set => Size = new Vector2D(Width, value);
        }

        public byte[] Data { get; set; }
        public Bitmap Image { get; set; }
        public TextureMagFilter Upscaling { get; set; } = TextureMagFilter.Nearest;

        public static List<Texture> AllTextures { get; set; } = new List<Texture>();

        public Texture(string name = "BaseObject") : base(name) => AllTextures.Add(this);

        public override void Delete()
        {
            AllTextures.Remove(this);
            GL.DeleteTexture(Handle);
            base.Delete();
        }

        public static void DeleteAll() { for (int i = 0; i < AllTextures.Count; i++) if (AllTextures[i] != null) AllTextures[i].Delete(); }

        public void Bind(TextureTarget textureTarget) => GL.BindTexture(textureTarget, Handle);

        public static void UnBind() => GL.BindTexture(TextureTarget.Texture2D, 0);

        public void LoadFromFile(string path)
        {
            if (!File.Exists("Textures/" + path))
            {
                Log.Error($"Le fichier pour la texture n'existe pas : {path}");
                //path = "Default.png";
                return;
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
            Image = image;
            Generate();
        }

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
            Image = image;
            Generate();
        }

        public void Generate(byte[] data)
        {
            Data = data;
            Generate();
        }

        void Generate()
        {
            Handle = GL.GenTexture();
            Bind(TextureTarget.Texture2D);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)Upscaling);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)Upscaling);

            UnBind();
        }
    }
}
