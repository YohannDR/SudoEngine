using OpenTK;
using OpenTK.Graphics.OpenGL;
using SudoEngine.Core;
using SudoEngine.Maths;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SudoEngine.Render
{
    public sealed class Shader : BaseObject
    {
        public int Handle { get; private set; }
        public static List<Shader> AllShaders = new List<Shader>();

        public Shader() : base() => AllShaders.Add(this);
        public Shader(string name) : base(name) => AllShaders.Add(this);

        void Generate(string VertexSource, string FragmentSource, string GeometrySource)
        {
            //Vertex Shader
            int Vertex = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(Vertex, VertexSource);
            GL.CompileShader(Vertex);
            CheckCompileError(Vertex, "Vertex");

            //Fragment Shader
            int Fragment = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(Fragment, FragmentSource);
            GL.CompileShader(Fragment);
            CheckCompileError(Fragment, "Fragment");

            //Geometry Shader (si fourni)
            int Geometry = 0;
            if (GeometrySource != null)
            {
                Geometry = GL.CreateShader(ShaderType.GeometryShader);
                GL.ShaderSource(Geometry, GeometrySource);
                GL.CompileShader(Geometry);
                CheckCompileError(Geometry, "Geometry");
            }

            //Rassemblage des shaders dans un programme
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, Vertex);
            GL.AttachShader(Handle, Fragment);
            if (GeometrySource != null) GL.AttachShader(Handle, Geometry);
            GL.LinkProgram(Handle);

            //Suppression des shaders (fourni dans le programme donc inutiles de les garder)
            GL.DetachShader(Handle, Vertex);
            GL.DetachShader(Handle, Fragment);
            if (GeometrySource != null) GL.DetachShader(Handle, Geometry);
            GL.DeleteShader(Vertex);
            GL.DeleteShader(Fragment);
            if (GeometrySource != null) GL.DeleteShader(Geometry);
        }


        public void Use() => GL.UseProgram(Handle);

        public void Dispose()
        {
            Delete();
            AllShaders.Remove(this);
            GL.DeleteProgram(Handle);
        }

        public static void DisposeAll() { for (int i = 0; i < AllShaders.Count; i++) if (AllShaders[i] != null) AllShaders[i].Dispose(); }

        public void SetAttribute(string name, object value)
        {
            if (value is int)
            {
                GL.Uniform1(GetAttribLocation(name), (int)value);
                return;
            }
            if (value is float || value is double)
            {
                GL.Uniform1(GetAttribLocation(name), (float)value);
                return;
            }
            if (value is Vector2 || value is Vector2D)
            {
                GL.Uniform2(GetAttribLocation(name), value is Vector2 ? (Vector2)value : (Vector2D)value);
                return;
            }
            if (value is Vector3 || value is Vector3D)
            {
                GL.Uniform3(GetAttribLocation(name), value is Vector3 ? (Vector3)value : (Vector3D)value);
                return;
            }
            if (value is Vector4 || value is Vector4D)
            {
                GL.Uniform4(GetAttribLocation(name), value is Vector4 ? (Vector4)value : (Vector4D)value);
                return;
            }
            if (value is Matrix4 || value is Matrix4D)
            {
                //var _value = value is Matrix4 ? (Matrix4)value : (Matrix4D)value;
                Matrix4 _value = (Matrix4)value;
                GL.UniformMatrix4(GetAttribLocation(name), false, ref _value);
                return;
            }
            if (value is bool) GL.Uniform1(GetAttribLocation(name), value is true ? 1 : 0);
        }

        public int GetAttribLocation(string name) => GL.GetUniformLocation(Handle, name);

        void CheckCompileError(int Object, string type)
        {
            string ErrorLog = GL.GetShaderInfoLog(Object);
            if (ErrorLog != "")
            {
                Log.Error($"Erreur dans {type} shader\n");
                Log.Error(ErrorLog);
            }
        }

        public void LoadFromFile(string Vpath, string Fpath, string Gpath)
        {
            string VertexSource, FragmentSource, GeometrySource;

            //Récupération des codes sources

            //Vertex shader
            if (!File.Exists("Shaders/" + Vpath))
            {
                Log.Error($"Le fichier pour le vertex shader n'a pas été trouvé : {Vpath}");
                return;
            }
            using (StreamReader reader = new StreamReader("Shaders/" + Vpath, Encoding.UTF8)) VertexSource = reader.ReadToEnd();

            //Fragment shader
            if (!File.Exists("Shaders/" + Fpath))
            {
                Log.Error($"Le fichier pour le fragment shader n'a pas été trouvé : {Fpath}");
                return;
            }
            using (StreamReader reader = new StreamReader("Shaders/" + Fpath, Encoding.UTF8)) FragmentSource = reader.ReadToEnd();

            //Geometry shader (si fourni)
            if (Gpath != null)
            {
                if (File.Exists("Shaders/" + Gpath))
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
