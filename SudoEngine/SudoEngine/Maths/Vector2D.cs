using System;

namespace SudoEngine.Maths
{
    public struct Vector2D
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double this[int i]
        {
            get
            {
                if (i == 0) return X;
                if (i == 1) return Y;
                throw new IndexOutOfRangeException("L'index était hors des limites du Vecteur, il doit être soit 0 ou 1");
            }

            set
            {
                if (i == 0) X = value;
                if (i == 1) Y = value;
                throw new IndexOutOfRangeException("L'index était hors des limites du Vecteur, il doit être soit 0 ou 1");
            }
        }

        /// <summary> Taile du vecteur  </summary>
        public double Length => Math.Sqrt(X * X + Y * Y);

        /// <summary> Racine carré de la taile du vecteur </summary>
        public double SquaredLength => X * X + Y * Y;

        /// <summary> Crée un Vecteur2D à partir d'un double </summary>
        public Vector2D(double xy) => (X, Y) = (xy, xy);

        /// <summary> Crée un Vecteur2D à partir de deux double </summary>
        public Vector2D(double x, double y) => (X, Y) = (x, y);

        /// <summary> Normalise le vecteur </summary>
        public Vector2D Normalize()
        {
            if (this == Zero) return this;
            else return this / Length;
        }

        /// <summary> Calcule la distance entre 2 vecteurs </summary>
        public static double Distance(Vector2D vector1, Vector2D vector2) => Math.Abs((vector1 - vector2).Length);
        /// <summary> Calcule la distance au carré entre 2 vecteurs </summary>
        public static double SquaredDistance(Vector2D vector1, Vector2D vector2) => Math.Abs((vector1 - vector2).SquaredLength);
        /// <summary> Multiple 2 vecteurs </summary>
        public static double Dot(Vector2D vector1, Vector2D vector2) => vector1.X * vector2.X + vector1.Y * vector2.Y;
        /// <summary> Calcule l'angle entre 2 vecteurs </summary>
        public static double Angle(Vector2D vector1, Vector2D vector2) => Math.Acos((vector1.Normalize() * vector2.Normalize()).Length) * (180.0D / Math.PI);
        /// <summary> Calcule l'angle signé entre 2 vecteurs </summary>
        public static double SignedAngle(Vector2D vector1, Vector2D vector2) => Angle(vector1, vector2) * Math.Sign(vector1.X * vector2.Y - vector1.Y * vector2.X);

        /// <summary> Crée un vecteur 0 </summary>
        public static Vector2D Zero => new Vector2D(0.0d);
        /// <summary> Crée un vecteur qui pointe vers le haut </summary>
        public static Vector2D Up => new Vector2D(0.0d, 1.0d);
        /// <summary> Crée un vecteur qui pointe vers le bas </summary>
        public static Vector2D Down => new Vector2D(0.0d, -1.0d);
        /// <summary> Crée un vecteur qui pointe vers la gauche </summary>
        public static Vector2D Right => new Vector2D(1.0d, 0.0d);
        /// <summary> Crée un vecteur qui pointe vers la droite </summary>
        public static Vector2D Left => new Vector2D(-1.0d, 0.0d);

        public bool Equals(Vector2D vector) => X == vector.X && Y == vector.Y;
        public override bool Equals(object obj) => obj is Vector2D v && Equals(v);

        public override string ToString() => $"Vector2D ({X}; {Y})";

        public override int GetHashCode() => base.GetHashCode();


        public static bool operator ==(Vector2D vector1, Vector2D vector2) => vector1.Equals(vector2);
        public static bool operator !=(Vector2D vector1, Vector2D vector2) => !vector1.Equals(vector2);
        public static Vector2D operator +(Vector2D vector1, Vector2D vector2) => new Vector2D(vector1.X + vector2.X, vector1.Y + vector2.Y);
        public static Vector2D operator -(Vector2D vector) => new Vector2D(vector.X * -1.0, vector.Y * -1.0);
        public static Vector2D operator -(Vector2D vector1, Vector2D vector2) => new Vector2D(vector1.X - vector2.X, vector1.Y - vector2.Y);
        public static Vector2D operator *(Vector2D vector, double n) => new Vector2D(vector.X * n, vector.Y * n);
        public static Vector2D operator *(double n, Vector2D vector) => vector * n;
        public static Vector2D operator *(Vector2D vector1, Vector2D vector2) => new Vector2D(vector1.X * vector2.X, vector1.Y * vector2.Y);
        public static Vector2D operator /(Vector2D vector, double n) => new Vector2D(vector.X / n, vector.Y / n);
        public static Vector2D operator /(Vector2D vector1, Vector2D vector2) => new Vector2D(vector1.X / vector2.X, vector1.Y / vector2.Y);
        public static implicit operator OpenTK.Vector2(Vector2D vector) => new OpenTK.Vector2((float)vector.X, (float)vector.Y);
    }
}
