using System;
using System.Runtime.InteropServices;
using SerializeField = UnityEngine.SerializeField;

namespace Kinematics.Mathematics
{
    /// <summary>
    /// Vector2f is an utility class for manipulating 2 dimensional
    /// vectors with float components
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2 : IEquatable<Vector2>
    {
        #region Properties
        /// <summary>X (horizontal) component of the vector</summary>
        [SerializeField]
        private float _x;
        public float X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>Y (vertical) component of the vector</summary>
        [SerializeField]
        private float _y;
        public float Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Shorthand for writing Vector2(0, 1)
        /// </summary>
        /// <returns>Vector up</returns>
        public static readonly Vector2 Up = new Vector2(1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector2(0, -1)
        /// </summary>
        /// <returns>Vector down</returns>
        public static readonly Vector2 Down = new Vector2(0f, -1f);

        /// <summary>
        /// Shorthand for writing Vector2(-1, 0)
        /// </summary>
        /// <returns>Vector left</returns>
        public static readonly Vector2 Left = new Vector2(-1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector2(1, 0)
        /// </summary>
        /// <returns>Vector right</returns>
        public static readonly Vector2 Right = new Vector2(1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector2(1, 1)
        /// </summary>
        /// <returns>Vector one</returns>
        public static readonly Vector2 One = new Vector2(1f, 1f);

        /// <summary>
        /// Shorthand for writing Vector2(0, 0)
        /// </summary>
        /// <returns>Vector zero</returns>
        public static readonly Vector2 Zero = new Vector2(0f, 0f);
        #endregion

        #region Construtors
        /// <summary>
        /// Construct the vector from its coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Construct the vector from its coordinates
        /// </summary>
        /// <param name="v">Vector2</param>
        public Vector2(Vector2 v)
        {
            _x = v._x;
            _y = v._y;
        }
        
        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="values">The values to assign to the X and Y components of the vector. This must be an array with three elements</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than three elements</exception>
        public Vector2(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Length != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(values), "There must be three and only three input values for Vector2.");
            }

            _x = values[0];
            _y = values[1];
        }
        #endregion

        #region Overloads
        /// <summary>
        /// Operator - overload ; returns the opposite of a vector
        /// </summary>
        /// <param name="v">Vector to negate</param>
        /// <returns>-v</returns>
        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v._x, -v._y);
        }

        /// <summary>
        /// Operator - overload ; subtracts two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 - v2</returns>
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1._x - v2._x, v1._y - v2._y);
        }

        /// <summary>
        /// Operator + overload ; add two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 + v2</returns>
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1._x + v2._x, v1._y + v2._y);
        }

        /// <summary>
        /// Operator * overload ; multiply a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="x">Scalar value</param>
        /// <returns>v * x</returns>
        public static Vector2 operator *(Vector2 v, float x)
        {
            return new Vector2(v._x * x, v._y * x);
        }

        /// <summary>
        /// Operator * overload ; multiply a scalar value by a vector
        /// </summary>
        /// <param name="x">Scalar value</param>
        /// <param name="v">Vector</param>
        /// <returns>x * v</returns>
        public static Vector2 operator *(float x, Vector2 v)
        {
            return new Vector2(v._x * x, v._y * x);
        }

        /// <summary>
        /// Operator / overload ; divide a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="x">Scalar value</param>
        /// <returns>v / x</returns>
        public static Vector2 operator /(Vector2 v, float x)
        {
            return new Vector2(v._x / x, v._y / x);
        }

        /// <summary>
        /// Performs an explicit conversion from Vector3 to Vector4D
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The result of the conversion./returns>
        public static explicit operator Vector3(Vector2 value)
        {
            return new Vector3(value, 0f);
        }

        /// <summary>
        /// Operator == overload ; check vector equality
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 == v2</returns>
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Operator != overload ; check vector inequality
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 != v2</returns>
        public static bool operator !=(Vector2 v1, Vector2 v2)
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
            return obj is Vector2 vector && Equals(vector);
        }

        /// <summary>
        /// Compare two vectors and checks if they are equal
        /// </summary>
        /// <param name="other">Vector to check</param>
        /// <returns>Vectors are equal</returns>
        public bool Equals(Vector2 other)
        {
            return _x.Equals(other._x) && _y.Equals(other._y);
        }

        /// <summary>
        /// Provide a integer describing the object
        /// </summary>
        /// <returns>Integer description of the object</returns>
        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode();
        }

        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString()
        {
            return $"[Vector2] X({_x}) Y({_y})";
        }

        #region Kinematics/Unity
        public static implicit operator Vector2(UnityEngine.Vector2 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static implicit operator UnityEngine.Vector2(Vector2 v)
        {
            return new UnityEngine.Vector2(v._x, v._y);
        }
        #endregion
    }
}