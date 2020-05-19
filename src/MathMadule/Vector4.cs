using System;
using System.Runtime.InteropServices;

namespace Kinematics.MathModule
{
    /// <summary>
    /// Represents a four dimensional mathematical vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 : IEquatable<Vector4>
    {
        /// <summary>
        /// The X component of the vector
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The Y component of the vector
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The Z component of the vector
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// The W component of the vector
        /// </summary>
        public float W { get; set; }

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="x">Initial value for the X component of the vector</param>
        /// <param name="y">Initial value for the Y component of the vector</param>
        /// <param name="z">Initial value for the Z component of the vector</param>
        /// <param name="w">Initial value for the W component of the vector</param>
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="vector">A vector containing the values with which to initialize the X, Y, and Z components</param>
        /// <param name="w">Initial value for the W component of the vector</param>
        public Vector4(Vector3 value, float w)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = w;
        }

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="values">The values to assign to the X, Y, Z and W components of the vector. This must be an array with three elements</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than three elements</exception>
        public Vector4(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            if (values.Length != 4)
                throw new ArgumentOutOfRangeException("values", "There must be three and only three input values for Vector4.");

            X = values[0];
            Y = values[1];
            Z = values[2];
            W = values[3];
        }

        /// <summary>
        /// Shorthand for writing Vector4(1, 0, 0, 0)
        /// </summary>
        public static readonly Vector4 UnitX = new Vector4(1f, 0f, 0f, 0f);

        /// <summary>
        /// Shorthand for writing Vector4(0, 1, 0, 0)
        /// </summary>
        public static readonly Vector4 UnitY = new Vector4(0f, 1f, 0f, 0f);

