using System;
using System.Runtime.InteropServices;

namespace Kinematics.Math
{ 
    /// <summary>
    /// Represents the right-handed 4x4 floating point matrix, which can store translation, scale and rotation information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix : IEquatable<Matrix>
    {
	    #region Properties

	    /// <summary>
        /// A first row and first column value
        /// </summary>
        public float M11;

        /// <summary>
        /// A first row and second column value
        /// </summary>
        public float M12;

        /// <summary>
        /// A first row and third column value
        /// </summary>
        public float M13;

        /// <summary>
        /// A first row and fourth column value
        /// </summary>
        public float M14;

        /// <summary>
        /// A second row and first column value
        /// </summary>
        public float M21;

        /// <summary>
        /// A second row and second column value
        /// </summary>
        public float M22;

        /// <summary>
        /// A second row and third column value
        /// </summary>
        public float M23;

        /// <summary>
        /// A second row and fourth column value
        /// </summary>
        public float M24;

        /// <summary>
        /// A third row and first column value
        /// </summary>
        public float M31;

        /// <summary>
        /// A third row and second column value
        /// </summary>
        public float M32;

        /// <summary>
        /// A third row and third column value
        /// </summary>
        public float M33;

        /// <summary>
        /// A third row and fourth column value
        /// </summary>
        public float M34;

        /// <summary>
        /// A fourth row and first column value
        /// </summary>
        public float M41;

        /// <summary>
        /// A fourth row and second column value
        /// </summary>
        public float M42;

        /// <summary>
        /// A fourth row and third column value
        /// </summary>
        public float M43;

        /// <summary>
        /// A fourth row and fourth column value
        /// </summary>
        public float M44;

        /// <summary>
        /// The forward vector formed from the third row -M31, -M32, -M33 elements
        /// </summary>
        public readonly Vector3 Forward => new Vector3(-M31, -M32, -M33);

        /// <summary>
        /// The backward vector formed from the third row M31, M32, M33 elements
        /// </summary>
        public readonly Vector3 Backward => new Vector3(M31, M32, M33);

        /// <summary>
        /// The upper vector formed from the second row M21, M22, M23 elements
        /// </summary>
        public readonly Vector3 Up => new Vector3(M21, M22, M23);

        /// <summary>
        /// The down vector formed from the second row -M21, -M22, -M23 elements
        /// </summary>
        public readonly Vector3 Down => new Vector3(-M21, -M22, -M23);

        /// <summary>
        /// The left vector formed from the first row -M11, -M12, -M13 elements
        /// </summary>
        public readonly Vector3 Left => new Vector3(-M11, -M12, -M13);

        /// <summary>
        /// The right vector formed from the first row M11, M12, M13 elements
        /// </summary>
        public readonly Vector3 Right => new Vector3(M11, M12, M13);

        /// <summary>
        /// Returns the identity matrix
        /// </summary>
        public static Matrix Identity => new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);

        /// <summary>
        /// Position stored in this matrix
        /// </summary>
        public readonly Vector3 Translation => new Vector3(M41, M42, M43);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a matrix
        /// </summary>
        /// <param name="m11">A first row and first column value</param>
        /// <param name="m12">A first row and second column value</param>
        /// <param name="m13">A first row and third column value</param>
        /// <param name="m14">A first row and fourth column value</param>
        /// <param name="m21">A second row and first column value</param>
        /// <param name="m22">A second row and second column value</param>
        /// <param name="m23">A second row and third column value</param>
        /// <param name="m24">A second row and fourth column value</param>
        /// <param name="m31">A third row and first column value</param>
        /// <param name="m32">A third row and second column value</param>
        /// <param name="m33">A third row and third column value</param>
        /// <param name="m34">A third row and fourth column value</param>
        /// <param name="m41">A fourth row and first column value</param>
        /// <param name="m42">A fourth row and second column value</param>
        /// <param name="m43">A fourth row and third column value</param>
        /// <param name="m44">A fourth row and fourth column value</param>
        public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31,
                      float m32, float m33, float m34, float m41, float m42, float m43, float m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        /// <summary>
        /// Constructs a matrix
        /// </summary>
        /// <param name="row1">A first row of the created matrix</param>
        /// <param name="row2">A second row of the created matrix</param>
        /// <param name="row3">A third row of the created matrix</param>
        /// <param name="row4">A fourth row of the created matrix</param>
        public Matrix(Vector4 row1, Vector4 row2, Vector4 row3, Vector4 row4)
        {
            M11 = row1.X;
            M12 = row1.Y;
            M13 = row1.Z;
            M14 = row1.W;
            M21 = row2.X;
            M22 = row2.Y;
            M23 = row2.Z;
            M24 = row2.W;
            M31 = row3.X;
            M32 = row3.Y;
            M33 = row3.Z;
            M34 = row3.W;
            M41 = row4.X;
            M42 = row4.Y;
            M43 = row4.Z;
            M44 = row4.W;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Adds two matrixes.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the add sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the add sign.</param>
        /// <returns>Sum of the matrixes.</returns>
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            matrix1.M11 += matrix2.M11;
            matrix1.M12 += matrix2.M12;
            matrix1.M13 += matrix2.M13;
            matrix1.M14 += matrix2.M14;
            matrix1.M21 += matrix2.M21;
            matrix1.M22 += matrix2.M22;
            matrix1.M23 += matrix2.M23;
            matrix1.M24 += matrix2.M24;
            matrix1.M31 += matrix2.M31;
            matrix1.M32 += matrix2.M32;
            matrix1.M33 += matrix2.M33;
            matrix1.M34 += matrix2.M34;
            matrix1.M41 += matrix2.M41;
            matrix1.M42 += matrix2.M42;
            matrix1.M43 += matrix2.M43;
            matrix1.M44 += matrix2.M44;

            return matrix1;
        }

