using Newtonsoft.Json;
using System;

namespace SudoEngine.Maths
{
    public struct Quaternion
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        /// <summary>
        /// A no-rotation quaternion (0.0, 0.0, 0.0, 1.0)
        /// </summary>
        public static Quaternion Identity => new Quaternion(0.0D, 0.0D, 0.0D, 1.0D);

        /// <summary>
        /// The length of that quaternion.
        /// </summary>
        [JsonIgnore]
        public double Length =>  Math.Sqrt(LengthSquared);

        /// <summary>
        /// The squared length of that quaternion. Faster than <see cref="Length"/> but has to be squared rooted.
        /// </summary>
        [JsonIgnore]
        public double LengthSquared => new Vector4D(X, Y, Z, W).Length;

        /// <summary>
        /// Get or set the euler rotation (degree) of this quaternion.
        /// </summary>
        [JsonIgnore]
        public Vector3D Euler
        {
            get
            {
                Quaternion q = this;
                Vector3D angles = new Vector3D(0.0D);

                // roll (X-axis rotation)
                double sinr_cosp = 2.0D * (q.W * q.X + q.Y * q.Z);
                double cosr_cosp = 1.0D - 2.0D * (q.X * q.X + q.Y * q.Y);
                angles.X = Math.Atan2(sinr_cosp, cosr_cosp);

                // pitch (Y-axis rotation)
                double sinp = 2.0D * (q.W * q.Y - q.Z * q.X);
                if (Math.Abs(sinp) >= 1.0D)
                    angles.Y = FMath.CopySign(Math.PI / 2.0D, sinp); // use 90 degrees if out of range
                else
                    angles.Y = Math.Asin(sinp);

                // yaw (Z-axis rotation)
                double siny_cosp = 2.0D * (q.W * q.Z + q.X * q.Y);
                double cosy_cosp = 1.0D - 2.0D * (q.Y * q.Y + q.Z * q.Z);
                angles.Z = Math.Atan2(siny_cosp, cosy_cosp);

                angles *= 180d / Math.PI;

                return angles;
            }
            set
            {
                this = new Quaternion(value);
            }
        }

        public Quaternion Normalized => NormalizeQuaternion(this);
        public Quaternion Conjugated => ConjugateQuaternion(this);
        public Quaternion Inverted => InverseQuaternion(this);


        public Quaternion(double x, double y, double z, double w) => (X, Y, Z, W) = (x, y, z, w);

        public Quaternion(Vector3D axis, double angle)
        {
            angle *= Math.PI / 180d;

            double halfAngle = angle * 0.5D;
            double s = Math.Sin(halfAngle);
            double c = Math.Cos(halfAngle);

            axis.Normalize();

            X = axis.X * s;
            Y = axis.Y * s;
            Z = axis.Z * s;
            W = c;
        }

        public Quaternion(Vector3D angles) : this(angles.X, angles.Y, angles.Z) { }

        public Quaternion(double xAngle, double yAngle, double zAngle)
        {
            xAngle *= (Math.PI / 180d);
            yAngle *= (Math.PI / 180d);
            zAngle *= (Math.PI / 180d);

            //  Roll first, about axis the object is facing, then
            //  pitch upward, then yaw to face into the new heading
            double sr, cr, sp, cp, sy, cy;

            double halfRoll = zAngle * 0.5D;
            sr = Math.Sin(halfRoll);
            cr = Math.Cos(halfRoll);

            double halfPitch = xAngle * 0.5D;
            sp = Math.Sin(halfPitch);
            cp = Math.Cos(halfPitch);

            double halfYaw = yAngle * 0.5D;
            sy = Math.Sin(halfYaw);
            cy = Math.Cos(halfYaw);

            X = cy * sp * cr + sy * cp * sr;
            Y = sy * cp * cr - cy * sp * sr;
            Z = cy * cp * sr - sy * sp * cr;
            W = cy * cp * cr + sy * sp * sr;
        }

        public static Quaternion FromCross(Vector3D up, Vector3D direction)
        {
            up.Normalize();
            direction.Normalize();

            double dot = up.X * direction.X + up.Y * direction.Y + up.Z * direction.Z;
            double angle = Math.Acos(FMath.Clamp(dot, -1, 1));

            return new Quaternion(
                new Vector3D(up.Y * direction.Z - up.Z * direction.Y, up.Z * direction.X - up.X * direction.Z, up.X * direction.Y - up.Y * direction.X),
                angle * (180D / Math.PI)).Normalized;
        }

        public Vector4D AxisAngle(bool radian = false)
        {
            Vector4D q = (Vector4D)this;
            if (Math.Abs(q.W) > 1.0f)
            {
                q.Normalize();
            }

            Vector4D result = new Vector4D
            {
                W = 2.0D * Math.Acos(q.W) * (radian ? 1.0D : 180D / Math.PI) // angle
            };

            double den = Math.Sqrt(1.0D - (q.W * q.W));
            if (den > 0.0001D)
            {
                result.XYZ = q.XYZ / den;
            }
            else
            {
                // This occurs when the angle is zero.
                // Not a problem: just set an arbitrary normalized axis.
                result.XYZ = Vector3D.Up;
            }

            return result;
        }

        public Quaternion Normalize() => this = NormalizeQuaternion(this);

        public Quaternion Conjugate() => this = ConjugateQuaternion(this);

        public Quaternion Inverse() => this = InverseQuaternion(this);

