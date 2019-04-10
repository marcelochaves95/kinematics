using System;
using System.Runtime.InteropServices;

namespace MathModule
{
    /// <summary>
    /// Represents a four dimensional mathematical vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4D : IEquatable<Vector4D>
    {
        /// <summary>
        /// The X component of the vector
        /// </summary>
        public float X;

        /// <summary>
        /// The Y component of the vector
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z component of the vector
        /// </summary>
        public float Z;

        /// <summary>
        /// The W component of the vector
        /// </summary>
        public float W;

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="x">Initial value for the X component of the vector</param>
        /// <param name="y">Initial value for the Y component of the vector</param>
        /// <param name="z">Initial value for the Z component of the vector</param>
        /// <param name="w">Initial value for the W component of the vector</param>
        public Vector4D(float x, float y, float z, float w)
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
        public Vector4D(Vector3D vector, float w)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
            W = w;
        }

        /// <summary>
        /// Shorthand for writing Vector4D(1, 0, 0, 0)
        /// </summary>
        public static readonly Vector4D UnitX = new Vector4D(1f, 0f, 0f, 0f);

        /// <summary>
        /// Shorthand for writing Vector4D(0, 1, 0, 0)
        /// </summary>
        public static readonly Vector4D UnitY = new Vector4D(0f, 1f, 0f, 0f);

