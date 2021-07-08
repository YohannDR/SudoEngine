using System;
using OpenTK;

namespace SudoEngine.Maths
{
    public struct Vector4D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Vector2D XY
        {
            get => new Vector2D(X, Y);
            set => (X, Y) = (value.X, value.Y);
        }
        public Vector2D XZ
        {
            get => new Vector2D(X, Z);
            set => (X, Z) = (value.X, value.Y);
        }
        public Vector2D XW
        {
            get => new Vector2D(X, W);
            set => (X, W) = (value.X, value.Y);
        }

        public Vector2D YX
        {
            get => new Vector2D(Y, X);
            set => (Y, X) = (value.X, value.Y);
        }
        public Vector2D YZ
        {
            get => new Vector2D(Y, Z);
            set => (Y, Z) = (value.X, value.Y);
        }
        public Vector2D YW
        {
            get => new Vector2D(Y, W);
            set => (Y, W) = (value.X, value.Y);
        }

        public Vector2D ZX
        {
            get => new Vector2D(Z, X);
            set => (Z, X) = (value.X, value.Y);
        }
        public Vector2D ZY
        {
            get => new Vector2D(Z, Y);
            set => (Z, Y) = (value.X, value.Y);
        }
        public Vector2D ZW
        {
            get => new Vector2D(Z, W);
            set => (Z, W) = (value.X, value.Y);
        }

        public Vector2D WX
        {
            get => new Vector2D(W, X);
            set => (W, X) = (value.X, value.Y);
        }
        public Vector2D WY
        {
            get => new Vector2D(W, Y);
            set => (W, Y) = (value.X, value.Y);
        }
        public Vector2D WZ
        {
            get => new Vector2D(W, Z);
            set => (W, Z) = (value.X, value.Y);
        }


        public Vector3D XYZ
        {
            get => new Vector3D(X, Y, Z);
            set => (X, Y, Z) = (value.X, value.Y, value.Z);
        }
        public Vector3D YZW
        {
            get => new Vector3D(Y, Z, W);
            set => (Y, Z, W) = (value.X, value.Y, value.Z);
        }

        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                }
                throw new IndexOutOfRangeException("L'index était hors des limites du Vecteur, il doit être compris entre 0 et 3");
            }
            set
            {
                switch (i)
                {
                    case 0:
                        X = value;
                        return;
                    case 1:
                        Y = value;
                        return;
                    case 2:
                        Z = value;
                        return;
                    case 3:
                        W = value;
                        return;
                }
                throw new IndexOutOfRangeException("L'index était hors des limites du Vecteur, il doit être compris entre 0 et 3");
            }
        }

        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        public double SquaredLength => X * X + Y * Y + Z * Z + W * W;


        public Vector4D(double xyzw) => (X, Y, Z, W) = (xyzw, xyzw, xyzw, xyzw);
        public Vector4D(double x, double y, double z, double w) => (X, Y, Z, W) = (x, y, z, w);
        public Vector4D(Vector2D xy, Vector2D zw) => (X, Y, Z, W) = (xy.X, xy.Y, zw.X, zw.Y);
        public Vector4D(Vector2D xy, double z, double w) => (X, Y, Z, W) = (xy.X, xy.Y, z, w);
        public Vector4D(Vector3D xyz, double w) => (X, Y, Z, W) = (xyz.X, xyz.Y, xyz.Z, w);
        public Vector4D(double x, Vector3D yzw) => (X, Y, Z, W) = (x, yzw.X, yzw.Y, yzw.Z);

        public Vector4D Zero => new Vector4D(0.0d);

        public static double Distance(Vector4D vector1, Vector4D vector2) => Math.Abs((vector1 - vector2).Length);
        public double Multiply() => X * Y * Z * W;
        public Vector4D Normalize()
        {
            if (this == Zero) return this;
            else return this / Length;
        }

        public override string ToString() => $"Vector4D ({X}; {Y}; {Z}; {W})";
        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj) => base.Equals(obj);

        public static bool operator ==(Vector4D vector1, Vector4D vector2) => vector1.X == vector2.X && vector1.Y == vector2.Y && vector1.Z == vector2.Z && vector1.W == vector2.W;
        public static bool operator !=(Vector4D vector1, Vector4D vector2) => !(vector1 == vector2);
        public static Vector4D operator +(Vector4D vector1, Vector4D vector2) => new Vector4D(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z, vector1.W + vector2.W);
        public static Vector4D operator -(Vector4D vector) => new Vector4D(vector.X * -1.0d, vector.Y * -1.0d, vector.Z * -1.0d, vector.W * -1.0d);
        public static Vector4D operator -(Vector4D vector1, Vector4D vector2) => new Vector4D(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z, vector1.W - vector2.W);
        public static Vector4D operator *(Vector4D vector1, Vector4D vector2) => new Vector4D(vector1.X * vector2.X, vector1.Y * vector2.Y, vector1.Z * vector2.Z, vector1.W * vector2.W);
        public static Vector4D operator *(Vector4D vector, double n) => new Vector4D(vector.X * n, vector.Y * n, vector.Z * n, vector.W * n);
        public static Vector4D operator /(Vector4D vector1, Vector4D vector2) => new Vector4D(vector1.X / vector2.X, vector1.Y / vector2.Y, vector1.Z / vector2.Z, vector1.W / vector2.W);
        public static Vector4D operator /(Vector4D vector, double n) => new Vector4D(vector.X / n, vector.Y / n, vector.Z / n, vector.W / n);
        public static implicit operator Vector4(Vector4D vector) => new Vector4((float)vector.X, (float)vector.Y, (float)vector.Z, (float)vector.W);
        public static implicit operator Vector4D(Vector4 vector) => new Vector4D(vector.X, vector.Y, vector.Z, vector.W);
    }
}
