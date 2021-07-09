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
        public int NbrSprite { get; set; }
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

        public Vector2D Position { get; set; }

        int VBO { get; set; }
        int VAO { get; set; }
        int EBO { get; set; }
        double[] Vertices { get; set; } = new double[] 
        {
            0.28, 0.0625, 0, 1, 1,
            -0.28, 0.0625, 0, 0, 1,
            -0.28, -0.0625, 0, 0, 0,
            0.28, -0.0625, 0, 1, 0
            /*0, 1, 0, 1, 1,
            -1, 1, 0, 0, 1,
            -1, 0, 0, 0, 0,
            0, 0, 0, 1, 0*/
        };

        readonly uint[] Indices =
        {
            0, 1, 2,
            2, 3, 0
        };


        public Sprite(string name = "BaseObject") : base(name) => AllSprites.Add(this);

        protected internal override void OnStart()
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
        int cycle = 0;
        protected internal override void OnUpdate()
        {
            switch (cycle)
            {
                case 0:
                    Vertices[3] = 0.33;
                    Vertices[4] = 1;
                    Vertices[8] = 0;
                    Vertices[9] = 1;
                    Vertices[13] = 0;
                    Vertices[14] = 0.5;
                    Vertices[18] = 0.33;
                    Vertices[19] = 0.5;
                    break;
                case 1:
                    Vertices[3] = 0.33;
                    Vertices[4] = 0.5;
                    Vertices[8] = 0;
                    Vertices[9] = 0.5;
                    Vertices[13] = 0;
                    Vertices[14] = 0;
                    Vertices[18] = 0.33;
                    Vertices[19] = 0;
                    break;
                case 2:
                    Vertices[3] = 0.66;
                    Vertices[4] = 1;
                    Vertices[8] = 0.33;
                    Vertices[9] = 1;
                    Vertices[13] = 0.33;
                    Vertices[14] = 0.5;
                    Vertices[18] = 0.66;
                    Vertices[19] = 0.5;
                    break;
                case 3:
                    Vertices[3] = 0.66;
                    Vertices[4] = 0.5;
                    Vertices[8] = 0.33;
                    Vertices[9] = 0.5;
                    Vertices[13] = 0.33;
                    Vertices[14] = 0;
                    Vertices[18] = 0.66;
                    Vertices[19] = 0;
                    break;
                case 4:
                    Vertices[3] = 1;
                    Vertices[4] = 1;
                    Vertices[8] = 0.66;
                    Vertices[9] = 1;
                    Vertices[13] = 0.66;
                    Vertices[14] = 0.5;
                    Vertices[18] = 1;
                    Vertices[19] = 0.5;
                    break;
            }
            if (cycle == 4) cycle = 0;
            else cycle++;
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
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(double), Vertices, BufferUsageHint.DynamicDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Double, false, 5 * sizeof(double), 3 * sizeof(double));
        }
    }
}