        /// <summary>
        /// Shorthand for writing Vector4D(0, 0, 1, 0)
        /// </summary>
        public static readonly Vector4D UnitZ = new Vector4D(0f, 0f, 1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector4D(0, 0, 0, 1)
        /// </summary>
        public static readonly Vector4D UnitW = new Vector4D(0f, 0f, 0f, 1f);

        /// <summary>
        /// Shorthand for writing Vector4D(1, 1, 1, 1)
        /// </summary>
        public static readonly Vector4D One = new Vector4D(1f, 1f, 1f, 1f);

        /// <summary>
        /// Shorthand for writing Vector4D(0, 0, 0, 0)
        /// </summary>
        public static readonly Vector4D Zero = new Vector4D(0f, 0f, 0f, 0f);

        /// <summary>
        /// Operator + overload ; add two vectors
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second vector</param>
        /// <returns>v1 + v2</returns>
        public static Vector4D operator +(Vector4D vector1, Vector4D vector2) => new Vector4D(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z, vector1.W + vector2.W);

        /// <summary>
        /// Operator - overload ; subtracts two vectors
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second vector</param>
        /// <returns>v1 - v2</returns>
        public static Vector4D operator -(Vector4D vector1, Vector4D vector2) => new Vector4D(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z, vector1.W - vector2.W);

        /// <summary>
        /// Operator - overload ; returns the opposite of a vector
        /// </summary>
        /// <param name="vector">Vector to negate</param>
        /// <returns>-v</returns>
        public static Vector4D operator -(Vector4D vector) => new Vector4D(-vector.X, -vector.Y, -vector.Z, -vector.W);

        /// <summary>
        /// Operator * overload ; multiply a vector by a scalar value
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="scalar">Scalar value</param>
        /// <returns>v * s</returns>
        public static Vector4D operator *(Vector4D vector, float scalar) => new Vector4D(vector.X * scalar, vector.Y * scalar, vector.Z * scalar, vector.W * scalar);

        /// <summary>
        /// Operator * overload ; multiply a scalar value by a vector
        /// </summary>
        /// <param name="scalar">Scalar value</param>
        /// <param name="vector">Vector</param>
        /// <returns>s * v</returns>
        public static Vector4D operator *(float scalar, Vector4D vector) => new Vector4D(vector.X * scalar, vector.Y * scalar, vector.Z * scalar, vector.W * scalar);

        /// <summary>
        /// Operator / overload ; divide a vector by a scalar value
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="scalar">Scalar value</param>
        /// <returns>v / s</returns>
        public static Vector4D operator /(Vector4D vector, float scalar) => new Vector4D(vector.X / scalar, vector.Y / scalar, vector.Z / scalar, vector.W / scalar);

        /// <summary>
        /// Operator == overload ; check vector equality
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second vector</param>
        /// <returns>v1 == v2</returns>
        public static bool operator ==(Vector4D vector1, Vector4D vector2) => vector1.Equals(vector2);

        /// <summary>
        /// Operator != overload ; check vector inequality
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second vector</param>
        /// <returns>v1 != v2</returns>
        public static bool operator !=(Vector4D vector1, Vector4D vector2) => !vector1.Equals(vector2);

        /// <summary>
        /// Performs an explicit conversion from Vector4D to Vector4D
        /// </summary>
        /// <param name="vector">The value</param>
        /// <returns>The result of the conversion</returns>
        public static explicit operator Vector3D(Vector4D vector) => Vector3D(vector.X, vector.Y, vector.Z);

        /// <summary>
        /// Compare vector and object and checks if they are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>Object and vector are equal</returns>
        public override bool Equals(object obj) => (obj is Vector4D) && Equals((Vector4D)obj);

        /// <summary>
        /// Compare two vectors and checks if they are equal
        /// </summary>
        /// <param name="other">Vector to check</param>
        /// <returns>Vectors are equal</returns>
        public bool Equals(Vector4D other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        /// <summary>
        /// Get's a value indicting whether this instance is normalized
        /// </summary>
        public bool IsNormalized() => Mathematics.Abs(X * X + Y * Y + Z * Z + W * W - 1f) < 0f;

        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        /// <returns>May be preferred when only the relative length is needed and speed is of the essence</returns>
        public static float Magnitude(Vector4D vector) => new Mathematics.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z, vector.W * vector.W);

        /// <summary>
        /// Converts the vector into a unit vector
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns>Normalized vector</returns>
        public static Vector4D Normalize(Vector4D vector) => vector / Magnitude(vector);

        /// <summary>
        /// Converts the vector into a unit vector
        /// </summary>
        /// <returns>Normalized vector</returns>
        public Vector4D Normalize() => this / Magnitude(this);

        /// <summary>
        /// Restricts a value to be within a specified range
        /// </summary>
        /// <param name="vector">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>The clamped value</returns>
        public static Vector4D Clamp(Vector4D vector, Vector4D min, Vector4D max)
        {
            Vector4D result;

            result.X = vector.X;
            result.X = (result.X > max.X) ? max.X : result.X;
            result.X = (result.X < min.X) ? min.X : result.X;

            result.Y = vector.Y;
            result.Y = (result.Y > max.Y) ? max.Y : result.Y;
            result.Y = (result.Y < min.Y) ? min.Y : result.Y;

            result.Z = vector.Z;
            result.Z = (result.Z > max.Z) ? max.Z : result.Z;
            result.Z = (result.Z < min.Z) ? min.Z : result.Z;

            result.W = vector.W;
            result.W = (result.W > max.W) ? max.W : result.W;
            result.W = (result.W < min.W) ? min.W : result.W;

            return result;
        }

        /// <summary>
        /// Calculates the distance between two vectors
        /// </summary>
        /// <param name="vector1">The first vector</param>
        /// <param name="vector2">The second vector</param>
        /// <returns>The distance between the two vectors</returns>
        public static float Distance(Vector4D vector1, Vector4D vector2)
        {
            float x = vector1.X - vector2.X;
            float y = vector1.Y - vector2.Y;
            float z = vector1.Z - vector2.Z;
            float w = vector1.W - vector2.W;

            return Mathematics.Sqrt(x * x + y * y + z * z + w * w);
        }

        /// <summary>
        /// Calculates the dot product of two vectors
        /// </summary>
        /// <param name="vector1">First source vector</param>
        /// <param name="vector2">Second source vector</param>
        /// <returns>The dot product of the two vectors</returns>
        public static float DotProduct(Vector4D vector1, Vector4D vector2) => vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W;

        /// <summary>
        /// Performs a linear interpolation between two vectors
        /// </summary>
        /// <param name="start">Start vector</param>
        /// <param name="end">End vector</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two vectors</returns>
        public static Vector4D Lerp(Vector4D start, Vector4D end, float amount)
        {
            return new Vector4D(
                start.X + ((end.X - start.X) * amount),
                start.Y + ((end.Y - start.Y) * amount),
                start.Z + ((end.Z - start.Z) * amount),
                start.W + ((end.W - start.W) * amount));
        }

        /// <summary>
        /// Performs a Hermite spline interpolation
        /// </summary>
        /// <param name="vector1">First source position vector</param>
        /// <param name="tangent1">First source tangent vector</param>
        /// <param name="vector2">Second source position vector</param>
        /// <param name="tangent2">Second source tangent vector</param>
        /// <param name="amount">Weighting factor</param>
        /// <returns>The result of the Hermite spline interpolation</returns>
        public static Vector4D Hermite(Vector4D vector1, Vector4D tangent1, Vector4D vector2, Vector4D tangent2, float amount)
        {
            float squared = amount * amount;
            float cubed = amount * squared;
            float part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
            float part2 = (-2.0f * cubed) + (3.0f * squared);
            float part3 = (cubed - (2.0f * squared)) + amount;
            float part4 = cubed - squared;

            return new Vector4D(
                (((vector1.X * part1) + (vector2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4),
                (((vector1.Y * part1) + (vector2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4),
                (((vector1.Z * part1) + (vector2.Z * part2)) + (tangent1.Z * part3)) + (tangent2.Z * part4),
                (((vector1.W * part1) + (vector2.W * part2)) + (tangent1.W * part3)) + (tangent2.W * part4));
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions
        /// </summary>
        /// <param name="vector1">The first position in the interpolation</param>
        /// <param name="vector2">The second position in the interpolation</param>
        /// <param name="vector3">The third position in the interpolation</param>
        /// <param name="vector4">The fourth position in the interpolation</param>
        /// <param name="amount">Weighting factor</param>
        /// <returns>A vector that is the result of the Catmull-Rom interpolation</returns>
        public static Vector4D CatmullRom(Vector4D vector1, Vector4D vector2, Vector4D vector3, Vector4D vector4, float amount)
        {
            float squared = amount * amount;
            float cubed = amount * squared;

            return new Vector4D(
                0.5f * ((((2.0f * vector2.X) + ((-vector1.X + vector3.X) * amount)) +
                        (((((2.0f * vector1.X) - (5.0f * vector2.X)) + (4.0f * vector3.X)) - vector4.X) * squared)) +
                    ((((-vector1.X + (3.0f * vector2.X)) - (3.0f * vector3.X)) + vector4.X) * cubed)),
                0.5f * ((((2.0f * vector2.Y) + ((-vector1.Y + vector3.Y) * amount)) +
                        (((((2.0f * vector1.Y) - (5.0f * vector2.Y)) + (4.0f * vector3.Y)) - vector4.Y) * squared)) +
                    ((((-vector1.Y + (3.0f * vector2.Y)) - (3.0f * vector3.Y)) + vector4.Y) * cubed)),
                0.5f * ((((2.0f * vector2.Z) + ((-vector1.Z + vector3.Z) * amount)) +
                        (((((2.0f * vector1.Z) - (5.0f * vector2.Z)) + (4.0f * vector3.Z)) - vector4.Z) * squared)) +
                    ((((-vector1.Z + (3.0f * vector2.Z)) - (3.0f * vector3.Z)) + vector4.Z) * cubed)),
                0.5f * ((((2.0f * vector2.Z) + ((-vector1.Z + vector3.Z) * amount)) +
                        (((((2.0f * vector1.Z) - (5.0f * vector2.Z)) + (4.0f * vector3.Z)) - vector4.Z) * squared)) +
                    ((((-vector1.Z + (3.0f * vector2.Z)) - (3.0f * vector3.Z)) + vector4.Z) * cubed)),
                0.5f * ((((2.0f * vector2.W) + ((-vector1.W + vector3.W) * amount)) +
                        (((((2.0f * vector1.W) - (5.0f * vector2.W)) + (4.0f * vector3.W)) - vector4.W) * squared)) +
                    ((((-vector1.W + (3.0f * vector2.W)) - (3.0f * vector3.W)) + vector4.W) * cubed)));
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors
        /// </summary>
        /// <param name="vector1">The first source vector</param>
        /// <param name="vector2">The second source vector</param>
        /// <returns>A vector containing the largest components of the source vectors</returns>
        public static Vector4D Max(Vector4D vector1, Vector4D vector2)
        {
            return new Vector4D(
                (vector1.X > vector2.X) ? vector1.X : vector2.X,
                (vector1.Y > vector2.Y) ? vector1.Y : vector2.Y,
                (vector1.Z > vector2.Z) ? vector1.Z : vector2.Z,
                (vector1.W > vector2.W) ? vector1.W : vector2.W);
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first source vector.</param>
        /// <param name="vector2">The second source vector.</param>
        /// <returns>A vector containing the smallest components of the source vectors.</returns>
        public static Vector4D Min(Vector4D vector1, Vector4D vector2)
        {
            return new Vector4D(
                (vector1.X < vector2.X) ? vector1.X : vector2.X,
                (vector1.Y < vector2.Y) ? vector1.Y : vector2.Y,
                (vector1.Z < vector2.Z) ? vector1.Z : vector2.Z,
                (vector1.W < vector2.W) ? vector1.W : vector2.W);
        }

        /// <summary>
        /// Transforms a 4D vector by the given rotation
        /// </summary>
        /// <param name="vector">The vector to rotate</param>
        /// <param name="rotation">The rotation to apply</param>
        /// <returns>The transformed</returns>
        public static Vector4D Transform(Vector4D vector, Quaternion rotation)
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

            float num1 = ((1.0f - yy) - zz);
            float num2 = (xy - wz);
            float num3 = (xz + wy);
            float num4 = (xy + wz);
            float num5 = ((1.0f - xx) - zz);
            float num6 = (yz - wx);
            float num7 = (xz - wy);
            float num8 = (yz + wx);
            float num9 = ((1.0f - xx) - yy);

            return new Vector4D(
                ((vector.X * num1) + (vector.Y * num2)) + (vector.Z * num3),
                ((vector.X * num4) + (vector.Y * num5)) + (vector.Z * num6),
                ((vector.X * num7) + (vector.Y * num8)) + (vector.Z * num9),
                vector.W);
        }

        /// <summary>
        /// Transforms a 4D vector by the given rotation
        /// </summary>
        /// <param name="vector">The source vector</param>
        /// <param name="transform">The transformation</param>
        /// <returns>The transformed</returns>
        public static Vector4D Transform(Vector4D vector, Matrix transform)
        {
            return new Vector4D(
                (vector.X * transform.M11) + (vector.Y * transform.M21) + (vector.Z * transform.M31) + (vector.W * transform.M41),
                (vector.X * transform.M12) + (vector.Y * transform.M22) + (vector.Z * transform.M32) + (vector.W * transform.M42),
                (vector.X * transform.M13) + (vector.Y * transform.M23) + (vector.Z * transform.M33) + (vector.W * transform.M43),
                (vector.X * transform.M14) + (vector.Y * transform.M24) + (vector.Z * transform.M34) + (vector.W * transform.M44));
        }

        /// <summary>
        /// Returns a hash code for this instance
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();

        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString() => $"[Vector4D] X({ X }) Y({ Y }) Z({ Z }) W({ W })";
    }
}