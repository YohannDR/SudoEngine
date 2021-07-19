using OpenTK;
using SudoEngine.Core;
using SudoEngine.Maths;
using System.Collections.Generic;

namespace SudoEngine.Render
{
    public enum Direcction
    {
        Left,
        Right,
        Up,
        Down
    }

    public sealed class Camera : BaseObject
    {
        public static Vector2D Resolution { get => new Vector2D(DisplayDevice.GetDisplay(DisplayIndex.Default).Width, DisplayDevice.GetDisplay(DisplayIndex.Default).Height); }
        public static double AspectRatio { get => Resolution.X / Resolution.Y; }
        public static Camera Main { get; set; }
        public static List<Camera> AllCameras { get; set; } = new List<Camera>();

        public Shader Shader { get; set; }

        public Vector4D MoveVector { get; set; } = new Vector4D(0);

        public Camera(string name = "Camera") : base(name)
        {
            AllCameras.Add(this);
            if (Main == null) Main = this;
        }

        public void Scroll(Direcction direction, double value)
        {
            switch (direction)
            {
                case Direcction.Left:
                    MoveVector = new Vector4D(MoveVector.X - value, MoveVector.YZW);
                    break;

                case Direcction.Right:
                    MoveVector = new Vector4D(MoveVector.X + value, MoveVector.YZW);
                    break;

                case Direcction.Up:
                    MoveVector = new Vector4D(MoveVector.X, MoveVector.Y + value, MoveVector.Y, MoveVector.W);
                    break;

                case Direcction.Down:
                    MoveVector = new Vector4D(MoveVector.X, MoveVector.Y - value, MoveVector.Y, MoveVector.W);
                    break;
            }
            Shader.SetAttribute("moveVector", MoveVector);
        }

        public override void Delete()
        {
            AllCameras.Remove(this);
            if (this == Main) Main = null;
            base.Delete();
        }
    }
}