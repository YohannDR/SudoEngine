using OpenTK.Graphics.OpenGL;
using SudoEngine.Maths;
using SudoEngine.Core;
using System.Collections.Generic;

namespace SudoEngine.Render
{
    public class Sprite : GameObject
    {
        public static List<Sprite> AllSprites { get; set; } = new List<Sprite>();
        public Shader Shader { get; set; }

        public Texture SpriteSheet { get; set; }
        public Vector2D Size { get; set; }
        public double Width
        {
            get => Size.X;
            set => Size = new Vector2D(value, Size.Y);
        }
        public double Height
        {
            get => Size.Y;
            set => Size = new Vector2D(Size.X, value);
        }

        public bool Visible { get; set; } = true;

        Vector2D _position;
        public Vector2D Position
        {
            get => _position;
            set
            {
                Vertices[0] = Vertices[15] = (value.X + Size.X) / Camera.Resolution.X;
                Vertices[1] = Vertices[6] = (value.Y + Size.Y) / Camera.Resolution.Y;

                Vertices[5] = Vertices[10] = (value.X - Size.X) / Camera.Resolution.X;
                Vertices[11] = Vertices[16] = (value.Y - Size.Y) / Camera.Resolution.Y;
                _position = value;
            }
        }

        public double RowInSpriteSheet { get; set; }

        double NbrRows => SpriteSheet.Height / Size.Y;

        int VBO { get; set; }
        int VAO { get; set; }
        int EBO { get; set; }
        double[] Vertices { get; set; } = new double[] 
        {
            1, 1, 0, 0, 0,
            -1, 1, 0, 0, 0,
            -1, -1, 0, 0, 0,
            1, -1, 0, 0, 0
        };

        readonly uint[] Indices =
        {
            0, 1, 2,
            2, 3, 0
        };


        public Sprite(string name = "Sprite") : base(name) => AllSprites.Add(this);

        public void Generate(Texture spriteSheet, Shader shader, double rowInSpriteSheet, Vector2D size)
        {
            SpriteSheet = spriteSheet;
            Shader = shader;
            RowInSpriteSheet = rowInSpriteSheet;
            Size = size;
        }

        protected internal override void OnStart()
        {
            Vertices[4] = Vertices[9] = 1 - RowInSpriteSheet / NbrRows;
            Vertices[14] = Vertices[19] = 1 - (RowInSpriteSheet + 1) / NbrRows;
            DisplayImage(0);
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

        protected internal override void OnRender()
        {
            if (Visible)
            {
                Bind();
                GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }
        
        public override void Delete()
        {
            AllSprites.Remove(this);
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(EBO);
            Shader.Delete();
            SpriteSheet.Delete();
            base.Delete();
        }

        public void Bind()
        {
            Shader.Use();
            SpriteSheet.Bind();
            //GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(double), Vertices, BufferUsageHint.DynamicDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Double, false, 5 * sizeof(double), 3 * sizeof(double));
        }


        public void DisplayImage(int idx)
        {
            Vertices[3] = Vertices[18] = (Width * idx + Width) / SpriteSheet.Width;
            Vertices[8] = Vertices[13] = Width * idx / SpriteSheet.Width;
        }
    }
}
