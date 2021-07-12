using OpenTK.Graphics.OpenGL;
using SudoEngine.Maths;
using SudoEngine.Core;
using System.Collections.Generic;

namespace SudoEngine.Render
{
    /// <summary>
    /// Classe abstract offrant un ensemble de propriétés et des méthodes permettant de créer des sprites et d'utiliser le système de scripting de 
    /// <see cref="GameObject"/> en plus d'automatiser le rendu
    /// <para>Hérite de <see cref="GameObject"/> et doit être héritée pour être utilisé</para>
    /// </summary>
    public abstract class Sprite : GameObject
    {
        /// <summary>Liste des tous les <see cref="Sprite"/> chargés en mémoire</summary>
        public static List<Sprite> AllSprites { get; set; } = new List<Sprite>();

        /// <summary><see cref="Render.Shader"/> associé au sprite</summary>
        public Shader Shader { get; set; }
        /// <summary><see cref="Texture"/> contenant les différentes frames de l'animation du Sprite</summary>
        public Texture SpriteSheet { get; set; }
        /// <summary><see cref="Vector2D"/> représenant la taille en pixels du sprite
        /// <para>Il est fortement recommandé que cette taille soit égale à la taille des frames dans le <see cref="SpriteSheet"/></para>
        /// </summary>
        public Vector2D Size { get; set; }
        /// <summary>Largueur du sprite</summary>
        public double Width
        {
            get => Size.X;
            set => Size = new Vector2D(value, Size.Y);
        }
        /// <summary>Hauteur du sprite</summary>
        public double Height
        {
            get => Size.Y;
            set => Size = new Vector2D(Size.X, value);
        }
        /// <summary><see cref="System.Boolean"/> indiquant si le sprite doit être render</summary>
        public bool Visible { get; set; } = true;

        Vector2D _position;
        /// <summary>Position du sprite dans la fenêtre
        /// <para>La position 0;0 se toruve au centre de la fenêtre et elle est relative à la taille de l'écran et non la fenêtre</para>
        /// </summary>
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

        double rowInSpriteSheet;
        /// <summary>La ligne sur laquelle se trouve les frames d'animations dans le <see cref="SpriteSheet"/></summary>
        public double RowInSpriteSheet
        {
            get => rowInSpriteSheet;
            set 
            {
                Vertices[4] = Vertices[9] = 1 - RowInSpriteSheet / NbrRows;
                Vertices[14] = Vertices[19] = 1 - (RowInSpriteSheet + 1) / NbrRows;
                rowInSpriteSheet = value;
            }
        }

        double NbrRows => SpriteSheet.Height / Size.Y;

        int VBO { get; set; }
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


        /// <summary>
        /// Crée un nouvel objet <see cref="Sprite"/> et appele le constructeur de <see cref="GameObject"/>
        /// </summary>
        /// <param name="name">Le nom interne de l'objet (Sprite par défaut)</param>
        protected internal Sprite(string name = "Sprite") : base(name) => AllSprites.Add(this);

        /// <summary>
        /// Génère un Sprite avec les paramètres données
        /// </summary>
        /// <param name="spriteSheet">La <see cref="Texture"/> contenant les frames d'animation</param>
        /// <param name="shader">Le <see cref="Shader"/> associé au Sprite</param>
        /// <param name="rowInSpriteSheet">Le numéro de ligne sur lequel se trouve les frames d'animation du Sprite dans le <see cref="SpriteSheet"/></param>
        /// <param name="size"><see cref="Vector2D"/> représentant la taille en pixels du Sprite</param>
        public void Generate(Texture spriteSheet, Shader shader, double rowInSpriteSheet, Vector2D size)
        {
            SpriteSheet = spriteSheet;
            Shader = shader;
            RowInSpriteSheet = rowInSpriteSheet;
            Size = size;
            Position = Vector2D.Zero;
        }

        protected internal override void OnStart()
        {
            Vertices[4] = Vertices[9] = 1 - RowInSpriteSheet / NbrRows;
            Vertices[14] = Vertices[19] = 1 - (RowInSpriteSheet + 1) / NbrRows;
            DisplayFrame(0);
            VBO = GL.GenBuffer();
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
        
        /// <summary>Supprime le Sprite</summary>
        public override void Delete()
        {
            AllSprites.Remove(this);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            Shader.Delete();
            SpriteSheet.Delete();
            base.Delete();
        }

        /// <summary>Supprime tous les <see cref="Sprite"/> </summary>
        public static void DeleteAll() { for (int i = 0; i < AllSprites.Count; i++) if (AllSprites[i]) AllSprites[i].Delete(); }

        /// <summary>Bind le Sprite</summary>
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

        /// <summary>
        /// Permet de changer la frame actuellement display depuis le <see cref="SpriteSheet"/> pour le Sprite
        /// </summary>
        /// <param name="idx">L'index de la frame dans le <see cref="SpriteSheet"/></param>
        public void DisplayFrame(int idx)
        {
            Vertices[3] = Vertices[18] = (Width * idx + Width) / SpriteSheet.Width;
            Vertices[8] = Vertices[13] = Width * idx / SpriteSheet.Width;
        }
    }
}