        /// <summary>
        /// Shorthand for writing Vector4(0, 0, 1, 0)
        /// </summary>
        public static readonly Vector4 UnitZ = new Vector4(0f, 0f, 1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector4(0, 0, 0, 1)
        /// </summary>
        public static readonly Vector4 UnitW = new Vector4(0f, 0f, 0f, 1f);

        /// <summary>
        /// Shorthand for writing Vector4(1, 1, 1, 1)
        /// </summary>
        public static readonly Vector4 One = new Vector4(1f, 1f, 1f, 1f);

        /// <summary>
        /// Shorthand for writing Vector4(0, 0, 0, 0)
        /// </summary>
        public static readonly Vector4 Zero = new Vector4(0f, 0f, 0f, 0f);

        /// <summary>
        /// Operator + overload ; add two vectors
        /// </summary>
        /// <param name="value1">First vector</param>
        /// <param name="value2">Second vector</param>
        /// <returns>v1 + v2</returns>
        public static Vector4 operator +(Vector4 value1, Vector4 value2) => new Vector4(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z, value1.W + value2.W);

        /// <summary>
        /// Operator - overload ; subtracts two vectors
        /// </summary>
        /// <param name="value1">First vector</param>
        /// <param name="value2">Second vector</param>
        /// <returns>v1 - v2</returns>
        public static Vector4 operator -(Vector4 value1, Vector4 value2) => new Vector4(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z, value1.W - value2.W);

        /// <summary>
        /// Operator - overload ; returns the opposite of a vector
        /// </summary>
        /// <param name="value">Vector to negate</param>
        /// <returns>-v</returns>
        public static Vector4 operator -(Vector4 value) => new Vector4(-value.X, -value.Y, -value.Z, -value.W);

        /// <summary>
        /// Operator * overload ; multiply a vector by a scalar value
        /// </summary>
        /// <param name="value">Vector</param>
        /// <param name="scalar">Scalar value</param>
        /// <returns>v * s</returns>
        public static Vector4 operator *(Vector4 value, float scalar) => new Vector4(value.X * scalar, value.Y * scalar, value.Z * scalar, value.W * scalar);

        /// <summary>
        /// Operator * overload ; multiply a scalar value by a vector
        /// </summary>
        /// <param name="scalar">Scalar value</param>
        /// <param name="value">Vector</param>
        /// <returns>s * v</returns>
        public static Vector4 operator *(float scalar, Vector4 value) => new Vector4(value.X * scalar, value.Y * scalar, value.Z * scalar, value.W * scalar);

        /// <summary>
        /// Operator / overload ; divide a vector by a scalar value
        /// </summary>
        /// <param name="value">Vector</param>
        /// <param name="scalar">Scalar value</param>
        /// <returns>v / s</returns>
        public static Vector4 operator /(Vector4 value, float scalar) => new Vector4(value.X / scalar, value.Y / scalar, value.Z / scalar, value.W / scalar);

        /// <summary>
        /// Operator == overload ; check vector equality
        /// </summary>
        /// <param name="value1">First vector</param>
        /// <param name="value2">Second vector</param>
        /// <returns>v1 == v2</returns>
        public static bool operator ==(Vector4 value1, Vector4 value2) => value1.Equals(value2);

        /// <summary>
        /// Operator != overload ; check vector inequality
        /// </summary>
        /// <param name="value1">First vector</param>
        /// <param name="value2">Second vector</param>
        /// <returns>v1 != v2</returns>
        public static bool operator !=(Vector4 value1, Vector4 value2) => !value1.Equals(value2);

        /// <summary>
        /// Performs an explicit conversion from Vector4 to Vector4
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The result of the conversion</returns>
        public static explicit operator Vector3(Vector4 value) => new Vector3(value.X, value.Y, value.Z);

        /// <summary>
        /// Compare vector and object and checks if they are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>Object and vector are equal</returns>
        public override bool Equals(object obj) => (obj is Vector4) && Equals((Vector4)obj);

        /// <summary>
        /// Compare two vectors and checks if they are equal
        /// </summary>
        /// <param name="other">Vector to check</param>
        /// <returns>Vectors are equal</returns>
        public bool Equals(Vector4 other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        /// <summary>
        /// Get's a value indicting whether this instance is normalized
        /// </summary>
        public bool IsNormalized() => Math.Abs(X * X + Y * Y + Z * Z + W * W - 1f) < 0f;

        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        /// <returns>May be preferred when only the relative length is needed and speed is of the essence</returns>
        public static float Magnitude(Vector4 value) => Math.Sqrt(Math.Pow(value.X, 2) + Math.Pow(value.Y, 2) + Math.Pow(value.Z, 2) + Math.Pow(value.W, 2));

        /// <summary>
        /// Converts the vector into a unit vector
        /// </summary>
        /// <param name="value">Vector</param>
        /// <returns>Normalized vector</returns>
        public static Vector4 Normalize(Vector4 value) => value / Magnitude(value);

        /// <summary>
        /// Converts the vector into a unit vector
        /// </summary>
        /// <returns>Normalized vector</returns>
        public Vector4 Normalize() => Normalize(this);

        /// <summary>
        /// Returns a containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 4D triangle
        /// </summary>
        /// <param name="value1">A containing the 4D Cartesian coordinates of vertex 1 of the triangle</param>
        /// <param name="value2">A containing the 4D Cartesian coordinates of vertex 2 of the triangle</param>
        /// <param name="value3">A containing the 4D Cartesian coordinates of vertex 3 of the triangle</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>)</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>)</param>
        /// <returns>A new containing the 4D Cartesian coordinates of the specified point</returns>
        public static Vector4 Barycentric(Vector4 value1, Vector4 value2, Vector4 value3, float amount1, float amount2)
        {
            return new Vector4(
                (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X)),
                (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y)),
                (value1.Z + (amount1 * (value2.Z - value1.Z))) + (amount2 * (value3.Z - value1.Z)),
                (value1.W + (amount1 * (value2.W - value1.W))) + (amount2 * (value3.W - value1.W)));
        }

