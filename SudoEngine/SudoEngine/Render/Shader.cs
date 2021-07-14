using OpenTK;
using OpenTK.Graphics.OpenGL;
using SudoEngine.Core;
using SudoEngine.Maths;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SudoEngine.Render
{
    /// <summary>
    /// Classe permettant de gérer des Shader, fourni des méthodes facilitant la manipulation des données
    /// <para>Hérite de <see cref="BaseObject"/> et ne peut pas être héritée</para>
    /// </summary>
    public sealed class Shader : BaseObject
    {
        /// <summary>Liste de tous les <see cref="Shader"/> actuellement chargés en mémoire</summary>
        public static List<Shader> AllShaders { get; set; } = new List<Shader>();
        /// <summary>Handle du shader (nécessaire au fonctionnement d'OpenGL)</summary>
        public int Handle { get; private set; }

        /// <summary>
        /// Crée un nouvel objet <see cref="Shader"/> et appele le constructeur de <see cref="BaseObject"/>
        /// </summary>
        /// <param name="name">Le nom interne de l'objet (Shader par défaut)</param>
        public Shader(string name = "Shader") : base(name) => AllShaders.Add(this);

        void Generate(string VertexSource, string FragmentSource, string GeometrySource)
        {
            int Vertex = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(Vertex, VertexSource);
            GL.CompileShader(Vertex);
            CheckCompileError(Vertex, "Vertex");

            int Fragment = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(Fragment, FragmentSource);
            GL.CompileShader(Fragment);
            CheckCompileError(Fragment, "Fragment");

            int Geometry = 0;
            if (GeometrySource != null)
            {
                Geometry = GL.CreateShader(ShaderType.GeometryShader);
                GL.ShaderSource(Geometry, GeometrySource);
                GL.CompileShader(Geometry);
                CheckCompileError(Geometry, "Geometry");
            }

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, Vertex);
            GL.AttachShader(Handle, Fragment);
            if (GeometrySource != null) GL.AttachShader(Handle, Geometry);
            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, Vertex);
            GL.DetachShader(Handle, Fragment);
            if (GeometrySource != null)
            {
                GL.DetachShader(Handle, Geometry);
                GL.DeleteShader(Geometry);
            }
            GL.DeleteShader(Vertex);
            GL.DeleteShader(Fragment);
        }

        /// <summary>Bind le shader</summary>
        public void Use() => GL.UseProgram(Handle);

        /// <summary>Supprime le shader </summary>
        public override void Delete()
        {
            AllShaders.Remove(this);
            GL.DeleteProgram(Handle);
            base.Delete();
        }

        /// <summary>Supprime tous les <see cref="Shader"/></summary>
        public static void DeleteAll() { for (int i = 0; i < AllShaders.Count; i++) if (AllShaders[i]) AllShaders[i].Delete(); }

        /// <summary>
        /// Permet de set un unifrom de type <see cref="int"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, int value) => GL.Uniform1(GetAttribLocation(name), value);

        public void SetAttribute(string name, int[] value) => GL.Uniform1(GetAttribLocation(name), value.Length, value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="float"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, float value) => GL.Uniform1(GetAttribLocation(name), value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="double"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, double value) => GL.Uniform1(GetAttribLocation(name), value);

        /// <summary>
        /// Permet de set un unifrom de type pointeur de <see cref="Vector2"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public unsafe void SetAttribute(string name, Vector2* value) => GL.Uniform2(GetAttribLocation(name), *value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="Vector2"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, Vector2 value) => GL.Uniform2(GetAttribLocation(name), value);

        /// <summary>
        /// Permet de set un unifrom de type pointeur de <see cref="Vector2D"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public unsafe void SetAttribute(string name, Vector2D* value) => GL.Uniform2(GetAttribLocation(name), *value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="Vector2D"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, Vector2D value) => GL.Uniform2(GetAttribLocation(name), value);

        /// <summary>
        /// Permet de set un unifrom de type pointeur de <see cref="Vector3"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public unsafe void SetAttribute(string name, Vector3* value) => GL.Uniform3(GetAttribLocation(name), *value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="Vector3"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, Vector3 value) => GL.Uniform3(GetAttribLocation(name), value);

        /// <summary>
        /// Permet de set un unifrom de type pointeur de <see cref="Vector3D"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public unsafe void SetAttribute(string name, Vector3D* value) => GL.Uniform3(GetAttribLocation(name), *value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="Vector3D"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, Vector3D value) => GL.Uniform3(GetAttribLocation(name), value);

        /// <summary>
        /// Permet de set un unifrom de type pointeur de <see cref="Vector4"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public unsafe void SetAttribute(string name, Vector4* value) => GL.Uniform4(GetAttribLocation(name), *value);

        /// <summary>
        /// Permet de set un unifrom de type pointeur de <see cref="Vector4"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, Vector4 value) => GL.Uniform4(GetAttribLocation(name), value);

        /// <summary>
        /// Permet de set un unifrom de type pointeur de <see cref="Vector4D"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public unsafe void SetAttribute(string name, Vector4D* value) => GL.Uniform4(GetAttribLocation(name), *value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="Vector4D"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, Vector4D value) => GL.Uniform4(GetAttribLocation(name), value);

        /// <summary>
        /// Permet de set un unifrom de type pointeur de <see cref="Matrix4"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public unsafe void SetAttribute(string name, Matrix4* value) => GL.UniformMatrix4(GetAttribLocation(name), false, ref *value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="Matrix4"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, Matrix4 value) => GL.UniformMatrix4(GetAttribLocation(name), false, ref value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="Matrix4D"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, Matrix4D value) => SetAttribute(name, (Matrix4)value);

        /// <summary>
        /// Permet de set un unifrom de type <see cref="bool"/> dans le shader
        /// </summary>
        /// <param name="name">Le nom de la variable dans le shader</param>
        /// <param name="value">La valeur a passer dans la variable</param>
        public void SetAttribute(string name, bool value) => GL.Uniform1(GetAttribLocation(name), value ? 1 : 0);

        /// <summary>
        /// Récupère l'emplacement mémoire d'une variable uniform
        /// </summary>
        /// <param name="name">Le nom de la variable</param>
        /// <returns>L'empllacement de la variable</returns>
        public int GetAttribLocation(string name) => GL.GetUniformLocation(Handle, name);

        void CheckCompileError(int Object, string type)
        {
            string ErrorLog = GL.GetShaderInfoLog(Object);
            if (ErrorLog != string.Empty)
            {
                Log.Error($"Erreur dans {type} shader\n");
                Log.Error(ErrorLog);
            }
        }

        /// <summary>
        /// Crée un shader a partir de fichiers (Shaders/ est automatiquement rajouté devant les chemins)
        /// </summary>
        /// <param name="Vpath">Chemin vers le Vertex Shader</param>
        /// <param name="Fpath">Cheminvers le Fragment Shader</param>
        /// <param name="Gpath">Chemin vers le Geometry Shader (<see langword="null"/> si non présent)</param>
        public void LoadFromFile(string Vpath, string Fpath, string Gpath)
        {
            string VertexSource, FragmentSource, GeometrySource;

            if (!File.Exists("Shaders/" + Vpath))
            {
                Log.Error($"Le fichier pour le vertex shader n'a pas été trouvé : {Vpath}");
                return;
            }
            using (StreamReader reader = new StreamReader("Shaders/" + Vpath, Encoding.UTF8)) VertexSource = reader.ReadToEnd();

            if (!File.Exists("Shaders/" + Fpath))
            {
                Log.Error($"Le fichier pour le fragment shader n'a pas été trouvé : {Fpath}");
                return;
            }
            using (StreamReader reader = new StreamReader("Shaders/" + Fpath, Encoding.UTF8)) FragmentSource = reader.ReadToEnd();

            if (Gpath != null)
            {
                if (!File.Exists("Shaders/" + Gpath))
                {
                    Log.Error($"Le fichier pour le geometry shader n'a pas été trouvé : {Gpath}");
                    return;
                }
                using (StreamReader reader = new StreamReader("Shaders/" + Gpath, Encoding.UTF8)) GeometrySource = reader.ReadToEnd();

                Generate(VertexSource, FragmentSource, GeometrySource);
            }
            else Generate(VertexSource, FragmentSource, null);
        }
    }
}
