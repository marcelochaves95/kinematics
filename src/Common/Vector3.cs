using System;
using System.Runtime.InteropServices;

namespace Kinematics.Common
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
        private float _x;
        public float X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>Y (vertical) component of the vector</summary>
        private float _y;
        public float Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>Z (depth) component of the vector</summary>
        private float _z;
        public float Z
        {
            get => _z;
            set => _z = value;
        }

        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 1)
        /// </summary>
        /// <returns>Vector forward</returns>
        public static readonly Vector3 Forward = new Vector3(0f, 0f, 1f);

        /// <summary>
        /// Shorthand for writing Vector3(0, 0, -1)
        /// </summary>
        /// <returns>Vector back</returns>
        public static readonly Vector3 Back = new Vector3(0f, 0f, -1f);

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

        #region Contrutors
        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="v">The v to assign to the X, Y, and Z components of the vector. This must be an array with three elements</param>
        public Vector3(Vector3 v)
        {
            _x = v._x;
            _y = v._y;
            _z = v._z;
        }

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="v">The v to assign to the X, Y, and Z components of the vector. This must be an array with three elements</param>
        public Vector3(float v)
        {
            _x = v;
            _y = v;
            _z = v;
        }

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="v">A vector containing the values with which to initialize the X, Y, and Z components</param>
        /// <param name="z">Initial value for the Z component of the vector</param>
        public Vector3(Vector2 v, float z)
        {
            _x = v.X;
            _y = v.Y;
            _z = z;
        }

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="values">The values to assign to the X, Y, and Z components of the vector. This must be an array with three elements</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than three elements</exception>
        public Vector3(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Length != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(values), "There must be three and only three input values for Vector3.");
            }

            _x = values[0];
            _y = values[1];
            _z = values[2];
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
            return new Vector3(v1._x + v2._x, v1.Y + v2._y, v1._z + v2._z);
        }

        /// <summary>
        /// Operator - overload ; subtracts two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 - v2</returns>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x - v2._x, v1._y - v2._y, v1._z - v2._z);
        }

        /// <summary>
        /// Operator - overload ; returns the opposite of a vector
        /// </summary>
        /// <param name="v">Vector to negate</param>
        /// <returns>-v</returns>
        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v._x, -v._y, -v._z);
        }

        /// <summary>
        /// Scales a vector by the given value
        /// </summary>
        /// <param name="v1">The vector to scale</param>
        /// <param name="v2">The amount by which to scale the vector</param>
        /// <returns>The scaled vector</returns>
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x * v2._x, v1._y * v2._y, v1._z * v2._z);
        }

        /// <summary>
        /// Operator * overload ; multiply a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="s">Scalar value</param>
        /// <returns>v * s</returns>
        public static Vector3 operator *(Vector3 v, float s)
        {
            return new Vector3(v._x * s, v._y * s, v._z * s);
        }

        /// <summary>
        /// Operator * overload ; multiply a scalar value by a vector
        /// </summary>
        /// <param name="s">Scalar value</param>
        /// <param name="v">Vector</param>
        /// <returns>s * v</returns>
        public static Vector3 operator *(float s, Vector3 v)
        {
            return new Vector3(v._x * s, v._y * s, v._z * s);
        }

        /// <summary>
        /// Scales a vector by the given value
        /// </summary>
        /// <param name="v1">The vector to scale</param>
        /// <param name="v2">The amount by which to scale the vector</param>
        /// <returns>The scaled vector</returns>
        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x / v2._x, v1._y / v2._y, v1._z / v2._z);
        }

        /// <summary>
        /// Operator / overload ; divide a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="s">Scalar value</param>
        /// <returns>v / s</returns>
        public static Vector3 operator /(Vector3 v, float s)
        {
            return new Vector3(v._x / s, v._y / s, v._z / s);
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
        #endregion

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
            return _x.Equals(other._x) && _y.Equals(other._y) && _z.Equals(other._z);
        }

        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        /// <returns>The length of the vector</returns>
        public static float Magnitude(Vector3 v)
        {
            return Mathf.Sqrt(Mathf.Pow(v._x, 2) + Mathf.Pow(v._y, 2) + Mathf.Pow(v._z, 2));
        }

        /// <summary>
        /// Converts the vector into a unit vector
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>Normalized vector</returns>
        public static Vector3 Normalize(Vector3 v)
        {
            return v / Magnitude(v);
        }

        /// <summary>
        /// Calculates the dot product of two vectors
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns>Dot Product of two vectors</returns>
        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1._x * v2._x + v1._y * v2._y + v1._z * v2._z;
        }

        /// <summary>
        /// Calculates the cross product of two vectors
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns>Cross Product of two vectors</returns>
        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._y * v2._z - v1._z * v2._y, v1._z * v2._x - v1._x * v2._z, v1._x * v2._y - v1._y * v2._x);
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
            return new Vector3((v1._x + (a1 * (v2._x - v1._x))) + (a2 * (v3._x - v1._x)),
                (v1._y + (a1 * (v2._y - v1._y))) + (a2 * (v3._y - v1._y)),
                (v1._z + (a1 * (v2._z - v1._z))) + (a2 * (v3._z - v1._z)));
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
            float x = v._x;
            x = x > max._x ? max._x : x;
            x = x < min._x ? min._x : x;

            float y = v._y;
            y = y > max._y ? max._y : y;
            y = y < min._y ? min._y : y;

            float z = v._z;
            z = z > max._z ? max._z : z;
            z = z < min._z ? min._z : z;

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
            float x = v1._x - v2._x;
            float y = v1._y - v2._y;
            float z = v1._z - v2._z;

            return Mathf.Sqrt(x * x + y * y + z * z);
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
            return new Vector3(s._x + (e._x - s._x) * a,
                            s._y + (e._y - s._y) * a,
                            s._z + (e._z - s._z) * a
            );
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

            return new Vector3(s._x + (e._x - s._x) * (newAmount * newAmount) * (3f - 2f * newAmount),
                            s._y + (e._y - s._y) * (newAmount * newAmount) * (3f - 2f * newAmount),
                            s._z + (e._z - s._z) * (newAmount * newAmount) * (3f - 2f * newAmount)
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

            return new Vector3(v1._x * part1 + v2._x * part2 + t1._x * part3 + t2._x * part4,
                            v1._y * part1 + v2._y * part2 + t1._y * part3 + t2._y * part4,
                            v1._z * part1 + v2._z * part2 + t1._z * part3 + t2._z * part4
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
                0.5f * ((((2f * v2._x) + ((-v1._x + v3._x) * a)) +
                        (((((2f * v1._x) - (5f * v2._x)) + (4f * v3._x)) - v4._x) * squared)) +
                    ((((-v1._x + (3f * v2._x)) - (3f * v3._x)) + v4._x) * cubed)),
                0.5f * ((((2f * v2._y) + ((-v1._y + v3._y) * a)) +
                        (((((2f * v1._y) - (5f * v2._y)) + (4f * v3._y)) - v4._y) * squared)) +
                    ((((-v1._y + (3f * v2._y)) - (3f * v3._y)) + v4._y) * cubed)),
                0.5f * ((((2f * v2._z) + ((-v1._z + v3._z) * a)) +
                        (((((2f * v1._z) - (5f * v2._z)) + (4f * v3._z)) - v4._z) * squared)) +
                    ((((-v1._z + (3f * v2._z)) - (3f * v3._z)) + v4._z) * cubed))
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
            return new Vector3(v1._x > v2._x ? v1._x : v2._x,
                            v1._y > v2._y ? v1._y : v2._y,
                            v1._z > v2._z ? v1._z : v2._z
            );
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors
        /// </summary>
        /// <param name="v1">The first source vector</param>
        /// <param name="v2">The second source vector</param>
        /// <returns>A vector containing the smallest components of the source vectors</returns>
        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x < v2._x ? v1._x : v2._x,
                            v1._y < v2._y ? v1._y : v2._y,
                            v1._z < v2._z ? v1._z : v2._z
            );
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

            return new Vector3(v._x - 2f * dot * n._x,
                            v._y - 2f * dot * n._y,
                            v._z - 2f * dot * n._z
            );
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

        /// <summary>
        /// Used to allow Vector3s to be used as keys in hash tables
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ (_y.GetHashCode() << 2) ^ (_z.GetHashCode() >> 2);
        }

        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString()
        {
            return $"[Vector3] X({_x}) Y({_y}) Z({_z})";
        }
    }
}
