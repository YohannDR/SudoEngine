using OpenTK;
using System.Collections.Generic;
using SudoEngine.Maths;
using SudoEngine.Core;

namespace SudoEngine.Render
{
    public sealed class Camera : BaseObject
    {
        public static Vector2D Resolution { get => new Vector2D(DisplayDevice.GetDisplay(DisplayIndex.Second).Width, DisplayDevice.GetDisplay(DisplayIndex.Second).Height); }
        public static double AspectRatio { get => Resolution.X / Resolution.Y; }
        public static Camera Main { get; set; }

        public static List<Camera> AllCameras = new List<Camera>();
       
        public Matrix4 Model { get; set; }
        public Matrix4 View { get; set; }
        public Matrix4 Projection { get; set; }
        public Shader Shader { get; set; }

        public Camera() : base()
        {
            AllCameras.Add(this);
            if (Main == null) Main = this;
        }
        public Camera(string name) : base(name)
        {
            AllCameras.Add(this);
            if (Main == null) Main = this;
        }

        public void Use()
        {
            Shader.Use();
            Shader.SetAttribute("model", Model);
            Shader.SetAttribute("view", View);
            Shader.SetAttribute("projection", Projection);
        }
       
        public void Scroll(Vector2D scroll)
        {
            Use();
        }

        public void Scroll(double x, double y)
        {
            Use();
        }
        public override void Delete()
        {
            AllCameras.Remove(this);
            if (this == Main) Main = null;
            base.Delete();
        }
    }
}