        /// <summary>
        /// Divides the elements of a <see cref="Matrix"/> by the elements of another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the div sign.</param>
        /// <param name="matrix2">Divisor <see cref="Matrix"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the matrixes.</returns>
        public static Matrix operator /(Matrix matrix1, Matrix matrix2)
        {
		    matrix1.M11 /= matrix2.M11;
		    matrix1.M12 /= matrix2.M12;
		    matrix1.M13 /= matrix2.M13;
		    matrix1.M14 /= matrix2.M14;
		    matrix1.M21 /= matrix2.M21;
		    matrix1.M22 /= matrix2.M22;
		    matrix1.M23 /= matrix2.M23;
		    matrix1.M24 /= matrix2.M24;
		    matrix1.M31 /= matrix2.M31;
		    matrix1.M32 /= matrix2.M32;
		    matrix1.M33 /= matrix2.M33;
		    matrix1.M34 /= matrix2.M34;
		    matrix1.M41 /= matrix2.M41;
		    matrix1.M42 /= matrix2.M42;
		    matrix1.M43 /= matrix2.M43;
		    matrix1.M44 /= matrix2.M44;

		    return matrix1;
        }

        /// <summary>
        /// Divides the elements of a <see cref="Matrix"/> by a scalar.
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a matrix by a scalar.</returns>
        public static Matrix operator /(Matrix matrix, float divider)
        {
		    float num = 1f / divider;
		    matrix.M11 *= num;
		    matrix.M12 *= num;
		    matrix.M13 *= num;
		    matrix.M14 *= num;
		    matrix.M21 *= num;
		    matrix.M22 *= num;
		    matrix.M23 *= num;
		    matrix.M24 *= num;
		    matrix.M31 *= num;
		    matrix.M32 *= num;
		    matrix.M33 *= num;
		    matrix.M34 *= num;
		    matrix.M41 *= num;
		    matrix.M42 *= num;
		    matrix.M43 *= num;
		    matrix.M44 *= num;

		    return matrix;
        }

        /// <summary>
        /// Compares whether two <see cref="Matrix"/> instances are equal without any tolerance.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the equal sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Matrix matrix1, Matrix matrix2)
        {
	        return matrix1.Equals(matrix2);
        }

        /// <summary>
        /// Compares whether two <see cref="Matrix"/> instances are not equal without any tolerance.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the not equal sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Matrix matrix1, Matrix matrix2)
        {
	        return !matrix1.Equals(matrix2);
        }

        /// <summary>
        /// Multiplies two matrixes.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the mul sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the mul sign.</param>
        /// <returns>Result of the matrix multiplication.</returns>
        /// <remarks>
        /// Using matrix multiplication algorithm - see http://en.wikipedia.org/wiki/Matrix_multiplication.
        /// </remarks>
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            float m11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
            float m12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
            float m13 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
            float m14 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
            float m21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
            float m22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
            float m23 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
            float m24 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
            float m31 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
            float m32 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
            float m33 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
            float m34 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
            float m41 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
            float m42 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
            float m43 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
           	float m44 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
            matrix1.M11 = m11;
			matrix1.M12 = m12;
			matrix1.M13 = m13;
			matrix1.M14 = m14;
			matrix1.M21 = m21;
			matrix1.M22 = m22;
			matrix1.M23 = m23;
			matrix1.M24 = m24;
			matrix1.M31 = m31;
			matrix1.M32 = m32;
			matrix1.M33 = m33;
			matrix1.M34 = m34;
			matrix1.M41 = m41;
			matrix1.M42 = m42;
			matrix1.M43 = m43;
			matrix1.M44 = m44;