        /// <summary>
        /// Restricts a value to be within a specified range
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>The clamped value</returns>
        public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max)
        {
            Vector4 result = new Vector4();

            result.X = value.X;
            result.X = (result.X > max.X) ? max.X : result.X;
            result.X = (result.X < min.X) ? min.X : result.X;

            result.Y = value.Y;
            result.Y = (result.Y > max.Y) ? max.Y : result.Y;
            result.Y = (result.Y < min.Y) ? min.Y : result.Y;

            result.Z = value.Z;
            result.Z = (result.Z > max.Z) ? max.Z : result.Z;
            result.Z = (result.Z < min.Z) ? min.Z : result.Z;

            result.W = value.W;
            result.W = (result.W > max.W) ? max.W : result.W;
            result.W = (result.W < min.W) ? min.W : result.W;

            return result;
        }

        /// <summary>
        /// Calculates the distance between two vectors
        /// </summary>
        /// <param name="value1">The first vector</param>
        /// <param name="value2">The second vector</param>
        /// <returns>The distance between the two vectors</returns>
        public static float Distance(Vector4 value1, Vector4 value2)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            float z = value1.Z - value2.Z;
            float w = value1.W - value2.W;

            return Math.Sqrt(x * x + y * y + z * z + w * w);
        }

        /// <summary>
        /// Calculates the dot product of two vectors
        /// </summary>
        /// <param name="value2">First source vector</param>
        /// <param name="value2">Second source vector</param>
        /// <returns>The dot product of the two vectors</returns>
        public static float DotProduct(Vector4 value1, Vector4 value2) => value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z + value1.W * value2.W;

        /// <summary>
        /// Performs a linear interpolation between two vectors
        /// </summary>
        /// <param name="start">Start vector</param>
        /// <param name="end">End vector</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two vectors</returns>
        public static Vector4 Lerp(Vector4 start, Vector4 end, float amount)
        {
            return new Vector4(
                start.X + ((end.X - start.X) * amount),
                start.Y + ((end.Y - start.Y) * amount),
                start.Z + ((end.Z - start.Z) * amount),
                start.W + ((end.W - start.W) * amount));
        }

        /// <summary>
        /// Performs a Hermite spline interpolation
        /// </summary>
        /// <param name="value1">First source position vector</param>
        /// <param name="tangent1">First source tangent vector</param>
        /// <param name="value2">Second source position vector</param>
        /// <param name="tangent2">Second source tangent vector</param>
        /// <param name="amount">Weighting factor</param>
        /// <returns>The result of the Hermite spline interpolation</returns>
        public static Vector4 Hermite(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
        {
            float squared = amount * amount;
            float cubed = amount * squared;
            float part1 = ((2f * cubed) - (3f * squared)) + 1f;
            float part2 = (-2f * cubed) + (3f * squared);
            float part3 = (cubed - (2f * squared)) + amount;
            float part4 = cubed - squared;

            return new Vector4(
                (((value1.X * part1) + (value2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4),
                (((value1.Y * part1) + (value2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4),
                (((value1.Z * part1) + (value2.Z * part2)) + (tangent1.Z * part3)) + (tangent2.Z * part4),
                (((value1.W * part1) + (value2.W * part2)) + (tangent1.W * part3)) + (tangent2.W * part4));
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions
        /// </summary>
        /// <param name="value1">The first position in the interpolation</param>
        /// <param name="value2">The second position in the interpolation</param>
        /// <param name="value3">The third position in the interpolation</param>
        /// <param name="value4">The fourth position in the interpolation</param>
        /// <param name="amount">Weighting factor</param>
        /// <returns>A vector that is the result of the Catmull-Rom interpolation</returns>
        public static Vector4 CatmullRom(Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount)
        {
            float squared = amount * amount;
            float cubed = amount * squared;

            return new Vector4(
                0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) +
                        (((((2f * value1.X) - (5.0f * value2.X)) + (4.0f * value3.X)) - value4.X) * squared)) +
                    ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * cubed)),
                0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) +
                        (((((2f * value1.Y) - (5.0f * value2.Y)) + (4.0f * value3.Y)) - value4.Y) * squared)) +
                    ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * cubed)),
                0.5f * ((((2f * value2.Z) + ((-value1.Z + value3.Z) * amount)) +
                        (((((2f * value1.Z) - (5.0f * value2.Z)) + (4.0f * value3.Z)) - value4.Z) * squared)) +
                    ((((-value1.Z + (3f * value2.Z)) - (3f * value3.Z)) + value4.Z) * cubed)),
                0.5f * ((((2f * value2.W) + ((-value1.W + value3.W) * amount)) +
                        (((((2f * value1.W) - (5.0f * value2.W)) + (4.0f * value3.W)) - value4.W) * squared)) +
                    ((((-value1.W + (3f * value2.W)) - (3f * value3.W)) + value4.W) * cubed)));
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors
        /// </summary>
        /// <param name="value1">The first source vector</param>
        /// <param name="value2">The second source vector</param>
        /// <returns>A vector containing the largest components of the source vectors</returns>
        public static Vector4 Max(Vector4 value1, Vector4 value2)
        {
            return new Vector4(
                (value1.X > value2.X) ? value1.X : value2.X,
                (value1.Y > value2.Y) ? value1.Y : value2.Y,
                (value1.Z > value2.Z) ? value1.Z : value2.Z,
                (value1.W > value2.W) ? value1.W : value2.W);
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors
        /// </summary>
        /// <param name="value1">The first source vector</param>
        /// <param name="value2">The second source vector</param>
        /// <returns>A vector containing the smallest components of the source vectors</returns>
        public static Vector4 Min(Vector4 value1, Vector4 value2)
        {
            return new Vector4(
                (value1.X < value2.X) ? value1.X : value2.X,
                (value1.Y < value2.Y) ? value1.Y : value2.Y,
                (value1.Z < value2.Z) ? value1.Z : value2.Z,
                (value1.W < value2.W) ? value1.W : value2.W);
        }

        /// <summary>
        /// Transforms a 4D vector by the given rotation
        /// </summary>
        /// <param name="value">The vector to rotate</param>
        /// <param name="rotation">The rotation to apply</param>
        /// <returns>The transformed</returns>
        public static Vector4 Transform(Vector4 value, Quaternion rotation)
        {
            float x = rotation.X + rotation.X;
            float y = rotation.Y + rotation.Y;
            float z = rotation.Z + rotation.Z;
            float wx = rotation.W * x;
            float wy = rotation.W * y;
            float wz = rotation.W * z;
            float xx = rotation.X * x;
            float xy = rotation.X * y;
            float xz = rotation.X * z;
            float yy = rotation.Y * y;
            float yz = rotation.Y * z;
            float zz = rotation.Z * z;

            float num1 = ((1f - yy) - zz);
            float num2 = (xy - wz);
            float num3 = (xz + wy);
            float num4 = (xy + wz);
            float num5 = ((1f - xx) - zz);
            float num6 = (yz - wx);
            float num7 = (xz - wy);
            float num8 = (yz + wx);
            float num9 = ((1f - xx) - yy);

            return new Vector4(
                ((value.X * num1) + (value.Y * num2)) + (value.Z * num3),
                ((value.X * num4) + (value.Y * num5)) + (value.Z * num6),
                ((value.X * num7) + (value.Y * num8)) + (value.Z * num9),
                value.W);
        }

        /// <summary>
        /// Returns a hash code for this instance
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table
        /// </returns>
        public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();

        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString() => $"[Vector4] X({ X }) Y({ Y }) Z({ Z }) W({ W })";

        #region Kinematics/Unity
        public static implicit operator Vector4(UnityEngine.Vector4 value)
        {
            return new Vector4(value.x, value.y, value.z, value.w);
        }

        public static implicit operator UnityEngine.Vector4(Vector4 value)
        {
            return new UnityEngine.Vector4(value.X, value.Y, value.Z, value.W);
        }
        #endregion
    }
}
