using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MathModule
{
    /// <summary>
    /// Represents a 4x4 mathematical matrix
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix : IEquatable<Matrix>
    {
        /// <summary>
        /// A matrix with all of its components set to zero
        /// </summary>
        public static readonly Matrix Zero = new Matrix();

        /// <summary>
        /// The identity matrix
        /// </summary>
        public static readonly Matrix Identity => new Matrix(M11 = 1f, M22 = 1f, M33 = 1f, M44 = 1f);

        /// <summary>
        /// Value at row 1 column 1 of the matrix
        /// </summary>
        public float M11 { get; set; }

        /// <summary>
        /// Value at row 1 column 2 of the matrix
        /// </summary>
        public float M12 { get; set; }

        /// <summary>
        /// Value at row 1 column 3 of the matrix
        /// </summary>
        public float M13 { get; set; }

        /// <summary>
        /// Value at row 1 column 4 of the matrix
        /// </summary>
        public float M14 { get; set; }

        /// <summary>
        /// Value at row 2 column 1 of the matrix
        /// </summary>
        public float M21 { get; set; }

        /// <summary>
        /// Value at row 2 column 2 of the matrix
        /// </summary>
        public float M22 { get; set; }

        /// <summary>
        /// Value at row 2 column 3 of the matrix
        /// </summary>
        public float M23 { get; set; }

        /// <summary>
        /// Value at row 2 column 4 of the matrix
        /// </summary>
        public float M24 { get; set; }

        /// <summary>
        /// Value at row 3 column 1 of the matrix
        /// </summary>
        public float M31 { get; set; }

        /// <summary>
        /// Value at row 3 column 2 of the matrix
        /// </summary>
        public float M32 { get; set; }

        /// <summary>
        /// Value at row 3 column 3 of the matrix
        /// </summary>
        public float M33 { get; set; }

        /// <summary>
        /// Value at row 3 column 4 of the matrix
        /// </summary>
        public float M34 { get; set; }

        /// <summary>
        /// Value at row 4 column 1 of the matrix
        /// </summary>
        public float M41 { get; set; }

        /// <summary>
        /// Value at row 4 column 2 of the matrix
        /// </summary>
        public float M42 { get; set; }

        /// <summary>
        /// Value at row 4 column 3 of the matrix
        /// </summary>
        public float M43 { get; set; }

        /// <summary>
        /// Value at row 4 column 4 of the matrix
        /// </summary>
        public float M44 { get; set; }

        /// <summary>
        /// Initializes a new instance of the matrix struct
        /// </summary>
        /// <param name="M11">The value to assign at row 1 column 1 of the matrix</param>
        /// <param name="M12">The value to assign at row 1 column 2 of the matrix</param>
        /// <param name="M13">The value to assign at row 1 column 3 of the matrix</param>
        /// <param name="M14">The value to assign at row 1 column 4 of the matrix</param>
        /// <param name="M21">The value to assign at row 2 column 1 of the matrix</param>
        /// <param name="M22">The value to assign at row 2 column 2 of the matrix</param>
        /// <param name="M23">The value to assign at row 2 column 3 of the matrix</param>
        /// <param name="M24">The value to assign at row 2 column 4 of the matrix</param>
        /// <param name="M31">The value to assign at row 3 column 1 of the matrix</param>
        /// <param name="M32">The value to assign at row 3 column 2 of the matrix</param>
        /// <param name="M33">The value to assign at row 3 column 3 of the matrix</param>
        /// <param name="M34">The value to assign at row 3 column 4 of the matrix</param>
        /// <param name="M41">The value to assign at row 4 column 1 of the matrix</param>
        /// <param name="M42">The value to assign at row 4 column 2 of the matrix</param>
        /// <param name="M43">The value to assign at row 4 column 3 of the matrix</param>
        /// <param name="M44">The value to assign at row 4 column 4 of the matrix</param>
        public Matrix(float M11, float M12, float M13, float M14,
            float M21, float M22, float M23, float M24,
            float M31, float M32, float M33, float M34,
            float M41, float M42, float M43, float M44)
        {
            this.M11 = M11;
            this.M12 = M12;
            this.M13 = M13;
            this.M14 = M14;
            this.M21 = M21;
            this.M22 = M22;
            this.M23 = M23;
            this.M24 = M24;
            this.M31 = M31;
            this.M32 = M32;
            this.M33 = M33;
            this.M34 = M34;
            this.M41 = M41;
            this.M42 = M42;
            this.M43 = M43;
            this.M44 = M44;
        }

        /// <summary>
        /// Initializes a new instance of the matrix struct
        /// </summary>
        /// <param name="values">The values to assign to the components of the matrix. This must be an array with sixteen elements</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than sixteen elements</exception>
        public Matrix(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length != 16)
            {
                throw new ArgumentOutOfRangeException("values", "There must be sixteen and only sixteen input values for Matrix.");
            }

            M11 = values[0];
            M12 = values[1];
            M13 = values[2];
            M14 = values[3];

            M21 = values[4];
            M22 = values[5];
            M23 = values[6];
            M24 = values[7];

            M31 = values[8];
            M32 = values[9];
            M33 = values[10];
            M34 = values[11];

            M41 = values[12];
            M42 = values[13];
            M43 = values[14];
            M44 = values[15];
        }

        /// <summary>
        /// gets or sets the basis matrix for the rotation
        /// </summary>
        public Matrix Basis
        {
            get
            {
                return new Matrix(
                    M11, M12, M13, 0,
                    M21, M22, M23, 0,
                    M31, M32, M33, 0,
                    0, 0, 0, 1);
            }
            set
            {
                M11 = value.M11;
                M12 = value.M12;
                M13 = value.M13;
                M21 = value.M21;
                M22 = value.M22;
                M23 = value.M23;
                M31 = value.M31;
                M32 = value.M32;
                M33 = value.M33;
            }
        }

        /// <summary>
        /// gets or sets the first row in the matrix; that is M11, M12, M13, and M14
        /// </summary>
        public Vector4D Row1
        {
            get { return new Vector4D(M11, M12, M13, M14); }
            set { M11 = value.X; M12 = value.Y; M13 = value.Z; M14 = value.W; }
        }

        /// <summary>
        /// gets or sets the second row in the matrix; that is M21, M22, M23, and M24
        /// </summary>
        public Vector4D Row2
        {
            get { return new Vector4D(M21, M22, M23, M24); }
            set { M21 = value.X; M22 = value.Y; M23 = value.Z; M24 = value.W; }
        }

        /// <summary>
        /// Gets or sets the third row in the matrix; that is M31, M32, M33, and M34
        /// </summary>
        public Vector4D Row3
        {
            get { return new Vector4D(M31, M32, M33, M34); }
            set { M31 = value.X; M32 = value.Y; M33 = value.Z; M34 = value.W; }
        }

        /// <summary>
        /// Gets or sets the fourth row in the matrix; that is M41, M42, M43, and M44
        /// </summary>
        public Vector4D Row4
        {
            get { return new Vector4D(M41, M42, M43, M44); }
            set { M41 = value.X; M42 = value.Y; M43 = value.Z; M44 = value.W; }
        }

        /// <summary>
        /// Gets or sets the first column in the matrix; that is M11, M21, M31, and M41
        /// </summary>
        public Vector4D Column1
        {
            get { return new Vector4D(M11, M21, M31, M41); }
            set { M11 = value.X; M21 = value.Y; M31 = value.Z; M41 = value.W; }
        }

        /// <summary>
        /// Gets or sets the second column in the matrix; that is M12, M22, M32, and M42
        /// </summary>
        public Vector4D Column2
        {
            get { return new Vector4D(M12, M22, M32, M42); }
            set { M12 = value.X; M22 = value.Y; M32 = value.Z; M42 = value.W; }
        }

        /// <summary>
        /// Gets or sets the third column in the matrix; that is M13, M23, M33, and M43
        /// </summary>
        public Vector4D Column3
        {
            get { return new Vector4D(M13, M23, M33, M43); }
            set { M13 = value.X; M23 = value.Y; M33 = value.Z; M43 = value.W; }
        }

        /// <summary>
        /// Gets or sets the fourth column in the matrix; that is M14, M24, M34, and M44
        /// </summary>
        public Vector4D Column4
        {
            get { return new Vector4D(M14, M24, M34, M44); }
            set { M14 = value.X; M24 = value.Y; M34 = value.Z; M44 = value.W; }
        }

        /// <summary>
        /// Gets or sets the translation of the matrix; that is M41, M42, and M43
        /// </summary>
        public Vector3D Origin
        {
            get { return new Vector3D(M41, M42, M43); }
            set { M41 = value.X; M42 = value.Y; M43 = value.Z; }
        }

        public Quaternion Orientation
        {
            get
            {
                float trace = M11 + M22 + M33;

                float[] temp = new float[4];

                if (trace > 0f)
                {
                    float s = Mathematics.Sqrt(trace + (1f));
                    temp[3] = (s * (0.5f));
                    s = (0.5f) / s;

                    temp[0] = ((M32 - M23) * s);
                    temp[1] = ((M13 - M31) * s);
                    temp[2] = ((M21 - M12) * s);
                }
                else
                {
                    int i = M11 < M22 ? (M22 < M33 ? 2 : 1) : (M11 < M33 ? 2 : 0);
                    int j = (i + 1) % 3;
                    int k = (i + 2) % 3;

                    float s = Mathematics.Sqrt(this [i, i] - this [j, j] - this [k, k] + 1f);
                    temp[i] = s * 0.5f;
                    s = 0.5f / s;

                    temp[3] = (this [k, j] - this [j, k]) * s;
                    temp[j] = (this [j, i] + this [i, j]) * s;
                    temp[k] = (this [k, i] + this [i, k]) * s;
                }
                return new Quaternion(temp[0], temp[1], temp[2], temp[3]);
            }
            set
            {
                float d = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;
                float s = 2f / d;
                float xs = value.X * s, ys = value.Y * s, zs = value.Z * s;
                float wx = value.W * xs, wy = value.W * ys, wz = value.W * zs;
                float xx = value.X * xs, xy = value.X * ys, xz = value.X * zs;
                float yy = value.Y * ys, yz = value.Y * zs, zz = value.Z * zs;

                M11 = 1f - (yy + zz);
                M12 = xy - wz;
                M13 = xz + wy;
                M21 = xy + wz;
                M22 = 1f - (xx + zz);
                M23 = yz - wx;
                M31 = xz - wy;
                M32 = yz + wx;
                M33 = 1f - (xx + yy);
            }
        }

        /// <summary>
        /// gets or sets the scale of the matrix; that is M11, M22, and M33
        /// </summary>
        public Vector3D ScaleVector
        {
            get { return new Vector3D(M11, M22, M33); }
            set { M11 = value.X; M22 = value.Y; M33 = value.Z; }
        }

        /// <summary>
        /// Decomposes a matrix into an orthonormalized matrix Q and a right traingular matrix R
        /// </summary>
        /// <param name="temp">When the method completes, contains the orthonormalized matrix of the decomposition</param>
        /// <param name="R">When the method completes, contains the right triangular matrix of the decomposition</param>
        public void DecomposeQR(Matrix Q, Matrix R)
        {
            Matrix temp;
            Q = Orthonormalize(this);
            temp = Q.Transpose();

            R = new Matrix();
            R.M11 = Vector4D.DotProduct(temp.Column1, Column1);
            R.M12 = Vector4D.DotProduct(temp.Column1, Column2);
            R.M13 = Vector4D.DotProduct(temp.Column1, Column3);
            R.M14 = Vector4D.DotProduct(temp.Column1, Column4);

            R.M22 = Vector4D.DotProduct(temp.Column2, Column2);
            R.M23 = Vector4D.DotProduct(temp.Column2, Column3);
            R.M24 = Vector4D.DotProduct(temp.Column2, Column4);

            R.M33 = Vector4D.DotProduct(temp.Column3, Column3);
            R.M34 = Vector4D.DotProduct(temp.Column3, Column4);

            R.M44 = Vector4D.DotProduct(temp.Column4, Column4);
        }

        /// <summary>
        /// Decomposes a matrix into a lower triangular matrix L and an orthonormalized matrix Q
        /// </summary>
        /// <param name="L">When the method completes, contains the lower triangular matrix of the decomposition</param>
        /// <param name="Q">When the method completes, contains the orthonormalized matrix of the decomposition</param>
        public void DecomposeLQ(Matrix L, Matrix Q)
        {
            Q = Orthonormalize(this);

            L = new Matrix();
            L.M11 = Vector4D.DotProduct(Q.Row1, Row1);

            L.M21 = Vector4D.DotProduct(Q.Row1, Row2);
            L.M22 = Vector4D.DotProduct(Q.Row2, Row2);

            L.M31 = Vector4D.DotProduct(Q.Row1, Row3);
            L.M32 = Vector4D.DotProduct(Q.Row2, Row3);
            L.M33 = Vector4D.DotProduct(Q.Row3, Row3);

            L.M41 = Vector4D.DotProduct(Q.Row1, Row4);
            L.M42 = Vector4D.DotProduct(Q.Row2, Row4);
            L.M43 = Vector4D.DotProduct(Q.Row3, Row4);
            L.M44 = Vector4D.DotProduct(Q.Row4, Row4);
        }

        /// <summary>
        /// Decomposes a matrix into a scale, rotation, and translation
        /// </summary>
        /// <param name="scale">When the method completes, contains the scaling component of the decomposed matrix</param>
        /// <param name="rotation">When the method completes, contains the rtoation component of the decomposed matrix</param>
        /// <param name="translation">When the method completes, contains the translation component of the decomposed matrix</param>
        public bool Decompose(Vector3D scale, Quaternion rotation, Vector3D translation)
        {
            translation.X = M41;
            translation.Y = M42;
            translation.Z = M43;

            scale.X = Mathematics.Sqrt((M11 * M11) + (M12 * M12) + (M13 * M13));
            scale.Y = Mathematics.Sqrt((M21 * M21) + (M22 * M22) + (M23 * M23));
            scale.Z = Mathematics.Sqrt((M31 * M31) + (M32 * M32) + (M33 * M33));

            if (Mathematics.Abs(scale.X) < 0 || Mathematics.Abs(scale.Y) < 0 || Mathematics.Abs(scale.Z) < 0)
            {
                rotation = Quaternion.Identity;
                return false;
            }

            Matrix rotationmatrix = new Matrix();
            rotationmatrix.M11 = M11 / scale.X;
            rotationmatrix.M12 = M12 / scale.X;
            rotationmatrix.M13 = M13 / scale.X;

            rotationmatrix.M21 = M21 / scale.Y;
            rotationmatrix.M22 = M22 / scale.Y;
            rotationmatrix.M23 = M23 / scale.Y;

            rotationmatrix.M31 = M31 / scale.Z;
            rotationmatrix.M32 = M32 / scale.Z;
            rotationmatrix.M33 = M33 / scale.Z;

            rotationmatrix.M44 = 1f;

            rotation = Quaternion.RotationMatrix(rotationmatrix);

            return true;
        }

        /// <summary>
        /// Performs the exponential operation on a matrix
        /// </summary>
        /// <param name="value">The matrix to perform the operation on</param>
        /// <param name="exponent">The exponent to raise the matrix to</param>
        /// <returns>The exponential matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="exponent"/> is negative</exception>
        public static Matrix Exponent(Matrix value, int exponent)
        {
            if (exponent < 0)
            {
                throw new ArgumentOutOfRangeException("exponent", "The exponent can not be negative.");
            }

            if (exponent == 0)
            {
                return Identity;
            }

            if (exponent == 1)
            {
                return value;
            }

            Matrix identity = Identity;
            Matrix temp = value;

            while (true)
            {
                if ((exponent & 1) != 0)
                {
                    identity = identity * temp;
                }

                exponent /= 2;

                if (exponent > 0)
                {
                    temp *= temp;
                }
                else
                {
                    break;
                }
            }

            return identity;
        }

        /// <summary>
        /// Performs a linear interpolation between two matricies
        /// </summary>
        /// <param name="start">Start matrix</param>
        /// <param name="end">End matrix</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/></param>
        /// <returns>The linear interpolation of the two matrices</returns>
        public static Matrix Lerp(Matrix start, Matrix end, float amount)
        {
            Matrix result;

            result.M11 = start.M11 + ((end.M11 - start.M11) * amount);
            result.M12 = start.M12 + ((end.M12 - start.M12) * amount);
            result.M13 = start.M13 + ((end.M13 - start.M13) * amount);
            result.M14 = start.M14 + ((end.M14 - start.M14) * amount);
            result.M21 = start.M21 + ((end.M21 - start.M21) * amount);
            result.M22 = start.M22 + ((end.M22 - start.M22) * amount);
            result.M23 = start.M23 + ((end.M23 - start.M23) * amount);
            result.M24 = start.M24 + ((end.M24 - start.M24) * amount);
            result.M31 = start.M31 + ((end.M31 - start.M31) * amount);
            result.M32 = start.M32 + ((end.M32 - start.M32) * amount);
            result.M33 = start.M33 + ((end.M33 - start.M33) * amount);
            result.M34 = start.M34 + ((end.M34 - start.M34) * amount);
            result.M41 = start.M41 + ((end.M41 - start.M41) * amount);
            result.M42 = start.M42 + ((end.M42 - start.M42) * amount);
            result.M43 = start.M43 + ((end.M43 - start.M43) * amount);
            result.M44 = start.M44 + ((end.M44 - start.M44) * amount);

            return result;
        }

        /// <summary>
        /// Performs a cubic interpolation between two matrices
        /// </summary>
        /// <param name="start">Start matrix</param>
        /// <param name="end">End matrix</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/></param>
        /// <returns>The cubic interpolation of the two matrices</returns>
        public static Matrix SmoothStep(Matrix start, Matrix end, float amount)
        {
            Matrix result;

            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));

            result.M11 = start.M11 + ((end.M11 - start.M11) * amount);
            result.M12 = start.M12 + ((end.M12 - start.M12) * amount);
            result.M13 = start.M13 + ((end.M13 - start.M13) * amount);
            result.M14 = start.M14 + ((end.M14 - start.M14) * amount);
            result.M21 = start.M21 + ((end.M21 - start.M21) * amount);
            result.M22 = start.M22 + ((end.M22 - start.M22) * amount);
            result.M23 = start.M23 + ((end.M23 - start.M23) * amount);
            result.M24 = start.M24 + ((end.M24 - start.M24) * amount);
            result.M31 = start.M31 + ((end.M31 - start.M31) * amount);
            result.M32 = start.M32 + ((end.M32 - start.M32) * amount);
            result.M33 = start.M33 + ((end.M33 - start.M33) * amount);
            result.M34 = start.M34 + ((end.M34 - start.M34) * amount);
            result.M41 = start.M41 + ((end.M41 - start.M41) * amount);
            result.M42 = start.M42 + ((end.M42 - start.M42) * amount);
            result.M43 = start.M43 + ((end.M43 - start.M43) * amount);
            result.M44 = start.M44 + ((end.M44 - start.M44) * amount);

            return result;
        }

        /// <summary>
        /// Calculates the transpose of the specified matrix
        /// </summary>
        /// <param name="value">The matrix whose transpose is to be calculated</param>
        /// <returns>The transpose of the specified matrix</returns>
        public static Matrix Transpose(Matrix value)
        {
            return new Matrix(
                value.M11, value.M12, value.M13, value.M14,
                value.M21, value.M22, value.M23, value.M24,
                value.M31, value.M32, value.M33, value.M34,
                value.M41, value.M42, value.M43, value.M44
            );
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix
        /// </summary>
        /// <param name="value">The matrix whose inverse is to be calculated</param>
        /// <returns>The inverse of the specified matrix</returns>
        public static Matrix Invert(Matrix value)
        {
            Matrix result;

            float b0 = (value.M31 * value.M42) - (value.M32 * value.M41);
            float b1 = (value.M31 * value.M43) - (value.M33 * value.M41);
            float b2 = (value.M34 * value.M41) - (value.M31 * value.M44);
            float b3 = (value.M32 * value.M43) - (value.M33 * value.M42);
            float b4 = (value.M34 * value.M42) - (value.M32 * value.M44);
            float b5 = (value.M33 * value.M44) - (value.M34 * value.M43);

            float d11 = value.M22 * b5 + value.M23 * b4 + value.M24 * b3;
            float d12 = value.M21 * b5 + value.M23 * b2 + value.M24 * b1;
            float d13 = value.M21 * -b4 + value.M22 * b2 + value.M24 * b0;
            float d14 = value.M21 * b3 + value.M22 * -b1 + value.M23 * b0;

            float det = value.M11 * d11 - value.M12 * d12 + value.M13 * d13 - value.M14 * d14;
            if (Mathematics.Abs(det) <= 0)
            {
                return Zero;
            }

            det = 1f / det;

            float a0 = (value.M11 * value.M22) - (value.M12 * value.M21);
            float a1 = (value.M11 * value.M23) - (value.M13 * value.M21);
            float a2 = (value.M14 * value.M21) - (value.M11 * value.M24);
            float a3 = (value.M12 * value.M23) - (value.M13 * value.M22);
            float a4 = (value.M14 * value.M22) - (value.M12 * value.M24);
            float a5 = (value.M13 * value.M24) - (value.M14 * value.M23);

            float d21 = value.M12 * b5 + value.M13 * b4 + value.M14 * b3;
            float d22 = value.M11 * b5 + value.M13 * b2 + value.M14 * b1;
            float d23 = value.M11 * -b4 + value.M12 * b2 + value.M14 * b0;
            float d24 = value.M11 * b3 + value.M12 * -b1 + value.M13 * b0;

            float d31 = value.M42 * a5 + value.M43 * a4 + value.M44 * a3;
            float d32 = value.M41 * a5 + value.M43 * a2 + value.M44 * a1;
            float d33 = value.M41 * -a4 + value.M42 * a2 + value.M44 * a0;
            float d34 = value.M41 * a3 + value.M42 * -a1 + value.M43 * a0;

            float d41 = value.M32 * a5 + value.M33 * a4 + value.M34 * a3;
            float d42 = value.M31 * a5 + value.M33 * a2 + value.M34 * a1;
            float d43 = value.M31 * -a4 + value.M32 * a2 + value.M34 * a0;
            float d44 = value.M31 * a3 + value.M32 * -a1 + value.M33 * a0;

            result.M11 = +d11 * det;
            result.M12 = -d21 * det;
            result.M13 = +d31 * det;
            result.M14 = -d41 * det;
            result.M21 = -d12 * det;
            result.M22 = +d22 * det;
            result.M23 = -d32 * det;
            result.M24 = +d42 * det;
            result.M31 = +d13 * det;
            result.M32 = -d23 * det;
            result.M33 = +d33 * det;
            result.M34 = -d43 * det;
            result.M41 = -d14 * det;
            result.M42 = +d24 * det;
            result.M43 = -d34 * det;
            result.M44 = +d44 * det;

            return result;
        }

        /// <summary>
        /// Orthogonalizes the specified matrix
        /// </summary>
        /// <param name="value">The matrix to orthogonalize</param>
        /// <returns>The orthogonalized matrix</returns>
        public static Matrix Orthogonalize(Matrix value)
        {
            Matrix result;

            result = value;

            result.Row2 = result.Row2 - (Vector4D.DotProduct(result.Row1, result.Row2) / Vector4D.DotProduct(result.Row1, result.Row1)) * result.Row1;

            result.Row3 = result.Row3 - (Vector4D.DotProduct(result.Row1, result.Row3) / Vector4D.DotProduct(result.Row1, result.Row1)) * result.Row1;
            result.Row3 = result.Row3 - (Vector4D.DotProduct(result.Row2, result.Row3) / Vector4D.DotProduct(result.Row2, result.Row2)) * result.Row2;

            result.Row4 = result.Row4 - (Vector4D.DotProduct(result.Row1, result.Row4) / Vector4D.DotProduct(result.Row1, result.Row1)) * result.Row1;
            result.Row4 = result.Row4 - (Vector4D.DotProduct(result.Row2, result.Row4) / Vector4D.DotProduct(result.Row2, result.Row2)) * result.Row2;
            result.Row4 = result.Row4 - (Vector4D.DotProduct(result.Row3, result.Row4) / Vector4D.DotProduct(result.Row3, result.Row3)) * result.Row3;

            return result;
        }

        /// <summary>
        /// Orthonormalizes the specified matrix
        /// </summary>
        /// <param name="value">The matrix to orthonormalize</param>
        /// <returns>The orthonormalized matrix</returns>
        /// <remarks>
        public static Matrix Orthonormalize(Matrix value)
        {
            Matrix result;

            result = value;

            result.Row1 = Vector4D.Normalize(result.Row1);

            result.Row2 = result.Row2 - Vector4D.DotProduct(result.Row1, result.Row2) * result.Row1;
            result.Row2 = Vector4D.Normalize(result.Row2);

            result.Row3 = result.Row3 - Vector4D.DotProduct(result.Row1, result.Row3) * result.Row1;
            result.Row3 = result.Row3 - Vector4D.DotProduct(result.Row2, result.Row3) * result.Row2;
            result.Row3 = Vector4D.Normalize(result.Row3);

            result.Row4 = result.Row4 - Vector4D.DotProduct(result.Row1, result.Row4) * result.Row1;
            result.Row4 = result.Row4 - Vector4D.DotProduct(result.Row2, result.Row4) * result.Row2;
            result.Row4 = result.Row4 - Vector4D.DotProduct(result.Row3, result.Row4) * result.Row3;
            result.Row4 = Vector4D.Normalize(result.Row4);

            return result;
        }

        /// <summary>
        /// Creates a spherical billboard that rotates around a specified object position
        /// </summary>
        /// <param name="objectPosition">The position of the object around which the billboard will rotate</param>
        /// <param name="cameraPosition">The position of the camera</param>
        /// <param name="cameraUpVector">The up vector of the camera</param>
        /// <param name="cameraForwardVector">The forward vector of the camera</param>
        /// <returns>The created billboard matrix</returns>
        public static Matrix Billboard(Vector3D objectPosition, Vector3D cameraPosition, Vector3D cameraUpVector, Vector3D cameraForwardVector)
        {
            Matrix result;
            Vector3D crossed, final;
            Vector3D difference = objectPosition - cameraPosition;

            float lengthsq = difference.MagnitudeSquared;

            if (lengthsq < 0)
            {
                difference = -cameraForwardVector;
            }
            else
            {
                difference *= (1 / Mathematics.Sqrt(lengthsq));
            }

            crossed = Vector3D.CrossProduct(cameraUpVector, difference);
            crossed = Vector3D.Normalize(crossed);
            final = Vector3D.CrossProduct(difference, crossed);

            result.M11 = crossed.X;
            result.M12 = crossed.Y;
            result.M13 = crossed.Z;
            result.M14 = 0f;
            result.M21 = final.X;
            result.M22 = final.Y;
            result.M23 = final.Z;
            result.M24 = 0f;
            result.M31 = difference.X;
            result.M32 = difference.Y;
            result.M33 = difference.Z;
            result.M34 = 0f;
            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a matrix that scales along the x-axis, y-axis, and y-axis
        /// </summary>
        /// <param name="scale">Scaling factor for all three axes</param>
        /// <returns>The created scaling matrix</returns>
        public static Matrix Scaling(Vector3D scale) => Scaling(scale.X, scale.Y, scale.Z);

        /// <summary>
        /// Creates a matrix that uniformally scales along all three axis
        /// </summary>
        /// <param name="scale">The uniform scale that is applied along all axis</param>
        /// <returns>The created scaling matrix</returns>
        public static Matrix Scaling(float scale)
        {
            Matrix result;

            result = Identity;
            result.M11 = scale;
            result.M22 = scale;
            result.M33 = scale;

            return result;
        }

        /// <summary>
        /// Creates a matrix that scales along the x-axis, y-axis, and y-axis
        /// </summary>
        /// <param name="x">Scaling factor that is applied along the x-axis</param>
        /// <param name="y">Scaling factor that is applied along the y-axis</param>
        /// <param name="z">Scaling factor that is applied along the z-axis</param>
        /// <returns>The created scaling matrix.</returns>
        public static Matrix Scaling(float x, float y, float z)
        {
            Matrix result;

            result = Identity;
            result.M11 = x;
            result.M22 = y;
            result.M33 = z;

            return result;
        }

        /// <summary>
        /// Creates a matrix that rotates around the x-axis
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin</param>
        /// <returns>The created rotation matrix</returns>
        public static Matrix RotationX(float angle)
        {
            Matrix result;

            float cos = Mathematics.Cos(angle);
            float sin = Mathematics.Sin(angle);

            result = Identity;
            result.M22 = cos;
            result.M23 = sin;
            result.M32 = -sin;
            result.M33 = cos;

            return result;
        }

        /// <summary>
        /// Creates a matrix that rotates around the y-axis
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin</param>
        /// <returns>The created rotation matrix</returns>
        public static Matrix RotationY(float angle)
        {
            Matrix result;

            float cos = Mathematics.Cos(angle);
            float sin = Mathematics.Sin(angle);

            result = Identity;
            result.M11 = cos;
            result.M13 = -sin;
            result.M31 = sin;
            result.M33 = cos;

            return result;
        }

        /// <summary>
        /// Creates a matrix that rotates around the z-axis
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin</param>
        /// <returns>The created rotation matrix</returns>
        public static Matrix RotationZ(float angle)
        {
            Matrix result;

            float cos = Mathematics.Cos(angle);
            float sin = Mathematics.Sin(angle);

            result = Identity;
            result.M11 = cos;
            result.M12 = sin;
            result.M21 = -sin;
            result.M22 = cos;

            return result;
        }

        /// <summary>
        /// Creates a matrix that rotates around an arbitary axis
        /// </summary>
        /// <param name="axis">The axis around which to rotate. This parameter is assumed to be normalized</param>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin</param>
        /// <returns>The created rotation matrix</returns>
        public static Matrix RotationAxis(Vector3D axis, float angle)
        {
            Matrix result;

            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float cos = Mathematics.Cos(angle);
            float sin = Mathematics.Sin(angle);
            float xx = x * x;
            float yy = y * y;
            float zz = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;

            result = Identity;
            result.M11 = xx + (cos * (1f - xx));
            result.M12 = (xy - (cos * xy)) + (sin * z);
            result.M13 = (xz - (cos * xz)) - (sin * y);
            result.M21 = (xy - (cos * xy)) - (sin * z);
            result.M22 = yy + (cos * (1f - yy));
            result.M23 = (yz - (cos * yz)) + (sin * x);
            result.M31 = (xz - (cos * xz)) + (sin * y);
            result.M32 = (yz - (cos * yz)) - (sin * x);
            result.M33 = zz + (cos * (1f - zz));

            return result;
        }

        /// <summary>
        /// Creates a rotation matrix from a quaternion
        /// </summary>
        /// <param name="rotation">The quaternion to use to build the matrix</param>
        /// <returns>The created rotation matrix</returns>
        public static Matrix RotationQuaternion(Quaternion rotation)
        {
            Matrix result;

            float xx = rotation.X * rotation.X;
            float yy = rotation.Y * rotation.Y;
            float zz = rotation.Z * rotation.Z;
            float xy = rotation.X * rotation.Y;
            float zw = rotation.Z * rotation.W;
            float zx = rotation.Z * rotation.X;
            float yw = rotation.Y * rotation.W;
            float yz = rotation.Y * rotation.Z;
            float xw = rotation.X * rotation.W;

            result = Identity;
            result.M11 = 1f - (2f * (yy + zz));
            result.M12 = 2f * (xy + zw);
            result.M13 = 2f * (zx - yw);
            result.M21 = 2f * (xy - zw);
            result.M22 = 1f - (2f * (zz + xx));
            result.M23 = 2f * (yz + xw);
            result.M31 = 2f * (zx + yw);
            result.M32 = 2f * (yz - xw);
            result.M33 = 1f - (2f * (yy + xx));

            return result;
        }

        /// <summary>
        /// Creates a rotation matrix with a specified yaw, pitch, and roll
        /// </summary>
        /// <param name="yaw">Yaw around the y-axis, in radians</param>
        /// <param name="pitch">Pitch around the x-axis, in radians</param>
        /// <param name="roll">Roll around the z-axis, in radians</param>
        /// <returns>The created rotation matrix</returns>
        public static Matrix RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            return RotationQuaternion(Quaternion.RotationYawPitchRoll(yaw, pitch, roll));
        }

        /// <summary>
        /// Creates a translation matrix using the specified offsets
        /// </summary>
        /// <param name="value">The offset for all three coordinate planes</param>
        /// <returns>The created translation matrix</returns>
        public static Matrix Translation(Vector3D value)
        {
            return Translation(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Creates a translation matrix using the specified offsets
        /// </summary>
        /// <param name="x">X-coordinate offset</param>
        /// <param name="y">Y-coordinate offset</param>
        /// <param name="z">Z-coordinate offset</param>
        /// <returns>The created translation matrix</returns>
        public static Matrix Translation(float x, float y, float z)
        {
            Matrix result;

            result = Identity;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;

            return result;
        }

        /// <summary>
        /// Creates a 3D affine transformation matrix
        /// </summary>
        /// <param name="scaling">Scaling factor</param>
        /// <param name="rotation">The rotation of the transformation</param>
        /// <param name="translation">The translation factor of the transformation</param>
        /// <returns>The created affine transformation matrix</returns>
        public static Matrix AffineTransformation(float scaling, Quaternion rotation, Vector3D translation)
        {
            return Scaling(scaling) * RotationQuaternion(rotation) * Translation(translation);
        }

        /// <summary>
        /// Creates a 3D affine transformation matrix
        /// </summary>
        /// <param name="scaling">Scaling factor</param>
        /// <param name="rotationCenter">The center of the rotation</param>
        /// <param name="rotation">The rotation of the transformation</param>
        /// <param name="translation">The translation factor of the transformation</param>
        /// <returns>The created affine transformation matrix</returns>
        public static Matrix AffineTransformation(float scaling, Vector3D rotationCenter, Quaternion rotation, Vector3D translation)
        {
            return Scaling(scaling) * Translation(-rotationCenter) * RotationQuaternion(rotation) * Translation(rotationCenter) * Translation(translation);
        }

        /// <summary>
        /// Creates a transformation matrix
        /// </summary>
        /// <param name="scalingCenter">Center point of the scaling operation</param>
        /// <param name="scalingRotation">Scaling rotation amount</param>
        /// <param name="scaling">Scaling factor</param>
        /// <param name="rotationCenter">The center of the rotation</param>
        /// <param name="rotation">The rotation of the transformation</param>
        /// <param name="translation">The translation factor of the transformation</param>
        /// <returns>The created transformation matrix</returns>
        public static Matrix Transformation(Vector3D scalingCenter, Quaternion scalingRotation, Vector3D scaling, Vector3D rotationCenter, Quaternion rotation, Vector3D translation)
        {
            return Translation(-scalingCenter) * Transpose(sr) * Scaling(scaling) * RotationQuaternion(scalingRotation) * Translation(scalingCenter) *
                Translation(-rotationCenter) * RotationQuaternion(rotation) * Translation(rotationCenter) * Translation(translation);
        }

        /// <summary>
        /// Adds two matricies
        /// </summary>
        /// <param name="value1">The first matrix to add</param>
        /// <param name="value2">The second matrix to add</param>
        /// <returns>The sum of the two matricies</returns>
        public static Matrix operator +(Matrix value1, Matrix value2)
        {
            return new Matrix(
                value1.M11 + value2.M11, value1.M12 + value2.M12, value1.M13 + value2.M13, value1.M14 + value2.M14,
                value1.M21 + value2.M21, value1.M22 + value2.M22, value1.M23 + value2.M23, value1.M24 + value2.M24,
                value1.M31 + value2.M31, value1.M32 + value2.M32, value1.M33 + value2.M33, value1.M34 + value2.M34,
                value1.M41 + value2.M41, value1.M42 + value2.M42, value1.M43 + value2.M43, value1.M44 + value2.M44
            );
        }

        /// <summary>
        /// Subtracts two matricies
        /// </summary>
        /// <param name="value1">The first matrix to subtract</param>
        /// <param name="value2">The second matrix to subtract</param>
        /// <returns>The difference between the two matricies</returns>
        public static Matrix operator -(Matrix value1, Matrix value2)
        {
            return new Matrix(
                value1.M11 - value2.M11, value1.M12 - value2.M12, value1.M13 - value2.M13, value1.M14 - value2.M14,
                value1.M21 - value2.M21, value1.M22 - value2.M22, value1.M23 - value2.M23, value1.M24 - value2.M24,
                value1.M31 - value2.M31, value1.M32 - value2.M32, value1.M33 - value2.M33, value1.M34 - value2.M34,
                value1.M41 - value2.M41, value1.M42 - value2.M42, value1.M43 - value2.M43, value1.M44 - value2.M44
            );
        }

        /// <summary>
        /// Negates a matrix.
        /// </summary>
        /// <param name="value">The matrix to negate</param>
        /// <returns>The negated matrix</returns>
        public static Matrix operator -(Matrix value)
        {
            return new Matrix(-value.M11, -value.M12, -value.M13, -value.M14, -value.M21, -value.M22, -value.M23, -value.M24, -value.M31, -value.M32, -value.M33, -value.M34, -value.M41, -value.M42, -value.M43, -value.M44);
        }

        /// <summary>
        /// Scales a matrix by a given value
        /// </summary>
        /// <param name="value">The matrix to scale</param>
        /// <param name="scalar">The amount by which to scale</param>
        /// <returns>The scaled matrix</returns>
        public static Matrix operator *(float scalar, Matrix value)
        {
            return new Matrix(
                value.M11 * scalar, value.M12 * scalar, value.M13 * scalar, value.M14 * scalar,
                value.M21 * scalar, value.M22 * scalar, value.M23 * scalar, value.M24 * scalar,
                value.M31 * scalar, value.M32 * scalar, value.M33 * scalar, value.M34 * scalar,
                value.M41 * scalar, value.M42 * scalar, value.M43 * scalar, value.M44 * scalar
            );
        }

        /// <summary>
        /// Scales a matrix by a given value
        /// </summary>
        /// <param name="value">The matrix to scale</param>
        /// <param name="scalar">The amount by which to scale</param>
        /// <returns>The scaled matrix</returns>
        public static Matrix operator *(Matrix value, float scalar)
        {
            return new Matrix(
                value.M11 * scalar, value.M12 * scalar, value.M13 * scalar, value.M14 * scalar,
                value.M21 * scalar, value.M22 * scalar, value.M23 * scalar, value.M24 * scalar,
                value.M31 * scalar, value.M32 * scalar, value.M33 * scalar, value.M34 * scalar,
                value.M41 * scalar, value.M42 * scalar, value.M43 * scalar, value.M44 * scalar
            );
        }

        /// <summary>
        /// Multiplies two matricies
        /// </summary>
        /// <param name="value1">The first matrix to multiply</param>
        /// <param name="value2">The second matrix to multiply</param>
        /// <returns>The product of the two matricies</returns>
        public static Matrix operator *(Matrix value1, Matrix value2)
        {
            Matrix result;

            result.M11 = (value1.M11 * value2.M11) + (value1.M12 * value2.M21) + (value1.M13 * value2.M31) + (value1.M14 * value2.M41);
            result.M12 = (value1.M11 * value2.M12) + (value1.M12 * value2.M22) + (value1.M13 * value2.M32) + (value1.M14 * value2.M42);
            result.M13 = (value1.M11 * value2.M13) + (value1.M12 * value2.M23) + (value1.M13 * value2.M33) + (value1.M14 * value2.M43);
            result.M14 = (value1.M11 * value2.M14) + (value1.M12 * value2.M24) + (value1.M13 * value2.M34) + (value1.M14 * value2.M44);
            result.M21 = (value1.M21 * value2.M11) + (value1.M22 * value2.M21) + (value1.M23 * value2.M31) + (value1.M24 * value2.M41);
            result.M22 = (value1.M21 * value2.M12) + (value1.M22 * value2.M22) + (value1.M23 * value2.M32) + (value1.M24 * value2.M42);
            result.M23 = (value1.M21 * value2.M13) + (value1.M22 * value2.M23) + (value1.M23 * value2.M33) + (value1.M24 * value2.M43);
            result.M24 = (value1.M21 * value2.M14) + (value1.M22 * value2.M24) + (value1.M23 * value2.M34) + (value1.M24 * value2.M44);
            result.M31 = (value1.M31 * value2.M11) + (value1.M32 * value2.M21) + (value1.M33 * value2.M31) + (value1.M34 * value2.M41);
            result.M32 = (value1.M31 * value2.M12) + (value1.M32 * value2.M22) + (value1.M33 * value2.M32) + (value1.M34 * value2.M42);
            result.M33 = (value1.M31 * value2.M13) + (value1.M32 * value2.M23) + (value1.M33 * value2.M33) + (value1.M34 * value2.M43);
            result.M34 = (value1.M31 * value2.M14) + (value1.M32 * value2.M24) + (value1.M33 * value2.M34) + (value1.M34 * value2.M44);
            result.M41 = (value1.M41 * value2.M11) + (value1.M42 * value2.M21) + (value1.M43 * value2.M31) + (value1.M44 * value2.M41);
            result.M42 = (value1.M41 * value2.M12) + (value1.M42 * value2.M22) + (value1.M43 * value2.M32) + (value1.M44 * value2.M42);
            result.M43 = (value1.M41 * value2.M13) + (value1.M42 * value2.M23) + (value1.M43 * value2.M33) + (value1.M44 * value2.M43);
            result.M44 = (value1.M41 * value2.M14) + (value1.M42 * value2.M24) + (value1.M43 * value2.M34) + (value1.M44 * value2.M44);

            return result;
        }

        /// <summary>
        /// Scales a matrix by a given value
        /// </summary>
        /// <param name="value">The matrix to scale</param>
        /// <param name="scalar">The amount by which to scale</param>
        /// <returns>The scaled matrix</returns>
        public static Matrix operator /(Matrix value, float scalar)
        {
            return new Matrix(
                value.M11 * (1f / scalar), value.M12 * (1f / scalar), value.M13 * (1f / scalar), value.M14 * (1f / scalar),
                value.M21 * (1f / scalar), value.M22 * (1f / scalar), value.M23 * (1f / scalar), value.M24 * (1f / scalar),
                value.M31 * (1f / scalar), value.M32 * (1f / scalar), value.M33 * (1f / scalar), value.M34 * (1f / scalar),
                value.M41 * (1f / scalar), value.M42 * (1f / scalar), value.M43 * (1f / scalar), value.M44 * (1f / scalar)
            );
        }

        /// <summary>
        /// Divides two matricies
        /// </summary>
        /// <param name="value1">The first matrix to divide</param>
        /// <param name="value2">The second matrix to divide</param>
        /// <returns>The quotient of the two matricies</returns>
        public static Matrix operator /(Matrix value1, Matrix value2)
        {
            return new Matrix(
                value1.M11 * value2.M11, value1.M12 * value2.M12, value1.M13 * value2.M13, value1.M14 * value2.M14,
                value1.M21 * value2.M21, value1.M22 * value2.M22, value1.M23 * value2.M23, value1.M24 * value2.M24,
                value1.M31 * value2.M31, value1.M32 * value2.M32, value1.M33 * value2.M33, value1.M34 * value2.M34,
                value1.M41 * value2.M41, value1.M42 * value2.M42, value1.M43 * value2.M43, value1.M44 * value2.M44
            );
        }

        /// <summary>
        /// Tests for equality between two objects
        /// </summary>
        /// <param name="value1">The first value to compare</param>
        /// <param name="value2">The second value to compare</param>
        /// <returns><c>true</c> if <paramref name="value1"/> has the same value as <paramref name="value2"/>; otherwise, <c>false</c></returns>
        public static bool operator ==(Matrix value1, Matrix value2) => value1.Equals(value2);

        /// <summary>
        /// Tests for inequality between two objects
        /// </summary>
        /// <param name="left">The first value to compare</param>
        /// <param name="right">The second value to compare</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c></returns>
        public static bool operator !=(Matrix value1, Matrix value2) => !value1.Equals(value2);

        /// <summary>
        /// Determines whether the specified object is equal to this instance
        /// </summary>
        /// <param name="obj">The object to compare with this instance</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>
        /// </returns>
        public override bool Equals(object obj) => (obj is Matrix) && Equals((Matrix)obj);

        /// <summary>
        /// Determines whether the specified matrix is equal to this instance
        /// </summary>
        /// <param name="other">The matrix to compare with this instance</param>
        /// <returns>
        /// <c>true</c> if the specified matrix is equal to this instance; otherwise, <c>false</c>
        /// </returns>
        public bool Equals(Matrix other)
        {
            return (M11.Equals(other.M11) && M12.Equals(other.M12) && M13.Equals(other.M13) && M14.Equals(other.M14) &&
                M21.Equals(other.M21) && M22.Equals(other.M22) && M23.Equals(other.M23) && M24.Equals(other.M24) &&
                M31.Equals(other.M31) && M32.Equals(other.M32) && M33.Equals(other.M33) && M34.Equals(other.M34) &&
                M41.Equals(other.M41) && M42.Equals(other.M42) && M43.Equals(other.M43) && M44.Equals(other.M44));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance
        /// </returns>
        public override string ToString()
        {
            return $"[Matrix] [M11:{M11} M12:{M12} M13:{M13} M14:{M14}] [M21:{M21} M22:{M22} M23:{M23} M24:{M24}] " +
                "[M31:{M31} M32:{M32} M33:{M33} M34:{M34}] [M41:{M41} M42:{M42} M43:{M43} M44:{M44}]";
        }

        /// <summary>
        /// Returns a hash code for this instance
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table
        /// </returns>
        public override int GetHashCode()
        {
            return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() +
                M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() +
                M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() +
                M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode();
        }
    }
}