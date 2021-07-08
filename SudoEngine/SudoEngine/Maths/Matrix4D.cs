using System;
using OpenTK;

namespace SudoEngine.Maths
{
    public struct Matrix4D
    {
        public Vector4D Row0 { get; set; }
        public Vector4D Row1 { get; set; }
        public Vector4D Row2 { get; set; }
        public Vector4D Row3 { get; set; }
        public Vector4D this[int i]
        {
            get
            {
                if (i == 0) return Row0;
                if (i == 1) return Row1;
                if (i == 2) return Row2;
                if (i == 3) return Row3;
                throw new IndexOutOfRangeException("L'index était hors des limites de la matrice, il doit être compris entre 0 et 3");
            }
            set
            {
                switch (i)
                {
                    case 0:
                        Row0 = value;
                        return;
                    case 1:
                        Row1 = value;
                        return;
                    case 2:
                        Row2 = value;
                        return;
                    case 3:
                        Row3 = value;
                        return;
                }
                throw new IndexOutOfRangeException("L'index était hors des limites de la matrice, il doit être compris entre 0 et 3");
            }
        }
        public double this[int i, int j]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return Row0[j];
                    case 1:
                        return Row1[j];
                    case 2:
                        return Row2[j];
                    case 3:
                        return Row3[j];
                }
                throw new IndexOutOfRangeException("L'index était hors des limites de la matrice, il doit être compris entre 0 et 3");
            }
            set
            {

                switch (i)
                {
                    case 0:
                        switch (j)
                        {
                            case 0:
                                Row0 = new Vector4D(value, Row0.Y, Row0.Z, Row0.W);
                                return;
                            case 1:
                                Row0 = new Vector4D(Row0.X, value, Row0.Z, Row0.W);
                                return;
                            case 2:
                                Row0 = new Vector4D(Row0.W, Row0.Y, value, Row0.W);
                                return;
                            case 3:
                                Row0 = new Vector4D(Row0.X, Row0.Y, Row0.Z, value);
                                return;
                        }
                        return;
                    case 1:
                        switch (j)
                        {
                            case 0:
                                Row1 = new Vector4D(value, Row1.Y, Row1.Z, Row1.W);
                                return;
                            case 1:
                                Row1 = new Vector4D(Row1.X, value, Row1.Z, Row1.W);
                                return;
                            case 2:
                                Row1 = new Vector4D(Row1.W, Row1.Y, value, Row1.W);
                                return;
                            case 3:
                                Row1 = new Vector4D(Row1.X, Row1.Y, Row1.Z, value);
                                return;
                        }
                        return;
                    case 2:
                        switch (j)
                        {
                            case 0:
                                Row2 = new Vector4D(value, Row2.Y, Row2.Z, Row2.W);
                                return;
                            case 1:
                                Row2 = new Vector4D(Row2.X, value, Row2.Z, Row2.W);
                                return;
                            case 2:
                                Row2 = new Vector4D(Row2.W, Row2.Y, value, Row2.W);
                                return;
                            case 3:
                                Row2 = new Vector4D(Row2.X, Row2.Y, Row2.Z, value);
                                return;
                        }
                        return;
                    case 3:
                        switch (j)
                        {
                            case 0:
                                Row3 = new Vector4D(value, Row3.Y, Row0.Z, Row3.W);
                                return;
                            case 1:
                                Row3 = new Vector4D(Row3.X, value, Row3.Z, Row3.W);
                                return;
                            case 2:
                                Row3 = new Vector4D(Row3.W, Row3.Y, value, Row3.W);
                                return;
                            case 3:
                                Row3 = new Vector4D(Row3.X, Row3.Y, Row3.Z, value);
                                return;
                        }
                        return;
                }
                throw new IndexOutOfRangeException("L'index était hors des limites de la matrice, il doit être compris entre 0 et 3");
            }
        }

        public Vector4D Col0
        {
            get => new Vector4D(Row0.X, Row1.X, Row2.X, Row3.X);
            set
            {
                Row0 = new Vector4D(value.X, Row0.Y, Row0.Z, Row0.W);
                Row1 = new Vector4D(value.Y, Row1.Y, Row1.Z, Row1.W);
                Row2 = new Vector4D(value.Z, Row2.Y, Row2.Z, Row0.W);
                Row3 = new Vector4D(value.W, Row3.Y, Row3.Z, Row3.W);
            }
        }

        public Vector4D Col1
        {
            get => new Vector4D(Row0.Y, Row1.Y, Row2.Y, Row3.Y);
            set
            {
                Row0 = new Vector4D(Row0.X, value.X, Row0.Z, Row0.W);
                Row1 = new Vector4D(Row1.X, value.Y, Row1.Z, Row1.W);
                Row2 = new Vector4D(Row2.X, value.Z, Row2.Z, Row0.W);
                Row3 = new Vector4D(Row3.X, value.W, Row3.Z, Row3.W);
            }
        }

        public Vector4D Col2
        {
            get => new Vector4D(Row0.Z, Row1.Z, Row2.Z, Row3.Z);
            set
            {
                Row0 = new Vector4D(Row0.X, Row0.Y, value.X, Row0.W);
                Row1 = new Vector4D(Row1.X, Row1.Y, value.Y, Row1.W);
                Row2 = new Vector4D(Row2.X, Row2.Y, value.Z, Row0.W);
                Row3 = new Vector4D(Row3.X, Row3.Y, value.W, Row3.W);
            }
        }

        public Vector4D Col3
        {
            get => new Vector4D(Row0.W, Row1.W, Row2.W, Row3.W);
            set
            {
                Row0 = new Vector4D(Row0.X, Row0.Y, Row0.Z, value.X);
                Row1 = new Vector4D(Row1.X, Row1.Y, Row1.Z, value.Y);
                Row2 = new Vector4D(Row2.X, Row2.Y, Row2.Z, value.Z);
                Row3 = new Vector4D(Row3.X, Row3.Y, Row3.Z, value.W);
            }
        }

        public Vector4D Diag0
        {
            get => new Vector4D(Row0.X, Row1.Y, Row2.Z, Row3.W);
            set
            {
                Row0 = new Vector4D(value.X, Row0.Y, Row0.Z, Row0.W);
                Row1 = new Vector4D(Row1.X, value.Y, Row1.Z, Row1.W);
                Row2 = new Vector4D(Row2.X, Row2.Y, value.Z, Row2.W);
                Row3 = new Vector4D(Row3.X, Row3.Y, Row3.Z, value.W);
            }
        }

        public Vector4D Diag1
        {
            get => new Vector4D(Row0.W, Row1.Z, Row2.Y, Row3.X);
            set
            {
                Row0 = new Vector4D(Row0.X, Row0.Y, Row0.Z, value.W);
                Row1 = new Vector4D(Row1.X, Row1.Y, value.Z, Row1.W);
                Row2 = new Vector4D(Row2.X, value.Y, Row2.Z, Row2.W);
                Row3 = new Vector4D(value.X, Row3.Y, Row3.Z, Row3.W);
            }
        }

        public Matrix4D(Vector4D row0, Vector4D row1, Vector4D row2, Vector4D row3) => (Row0, Row1, Row2, Row3) = (row0, row1, row2, row3);
        public Matrix4D(double r00, double r01, double r02, double r03, double r10, double r11, double r12, double r13, double r20, double r21, double r22, double r23, double r30, double r31, double r32, double r33)
            => (Row0, Row1, Row2, Row3) = (new Vector4D(r00, r01, r02, r03), new Vector4D(r10, r11, r12, r13), new Vector4D(r20, r21, r22, r23), new Vector4D(r30, r31, r32, r33));

        public Matrix4D(double n) => (Row0, Row1, Row2, Row3) = (new Vector4D(n), new Vector4D(n), new Vector4D(n), new Vector4D(n));
        public Matrix4D(double[] array) => (Row0, Row1, Row2, Row3) = (new Vector4D(array[0], array[1], array[2], array[3]), new Vector4D(array[4], array[5], array[6], array[7]), new Vector4D(array[8], array[9], array[10], array[11]), new Vector4D(array[12], array[13], array[14], array[15]));

        public Matrix4D(double fovy, double aspect, double depthNear, double depthFar)
        {
            if (fovy <= 0.0D || fovy > Math.PI) throw new ArgumentOutOfRangeException(nameof(fovy));

            if (aspect <= 0) throw new ArgumentOutOfRangeException(nameof(aspect));
            if (depthNear <= 0) throw new ArgumentOutOfRangeException(nameof(depthNear));
            if (depthFar <= 0) throw new ArgumentOutOfRangeException(nameof(depthFar));

            double maxY = depthNear * Math.Tan(0.5D * fovy);
            double minY = -maxY;
            double minX = minY * aspect;
            double maxX = maxY * aspect;

            this = new Matrix4D(minX, maxX, minY, maxY, depthNear, depthFar);
        }

        public Matrix4D(double left, double right, double bottom, double top, double depthNear, double depthFar)
        {
            if (depthNear <= 0.0D) throw new ArgumentOutOfRangeException(nameof(depthNear));
            if (depthFar <= 0.0D) throw new ArgumentOutOfRangeException(nameof(depthFar));
            if (depthNear >= depthFar) throw new ArgumentOutOfRangeException(nameof(depthNear));

            double x = 2.0D * depthNear / (right - left);
            double y = 2.0D * depthNear / (top - bottom);
            double a = (right + left) / (right - left);
            double b = (top + bottom) / (top - bottom);
            double c = -(depthFar + depthNear) / (depthFar - depthNear);
            double d = -(2.0D * depthFar * depthNear) / (depthFar - depthNear);

            Row0 = new Vector4D(x, 0.0D, 0.0D, 0.0D);
            Row1 = new Vector4D(0.0D, y, 0.0D, 0.0D);
            Row2 = new Vector4D(a, b, c, -1.0D);
            Row3 = new Vector4D(0.0D, 0.0D, d, 0.0D);
        }

        public Matrix4D(Vector3D vector, bool scale = false)
        {
            this = Identity;
            if (scale)
            {
                this[0, 0] = vector.X;
                this[1, 1] = vector.Y;
                this[2, 2] = vector.Z;
            }
            else
            {
                this[3, 1] = vector.X;
                this[3, 2] = vector.Y;
                this[3, 3] = vector.Z;
            }
        }


        public Matrix4D(Quaternion rot)
        {
            Vector4D axisAngle = rot.AxisAngle(true);

            Vector3D axis = axisAngle.XYZ;
            double angle = axisAngle.W;

            // normalize and create a local copy of the vector.
            axis.Normalize();
            double axisX = axis.X, axisY = axis.Y, axisZ = axis.Z;

            // calculate angles
            double cos = Math.Cos(-angle);
            double sin = Math.Sin(-angle);
            double t = 1.0D - cos;

            // do the conversion math once
            double tXX = t * axisX * axisX;
            double tXY = t * axisX * axisY;
            double tXZ = t * axisX * axisZ;
            double tYY = t * axisY * axisY;
            double tYZ = t * axisY * axisZ;
            double tZZ = t * axisZ * axisZ;

            double sinX = sin * axisX;
            double sinY = sin * axisY;
            double sinZ = sin * axisZ;

            Row0 = new Vector4D(tXX + cos, tXY - sinZ, tXZ + sinY, 0.0D);
            Row1 = new Vector4D(tXY + sinZ, tYY + cos, tYZ - sinX, 0.0D);
            Row2 = new Vector4D(tXZ - sinY, tYZ + sinX, tZZ + cos, 0.0D);
            Row3 = new Vector4D(0.0d, 0.0d, 0.0d, 1.0d);
        }

        public Matrix4D(Vector3D eye, Vector3D target, Vector3D up)
        {
            Vector3D z = (eye - target).Normalize();
            Vector3D x = Vector3D.Cross(up, z).Normalize();
            Vector3D y = Vector3D.Cross(z, x).Normalize();

            this.Row0 = new Vector4D(x.X, y.X, z.X, 0.0D);
            this.Row1 = new Vector4D(x.Y, y.Y, z.Y, 0.0D);
            this.Row2 = new Vector4D(x.Z, y.Z, z.Z, 0.0D);
            this.Row3 = new Vector4D(
                -((x.X * eye.X) + (x.Y * eye.Y) + (x.Z * eye.Z)),
                -((y.X * eye.X) + (y.Y * eye.Y) + (y.Z * eye.Z)),
                -((z.X * eye.X) + (z.Y * eye.Y) + (z.Z * eye.Z)),
                1.0D);
        }

        public static Matrix4D Zero => new Matrix4D(0.0d);
        public static Matrix4D Identity => new Matrix4D(new Vector4D(1.0d, 0.0d, 0.0d, 0.0d), new Vector4D(0.0d, 1.0d, 0.0d, 0.0d), new Vector4D(0.0d, 0.0d, 1.0d, 0.0d), new Vector4D(0.0d, 0.0d, 0.0d, 1.0d));
        //public double 

        public double[] ToArray()
        {
            double[] array =
            {
                this[0, 0], this[0, 1], this[0, 2], this[0, 3],
                this[1, 0], this[1, 1], this[1, 2], this[1, 3],
                this[2, 0], this[2, 1], this[2, 2], this[2, 3],
                this[3, 0], this[3, 1], this[3, 2], this[3, 3]
            };
            return array;
        }

        public static Matrix4D Multiply(Matrix4D matrix1, Matrix4D matrix2) => new Matrix4D()
        {
            Row0 = new Vector4D(
                    matrix1[0, 0] * matrix2[0, 0] + matrix1[0, 1] * matrix2[1, 0] + matrix1[0, 2] * matrix2[2, 0] + matrix1[0, 3] * matrix2[3, 0],
                    matrix1[0, 0] * matrix2[0, 1] + matrix1[0, 1] * matrix2[1, 1] + matrix1[0, 2] * matrix2[2, 1] + matrix1[0, 3] * matrix2[3, 1],
                    matrix1[0, 0] * matrix2[0, 2] + matrix1[0, 1] * matrix2[1, 2] + matrix1[0, 2] * matrix2[2, 2] + matrix1[0, 3] * matrix2[3, 2],
                    matrix1[0, 0] * matrix2[0, 3] + matrix1[0, 1] * matrix2[1, 3] + matrix1[0, 2] * matrix2[2, 3] + matrix1[0, 3] * matrix2[3, 3]),
            Row1 = new Vector4D(
                    matrix1[1, 0] * matrix2[0, 0] + matrix1[1, 1] * matrix2[1, 0] + matrix1[1, 2] * matrix2[2, 0] + matrix1[1, 3] * matrix2[3, 0],
                    matrix1[1, 0] * matrix2[0, 1] + matrix1[1, 1] * matrix2[1, 1] + matrix1[1, 2] * matrix2[2, 1] + matrix1[1, 3] * matrix2[3, 1],
                    matrix1[1, 0] * matrix2[0, 2] + matrix1[1, 1] * matrix2[1, 2] + matrix1[1, 2] * matrix2[2, 2] + matrix1[1, 3] * matrix2[3, 2],
                    matrix1[1, 0] * matrix2[0, 3] + matrix1[1, 1] * matrix2[1, 3] + matrix1[1, 2] * matrix2[2, 3] + matrix1[1, 3] * matrix2[3, 3]),
            Row2 = new Vector4D(
                    matrix1[2, 0] * matrix2[0, 0] + matrix1[2, 1] * matrix2[1, 0] + matrix1[2, 2] * matrix2[2, 0] + matrix1[2, 3] * matrix2[3, 0],
                    matrix1[2, 0] * matrix2[0, 1] + matrix1[2, 1] * matrix2[1, 1] + matrix1[2, 2] * matrix2[2, 1] + matrix1[2, 3] * matrix2[3, 1],
                    matrix1[2, 0] * matrix2[0, 2] + matrix1[2, 1] * matrix2[1, 2] + matrix1[2, 2] * matrix2[2, 2] + matrix1[2, 3] * matrix2[3, 2],
                    matrix1[2, 0] * matrix2[0, 3] + matrix1[2, 1] * matrix2[1, 3] + matrix1[2, 2] * matrix2[2, 3] + matrix1[2, 3] * matrix2[3, 3]),
            Row3 = new Vector4D(
                    matrix1[3, 0] * matrix2[0, 0] + matrix1[3, 1] * matrix2[1, 0] + matrix1[3, 2] * matrix2[2, 0] + matrix1[3, 3] * matrix2[3, 0],
                    matrix1[3, 0] * matrix2[0, 1] + matrix1[3, 1] * matrix2[1, 1] + matrix1[3, 2] * matrix2[2, 1] + matrix1[3, 3] * matrix2[3, 1],
                    matrix1[3, 0] * matrix2[0, 2] + matrix1[3, 1] * matrix2[1, 2] + matrix1[3, 2] * matrix2[2, 2] + matrix1[3, 3] * matrix2[3, 2],
                    matrix1[3, 0] * matrix2[0, 3] + matrix1[3, 1] * matrix2[1, 3] + matrix1[3, 2] * matrix2[2, 3] + matrix1[3, 3] * matrix2[3, 3])
        };

        public override string ToString() => $"Matrix4D (Row0 : {Row0}; Row1 : {Row1}; Row2 : {Row2}; Row3 : {Row3})";
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(Matrix4D matrix1, Matrix4D matrix2) => matrix1.Row0 == matrix2.Row0 && matrix1.Row1 == matrix2.Row1 && matrix1.Row2 == matrix2.Row2 && matrix1.Row3 == matrix2.Row3;
        public static bool operator !=(Matrix4D matrix1, Matrix4D matrix2) => !(matrix1 == matrix2);
        public static Matrix4D operator +(Matrix4D matrix1, Matrix4D matrix2) => new Matrix4D(matrix1.Row0 + matrix2.Row0, matrix1.Row1 + matrix2.Row1, matrix1.Row2 + matrix2.Row2, matrix1.Row3 + matrix2.Row3);
        public static Matrix4D operator -(Matrix4D matrix) => new Matrix4D(-matrix.Row0, -matrix.Row1, -matrix.Row2, -matrix.Row3);
        public static Matrix4D operator -(Matrix4D matrix1, Matrix4D matrix2) => new Matrix4D(matrix1.Row0 - matrix2.Row0, matrix1.Row1 - matrix2.Row1, matrix1.Row2 - matrix2.Row2, matrix1.Row3 - matrix2.Row3);
        public static Matrix4D operator *(Matrix4D matrix1, Matrix4D matrix2) => Multiply(matrix1, matrix2);

        public static implicit operator Matrix4(Matrix4D matrix) => new Matrix4(matrix.Row0, matrix.Row1, matrix.Row2, matrix.Row3);
        public static implicit operator Matrix4D(Matrix4 matrix) => new Matrix4D(matrix.Row0, matrix.Row1, matrix.Row2, matrix.Row3);
    }
}
