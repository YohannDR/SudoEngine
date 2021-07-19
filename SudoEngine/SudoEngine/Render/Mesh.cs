using OpenTK.Graphics.OpenGL;
using SudoEngine.Core;
using SudoEngine.Maths;
using System.Collections.Generic;

namespace SudoEngine.Render
{
    /// <summary>
    /// Classe permettant de gérer des Meshes, fourni un ensemble de méthodes et de propriétés qui facilitent la création et le rendu
    /// <para>Hérite de <see cref="BaseObject"/> et ne peut pas être héritée</para>
    /// </summary>
    public sealed class Mesh : BaseObject
    {
        public static List<Mesh> Meshes { get; set; } = new List<Mesh>();
        public Shader Shader { get; set; }
        public Texture GFX { get; set; }

        public bool Wire { get; set; } = false;

        public Vector3D[] Vertices { get; set; }
        public Vector2D[] UVs { get; set; }
        public double[] Vertex { get; set; }

        private int VBO { get; set; }
        private int VAO { get; set; }
        private int EBO { get; set; }

        public uint[] Indices { get; set; }

        /// <summary>
        /// Crée un nouvel objet <see cref="Mesh"/> et appele le constructeur de <see cref="BaseObject"/>
        /// </summary>
        /// <param name="name">Le nom interne de l'objet (Mesh par défaut)</param>
        public Mesh(string name = "Mesh") : base(name) => Meshes.Add(this);

        public void Generate(Shader shader, Texture gfx, Vector3D[] vertices, Vector2D[] uvs, uint[] indices)
        {
            Shader = shader;
            GFX = gfx;
            Vertices = vertices;
            UVs = uvs;

            double[] vertex = new double[Vertices.Length * 5];

            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector3D vertice = Vertices[i];
                vertex[i * 5] = vertice.X;
                vertex[i * 5 + 1] = vertice.Y;
                vertex[i * 5 + 2] = vertice.Z;

                Vector2D uv = UVs[i];
                vertex[i * 5 + 3] = uv.X;
                vertex[i * 5 + 4] = uv.Y;
            }

            Vertex = vertex;
            Indices = indices;
            InitGL();
        }

        /// <summary>Bind les ressources du Mesh</summary>
        public void Bind()
        {
            Shader.Use();
            GFX.Bind();
            GL.BindVertexArray(VAO);
        }

        private void InitGL()
        {
            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertex.Length * sizeof(double), Vertex, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Double, false, 5 * sizeof(double), 3 * sizeof(double));
            Vertices = null;
            UVs = null;
        }

        /// <summary>Render le Mesh</summary>
        public void Render()
        {
            Bind();
            GL.DrawElements(Wire ? PrimitiveType.LineLoop : PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        /// <summary>Render tous les Mesh non <see langword="null"/></summary>
        public static void RenderAll() { for (int i = 0; i < Meshes.Count; i++) if (Meshes[i]) Meshes[i].Render(); }

        /// <summary>Supprime le Mesh</summary>
        public override void Delete()
        {
            Meshes.Remove(this);
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(EBO);
            Shader.Delete();
            GFX.Delete();
            base.Delete();
        }
    }
}