using System;
using System.Runtime.InteropServices;

namespace SesanoEngine.Core
{
	/// <summary>
	/// Vector2D is an utility class for manipulating 2 dimensional
	/// vectors with float components
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    public struct Vector2D : IEquatable<Vector2D>
	{
		/// <summary>X (horizontal) component of the vector</summary>
		public float X { get; set; }

		/// <summary>Y (vertical) component of the vector</summary>
		public float Y { get; set; }

		/// <summary>
		/// Construct the vector from it's coordinates
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		/// <param name="z">Z coordinate</param>
		public Vector2D(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Shorthand for writing Vector2D(0, 1)
		/// </summary>
		/// <returns>Vector up</returns>
		public static Vector2D Up() => new Vector2D(0, 1);

		/// <summary>
		/// Shorthand for writing Vector2D(0, -1)
		/// </summary>
		/// <returns>Vector down</returns>
		public static Vector2D Down() => new Vector2D(0, -1);

		/// <summary>
		/// Shorthand for writing Vector2D(-1, 0)
		/// </summary>
		/// <returns>Vector left</returns>
		public static Vector2D Left() => new Vector2D(-1, 0);

		/// <summary>
		/// Shorthand for writing Vector2D(1, 0)
		/// </summary>
		/// <returns>Vector right</returns>
		public static Vector2D Right() => new Vector2D(1, 0);

		/// <summary>
		/// Shorthand for writing Vector2D(1, 1)
		/// </summary>
		/// <returns>Vector one</returns>
		public static Vector2D One() => new Vector2D(1, 1);

		/// <summary>
		/// Shorthand for writing Vector2D(0, 0)
		/// </summary>
		/// <returns>Vector zero</returns>
		public static Vector2D Zero() => new Vector2D(0, 0);

		/// <summary>
		/// Operator + overload ; add two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 + v2</returns>
		public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.X + v2.X, v1.Y + v2.Y);

		/// <summary>
		/// Operator - overload ; subtracts two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 - v2</returns>
		public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.X - v2.X, v1.Y - v2.Y);

		/// <summary>
		/// Operator - overload ; returns the opposite of a vector
		/// </summary>
		/// <param name="v">Vector to negate</param>
		/// <returns>-v</returns>
		public static Vector2D operator -(Vector2D v) => new Vector2D(-v.X, -v.Y);

		/// <summary>
		/// Operator * overload ; multiply a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="s">Scalar value</param>
		/// <returns>v * s</returns>
		public static Vector2D operator *(Vector2D v, float s) => new Vector2D(v.X * s, v.Y * s);

		/// <summary>
		/// Operator * overload ; multiply a scalar value by a vector
		/// </summary>
		/// <param name="s">Scalar value</param>
		/// <param name="v">Vector</param>
		/// <returns>s * v</returns>
		public static Vector2D operator *(float s, Vector2D v) => new Vector2D(v.X * s, v.Y * s);

		/// <summary>
		/// Operator / overload ; divide a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="s">Scalar value</param>
		/// <returns>v / s</returns>
		public static Vector2D operator /(Vector2D v, float s) => new Vector2D(v.X / s, v.Y / s);

		/// <summary>
		/// Operator == overload ; check vector equality
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 == v2</returns>
		public static bool operator ==(Vector2D v1, Vector2D v2) => v1.Equals(v2);

		/// <summary>
		/// Operator != overload ; check vector inequality
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 != v2</returns>
		public static bool operator !=(Vector2D v1, Vector2D v2) => !v1.Equals(v2);

		/// <summary>
		/// Compare vector and object and checks if they are equal
		/// </summary>
		/// <param name="obj">Object to check</param>
		/// <returns>Object and vector are equal</returns>
		public override bool Equals(object obj) => (obj is Vector2D) && Equals((Vector2D)obj);

		/// <summary>
		/// Compare two vectors and checks if they are equal
		/// </summary>
		/// <param name="other">Vector to check</param>
		/// <returns>Vectors are equal</returns>
		public bool Equals(Vector2D other) => (this.X == other.X) && (this.Y == other.Y);

        /// <summary>
        /// Magnitude
        /// </summary>
        /// <returns>The length of the vector</returns>
        public static float Magnitude(Vector2D v) => Math.Sqrt(v.X * v.X + v.Y * v.Y);

		/// <summary>
		/// Makes this vector have a magnitude of 1
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns>Normalized vector</returns>
		public static Vector2D Normalize(Vector2D v) => v / this.Magnitude(v);

		/// <summary>
		/// Makes this vector have a magnitude of 1
		/// </summary>
		/// <returns>Normalized vector</returns>
		public static Vector2D Normalize() => this / this.Magnitude(v);

		/// <summary>
		/// Dot Product
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns>Dot Product of two vectors</returns>
		public static float DotProduct(Vector2D v) => this.X * v.X + this.Y * v.Y;

		/// <summary>
		/// Provide a string describing the object
		/// </summary>
		/// <returns>String description of the object</returns>
		public override string ToString() => $"[Vector2D] X({ this.X }) Y({ this.Y })";
	}
}