			return matrix1;
        }

        /// <summary>
        /// Multiplies the elements of matrix by a scalar.
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the matrix multiplication with a scalar.</returns>
        public static Matrix operator *(Matrix matrix, float scaleFactor)
        {
		    matrix.M11 *= scaleFactor;
		    matrix.M12 *= scaleFactor;
		    matrix.M13 *= scaleFactor;
		    matrix.M14 *= scaleFactor;
		    matrix.M21 *= scaleFactor;
		    matrix.M22 *= scaleFactor;
		    matrix.M23 *= scaleFactor;
		    matrix.M24 *= scaleFactor;
		    matrix.M31 *= scaleFactor;
		    matrix.M32 *= scaleFactor;
		    matrix.M33 *= scaleFactor;
		    matrix.M34 *= scaleFactor;
		    matrix.M41 *= scaleFactor;
		    matrix.M42 *= scaleFactor;
		    matrix.M43 *= scaleFactor;
		    matrix.M44 *= scaleFactor;

		    return matrix;
        }

        /// <summary>
        /// Subtracts the values of one <see cref="Matrix"/> from another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the sub sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the sub sign.</param>
        /// <returns>Result of the matrix subtraction.</returns>
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
		    matrix1.M11 -= matrix2.M11;
		    matrix1.M12 -= matrix2.M12;
		    matrix1.M13 -= matrix2.M13;
		    matrix1.M14 -= matrix2.M14;
		    matrix1.M21 -= matrix2.M21;
		    matrix1.M22 -= matrix2.M22;
		    matrix1.M23 -= matrix2.M23;
		    matrix1.M24 -= matrix2.M24;
		    matrix1.M31 -= matrix2.M31;
		    matrix1.M32 -= matrix2.M32;
		    matrix1.M33 -= matrix2.M33;
		    matrix1.M34 -= matrix2.M34;
		    matrix1.M41 -= matrix2.M41;
		    matrix1.M42 -= matrix2.M42;
		    matrix1.M43 -= matrix2.M43;
		    matrix1.M44 -= matrix2.M44;

		    return matrix1;
        }

        /// <summary>
        /// Inverts values in the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Matrix operator -(Matrix matrix)
        {
		    matrix.M11 = -matrix.M11;
		    matrix.M12 = -matrix.M12;
		    matrix.M13 = -matrix.M13;
		    matrix.M14 = -matrix.M14;
		    matrix.M21 = -matrix.M21;
		    matrix.M22 = -matrix.M22;
		    matrix.M23 = -matrix.M23;
		    matrix.M24 = -matrix.M24;
		    matrix.M31 = -matrix.M31;
		    matrix.M32 = -matrix.M32;
		    matrix.M33 = -matrix.M33;
		    matrix.M34 = -matrix.M34;
		    matrix.M41 = -matrix.M41;
		    matrix.M42 = -matrix.M42;
		    matrix.M43 = -matrix.M43;
		    matrix.M44 = -matrix.M44;

			return matrix;
        }

        #endregion

        #region Overrides

        /// <summary>
		/// Compares whether current instance is equal to specified <see cref="Object"/> without any tolerance.
		/// </summary>
		/// <param name="obj">The <see cref="Object"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public override bool Equals(object obj)
		{
			return obj is Matrix m && Equals(m);
		}

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Matrix"/> without any tolerance.
        /// </summary>
        /// <param name="other">The <see cref="Matrix"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Matrix other)
        {
	        return M11.Equals(other.M11) &&
	               M22.Equals(other.M22) &&
	               M33.Equals(other.M33) &&
	               M44.Equals(other.M44) &&
	               M12.Equals(other.M12) &&
	               M13.Equals(other.M13) &&
	               M14.Equals(other.M14) &&
	               M21.Equals(other.M21) &&
	               M23.Equals(other.M23) &&
	               M24.Equals(other.M24) &&
	               M31.Equals(other.M31) &&
	               M32.Equals(other.M32) &&
	               M34.Equals(other.M34) &&
	               M41.Equals(other.M41) &&
	               M42.Equals(other.M42) &&
	               M43.Equals(other.M43);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Matrix"/>.</returns>
        public override int GetHashCode()
        {
            return M11.GetHashCode() +
                   M12.GetHashCode() +
                   M13.GetHashCode() +
                   M14.GetHashCode() +
                   M21.GetHashCode() +
                   M22.GetHashCode() +
                   M23.GetHashCode() +
                   M24.GetHashCode() +
                   M31.GetHashCode() +
                   M32.GetHashCode() +
                   M33.GetHashCode() +
                   M34.GetHashCode() +
                   M41.GetHashCode() +
                   M42.GetHashCode() +
                   M43.GetHashCode() +
                   M44.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Matrix"/> in the format:
        /// {M11:[<see cref="M11"/>] M12:[<see cref="M12"/>] M13:[<see cref="M13"/>] M14:[<see cref="M14"/>]}
        /// {M21:[<see cref="M21"/>] M12:[<see cref="M22"/>] M13:[<see cref="M23"/>] M14:[<see cref="M24"/>]}
        /// {M31:[<see cref="M31"/>] M32:[<see cref="M32"/>] M33:[<see cref="M33"/>] M34:[<see cref="M34"/>]}
        /// {M41:[<see cref="M41"/>] M42:[<see cref="M42"/>] M43:[<see cref="M43"/>] M44:[<see cref="M44"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Matrix"/>.</returns>
        public override string ToString()
        {
	        return "{M11:" + M11 + " M12:" + M12 + " M13:" + M13 + " M14:" + M14 + "}" +
	               " {M21:" + M21 + " M22:" + M22 + " M23:" + M23 + " M24:" + M24 + "}" +
	               " {M31:" + M31 + " M32:" + M32 + " M33:" + M33 + " M34:" + M34 + "}" +
	               " {M41:" + M41 + " M42:" + M42 + " M43:" + M43 + " M44:" + M44 + "}";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new <see cref="Matrix"/> for spherical billboarding that rotates around specified object position.
        /// </summary>
        /// <param name="objectPosition">Position of billboard object. It will rotate around that vector.</param>
        /// <param name="cameraPosition">The camera position.</param>
        /// <param name="cameraUpVector">The camera up vector.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <returns>The <see cref="Matrix"/> for spherical billboarding.</returns>
        public static Matrix CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3? cameraForwardVector)
        {
            Vector3 vector;
            vector.X = objectPosition.X - cameraPosition.X;
            vector.Y = objectPosition.Y - cameraPosition.Y;
            vector.Z = objectPosition.Z - cameraPosition.Z;
            float lengthSquared = vector.LengthSquared();
            if (lengthSquared < Mathf.Epsilon)
            {
	            vector = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            }
            else
            {
	            vector *= 1f / Mathf.Sqrt(lengthSquared);
            }

            Vector3 vector3 = Vector3.Cross(cameraUpVector, vector);
            vector3.Normalize();
            Vector3 vector2 = Vector3.Cross(vector, vector3);

            Matrix result;
            result.M11 = vector3.X;
            result.M12 = vector3.Y;
            result.M13 = vector3.Z;
            result.M14 = 0;
            result.M21 = vector2.X;
            result.M22 = vector2.Y;
            result.M23 = vector2.Z;
            result.M24 = 0;
            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M34 = 0;
            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1;
            
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix"/> for cylindrical billboarding that rotates around specified axis.
        /// </summary>
        /// <param name="objectPosition">Object position the billboard will rotate around.</param>
        /// <param name="cameraPosition">Camera position.</param>
        /// <param name="rotateAxis">Axis of billboard for rotation.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <param name="objectForwardVector">Optional object forward vector.</param>
        /// <returns>The <see cref="Matrix"/> for cylindrical billboarding.</returns>
        public static Matrix CreateConstrainedBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector)
        {
            Vector3 vector;
		    Vector3 vector2;
		    Vector3 vector3;
		    vector2.X = objectPosition.X - cameraPosition.X;
		    vector2.Y = objectPosition.Y - cameraPosition.Y;
		    vector2.Z = objectPosition.Z - cameraPosition.Z;
		    float lengthSquared = vector2.LengthSquared();
		    if (lengthSquared < 0.0001f)
		    {
		        vector2 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
		    }
		    else
		    {
			    vector2 *= 1f / Mathf.Sqrt(lengthSquared);
		    }

		    Vector3 vector4 = rotateAxis;
		    float num = Vector3.Dot(rotateAxis, vector2);
		    const float minAbs = 0.9982547f;
		    if (Mathf.Abs(num) > minAbs)
		    {
		        if (objectForwardVector.HasValue)
		        {
		            vector = objectForwardVector.Value;
		            num = Vector3.Dot(rotateAxis, vector);
		            if (Mathf.Abs(num) > minAbs)
		            {
		                num = rotateAxis.X * Vector3.Forward.X + rotateAxis.Y * Vector3.Forward.Y + rotateAxis.Z * Vector3.Forward.Z;
		                vector = Mathf.Abs(num) > minAbs ? Vector3.Right : Vector3.Forward;
		            }
		        }
		        else
		        {
		            num = rotateAxis.X * Vector3.Forward.X + rotateAxis.Y * Vector3.Forward.Y + rotateAxis.Z * Vector3.Forward.Z;
		            vector = Mathf.Abs(num) > minAbs ? Vector3.Right : Vector3.Forward;
		        }
		        vector3 = Vector3.Cross(rotateAxis, vector);
		        vector3.Normalize();
		        vector = Vector3.Cross(vector3, rotateAxis);
		        vector.Normalize();
		    }
		    else
		    {
			    vector3 = Vector3.Cross(rotateAxis, vector2);
		        vector3.Normalize();
		        vector = Vector3.Cross(vector3, vector4);
		        vector.Normalize();
		    }

		    Matrix result;
		    result.M11 = vector3.X;
		    result.M12 = vector3.Y;
		    result.M13 = vector3.Z;
		    result.M14 = 0;
		    result.M21 = vector4.X;
		    result.M22 = vector4.Y;
		    result.M23 = vector4.Z;
		    result.M24 = 0;
		    result.M31 = vector.X;
		    result.M32 = vector.Y;
		    result.M33 = vector.Z;
		    result.M34 = 0;
		    result.M41 = objectPosition.X;
		    result.M42 = objectPosition.Y;
		    result.M43 = objectPosition.Z;
		    result.M44 = 1;

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix"/> which contains the rotation moment around specified axis.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation in radians.</param>
        /// <returns>The rotation <see cref="Matrix"/>.</returns>
        public static Matrix CreateFromAxisAngle(Vector3 axis, float angle)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float num2 = Mathf.Sin(angle);
            float num = Mathf.Cos(angle);
            float num11 = x * x;
            float num10 = y * y;
            float num9 = z * z;
            float num8 = x * y;
            float num7 = x * z;
            float num6 = y * z;

            Matrix result;
            result.M11 = num11 + num * (1f - num11);
            result.M12 = num8 - num * num8 + num2 * z;
            result.M13 = num7 - num * num7 - num2 * y;
            result.M14 = 0;
            result.M21 = num8 - num * num8 - num2 * z;
            result.M22 = num10 + num * (1f - num10);
            result.M23 = num6 - num * num6 + num2 * x;
            result.M24 = 0;
            result.M31 = num7 - num * num7 + num2 * y;
            result.M32 = num6 - num * num6 - num2 * x;
            result.M33 = num9 + num * (1f - num9);
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;

            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix"/> from a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion"><see cref="Quaternion"/> of rotation moment.</param>
        /// <returns>The rotation <see cref="Matrix"/>.</returns>
        public static Matrix CreateFromQuaternion(Quaternion quaternion)
        {
            float num9 = quaternion.X * quaternion.X;
            float num8 = quaternion.Y * quaternion.Y;
            float num7 = quaternion.Z * quaternion.Z;
            float num6 = quaternion.X * quaternion.Y;
            float num5 = quaternion.Z * quaternion.W;
            float num4 = quaternion.Z * quaternion.X;
            float num3 = quaternion.Y * quaternion.W;
            float num2 = quaternion.Y * quaternion.Z;
            float num = quaternion.X * quaternion.W;

            Matrix result;
            result.M11 = 1f - 2f * (num8 + num7);
            result.M12 = 2f * (num6 + num5);
            result.M13 = 2f * (num4 - num3);
            result.M14 = 0f;
            result.M21 = 2f * (num6 - num5);
            result.M22 = 1f - 2f * (num7 + num9);
            result.M23 = 2f * (num2 + num);
            result.M24 = 0f;
            result.M31 = 2f * (num4 + num3);
            result.M32 = 2f * (num2 - num);
            result.M33 = 1f - 2f * (num8 + num9);
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix"/> from the specified yaw, pitch and roll values.
        /// </summary>
        /// <param name="yaw">The yaw rotation value in radians.</param>
        /// <param name="pitch">The pitch rotation value in radians.</param>
        /// <param name="roll">The roll rotation value in radians.</param>
        /// <returns>The rotation <see cref="Matrix"/>.</returns>
        /// <remarks>For more information about yaw, pitch and roll visit http://en.wikipedia.org/wiki/Euler_angles.
        /// </remarks>
		public static Matrix CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
			Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
			Matrix result = CreateFromQuaternion(quaternion);
		    return result;
		}

        /// <summary>
        /// Creates a new viewing <see cref="Matrix"/>.
        /// </summary>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraTarget">Lookup vector of the camera.</param>
        /// <param name="cameraUpVector">The direction of the upper edge of the camera.</param>
        /// <returns>The viewing <see cref="Matrix"/>.</returns>
        public static Matrix CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            Vector3 vector = Vector3.Normalize(cameraPosition - cameraTarget);
            Vector3 vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
            Vector3 vector3 = Vector3.Cross(vector, vector2);

            Matrix result;
            result.M11 = vector2.X;
            result.M12 = vector3.X;
            result.M13 = vector.X;
            result.M14 = 0f;
            result.M21 = vector2.Y;
            result.M22 = vector3.Y;
            result.M23 = vector.Y;
            result.M24 = 0f;
            result.M31 = vector2.Z;
            result.M32 = vector3.Z;
            result.M33 = vector.Z;
            result.M34 = 0f;
            result.M41 = -Vector3.Dot(vector2, cameraPosition);
            result.M42 = -Vector3.Dot(vector3, cameraPosition);
            result.M43 = -Vector3.Dot(vector, cameraPosition);
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix"/> for orthographic view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <returns>The new projection <see cref="Matrix"/> for orthographic view.</returns>
        public static Matrix CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
        {
            Matrix result;
            result.M11 = 2f / width;
            result.M12 = result.M13 = result.M14 = 0f;
            result.M22 = 2f / height;
            result.M21 = result.M23 = result.M24 = 0f;
            result.M33 = 1f / (zNearPlane - zFarPlane);
            result.M31 = result.M32 = result.M34 = 0f;
            result.M41 = result.M42 = 0f;
            result.M43 = zNearPlane / (zNearPlane - zFarPlane);
            result.M44 = 1f;

		    return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix"/> for customized orthographic view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <returns>The new projection <see cref="Matrix"/> for customized orthographic view.</returns>
        public static Matrix CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
        {
			Matrix result;
			result.M11 = 2.0f / (right - left);
			result.M12 = 0.0f;
			result.M13 = 0.0f;
			result.M14 = 0.0f;
			result.M21 = 0.0f;
			result.M22 = 2.0f / (top - bottom);
			result.M23 = 0.0f;
			result.M24 = 0.0f;
			result.M31 = 0.0f;
			result.M32 = 0.0f;
			result.M33 = 1.0f / (zNearPlane - zFarPlane);
			result.M34 = 0.0f;
			result.M41 = (left + right) / (left - right);
			result.M42 = (top + bottom) / (bottom - top);
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1.0f;

			return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix"/> for perspective view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <returns>The new projection <see cref="Matrix"/> for perspective view.</returns>
        public static Matrix CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
        {
            if (nearPlaneDistance <= 0f)
            {
	            throw new ArgumentException("nearPlaneDistance <= 0");
            }

            if (farPlaneDistance <= 0f)
            {
	            throw new ArgumentException("farPlaneDistance <= 0");
            }

            if (nearPlaneDistance >= farPlaneDistance)
            {
	            throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }

            Matrix result;
            result.M11 = 2f * nearPlaneDistance / width;
            result.M12 = result.M13 = result.M14 = 0f;
            result.M22 = 2f * nearPlaneDistance / height;
            result.M21 = result.M23 = result.M24 = 0f;
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M31 = result.M32 = 0f;
            result.M34 = -1f;
            result.M41 = result.M42 = result.M44 = 0f;
            result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

		    return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix"/> for perspective view with field of view.
        /// </summary>
        /// <param name="fieldOfView">Field of view in the y direction in radians.</param>
        /// <param name="aspectRatio">Width divided by height of the viewing volume.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <returns>The new projection <see cref="Matrix"/> for perspective view with FOV.</returns>
        public static Matrix CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        {
            if (fieldOfView <= 0f || fieldOfView >= 3.141593f)
            {
	            throw new ArgumentException("fieldOfView <= 0 or >= PI");
            }

            if (nearPlaneDistance <= 0f)
            {
	            throw new ArgumentException("nearPlaneDistance <= 0");
            }
            if (farPlaneDistance <= 0f)
            {
	            throw new ArgumentException("farPlaneDistance <= 0");
            }

            if (nearPlaneDistance >= farPlaneDistance)
            {
	            throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }

            float num = 1f / Mathf.Tan(fieldOfView * 0.5f);
            float num9 = num / aspectRatio;
            Matrix result;
            result.M11 = num9;
            result.M12 = result.M13 = result.M14 = 0;
            result.M22 = num;
            result.M21 = result.M23 = result.M24 = 0;
            result.M31 = result.M32 = 0f;
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1;
            result.M41 = result.M42 = result.M44 = 0;
            result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix"/> for customized perspective view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <returns>The new <see cref="Matrix"/> for customized perspective view.</returns>
        public static Matrix CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
        {
            if (nearPlaneDistance <= 0f)
            {
	            throw new ArgumentException("nearPlaneDistance <= 0");
            }

            if (farPlaneDistance <= 0f)
            {
	            throw new ArgumentException("farPlaneDistance <= 0");
            }

            if (nearPlaneDistance >= farPlaneDistance)
            {
	            throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }

            Matrix result;
            result.M11 = 2f * nearPlaneDistance / (right - left);
            result.M12 = result.M13 = result.M14 = 0;
            result.M22 = 2f * nearPlaneDistance / (top - bottom);
            result.M21 = result.M23 = result.M24 = 0;
            result.M31 = (left + right) / (right - left);
            result.M32 = (top + bottom) / (top - bottom);
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1;
            result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M41 = result.M42 = result.M44 = 0;

            return result;
        }


        /// <summary>
        /// Creates a new rotation <see cref="Matrix"/> around X axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>The rotation <see cref="Matrix"/> around X axis.</returns>
        public static Matrix CreateRotationX(float radians)
        {
	        Matrix result = Identity;

            float val1 = Mathf.Cos(radians);
            float val2 = Mathf.Sin(radians);

            result.M22 = val1;
            result.M23 = val2;
            result.M32 = -val2;
            result.M33 = val1;

            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix"/> around Y axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>The rotation <see cref="Matrix"/> around Y axis.</returns>
        public static Matrix CreateRotationY(float radians)
        {
	        Matrix result = Identity;

            float val1 = Mathf.Cos(radians);
            float val2 = Mathf.Sin(radians);

            result.M11 = val1;
            result.M13 = -val2;
            result.M31 = val2;
            result.M33 = val1;

            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix"/> around Z axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>The rotation <see cref="Matrix"/> around Z axis.</returns>
        public static Matrix CreateRotationZ(float radians)
        {
	        Matrix result = Identity;

            float val1 = Mathf.Cos(radians);
            float val2 = Mathf.Sin(radians);

            result.M11 = val1;
            result.M12 = val2;
            result.M21 = -val2;
            result.M22 = val1;

            return result;
        }

        /// <summary>
        /// Creates a new scaling <see cref="Matrix"/>.
        /// </summary>
        /// <param name="xScale">Scale value for X axis.</param>
        /// <param name="yScale">Scale value for Y axis.</param>
        /// <param name="zScale">Scale value for Z axis.</param>
        /// <returns>The scaling <see cref="Matrix"/>.</returns>
        public static Matrix CreateScale(float xScale, float yScale, float zScale)
        {
	        Matrix result;
	        result.M11 = xScale;
	        result.M12 = 0;
	        result.M13 = 0;
	        result.M14 = 0;
	        result.M21 = 0;
	        result.M22 = yScale;
	        result.M23 = 0;
	        result.M24 = 0;
	        result.M31 = 0;
	        result.M32 = 0;
	        result.M33 = zScale;
	        result.M34 = 0;
	        result.M41 = 0;
	        result.M42 = 0;
	        result.M43 = 0;
	        result.M44 = 1;

            return result;
        }

        /// <summary>
        /// Creates a new scaling <see cref="Matrix"/>.
        /// </summary>
        /// <param name="scales"><see cref="Vector3"/> representing x,y and z scale values.</param>
        /// <returns>The scaling <see cref="Matrix"/>.</returns>
        public static Matrix CreateScale(Vector3 scales)
        {
            Matrix result;
            result.M11 = scales.X;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = scales.Y;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = scales.Z;
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;

            return result;
        }
        
        /// <summary>
        /// Creates a new translation <see cref="Matrix"/>.
        /// </summary>
        /// <param name="xPosition">X coordinate of translation.</param>
        /// <param name="yPosition">Y coordinate of translation.</param>
        /// <param name="zPosition">Z coordinate of translation.</param>
        /// <returns>The translation <see cref="Matrix"/>.</returns>
        public static Matrix CreateTranslation(float xPosition, float yPosition, float zPosition)
        {
            Matrix result;
            result.M11 = 1;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = 1;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = 1;
            result.M34 = 0;
            result.M41 = xPosition;
            result.M42 = yPosition;
            result.M43 = zPosition;
            result.M44 = 1;

            return result;
        }

        /// <summary>
        /// Creates a new translation <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">X,Y and Z coordinates of translation.</param>
        /// <returns>The translation <see cref="Matrix"/>.</returns>
        public static Matrix CreateTranslation(Vector3 position)
        {
			return CreateTranslation(position.X, position.Y, position.Z);
        }

        /// <summary>
        /// Decomposes this matrix to translation, rotation and scale elements. Returns <c>true</c> if matrix can be decomposed; <c>false</c> otherwise.
        /// </summary>
        /// <param name="scale">Scale vector as an output parameter.</param>
        /// <param name="translation">Translation vector as an output parameter.</param>
        /// <returns><c>true</c> if matrix can be decomposed; <c>false</c> otherwise.</returns>
        public bool Decompose(Vector3 scale, Vector3 translation)
        {
            translation.X = M41;
            translation.Y = M42;
            translation.Z = M43;

            float xs = Mathf.Sign(M11 * M12 * M13 * M14) < 0 ? -1 : 1;
            float ys = Mathf.Sign(M21 * M22 * M23 * M24) < 0 ? -1 : 1;
            float zs = Mathf.Sign(M31 * M32 * M33 * M34) < 0 ? -1 : 1;

            scale.X = xs * Mathf.Sqrt(M11 * M11 + M12 * M12 + M13 * M13);
            scale.Y = ys * Mathf.Sqrt(M21 * M21 + M22 * M22 + M23 * M23);
            scale.Z = zs * Mathf.Sqrt(M31 * M31 + M32 * M32 + M33 * M33);

            if (Mathf.Abs(scale.X) < Mathf.Epsilon || Mathf.Abs(scale.Y) < Mathf.Epsilon || Mathf.Abs(scale.Z) < Mathf.Epsilon)
            {
	            return false;
            }

            Matrix m1 = new Matrix(M11 / scale.X, M12 / scale.X, M13 / scale.X, 0,
                                   M21 / scale.Y, M22 / scale.Y, M23 / scale.Y, 0,
                                   M31 / scale.Z, M32 / scale.Z, M33 / scale.Z, 0,
                                   0, 0, 0, 1);

            Quaternion.CreateFromRotationMatrix(m1);
            return true;
        }

		/// <summary>
        /// Returns a determinant of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>Determinant of this <see cref="Matrix"/></returns>
        /// <remarks>See more about determinant here - http://en.wikipedia.org/wiki/Determinant.
        /// </remarks>
        public float Determinant()
        {
            float num22 = M11;
		    float num21 = M12;
		    float num20 = M13;
		    float num19 = M14;
		    float num12 = M21;
		    float num11 = M22;
		    float num10 = M23;
		    float num9 = M24;
		    float num8 = M31;
		    float num7 = M32;
		    float num6 = M33;
		    float num5 = M34;
		    float num4 = M41;
		    float num3 = M42;
		    float num2 = M43;
		    float num = M44;
		    float num18 = num6 * num - num5 * num2;
		    float num17 = num7 * num - num5 * num3;
		    float num16 = num7 * num2 - num6 * num3;
		    float num15 = num8 * num - num5 * num4;
		    float num14 = num8 * num2 - num6 * num4;
		    float num13 = num8 * num3 - num7 * num4;
		    return num22 * (num11 * num18 - num10 * num17 + num9 * num16) - num21 * (num12 * num18 - num10 * num15 + num9 * num14) + num20 * (num12 * num17 - num11 * num15 + num9 * num13) - num19 * (num12 * num16 - num11 * num14 + num10 * num13);
        }

		/// <summary>
        /// Creates a new <see cref="Matrix"/> which contains inversion of the specified matrix. 
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix"/>.</param>
        /// <returns>The inverted matrix.</returns>
        public static Matrix Invert(Matrix matrix)
        {
            Matrix result;
            float num1 = matrix.M11;
			float num2 = matrix.M12;
			float num3 = matrix.M13;
			float num4 = matrix.M14;
			float num5 = matrix.M21;
			float num6 = matrix.M22;
			float num7 = matrix.M23;
			float num8 = matrix.M24;
			float num9 = matrix.M31;
			float num10 = matrix.M32;
			float num11 = matrix.M33;
			float num12 = matrix.M34;
			float num13 = matrix.M41;
			float num14 = matrix.M42;
			float num15 = matrix.M43;
			float num16 = matrix.M44;
			float num17 = num11 * num16 - num12 * num15;
			float num18 = num10 * num16 - num12 * num14;
			float num19 = num10 * num15 - num11 * num14;
			float num20 = num9 * num16 - num12 * num13;
			float num21 = num9 * num15 - num11 * num13;
			float num22 = num9 * num14 - num10 * num13;
			float num23 = num6 * num17 - num7 * num18 + num8 * num19;
			float num24 = -(num5 * num17 - num7 * num20 + num8 * num21);
			float num25 = num5 * num18 - num6 * num20 + num8 *  num22;
			float num26 = -(num5 * num19 - num6 * num21 + num7 * num22);
			float num27 = 1.0f / (num1 * num23 + num2 * num24 + num3 * num25 + num4 * num26);
			
			result.M11 = num23 * num27;
			result.M21 = num24 * num27;
			result.M31 = num25 * num27;
			result.M41 = num26 * num27;
			result.M12 = -(num2 * num17 - num3 * num18 + num4 * num19) * num27;
			result.M22 = (num1 * num17 - num3 * num20 + num4 * num21) * num27;
			result.M32 = -(num1 * num18 - num2 * num20 + num4 * num22) * num27;
			result.M42 = (num1 * num19 - num2 * num21 + num3 * num22) * num27;
			float num28 = num7 * num16 - num8 * num15;
			float num29 = num6 * num16 - num8 * num14;
			float num30 = num6 * num15 - num7 * num14;
			float num31 = num5 * num16 - num8 * num13;
			float num32 = num5 * num15 - num7 * num13;
			float num33 = num5 * num14 - num6 * num13;
			result.M13 = (num2 * num28 - num3 * num29 + num4 * num30) * num27;
			result.M23 = -(num1 * num28 - num3 * num31 + num4 * num32) * num27;
			result.M33 = (num1 * num29 - num2 * num31 + num4 * num33) * num27;
			result.M43 = -(num1 * num30 - num2 * num32 + num3 * num33) * num27;
			float num34 = num7 * num12 - num8 * num11;
			float num35 = num6 * num12 - num8 * num10;
			float num36 = num6 * num11 - num7 * num10;
			float num37 = num5 * num12 - num8 * num9;
			float num38 = num5 * num11 - num7 * num9;
			float num39 = num5 * num10 - num6 * num9;
			result.M14 = -(num2 * num34 - num3 * num35 + num4 * num36) * num27;
			result.M24 = (num1 * num34 - num3 * num37 + num4 * num38) * num27;
			result.M34 = -(num1 * num35 - num2 * num37 + num4 * num39) * num27;
			result.M44 = (num1 * num36 - num2 * num38 + num3 * num39) * num27;

            return result;
        }
        /// <summary>
        /// Creates a new <see cref="Matrix"/> that contains linear interpolation of the values in specified matrixes.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix"/>.</param>
        /// <param name="matrix2">The second <see cref="Vector2"/>.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>>The result of linear interpolation of the specified matrixes.</returns>
        public static Matrix Lerp(Matrix matrix1, Matrix matrix2, float amount)
        {
		    matrix1.M11 += (matrix2.M11 - matrix1.M11) * amount;
		    matrix1.M12 += (matrix2.M12 - matrix1.M12) * amount;
		    matrix1.M13 += (matrix2.M13 - matrix1.M13) * amount;
		    matrix1.M14 += (matrix2.M14 - matrix1.M14) * amount;
		    matrix1.M21 += (matrix2.M21 - matrix1.M21) * amount;
		    matrix1.M22 += (matrix2.M22 - matrix1.M22) * amount;
		    matrix1.M23 += (matrix2.M23 - matrix1.M23) * amount;
		    matrix1.M24 += (matrix2.M24 - matrix1.M24) * amount;
		    matrix1.M31 += (matrix2.M31 - matrix1.M31) * amount;
		    matrix1.M32 += (matrix2.M32 - matrix1.M32) * amount;
		    matrix1.M33 += (matrix2.M33 - matrix1.M33) * amount;
		    matrix1.M34 += (matrix2.M34 - matrix1.M34) * amount;
		    matrix1.M41 += (matrix2.M41 - matrix1.M41) * amount;
		    matrix1.M42 += (matrix2.M42 - matrix1.M42) * amount;
		    matrix1.M43 += (matrix2.M43 - matrix1.M43) * amount;
		    matrix1.M44 += (matrix2.M44 - matrix1.M44) * amount;

		    return matrix1;
        }

        /// <summary>
        /// Copy the values of specified <see cref="Matrix"/> to the float array.
        /// </summary>
        /// <param name="matrix">The source <see cref="Matrix"/>.</param>
        /// <returns>The array which matrix values will be stored.</returns>
        /// <remarks>
        /// Required for OpenGL 2.0 projection matrix stuff.
        /// </remarks>
        public static float[] ToFloatArray(Matrix matrix)
        {
            float[] array =
            {
	            matrix.M11, matrix.M12, matrix.M13, matrix.M14,
	            matrix.M21, matrix.M22, matrix.M23, matrix.M24,
	            matrix.M31, matrix.M32, matrix.M33, matrix.M34,
	            matrix.M41, matrix.M42, matrix.M43, matrix.M44
            };

            return array;
        }

        /// <summary>
        /// Swap the matrix rows and columns.
        /// </summary>
        /// <param name="matrix">The matrix for transposing operation.</param>
        /// <returns>The new <see cref="Matrix"/> which contains the transposing result.</returns>
        public static Matrix Transpose(Matrix matrix)
        {
            Matrix result;
            result.M11 = matrix.M11;
            result.M12 = matrix.M21;
            result.M13 = matrix.M31;
            result.M14 = matrix.M41;
            result.M21 = matrix.M12;
            result.M22 = matrix.M22;
            result.M23 = matrix.M32;
            result.M24 = matrix.M42;
            result.M31 = matrix.M13;
            result.M32 = matrix.M23;
            result.M33 = matrix.M33;
            result.M34 = matrix.M43;
            result.M41 = matrix.M14;
            result.M42 = matrix.M24;
            result.M43 = matrix.M34;
            result.M44 = matrix.M44;

            return result;
        }

        #endregion
    }
}