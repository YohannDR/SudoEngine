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
         

        int VBO { get; set; }
        int VAO { get; set; }
        int EBO { get; set; }

        public Mesh(string name = "Mesh") : base(name) => Meshes.Add(this);

        public void Generate()
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
