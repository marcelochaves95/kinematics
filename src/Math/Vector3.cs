using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kinematics.Math
{
    /// <summary>
    /// Vector3 is an utility class for manipulating 3 dimensional
    /// vectors with float components
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 : IEquatable<Vector3>
    {
        #region Properties

        /// <summary>X (horizontal) component of the vector</summary>
        public float X;

        /// <summary>Y (vertical) component of the vector</summary>
        public float Y;

        /// <summary>Z (depth) component of the vector</summary>
        public float Z;

        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 1)
        /// </summary>
        /// <returns>Vector forward</returns>
        public static readonly Vector3 Forward = new Vector3(0f, 0f, 1f);

        /// <summary>
        /// Shorthand for writing Vector3(0, 0, -1)
        /// </summary>
        /// <returns>Vector back</returns>
        public static readonly Vector3 Backward = new Vector3(0f, 0f, -1f);

        /// <summary>
        /// Shorthand for writing Vector3(0, 1, 0)
        /// </summary>
        /// <returns>Vector up</returns>
        public static readonly Vector3 Up = new Vector3(0f, 1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector3(0, -1, 0)
        /// </summary>
        /// <returns>Vector down</returns>
        public static readonly Vector3 Down = new Vector3(0f, -1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector3(-1, 0, 0)
        /// </summary>
        /// <returns>Vector left</returns>
        public static readonly Vector3 Left = new Vector3(-1f, 0f, 0f);

        /// <summary>
        /// Shorthand for writing Vector3(1, 0, 0)
        /// </summary>
        /// <returns>Vector right</returns>
        public static readonly Vector3 Right = new Vector3(1f, 0f, 0f);

        /// <summary>
        /// Shorthand for writing Vector3(1, 1, 1)
        /// </summary>
        /// <returns>Vector one</returns>
        public static readonly Vector3 One = new Vector3(1f, 1f, 1f);

        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 0)
        /// </summary>
        /// <returns>Vector zero</returns>
        public static readonly Vector3 Zero = new Vector3(0f, 0f, 0f);

        #endregion

        #region Constructors

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Construct the vector from its coordinates
        /// </summary>
        /// <param name="v">Vector3</param>
        public Vector3(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Construct the vector from its coordinates
        /// </summary>
        /// <param name="v">Vector2</param>
        public Vector3(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
            Z = 0.0f;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Operator + overload ; add two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 + v2</returns>
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Operator - overload ; subtracts two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 - v2</returns>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Operator - overload ; returns the opposite of a vector
        /// </summary>
        /// <param name="v">Vector to negate</param>
        /// <returns>-v</returns>
        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        /// <summary>
        /// Scales a vector by the given value
        /// </summary>
        /// <param name="v1">The vector to scale</param>
        /// <param name="v2">The amount by which to scale the vector</param>
        /// <returns>The scaled vector</returns>
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        /// <summary>
        /// Operator * overload ; multiply a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="s">Scalar value</param>
        /// <returns>v * s</returns>
        public static Vector3 operator *(Vector3 v, float s)
        {
            return new Vector3(v.X * s, v.Y * s, v.Z * s);
        }

        /// <summary>
        /// Operator * overload ; multiply a scalar value by a vector
        /// </summary>
        /// <param name="s">Scalar value</param>
        /// <param name="v">Vector</param>
        /// <returns>s * v</returns>
        public static Vector3 operator *(float s, Vector3 v)
        {
            return new Vector3(v.X * s, v.Y * s, v.Z * s);
        }

        /// <summary>
        /// Scales a vector by the given value
        /// </summary>
        /// <param name="v1">The vector to scale</param>
        /// <param name="v2">The amount by which to scale the vector</param>
        /// <returns>The scaled vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        /// <summary>
        /// Operator / overload ; divide a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="s">Scalar value</param>
        /// <returns>v / s</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 v, float s)
        {
            return new Vector3(v.X / s, v.Y / s, v.Z / s);
        }

        /// <summary>
        /// Operator == overload ; check vector equality
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 == v2</returns>
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Operator != overload ; check vector inequality
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 != v2</returns>
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !v1.Equals(v2);
        }

        /// <summary>
        /// Performs an explicit conversion from Vector3 to Vector2
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>The result of the conversion</returns>
        public static explicit operator Vector2(Vector3 v)
        {
            return new Vector2(v);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Compare vector and object and checks if they are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>Object and vector are equal</returns>
        public override bool Equals(object obj)
        {
            return obj is Vector3 v && Equals(v);
        }

        /// <summary>
        /// Compare two vectors and checks if they are equal
        /// </summary>
        /// <param name="other">Vector to check</param>
        /// <returns>Vectors are equal</returns>
        public bool Equals(Vector3 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        /// <summary>
        /// Used to allow Vector3s to be used as keys in hash tables
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2);
        }

        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString()
        {
            return $"[Vector3] X({X}) Y({Y}) Z({Z})";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        /// <returns>The length of the vector</returns>
        public static float Length(Vector3 v)
        {
            return Mathf.Sqrt(Mathf.Pow(v.X, 2) + Mathf.Pow(v.Y, 2) + Mathf.Pow(v.Z, 2));
        }

        /// <summary>
        /// Returns the squared length of this <see cref="Vector3"/>
        /// </summary>
        /// <returns>The squared length of this <see cref="Vector3"/></returns>
        public float LengthSquared()
        {
            return Mathf.Pow(X, 2) + Mathf.Pow(Y, 2) + Mathf.Pow(Z, 2);
        }

        /// <summary>
        /// Turns this <see cref="Vector3"/> to a unit vector with the same direction
        /// </summary>
        public void Normalize()
        {
            float val = 1.0f / Length(this);
            X *= val;
            Y *= val;
            Z *= val;
        }

        /// <summary>
        /// Converts the vector into a unit vector
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>Normalized vector</returns>
        public static Vector3 Normalize(Vector3 v)
        {
            float val = 1.0f / Length(v);
            v.X *= val;
            v.Y *= val;
            v.Z *= val;
            return v;
        }

        /// <summary>
        /// Calculates the dot product of two vectors
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns>Dot Product of two vectors</returns>
        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        /// <summary>
        /// Calculates the cross product of two vectors
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns>Cross Product of two vectors</returns>
        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        /// <summary>
        /// Returns a <see cref="Vector3"/> containing the 3D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 3D triangle
        /// </summary>
        /// <param name="v1">A <see cref="Vector3"/> containing the 3D Cartesian coordinates of vertex 1 of the triangle</param>
        /// <param name="v2">A <see cref="Vector3"/> containing the 3D Cartesian coordinates of vertex 2 of the triangle</param>
        /// <param name="v3">A <see cref="Vector3"/> containing the 3D Cartesian coordinates of vertex 3 of the triangle</param>
        /// <param name="a1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="v2"/>)</param>
        /// <param name="a2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="v3"/>)</param>
        /// <returns>A new <see cref="Vector3"/> containing the 3D Cartesian coordinates of the specified point</returns>
        public static Vector3 Barycentric(Vector3 v1, Vector3 v2, Vector3 v3, float a1, float a2)
        {
            return new Vector3((v1.X + (a1 * (v2.X - v1.X))) + (a2 * (v3.X - v1.X)),
                (v1.Y + (a1 * (v2.Y - v1.Y))) + (a2 * (v3.Y - v1.Y)),
                (v1.Z + (a1 * (v2.Z - v1.Z))) + (a2 * (v3.Z - v1.Z)));
        }

        /// <summary>
        /// Restricts a value to be within a specified range
        /// </summary>
        /// <param name="v">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>The clamped value</returns>
        public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
        {
            float x = v.X;
            x = x > max.X ? max.X : x;
            x = x < min.X ? min.X : x;

            float y = v.Y;
            y = y > max.Y ? max.Y : y;
            y = y < min.Y ? min.Y : y;

            float z = v.Z;
            z = z > max.Z ? max.Z : z;
            z = z < min.Z ? min.Z : z;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Calculates the distance between two vectors
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <returns>The distance between the two vectors</returns>
        public static float Distance(Vector3 v1, Vector3 v2)
        {
            float x = v1.X - v2.X;
            float y = v1.Y - v2.Y;
            float z = v1.Z - v2.Z;
            Vector3 v3 = new Vector3(x, y, z);
            return Length(v3);
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors
        /// </summary>
        /// <param name="s">Start vector</param>
        /// <param name="e">End vector</param>
        /// <param name="a">Value between 0 and 1 indicating the weight of <paramref name="e"/></param>
        /// <returns>The linear interpolation of the two vectors</returns>
        public static Vector3 Lerp(Vector3 s, Vector3 e, float a)
        {
            return new Vector3(s.X + (e.X - s.X) * a, s.Y + (e.Y - s.Y) * a, s.Z + (e.Z - s.Z) * a);
        }

        /// <summary>
        /// Performs a cubic interpolation between two vectors
        /// </summary>
        /// <param name="s">Start vector</param>
        /// <param name="e">End vector</param>
        /// <param name="a">Value between 0 and 1 indicating the weight of <paramref name="e"/></param>
        /// <returns>The cubic interpolation of the two vectors</returns>
        public static Vector3 SmoothStep(Vector3 s, Vector3 e, float a)
        {
            float newAmount = a > 1f ? 1f : a < 0f ? 0f : a;

            return new Vector3(s.X + (e.X - s.X) * (newAmount * newAmount) * (3f - 2f * newAmount),
                            s.Y + (e.Y - s.Y) * (newAmount * newAmount) * (3f - 2f * newAmount),
                            s.Z + (e.Z - s.Z) * (newAmount * newAmount) * (3f - 2f * newAmount)
            );
        }

        /// <summary>
        /// Performs a Hermite spline interpolation
        /// </summary>
        /// <param name="v1">First source position vector</param>
        /// <param name="t1">First source tangent vector</param>
        /// <param name="v2">Second source position vector</param>
        /// <param name="t2">Second source tangent vector</param>
        /// <param name="a">Weighting factor</param>
        /// <returns>The result of the Hermite spline interpolation</returns>
        public static Vector3 Hermite(Vector3 v1, Vector3 t1, Vector3 v2, Vector3 t2, float a)
        {
            float squared = a * a;
            float cubed = a * squared;
            float part1 = 2f * cubed - 3f * squared + 1f;
            float part2 = -2f * cubed + 3f * squared;
            float part3 = cubed - 2f * squared + a;
            float part4 = cubed - squared;

            return new Vector3(v1.X * part1 + v2.X * part2 + t1.X * part3 + t2.X * part4,
                            v1.Y * part1 + v2.Y * part2 + t1.Y * part3 + t2.Y * part4,
                            v1.Z * part1 + v2.Z * part2 + t1.Z * part3 + t2.Z * part4
            );
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions
        /// </summary>
        /// <param name="v1">The first position in the interpolation</param>
        /// <param name="v2">The second position in the interpolation</param>
        /// <param name="v3">The third position in the interpolation</param>
        /// <param name="v4">The fourth position in the interpolation</param>
        /// <param name="a">Weighting factor</param>
        /// <returns>A vector that is the result of the Catmull-Rom interpolation</returns>
        public static Vector3 CatmullRom(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float a)
        {
            float squared = a * a;
            float cubed = a * squared;

            return new Vector3(
                0.5f * ((((2f * v2.X) + ((-v1.X + v3.X) * a)) +
                        (((((2f * v1.X) - (5f * v2.X)) + (4f * v3.X)) - v4.X) * squared)) +
                    ((((-v1.X + (3f * v2.X)) - (3f * v3.X)) + v4.X) * cubed)),
                0.5f * ((((2f * v2.Y) + ((-v1.Y + v3.Y) * a)) +
                        (((((2f * v1.Y) - (5f * v2.Y)) + (4f * v3.Y)) - v4.Y) * squared)) +
                    ((((-v1.Y + (3f * v2.Y)) - (3f * v3.Y)) + v4.Y) * cubed)),
                0.5f * ((((2f * v2.Z) + ((-v1.Z + v3.Z) * a)) +
                        (((((2f * v1.Z) - (5f * v2.Z)) + (4f * v3.Z)) - v4.Z) * squared)) +
                    ((((-v1.Z + (3f * v2.Z)) - (3f * v3.Z)) + v4.Z) * cubed))
            );
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors
        /// </summary>
        /// <param name="v1">The first source vector</param>
        /// <param name="v2">The second source vector</param>
        /// <returns>A vector containing the largest components of the source vectors</returns>
        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X > v2.X ? v1.X : v2.X, v1.Y > v2.Y ? v1.Y : v2.Y, v1.Z > v2.Z ? v1.Z : v2.Z);
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors
        /// </summary>
        /// <param name="v1">The first source vector</param>
        /// <param name="v2">The second source vector</param>
        /// <returns>A vector containing the smallest components of the source vectors</returns>
        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X < v2.X ? v1.X : v2.X, v1.Y < v2.Y ? v1.Y : v2.Y, v1.Z < v2.Z ? v1.Z : v2.Z);
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal
        /// </summary>
        /// <param name="v">The source vector</param>
        /// <param name="n">Normal of the surface</param>
        /// <returns>The reflected vector</returns>
        public static Vector3 Reflect(Vector3 v, Vector3 n)
        {
            float dot = Dot(v, n);
            return new Vector3(v.X - 2f * dot * n.X, v.Y - 2f * dot * n.Y, v.Z - 2f * dot * n.Z);
        }

        /// <summary>
        /// Returns the fraction of a vector off a surface that has the specified normal and index
        /// </summary>
        /// <param name="v">The source vector</param>
        /// <param name="n">Normal of the surface</param>
        /// <param name="i">Index of refraction</param>
        /// <returns>The refracted vector</returns>
        public static Vector3 Refract(Vector3 v, Vector3 n, float i)
        {
            float cos1 = Dot(v, n);

            float radians = 1f - i * i * (1f - cos1 * cos1);

            if (radians < 0f)
            {
                return Zero;
            }

            float cos2 = Mathf.Sqrt(radians);
            return i * v + (cos2 - i * cos1) * n;
        }

        #endregion
    }
}
