using System;
using OpenTK;

namespace SudoEngine.Maths
{
    public struct Vector3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector2D XY 
        { 
            get => new Vector2D(X, Y);
            set => (X, Y) = (value.X, value.Y);
        }
        public Vector2D YZ
        {
            get => new Vector2D(Y, Z);
            set => (Y, Z) = (value.X, value.Y);
        }
        public Vector2D XZ
        {
            get => new Vector2D(X, Z);
            set => (X, Z) = (value.X, value.Y);
        }
        
        public Vector2D YX
        {
            get => new Vector2D(Y, X);
            set => (Y, X) = (value.X, value.Y);
        }
        public Vector2D ZY
        {
            get => new Vector2D(Z, Y);
            set => (Z, Y) = (value.X, value.X);
        }
        public Vector2D ZX
        {
            get => new Vector2D(Z, X);
            set => (Z, X) = (value.X, value.Y);
        }

        public double this[int i]
        {
            get
            {
                if (i == 0) return X;
                if (i == 1) return Y;
                if (i == 2) return Z;
                throw new IndexOutOfRangeException("L'index était hors des limites du Vecteur, il doit être compris entre 0 et 2");
            }
            set
            {
                if (i == 0) X = value;
                else if (i == 1) Y = value;
                else if (i == 2) Z = value;
                else throw new IndexOutOfRangeException("L'index était hors des limites du Vecteur, il doit être compris entre 0 et 2");
            }
        }

        public Vector3D(double xyz) => (X, Y, Z) = (xyz, xyz, xyz);
        public Vector3D(double x, double y) => (X, Y, Z) = (x, y, 0);
        public Vector3D(double x, double y, double z) => (X, Y, Z) = (x, y, z);
        public Vector3D(Vector2D xy) => (X, Y, Z) = (xy.X, xy.Y, 0);
        public Vector3D(Vector2D xy, double z) => (X, Y, Z) = (xy.X, xy.Y, z);
        public Vector3D(double x, Vector2D yz) => (X, Y, Z) = (x, yz.X, yz.Y);


        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);
        public double SquaredLength => X * X + Y * Y + Z * Z;

        public static double Distance(Vector3D vector1, Vector3D vector2) => Math.Abs((vector1 - vector2).Length);
        public static double SquaredDistance(Vector3D vector1, Vector3D vector2) => Math.Abs((vector1 - vector2).SquaredLength);
        public static double Dot(Vector3D vector1, Vector3D vector2) => vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        public static double Angle(Vector3D vector1, Vector3D vector2) => Math.Acos((vector1.Normalize() * vector2.Normalize()).Length) * 180D / Math.PI;
        //public static double SignedAngle(Vector3D vector1, Vector3D vector2) => ;
        public static Vector3D Cross(Vector3D vector1, Vector3D vector2) => new Vector3D(vector1.Y * vector2.Z - vector1.Z * vector2.Y, vector1.Z * vector2.X - vector1.X * vector2.Z, vector1.X * vector2.Y - vector1.Y * vector2.X);

        public Vector3D Normalize()
        {
            if (this == Zero) return this;
            else return this / Length;
        } 

        public static Vector3D Zero => new Vector3D(0.0d);
        public static Vector3D One => new Vector3D(1.0d);
        public static Vector3D Up => new Vector3D(0.0d, 1.0d, 0.0d);
        public static Vector3D Down => new Vector3D(0.0d, -1.0d, 0.0d);
        public static Vector3D Right => new Vector3D(1.0d, 0.0d, 0.0d);
        public static Vector3D Left => new Vector3D(-1.0d, 0.0d, 0.0d);
        public static Vector3D Forward => new Vector3D(0.0d, 0.0d, 1.0d);
        public static Vector3D Backward => new Vector3D(0.0d, 0.0d, -1.0d);

        public override string ToString() => $"Vector3D ({X}; {Y}; {Z})";

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj) => base.Equals(obj);


        public static bool operator ==(Vector3D vector1, Vector3D vector2) => vector1.XY == vector2.XY && vector1.Z == vector2.Z;
        public static bool operator !=(Vector3D vector1, Vector3D vector2) => !(vector1 == vector2);
        public static Vector3D operator +(Vector3D vector1, Vector3D vector2) => new Vector3D(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);
        public static Vector3D operator -(Vector3D vector) => new Vector3D(vector.X * -1.0d, vector.Y * -1.0d, vector.Z * -1.0d);
        public static Vector3D operator -(Vector3D vector1, Vector3D vector2) => new Vector3D(vector1.X - vector2.X, vector1.Y + vector2.Y, vector1.Z - vector2.Y);
        public static Vector3D operator *(Vector3D vector, double n) => new Vector3D(vector.X * n, vector.Y * n, vector.Z * n);
        public static Vector3D operator *(Vector3D vector1, Vector3D vector2) => new Vector3D(vector1.X * vector2.X, vector1.Y * vector2.Y, vector1.Z * vector2.Z);
        public static Vector3D operator /(Vector3D vector, double n) => new Vector3D(vector.X / n, vector.Y / n, vector.Z / n);
        public static Vector3D operator /(Vector3D vector1, Vector3D vector2) => new Vector3D(vector1.X / vector2.X, vector1.Y / vector2.Y, vector1.Z / vector2.Z);
        public static implicit operator Vector3(Vector3D vector) => new Vector3((float)vector.X, (float)vector.Y, (float)vector.Z);
    }
}
