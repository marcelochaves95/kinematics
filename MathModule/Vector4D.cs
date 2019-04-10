using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace MathModule
{
    /// <summary>
    /// Represents a four dimensional mathematical vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vector4D : IEquatable<Vector4D>, IFormattable
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
        /// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components</param>
        /// <param name="w">Initial value for the W component of the vector</param>
        public Vector4D(Vector4D value, float w)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
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
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 + v2</returns>
        public static Vector4D operator +(Vector4D v1, Vector4D v2) => new Vector4D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W + v2.W);

        /// <summary>
        /// Operator - overload ; subtracts two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 - v2</returns>
        public static Vector4D operator -(Vector4D v1, Vector4D v2) => new Vector4D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W - v2.W);

        /// <summary>
        /// Operator - overload ; returns the opposite of a vector
        /// </summary>
        /// <param name="v">Vector to negate</param>
        /// <returns>-v</returns>
        public static Vector4D operator -(Vector4D v) => new Vector4D(-v.X, -v.Y, -v.Z, -v.W);

        /// <summary>
        /// Operator * overload ; multiply a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="s">Scalar value</param>
        /// <returns>v * s</returns>
        public static Vector4D operator *(Vector4D v, float s) => new Vector4D(v.X * s, v.Y * s, v.Z * s, v.W * s);

        /// <summary>
        /// Operator * overload ; multiply a scalar value by a vector
        /// </summary>
        /// <param name="s">Scalar value</param>
        /// <param name="v">Vector</param>
        /// <returns>s * v</returns>
        public static Vector4D operator *(float s, Vector4D v) => new Vector4D(v.X * s, v.Y * s, v.Z * s, v.W * s);

        /// <summary>
        /// Operator / overload ; divide a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="s">Scalar value</param>
        /// <returns>v / s</returns>
        public static Vector4D operator /(Vector4D v, float s) => new Vector4D(v.X / s, v.Y / s, v.Z / s, v.W / s);

        /// <summary>
        /// Operator == overload ; check vector equality
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 == v2</returns>
        public static bool operator ==(Vector4D v1, Vector4D v2) => v1.Equals(v2);

        /// <summary>
        /// Operator != overload ; check vector inequality
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 != v2</returns>
        public static bool operator !=(Vector4D v1, Vector4D v2) => !v1.Equals(v2);

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
        public static float Magnitude(Vector4D v) => new Mathematics.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z, v.W * v.W);

        /// <summary>
        /// Converts the vector into a unit vector
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>Normalized vector</returns>
        public static Vector4D Normalize(Vector4D v) => v / Magnitude(v);

        /// <summary>
        /// Converts the vector into a unit vector
        /// </summary>
        /// <returns>Normalized vector</returns>
        public Vector4D Normalize() => this / Magnitude(this);

        /// <summary>
        /// Restricts a value to be within a specified range
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>The clamped value</returns>
        public static Vector4D Clamp(Vector4D value, Vector4D min, Vector4D max)
        {
            Vector4D result;

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
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <returns>The distance between the two vectors</returns>
        public static float Distance(Vector4D v1, Vector4D v2) => Mathematics.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y) + (v1.Z - v2.Z) * (v1.Z - v2.Z) + (v1.W - v2.W) * (v1.W - v2.W));

        /// <summary>
        /// Calculates the dot product of two vectors
        /// </summary>
        /// <param name="v1">First source vector</param>
        /// <param name="v2">Second source vector</param>
        /// <returns>The dot product of the two vectors</returns>
        public static float DotProduct(Vector4D v1, Vector4D v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;

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
        /// <param name="v1">First source position vector</param>
        /// <param name="t1">First source tangent vector</param>
        /// <param name="v2">Second source position vector</param>
        /// <param name="t2">Second source tangent vector</param>
        /// <param name="amount">Weighting factor</param>
        /// <returns>The result of the Hermite spline interpolation</returns>
        public static Vector4D Hermite(Vector4D v1, Vector4D t1, Vector4D v2, Vector4D t2, float amount)
        {
            float squared = amount * amount;
            float cubed = amount * squared;
            float part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
            float part2 = (-2.0f * cubed) + (3.0f * squared);
            float part3 = (cubed - (2.0f * squared)) + amount;
            float part4 = cubed - squared;

            return new Vector4D(
                (((v1.X * part1) + (v2.X * part2)) + (t1.X * part3)) + (t2.X * part4),
                (((v1.Y * part1) + (v2.Y * part2)) + (t1.Y * part3)) + (t2.Y * part4),
                (((v1.Z * part1) + (v2.Z * part2)) + (t1.Z * part3)) + (t2.Z * part4),
                (((v1.W * part1) + (v2.W * part2)) + (t1.W * part3)) + (t2.W * part4));
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions
        /// </summary>
        /// <param name="v1">The first position in the interpolation</param>
        /// <param name="v2">The second position in the interpolation</param>
        /// <param name="v3">The third position in the interpolation</param>
        /// <param name="v4">The fourth position in the interpolation</param>
        /// <param name="amount">Weighting factor</param>
        /// <returns>A vector that is the result of the Catmull-Rom interpolation</returns>
        public static Vector4D CatmullRom(Vector4D v1, Vector4D v2, Vector4D v3, Vector4D v4, float amount)
        {
            float squared = amount * amount;
            float cubed = amount * squared;

            return new Vector4D(
                0.5f * ((((2.0f * v2.X) + ((-v1.X + v3.X) * amount)) +
                        (((((2.0f * v1.X) - (5.0f * v2.X)) + (4.0f * v3.X)) - v4.X) * squared)) +
                    ((((-v1.X + (3.0f * v2.X)) - (3.0f * v3.X)) + v4.X) * cubed)),
                0.5f * ((((2.0f * v2.Y) + ((-v1.Y + v3.Y) * amount)) +
                        (((((2.0f * v1.Y) - (5.0f * v2.Y)) + (4.0f * v3.Y)) - v4.Y) * squared)) +
                    ((((-v1.Y + (3.0f * v2.Y)) - (3.0f * v3.Y)) + v4.Y) * cubed)),
                0.5f * ((((2.0f * v2.Z) + ((-v1.Z + v3.Z) * amount)) +
                        (((((2.0f * v1.Z) - (5.0f * v2.Z)) + (4.0f * v3.Z)) - v4.Z) * squared)) +
                    ((((-v1.Z + (3.0f * v2.Z)) - (3.0f * v3.Z)) + v4.Z) * cubed)),
                0.5f * ((((2.0f * v2.Z) + ((-v1.Z + v3.Z) * amount)) +
                        (((((2.0f * v1.Z) - (5.0f * v2.Z)) + (4.0f * v3.Z)) - v4.Z) * squared)) +
                    ((((-v1.Z + (3.0f * v2.Z)) - (3.0f * v3.Z)) + v4.Z) * cubed)),
                0.5f * ((((2.0f * v2.W) + ((-v1.W + v3.W) * amount)) +
                        (((((2.0f * v1.W) - (5.0f * v2.W)) + (4.0f * v3.W)) - v4.W) * squared)) +
                    ((((-v1.W + (3.0f * v2.W)) - (3.0f * v3.W)) + v4.W) * cubed)));
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors
        /// </summary>
        /// <param name="v1">The first source vector</param>
        /// <param name="v2">The second source vector</param>
        /// <returns>A vector containing the largest components of the source vectors</returns>
        public static Vector4D Max(Vector4D v1, Vector4D v2)
        {
            return new Vector4D(
                (v1.X > v2.X) ? v1.X : v2.X,
                (v1.Y > v2.Y) ? v1.Y : v2.Y,
                (v1.Z > v2.Z) ? v1.Z : v2.Z,
                (v1.W > v2.W) ? v1.W : v2.W);
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="v1">The first source vector.</param>
        /// <param name="v2">The second source vector.</param>
        /// <returns>A vector containing the smallest components of the source vectors.</returns>
        public static Vector4D Min(Vector4D v1, Vector4D v2)
        {
            return new Vector4D(
                (v1.X < v2.X) ? v1.X : v2.X,
                (v1.Y < v2.Y) ? v1.Y : v2.Y,
                (v1.Z < v2.Z) ? v1.Z : v2.Z,
                (v1.W < v2.W) ? v1.W : v2.W);
        }

        /// <summary>
        /// Orthogonalizes a list of vectors.
        /// </summary>
        /// <param name="destination">The list of orthogonalized vectors.</param>
        /// <param name="source">The list of vectors to orthogonalize.</param>
        /// <remarks>
        /// <para>Orthogonalization is the process of making all vectors orthogonal to each other. This
        /// means that any given vector in the list will be orthogonal to any other given vector in the
        /// list.</para>
        /// <para>Because this method uses the modified Gram-Schmidt process, the resulting vectors
        /// tend to be numerically unstable. The numeric stability decreases according to the vectors
        /// position in the list so that the first vector is the most stable and the last vector is the
        /// least stable.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Orthogonalize(Vector4D[] destination, params Vector4D[] source)
        {
            //Uses the modified Gram-Schmidt process.
            //q1 = m1
            //q2 = m2 - ((q1 ⋅ m2) / (q1 ⋅ q1)) * q1
            //q3 = m3 - ((q1 ⋅ m3) / (q1 ⋅ q1)) * q1 - ((q2 ⋅ m3) / (q2 ⋅ q2)) * q2
            //q4 = m4 - ((q1 ⋅ m4) / (q1 ⋅ q1)) * q1 - ((q2 ⋅ m4) / (q2 ⋅ q2)) * q2 - ((q3 ⋅ m4) / (q3 ⋅ q3)) * q3
            //q5 = ...

            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i)
            {
                Vector4D newvector = source[i];

                for (int r = 0; r < i; ++r)
                {
                    newvector -= (Vector4D.DotProduct(destination[r], newvector) / Vector4D.DotProduct(destination[r], destination[r])) * destination[r];
                }

                destination[i] = newvector;
            }
        }

        /// <summary>
        /// Orthonormalizes a list of vectors.
        /// </summary>
        /// <param name="destination">The list of orthonormalized vectors.</param>
        /// <param name="source">The list of vectors to orthonormalize.</param>
        /// <remarks>
        /// <para>Orthonormalization is the process of making all vectors orthogonal to each
        /// other and making all vectors of unit length. This means that any given vector will
        /// be orthogonal to any other given vector in the list.</para>
        /// <para>Because this method uses the modified Gram-Schmidt process, the resulting vectors
        /// tend to be numerically unstable. The numeric stability decreases according to the vectors
        /// position in the list so that the first vector is the most stable and the last vector is the
        /// least stable.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Orthonormalize(Vector4D[] destination, params Vector4D[] source)
        {
            //Uses the modified Gram-Schmidt process.
            //Because we are making unit vectors, we can optimize the math for orthogonalization
            //and simplify the projection operation to remove the division.
            //q1 = m1 / |m1|
            //q2 = (m2 - (q1 ⋅ m2) * q1) / |m2 - (q1 ⋅ m2) * q1|
            //q3 = (m3 - (q1 ⋅ m3) * q1 - (q2 ⋅ m3) * q2) / |m3 - (q1 ⋅ m3) * q1 - (q2 ⋅ m3) * q2|
            //q4 = (m4 - (q1 ⋅ m4) * q1 - (q2 ⋅ m4) * q2 - (q3 ⋅ m4) * q3) / |m4 - (q1 ⋅ m4) * q1 - (q2 ⋅ m4) * q2 - (q3 ⋅ m4) * q3|
            //q5 = ...

            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i)
            {
                Vector4D newvector = source[i];

                for (int r = 0; r < i; ++r)
                {
                    newvector -= Vector4D.DotProduct(destination[r], newvector) * destination[r];
                }

                newvector.Normalize();
                destination[i] = newvector;
            }
        }

        /// <summary>
        /// Transforms a 4D vector by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SlimMath.Vector4D"/>.</param>
        public static void Transform(ref Vector4D vector, ref Quaternion rotation, out Vector4D result)
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

            result = new Vector4D(
                ((vector.X * num1) + (vector.Y * num2)) + (vector.Z * num3),
                ((vector.X * num4) + (vector.Y * num5)) + (vector.Z * num6),
                ((vector.X * num7) + (vector.Y * num8)) + (vector.Z * num9),
                vector.W);
        }

        /// <summary>
        /// Transforms a 4D vector by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <returns>The transformed <see cref="SlimMath.Vector4D"/>.</returns>
        public static Vector4D Transform(Vector4D vector, Quaternion rotation)
        {
            Vector4D result;
            Transform(ref vector, ref rotation, out result);
            return result;
        }

        /// <summary>
        /// Transforms an array of vectors by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.
        /// This array may be the same array as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Transform(Vector4D[] source, ref Quaternion rotation, Vector4D[] destination)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

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

            for (int i = 0; i < source.Length; ++i)
            {
                destination[i] = new Vector4D(
                    ((source[i].X * num1) + (source[i].Y * num2)) + (source[i].Z * num3),
                    ((source[i].X * num4) + (source[i].Y * num5)) + (source[i].Z * num6),
                    ((source[i].X * num7) + (source[i].Y * num8)) + (source[i].Z * num9),
                    source[i].W);
            }
        }

        /// <summary>
        /// Transforms a 4D vector by the given <see cref="SlimMath.Matrix"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix"/>.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SlimMath.Vector4D"/>.</param>
        public static void Transform(ref Vector4D vector, ref Matrix transform, out Vector4D result)
        {
            result = new Vector4D(
                (vector.X * transform.M11) + (vector.Y * transform.M21) + (vector.Z * transform.M31) + (vector.W * transform.M41),
                (vector.X * transform.M12) + (vector.Y * transform.M22) + (vector.Z * transform.M32) + (vector.W * transform.M42),
                (vector.X * transform.M13) + (vector.Y * transform.M23) + (vector.Z * transform.M33) + (vector.W * transform.M43),
                (vector.X * transform.M14) + (vector.Y * transform.M24) + (vector.Z * transform.M34) + (vector.W * transform.M44));
        }

        /// <summary>
        /// Transforms a 4D vector by the given <see cref="SlimMath.Matrix"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix"/>.</param>
        /// <returns>The transformed <see cref="SlimMath.Vector4D"/>.</returns>
        public static Vector4D Transform(Vector4D vector, Matrix transform)
        {
            Vector4D result;
            Transform(ref vector, ref transform, out result);
            return result;
        }

        /// <summary>
        /// Transforms an array of 4D vectors by the given <see cref="SlimMath.Matrix"/>.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix"/>.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.
        /// This array may be the same array as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Transform(Vector4D[] source, ref Matrix transform, Vector4D[] destination)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i)
            {
                Transform(ref source[i], ref transform, out destination[i]);
            }
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector4D operator +(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }

        /// <summary>
        /// Assert a vector (return it unchanged).
        /// </summary>
        /// <param name="value">The vector to assert (unchange).</param>
        /// <returns>The asserted (unchanged) vector.</returns>
        public static Vector4D operator +(Vector4D value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector4D operator -(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static Vector4D operator -(Vector4D value)
        {
            return new Vector4D(-value.X, -value.Y, -value.Z, -value.W);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D operator *(float scalar, Vector4D value)
        {
            return new Vector4D(value.X * scalar, value.Y * scalar, value.Z * scalar, value.W * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D operator *(Vector4D value, float scalar)
        {
            return new Vector4D(value.X * scalar, value.Y * scalar, value.Z * scalar, value.W * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D operator /(Vector4D value, float scalar)
        {
            return new Vector4D(value.X / scalar, value.Y / scalar, value.Z / scalar, value.W / scalar);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector4D left, Vector4D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector4D left, Vector4D right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Vector4D"/> to <see cref="SlimMath.Vector4D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector4D(Vector4D value)
        {
            return new Vector4D(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", X, Y, Z, W);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", X.ToString(format, CultureInfo.CurrentCulture),
                Y.ToString(format, CultureInfo.CurrentCulture), Z.ToString(format, CultureInfo.CurrentCulture), W.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", X, Y, Z, W);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                ToString(formatProvider);

            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", X.ToString(format, formatProvider),
                Y.ToString(format, formatProvider), Z.ToString(format, formatProvider), W.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Vector4D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Vector4D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Vector4D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector4D other)
        {
            return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z) && (this.W == other.W);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Vector4D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Vector4D"/> to compare with this instance.</param>
        /// <param name="epsilon">The amount of error allowed.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Vector4D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector4D other, float epsilon)
        {
            return ((float)System.Math.Abs(other.X - X) < epsilon &&
                (float)System.Math.Abs(other.Y - Y) < epsilon &&
                (float)System.Math.Abs(other.Z - Z) < epsilon &&
                (float)System.Math.Abs(other.W - W) < epsilon);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Vector4D)obj);
        }

#if SlimDX1xInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector4D"/> to <see cref="SlimDX.Vector4D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SlimDX.Vector4D(Vector4D value)
        {
            return new SlimDX.Vector4D(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimDX.Vector4D"/> to <see cref="SlimMath.Vector4D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector4D(SlimDX.Vector4D value)
        {
            return new Vector4D(value.X, value.Y, value.Z, value.W);
        }
#endif

#if WPFInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector4D"/> to <see cref="System.Windows.Media.Media3D.Point4D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator System.Windows.Media.Media3D.Point4D(Vector4D value)
        {
            return new System.Windows.Media.Media3D.Point4D(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Windows.Media.Media3D.Point4D"/> to <see cref="SlimMath.Vector4D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector4D(System.Windows.Media.Media3D.Point4D value)
        {
            return new Vector4D((float)value.X, (float)value.Y, (float)value.Z, (float)value.W);
        }
#endif

#if XnaInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector4D"/> to <see cref="Microsoft.Xna.Framework.Vector4D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Microsoft.Xna.Framework.Vector4D(Vector4D value)
        {
            return new Microsoft.Xna.Framework.Vector4D(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Microsoft.Xna.Framework.Vector4D"/> to <see cref="SlimMath.Vector4D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector4D(Microsoft.Xna.Framework.Vector4D value)
        {
            return new Vector4D(value.X, value.Y, value.Z, value.W);
        }
#endif
    }
}