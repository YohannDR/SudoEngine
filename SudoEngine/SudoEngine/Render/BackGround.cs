using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Collections.Generic;
using SudoEngine.Core;
using SudoEngine.Maths;
using System.Diagnostics;

namespace SudoEngine.Render
{
    public sealed class BackGround : BaseObject
    {
        public static List<BackGround> AllBackGrounds = new List<BackGround>(5);

        public Layer BGType { get; private set; }
        public Texture GFX { get; set; }
        public Shader Shader { get; private set; }

        float _transparency = 0;
        public float Transparency
        {
            get => _transparency;
            set
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("La transparence doit être comprise entre 0 et 1");
                else _transparency = value;
            }
        }
        public bool Visible { get; set; } = true;

        public Vector2D Size { get; set; }

        public double Width
        {
            get => Size.X;
            set { Size = new Vector2D(value, Size.Y); CalculateVertices(); }
        }
        public double Height
        {
            get => Size.Y;
            set { Size = new Vector2D(Size.X, value); CalculateVertices(); }
        }

        int VBO;
        int VAO;
        int EBO;
        float[] Vertices;

        readonly uint[] Indices =
        {
            0, 1, 2,
            2, 3, 0
        };

        public BackGround(Layer layer, Shader shader, Texture gfx, Vector2D size, string name) : base(name)
        {
            BGType = layer;
            Shader = shader;
            GFX = gfx;
            Size = size;
            CalculateVertices();
            AllBackGrounds[(int)BGType] = this;
            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 6 * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        void CalculateVertices()
        {
            float[] _vertices =
            {
                /*(float)Width, (float)Height, 0.0f, 1.0f, 1.0f,
               -(float)Width, (float)Height, 0.0f, 0.0f, 1.0f,
               -(float)Width, -(float)Height, 0.0f, 0.0f, 0.0f,
                (float)Width, -(float)Height, 0.0f, 1.0f, 0.0f*/
                -1 + (float)Width * 2, 1, 0.0f, 1.0f, 1.0f,
                -1, 1, 0.0f, 0.0f, 1.0f,
                -1, 1 - (float)Height * 2, 0.0f, 0.0f, 0.0f,
                -1 + (float)Width * 2, 1 - (float)Height * 2, 0.0f, 1.0f, 0.0f
            };
            Vertices = _vertices;
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
        }

        public void Bind()
        {
            Shader.Use();
            GFX.Bind(TextureTarget.Texture2D);
        }

        public void Dispose()
        {
            Delete();
            GFX.Dispose();
            Shader.Dispose();
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(EBO);
            AllBackGrounds[(int)BGType] = null;
        }
        public static void DisposeAll() { for (int i = 0; i < 5; i++) if (AllBackGrounds[i] != null) AllBackGrounds[i].Dispose(); }

        public void Render()
        {
            if (Visible)
            {
                Bind();
                Shader.SetAttribute("layer", (int)BGType);
                if (Transparency != 0) Shader.SetAttribute("transparency", Transparency);
                GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }

        public void Generate(int[,] data, Bitmap tileset)
        {
            Bitmap Gfx = new Bitmap(data.GetLength(1) * 16, data.GetLength(0) * 16);
            Size = new Vector2D(Gfx.Width / (float)1920, Gfx.Height / (float)1080);
            CalculateVertices();

            for (int x = 0; x < data.GetLength(0); x++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    int row = data[x, y] / 16;
                    for (int a = 0; a < 16; a++)
                    {
                        for (int b = 0; b < 16; b++)
                        {
                            Gfx.SetPixel(y * 16 + a, x * 16 + b, tileset.GetPixel((data[x, y] - row * 16) * 16 + a, row * 16 + b));
                        }
                    }
                }
            }
            Texture tex = new Texture();
            tex.LoadFromBitmap(Gfx, false);
            GFX = tex;
        }

        public static void RenderAll() {  foreach (BackGround bg in AllBackGrounds) if (bg != null) bg.Render(); }
        public static void CreateList() { for (int i = 0; i < 5; i++) AllBackGrounds.Add(null); }
    }
}