        private static Quaternion NormalizeQuaternion(Quaternion q)
        {
            double inv = 1.0D / q.Length;

            q.X *= inv;
            q.Y *= inv;
            q.Z *= inv;
            q.W *= inv;

            return q;
        }
        private static Quaternion ConjugateQuaternion(Quaternion q)
        {
            q.X = -q.X;
            q.Y = -q.Y;
            q.Z = -q.Z;

            return q;
        }

        public static Quaternion InverseQuaternion(Quaternion q)
        {
            double inv = 1.0D / q.Length;

            q.X = -q.X * inv;
            q.Y = -q.Y * inv;
            q.Z = -q.Z * inv;
            q.W *= inv;

            return q;
        }

        public static Quaternion Slerp(Quaternion q1, Quaternion q2, double t)
        {
            double cosOmega = q1.X * q2.X + q1.Y * q2.Y +
                             q1.Z * q2.Z + q1.W * q2.W;

            bool flip = false;

            if (cosOmega < 0.0D)
            {
                flip = true;
                cosOmega = -cosOmega;
            }

            double s1, s2;

            if (cosOmega > (1.0D - double.Epsilon))
            {
                // Too close, do straight linear interpolation.
                s1 = 1.0D - t;
                s2 = (flip) ? -t : t;
            }
            else
            {
                double omega = Math.Acos(cosOmega);
                double invSinOmega = 1.0D / Math.Sin(omega);

                s1 = Math.Sin((1.0D - t) * omega) * invSinOmega;
                s2 = (flip)
                    ? -Math.Sin(t * omega) * invSinOmega
                    : Math.Sin(t * omega) * invSinOmega;
            }

            Quaternion ans = new Quaternion
            {
                X = s1 * q1.X + s2 * q2.X,
                Y = s1 * q1.Y + s2 * q2.Y,
                Z = s1 * q1.Z + s2 * q2.Z,
                W = s1 * q1.W + s2 * q2.W
            };

            return ans;
        }

        public static Quaternion Lerp(Quaternion q1, Quaternion q2, double t)
        {
            double t1 = 1.0D - t;

            Quaternion r = new Quaternion();

            double dot = q1.X * q2.X + q1.Y * q2.Y +
                        q1.Z * q2.Z + q1.W * q2.W;

            if (dot >= 0.0D)
            {
                r.X = t1 * q1.X + t * q2.X;
                r.Y = t1 * q1.Y + t * q2.Y;
                r.Z = t1 * q1.Z + t * q2.Z;
                r.W = t1 * q1.W + t * q2.W;
            }
            else
            {
                r.X = t1 * q1.X - t * q2.X;
                r.Y = t1 * q1.Y - t * q2.Y;
                r.Z = t1 * q1.Z - t * q2.Z;
                r.W = t1 * q1.W - t * q2.W;
            }

            // Normalize it.
            double ls = r.Length;
            double invNorm = 1.0D / Math.Sqrt(ls);

            r.X *= invNorm;
            r.Y *= invNorm;
            r.Z *= invNorm;
            r.W *= invNorm;

            return r;
        }

        public static double Angle(Quaternion a, Quaternion b)
        {
            double dot = Dot(a, b);
            return IsEqualUsingDot(dot) ? 0.0D : Math.Acos(Math.Min(Math.Abs(dot), 1.0D)) * 2.0D * 180D / Math.PI;
        }

        public static double Dot(Quaternion a, Quaternion b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }

        private const double kEpsilon = 0.000001D;

        private static bool IsEqualUsingDot(double dot)
        {
            return dot > 1.0D - kEpsilon;
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            return new Quaternion(
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X,
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z);
        }

        public static double AngleY(Quaternion a, Quaternion b)
        {
            Vector3D aDir = a * Vector3D.Forward;
            Vector3D bDir = b * Vector3D.Forward;

            double aAngle = Math.Atan2(aDir.X, aDir.Z) * 180D / Math.PI;
            double bAngle = Math.Atan2(bDir.X, bDir.Z) * 180D / Math.PI;

            return FMath.DeltaAngle(aAngle, bAngle);
        }

        public override string ToString() => $"Quaternion ({Math.Round(X, 4)}; {Math.Round(Y, 4)}; {Math.Round(Z, 4)}; {Math.Round(W, 4)})";


        public static Vector3D operator *(Quaternion rotation, Vector3D point)
        {
            double X = rotation.X * 2.0D;
            double Y = rotation.Y * 2.0D;
            double Z = rotation.Z * 2.0D;
            double XX = rotation.X * X;
            double YY = rotation.Y * Y;
            double ZZ = rotation.Z * Z;
            double XY = rotation.X * Y;
            double XZ = rotation.X * Z;
            double YZ = rotation.Y * Z;
            double WX = rotation.W * X;
            double WY = rotation.W * Y;
            double WZ = rotation.W * Z;

            Vector3D res = Vector3D.Zero;
            res.X = (1.0D - (YY + ZZ)) * point.X + (XY - WZ) * point.Y + (XZ + WY) * point.Z;
            res.Y = (XY + WZ) * point.X + (1.0D - (XX + ZZ)) * point.Y + (YZ - WX) * point.Z;
            res.Z = (XZ - WY) * point.X + (YZ + WX) * point.Y + (1.0D - (XX + YY)) * point.Z;
            return res;
        }


        public static explicit operator Vector4D(Quaternion q) => new Vector4D(q.X, q.Y, q.Z, q.W);
    }
}
