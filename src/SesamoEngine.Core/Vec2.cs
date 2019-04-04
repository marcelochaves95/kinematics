using System;
using System.Runtime.InteropServices;

namespace SesanoEngine.Core
{
	/// <summary>
	/// Vec2 is an utility class for manipulating 2 dimensional
	/// vectors with float components
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    public struct Vec2 : IEquatable<Vec2>
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
		public Vec2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Shorthand for writing Vec2(0, 1)
		/// </summary>
		/// <returns>Vector up</returns>
		public static Vec2 Up() => new Vec2(0, 1);

		/// <summary>
		/// Shorthand for writing Vec2(0, -1)
		/// </summary>
		/// <returns>Vector down</returns>
		public static Vec2 Down() => new Vec2(0, -1);

		/// <summary>
		/// Shorthand for writing Vec2(-1, 0)
		/// </summary>
		/// <returns>Vector left</returns>
		public static Vec2 Left() => new Vec2(-1, 0);

		/// <summary>
		/// Shorthand for writing Vec2(1, 0)
		/// </summary>
		/// <returns>Vector right</returns>
		public static Vec2 Right() => new Vec2(1, 0);

		/// <summary>
		/// Shorthand for writing Vec2(1, 1)
		/// </summary>
		/// <returns>Vector one</returns>
		public static Vec2 One() => new Vec2(1, 1);

		/// <summary>
		/// Shorthand for writing Vec2(0, 0)
		/// </summary>
		/// <returns>Vector zero</returns>
		public static Vec2 Zero() => new Vec2(0, 0);

		/// <summary>
		/// Operator + overload ; add two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 + v2</returns>
		public static Vec2 operator +(Vec2 v1, Vec2 v2) => new Vec2(v1.X + v2.X, v1.Y + v2.Y);

		/// <summary>
		/// Operator - overload ; subtracts two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 - v2</returns>
		public static Vec2 operator -(Vec2 v1, Vec2 v2) => new Vec2(v1.X - v2.X, v1.Y - v2.Y);

		/// <summary>
		/// Operator - overload ; returns the opposite of a vector
		/// </summary>
		/// <param name="v">Vector to negate</param>
		/// <returns>-v</returns>
		public static Vec2 operator -(Vec2 v) => new Vec2(-v.X, -v.Y);

		/// <summary>
		/// Operator * overload ; multiply a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="s">Scalar value</param>
		/// <returns>v * s</returns>
		public static Vec2 operator *(Vec2 v, float s) => new Vec2(v.X * s, v.Y * s);

		/// <summary>
		/// Operator * overload ; multiply a scalar value by a vector
		/// </summary>
		/// <param name="s">Scalar value</param>
		/// <param name="v">Vector</param>
		/// <returns>s * v</returns>
		public static Vec2 operator *(float s, Vec2 v) => new Vec2(v.X * s, v.Y * s);

		/// <summary>
		/// Operator / overload ; divide a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="s">Scalar value</param>
		/// <returns>v / s</returns>
		public static Vec2 operator /(Vec2 v, float s) => new Vec2(v.X / s, v.Y / s);

		/// <summary>
		/// Operator == overload ; check vector equality
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 == v2</returns>
		public static bool operator ==(Vec2 v1, Vec2 v2) => v1.Equals(v2);

		/// <summary>
		/// Operator != overload ; check vector inequality
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 != v2</returns>
		public static bool operator !=(Vec2 v1, Vec2 v2) => !v1.Equals(v2);

		/// <summary>
		/// Compare vector and object and checks if they are equal
		/// </summary>
		/// <param name="obj">Object to check</param>
		/// <returns>Object and vector are equal</returns>
		public override bool Equals(object obj) => (obj is Vec2) && Equals((Vec2)obj);

		/// <summary>
		/// Compare two vectors and checks if they are equal
		/// </summary>
		/// <param name="other">Vector to check</param>
		/// <returns>Vectors are equal</returns>
		public bool Equals(Vec2 other) => (this.X == other.X) && (this.Y == other.Y);

		/// <summary>
		/// 
		/// </summary>
		/// <returns>The length of the vector</returns>
		public static float Magnitude(Vec2 v) { return Mathf.Sqrt(v.X * v.X + v.Y * v.Y); }

		/// <summary>
		/// Makes this vector have a magnitude of 1
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns>Normalized vector</returns>
		public static Vec2 Normalize(Vec2 v)
        {
            float magnitude = this.Magnitude(v);
            if (magnitude > 0)
                return v / magnitude;
            else
                return Vec2.Zero();
        }

		/// <summary>
		/// Makes this vector have a magnitude of 1
		/// </summary>
		/// <returns>Normalized vector</returns>
		public static Vec2 Normalize()
		{
			float magnitude = this.Magnitude(this);
            if (magnitude > 0)
                this = this / magnitude;
            else
                this = Vec2.Zero();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns>Dot Product of two vectors</returns>
		public static float DotProduct(Vec2 v) => this.X * v.X + this.Y * v.Y;

		/// <summary>
		/// Provide a string describing the object
		/// </summary>
		/// <returns>String description of the object</returns>
		public override string ToString() => $"[Vec2] X({ this.X }) Y({ this.Y })";
	}
}