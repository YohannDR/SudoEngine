using OpenTK.Graphics.OpenGL;
using SudoEngine.Core;
using SudoEngine.Maths;
using System;
using System.Collections.Generic;

namespace SudoEngine.Render
{
    public sealed class Mesh : BaseObject
    {
        public static List<Mesh> Meshes { get; set; } = new List<Mesh>();
        public Shader Shader { get; set; }
        public Texture GFX { get; set; }

        public bool Wire { get; set; } = false;
        
        public Vector3D[] Vertices { get; set; }
        public Vector2D[] Uvs { get; set; }
        public uint Indices { get; set; }

        int VBO { get; set; }
        int VAO { get; set; }
        int EBO { get; set; }

        public Mesh(string name = "Mesh") : base(name) => Meshes.Add(this);

        public void Generate(Shader shader, Texture gfx)
        {
            InitGL();
        }

        public void Bind()
        {
        }
        
        void InitGL()
        {
            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            EBO = GL.GenBuffer();

            /*GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(double), Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 6 * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Double, false, 5 * sizeof(double), 3 * sizeof(double));



            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertex.Length * sizeof(double), Vertex, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (int)Indices * sizeof(uint), Triangles, BufferUsageHint.StaticDraw);*/
        }

        public void Render()
        {
            Bind();
            GL.DrawElements(Wire ? PrimitiveType.LineLoop : PrimitiveType.Triangles, 0, DrawElementsType.UnsignedInt, 0);
        }

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
