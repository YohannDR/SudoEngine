using OpenTK.Graphics.OpenGL;
using SudoEngine.Core;
using SudoEngine.Maths;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SudoEngine.Render
{
    /// <summary>Couche sur lequel un <see cref="Render.BackGround"/> se trouve
    /// <para>Affecte l'ordre de Render ainsi que l'accessibilité de certaines propriétés</para>
    /// </summary>
    public enum Layer
    {
        /// <summary>Le Layer le plus au fond</summary>
        BackGround,

        /// <summary>Le Layer entre le <see cref="PlayerLayer"/> et le <see cref="BackGround"/></summary>
        CloseBackGround,

        /// <summary>Le Layer sur lequel se trouve le joueur</summary>
        PlayerLayer,

        /// <summary>Le Layer entre le <see cref="PlayerLayer"/> et le <see cref="ForeGround"/></summary>
        CloseForeGround,

        /// <summary>Le Layer le plus proche</summary>
        ForeGround
    }

    /// <summary>
    /// Classe permettant de gérer des BackGround, fourni un ensemble de méthodes et de propriétés qui facilitent la création et le rendu
    /// <para>Hérite de <see cref="BaseObject"/> et ne peut pas être héritée</para>
    /// </summary>
    public sealed class BackGround : BaseObject
    {
        /// <summary>Liste des 5 <see cref="BackGround"/> ayant un layer</summary>
        public static List<BackGround> AllBackGrounds = new List<BackGround>(5)
        {
            null, null, null, null, null
        };

        /// <summary> Indique le layer sur lequel se trouve le BackGround </summary>
        public Layer Layer { get; private set; }

        /// <summary> La texture attaché au BackGround </summary>
        public Texture GFX { get; set; }

        /// <summary> Le shader attaché au BackGround </summary>
        public Shader Shader { get; set; }

        private double _transparency = 0;

        /// <summary> La transparence du BackGround, entre 0 (opaque) et 1 (complètement transparent) </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public double Transparency
        {
            get => _transparency;
            set
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("La transparence doit être comprise entre 0 et 1");
                _transparency = value;
            }
        }

        /// <summary> Indique si oui ou non le BackGround doit être render (<see langword="true"/> par défaut)</summary>
        public bool Visible { get; set; } = true;

        /// <summary> La taille (en écrans) du BackGround </summary>
        public Vector2D Size { get; set; }

        /// <summary> La largeur (en écrans) du BackGround </summary>
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

        private int VAO { get; set; }
        private int VBO { get; set; }
        private int EBO { get; set; }

        private double[] Vertices { get; set; } = new double[]
        {
            0, 1, 0, 1, 1,
            -1, 1, 0, 0, 1,
            -1, 0, 0, 0, 0,
            0, 0, 0, 1, 0
        };

        private Mesh Mesh { get; set; } = null;

        private uint[] Indices { get; set; } =
        {
            0, 1, 2,
            2, 3, 0
        };

        /// <summary>
        /// Crée un nouvel objet <see cref="BackGround"/> et appele le constructeur de <see cref="BaseObject"/>
        /// </summary>
        /// <param name="name">Le nom interne de l'objet (BackGround par défaut)</param>
        public BackGround(string name = "BackGround") : base(name) { }

        /// <summary>Bind les ressources du BackGround</summary>
        public void Bind()
        {
            Shader.Use();
            GFX.Bind();
            GL.BindVertexArray(VAO);
        }

        /// <summary>Supprime le BackGround</summary>
        public override void Delete()
        {
            if (Mesh) Mesh.Delete();
            else
            {
                GFX.Delete();
                Shader.Delete();
                GL.DeleteBuffer(VBO);
                GL.DeleteBuffer(EBO);
                GL.DeleteVertexArray(VAO);
            }
            AllBackGrounds[(int)Layer] = null;
            base.Delete();
        }

        /// <summary>Render le BackGround</summary>
        public void Render()
        {
            if (Visible && Transparency != 1)
            {
                if (Mesh)
                {
                    if (Transparency != 0) Mesh.Shader.SetAttribute("transparency", Transparency);
                    Mesh.Render();
                }
                else
                {
                    Bind();
                    if (Transparency != 0) Shader.SetAttribute("transparency", Transparency);
                    GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }
        }

        /// <summary>
        /// Génère le BackGround avec les paramètres donnés (<see cref="Render.Layer"/>, <see cref="Render.Shader"/>, <see cref="Texture"/> et <see cref="Vector2D"/>)
        /// </summary>
        /// <param name="layer">Le <see cref="Render.Layer"/> sur lequel se trouve le BackGround</param>
        /// <param name="shader">Le <see cref="Render.Shader"/> associé au BackGround</param>
        /// <param name="gfx">La <see cref="Texture"/> du BackGround, ces graphismes</param>
        /// <param name="size"><see cref="Vector2D"/> représentant la taille du BackGround en écrans</param>
        public void Generate(Layer layer, Shader shader, Texture gfx, Vector2D size)
        {
            Layer = layer;
            Shader = shader;
            GFX = gfx;
            Size = size;
            CalculateVertices();
            AllBackGrounds[(int)Layer] = this;
            InitGL();
        }

        /// <summary>
        /// Génère le BackGround avec les paramètres données (<see cref="Render.Layer"/>, <see cref="Render.Shader"/>, <see langword="int[]"/> et <see cref="Bitmap"/>)
        /// </summary>
        /// <param name="layer">Le <see cref="Render.Layer"/> sur lequel se trouve le BackGround</param>
        /// <param name="shader">Le <see cref="Render.Shader"/> associé au BackGround</param>
        /// <param name="data">Array d'<see langword="int[]"/> représentant les données des tiles</param>
        /// <param name="tileset"><see cref="Bitmap"/> représentant le tileset</param>
        public void Generate(Layer layer, Shader shader, int[] data, Bitmap tileset, Vector2D size)
        {
            Layer = layer;
            Shader = shader;
            Generate(data, tileset, size);
            AllBackGrounds[(int)Layer] = this;
        }

        /// <summary>
        /// Génère le BackGround avec les paramètres données (<see cref="Texture"/> et <see cref="Vector2D"/>)
        /// </summary>
        /// <param name="gfx">La <see cref="Texture"/> du BackGround, ces graphismes</param>
        /// <param name="size"><see cref="Vector2D"/> représentant la taille du BackGround en écrans</param>
        public void Generate(Texture gfx, Vector2D size)
        {
            GFX = gfx;
            Size = size;
        }

        /// <summary>
        /// Génère le BackGround avec les paramètres données (<see langword="int[]"/> et <see cref="Bitmap"/>)
        /// </summary>
        /// <param name="data">Array d'<see langword="int[]"/> représentant les données des tiles</param>
        /// <param name="tileset"><see cref="Bitmap"/> représentant le tileset</param>
        public void Generate(int[] data, Bitmap tileset, Vector2D size)
        {
            Mesh = new Mesh();
            Texture a = new Texture();
            Size = size;
            a.LoadFromBitmap(tileset, true);

            Vector3D[] vertices = new Vector3D[]
            {
                new Vector3D(0)
            };

            Vector2D[] uvs = new Vector2D[]
            {
                new Vector2D(0)
            };

            uint[] indices = new uint[]
            {
                0
            };

            Mesh.Generate(Shader, a, vertices, uvs, indices);
        }

        private void InitGL()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(double), Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Double, false, 5 * sizeof(double), 3 * sizeof(double));
        }

        private void CalculateVertices()
        {
            Vertices[0] = -1 + Width * 2;
            Vertices[11] = 1 - Height * 2;
            Vertices[15] = -1 + Width * 2;
            Vertices[16] = 1 - Height * 2;

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(double), Vertices, BufferUsageHint.StaticDraw);
        }

        /// <summary>Render tous les BackGround non <see langword="null"/>/></summary>
        public static void RenderAll() { foreach (BackGround bg in AllBackGrounds) if (bg) bg.Render(); }

        /// <summary>Supprime tous les BackGround/></summary>
        public static void DeleteAll() { for (int i = 0; i < 5; i++) if (AllBackGrounds[i]) AllBackGrounds[i].Delete(); }
    }
}