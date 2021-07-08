using OpenTK.Graphics.OpenGL;
using SudoEngine.Core;
using SudoEngine.Maths;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace SudoEngine.Render
{
    public enum Layer
    {
        BackGround,
        CloseBackGround,
        PlayerLayer,
        CloseForeGround,
        ForeGround
    }


    public sealed class BackGround : BaseObject
    {
        public static List<BackGround> AllBackGrounds = new List<BackGround>(5);

        /// <summary> Indique le layer sur lequel se trouve le BackGround </summary>
        public Layer BGType { get; private set; }
        /// <summary> La texture attaché au BackGround </summary>
        public Texture GFX { get; set; }
        /// <summary> Le shader attaché au BackGround </summary>
        public Shader Shader { get; set; }
        float _transparency = 0;
        /// <summary> La transparence du BackGround, entre 0 (opaque) et 1 (complètement transparent) </summary>
        public float Transparency
        {
            get => _transparency;
            set
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("La transparence doit être comprise entre 0 et 1");
                else _transparency = value;
            }
        }
        /// <summary> Indique si oui ou non le BackGround doit être render </summary>
        public bool Visible { get; set; } = true;
        /// <summary> La taille (en écrans) du BackGround </summary>
        public Vector2D Size { get; set; }
        /// <summary> La largeur (en écrans) du background </summary>
        public double Width
        {
            get => Size.X;
            set { Size = new Vector2D(value, Size.Y); CalculateVertices(); }
        }
        /// <summary> La hauteur (en écrans) du BackGround </summary>
        public double Height
        {
            get => Size.Y;
            set { Size = new Vector2D(Size.X, value); CalculateVertices(); }
        }

        int VBO { get; set; }
        int VAO { get; set; }
        int EBO { get; set; }
        double[] Vertices { get; set; } = new double[]
        {
            0, 1, 0, 1, 1,
            -1, 1, 0, 0, 1,
            -1, 0, 0, 0, 0,
            0, 0, 0, 1, 0
        };
        
        readonly uint[] Indices =
        {
            0, 1, 2,
            2, 3, 0
        };

        public BackGround(string name = "BaseObject") : base(name) { }


        public void Bind()
        {
            Shader.Use();
            GFX.Bind(TextureTarget.Texture2D);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(double), Vertices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);
        }
        public  override void Delete()
        {
            GFX.Delete();
            Shader.Delete();
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(EBO);
            AllBackGrounds[(int)BGType] = null;
            base.Delete();
        }

        public void Render()
        {
            if (Visible && Transparency != 1)
            {
                Bind();
                Shader.SetAttribute("layer", (int)BGType);
                if (Transparency != 0) Shader.SetAttribute("transparency", Transparency);
                GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }


        public void Generate(Layer layer, Shader shader, Texture gfx, Vector2D size)
        {
            BGType = layer;
            Shader = shader;
            GFX = gfx;
            Size = size;
            CalculateVertices();
            AllBackGrounds[(int)BGType] = this;
            InitGL();
        }

        public unsafe void Generate(Layer layer, Shader shader, int[,] data, Bitmap tileset)
        {
            BGType = layer;
            Shader = shader;
            Generate(data, tileset);
            AllBackGrounds[(int)BGType] = this;
            InitGL();
        }

        public void Generate(Texture gfx, Vector2D size)
        {
            GFX = gfx;
            Size = size;
        }

        public void Generate(int[,] data, Bitmap tileset)
        {
            Bitmap Gfx = new Bitmap(data.GetLength(1) * 32, data.GetLength(0) * 32);
            Size = new Vector2D(Gfx.Width / (float)1920, Gfx.Height / (float)1080);
            CalculateVertices();

            int tilePerRow = tileset.Width / 32;
            for (int x = 0; x < data.GetLength(0); x++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    int row = data[x, y] / tilePerRow;
                    for (int a = 0; a < 32; a++)
                    {
                        for (int b = 0; b < 32; b++)
                        {
                            Gfx.SetPixel(y * 32 + a, x * 32 + b, tileset.GetPixel((data[x, y] - row * tilePerRow) * 32 + a, row * 32 + b));
                        }
                    }
                }
            }
            Texture tex = new Texture();
            tex.LoadFromBitmap(Gfx, true);
            GFX = tex;
        }

        public void DeleteTile(int index)
        {
            int tilePerRow = GFX.Width / 32;
            Log.Info(ConvertIndex(index));
            int row = index / tilePerRow;
            byte[] data = GFX.Data;
            int f = index % tilePerRow * 128;
            int e = row * 4096 * tilePerRow;
            for (int a = 0; a < 32; a++)
            {
                int d = a * tilePerRow * 128;
                for (int b = 0; b < 32; b++)
                {
                    int c = b * 4;
                    data[f + e + c + d] = 0;
                    data[f + e + c + d + 1] = 0;
                    data[f + e + c + d + 2] = 0;
                    data[f + e + c + d + 3] = 1;
                }
            }
            GFX.Generate(data);
        }

        void InitGL()
        {
            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(double), Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 6 * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Double, false, 5 * sizeof(double), 3 * sizeof(double));
        }

        int ConvertIndex(int index)
        {
            int tilePerRow = GFX.Width / 32;
            int tilePerColumn = GFX.Height / 32;
            return index;
        }

        void CalculateVertices()
        {
            Vertices[0] = -1 + Width * 2;
            Vertices[11] = 1 - Height * 2;
            Vertices[15] = -1 + Width * 2;
            Vertices[16] = 1 - Height * 2;
        }

        public static void RenderAll() { foreach (BackGround bg in AllBackGrounds) if (bg != null) bg.Render(); }
        public static void DeleteAll() { for (int i = 0; i < 5; i++) if (AllBackGrounds[i] != null) AllBackGrounds[i].Delete(); }
        public static void CreateList() { for (int i = 0; i < 5; i++) AllBackGrounds.Add(null); }
    }
}