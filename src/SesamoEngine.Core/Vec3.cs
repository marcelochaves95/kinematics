using System;
using System.Runtime.InteropServices;

namespace SesanoEngine.Core
{
	/// <summary>
	/// Vec3 is an utility class for manipulating 3 dimensional
	/// vectors with float components
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vec3 : IEquatable<Vec3>
	{
		/// <summary>X (horizontal) component of the vector</summary>
		public float X { get; set; }

		/// <summary>Y (vertical) component of the vector</summary>
		public float Y { get; set; }

		/// <summary>Z (depth) component of the vector</summary>
		public float Z { get; set; }

		/// <summary>
		/// Construct the vector from it's coordinates
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		/// <param name="z">Z coordinate</param>
		public Vec3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		/// <summary>
		/// Shorthand for writing Vec3(0, 0, 1)
		/// </summary>
		/// <returns>Vector forward</returns>
		public static Vec3 Forward() => new Vec3(0, 0, 1);

		/// <summary>
		/// Shorthand for writing Vec3(0, 0, -1)
		/// </summary>
		/// <returns>Vector back</returns>
		public static Vec3 Back() => new Vec3(0, 0, -1);
		
		/// <summary>
		/// Shorthand for writing Vec3(0, 1, 0)
		/// </summary>
		/// <returns>Vector up</returns>
		public static Vec3 Up() => new Vec3(0, 1, 0);

		/// <summary>
		/// Shorthand for writing Vec3(0, -1, 0)
		/// </summary>
		/// <returns>Vector down</returns>
		public static Vec3 Down() => new Vec3(0, -1, 0);

		/// <summary>
		/// Shorthand for writing Vec3(-1, 0, 0)
		/// </summary>
		/// <returns>Vector left</returns>
		public static Vec3 Left() => new Vec3(-1, 0, 0);

		/// <summary>
		/// Shorthand for writing Vec3(1, 0, 0)
		/// </summary>
		/// <returns>Vector right</returns>
		public static Vec3 Right() => new Vec3(1, 0, 0);

		/// <summary>
		/// Shorthand for writing Vec3(1, 1, 1)
		/// </summary>
		/// <returns>Vector one</returns>
		public static Vec3 One() => new Vec3(1, 1, 1);

		/// <summary>
		/// Shorthand for writing Vec3(0, 0, 0)
		/// </summary>
		/// <returns>Vector zero</returns>
		public static Vec3 Zero() => new Vec3(0, 0, 0);

		/// <summary>
		/// Operator + overload ; add two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 + v2</returns>
		public static Vec3 operator +(Vec3 v1, Vec3 v2) => new Vec3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

		/// <summary>
		/// Operator - overload ; subtracts two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 - v2</returns>
		public static Vec3 operator -(Vec3 v1, Vec3 v2) => new Vec3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

		/// <summary>
		/// Operator - overload ; returns the opposite of a vector
		/// </summary>
		/// <param name="v">Vector to negate</param>
		/// <returns>-v</returns>
		public static Vec3 operator -(Vec3 v) => new Vec3(-v.X, -v.Y, -v.Z);

		/// <summary>
		/// Operator * overload ; multiply a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="s">Scalar value</param>
		/// <returns>v * s</returns>
		public static Vec3 operator *(Vec3 v, float s) => new Vec3(v.X * s, v.Y * s, v.Z * s);

		/// <summary>
		/// Operator * overload ; multiply a scalar value by a vector
		/// </summary>
		/// <param name="s">Scalar value</param>
		/// <param name="v">Vector</param>
		/// <returns>s * v</returns>
		public static Vec3 operator *(float s, Vec3 v) => new Vec3(v.X * s, v.Y * s, v.Z * s);

		/// <summary>
		/// Operator / overload ; divide a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="s">Scalar value</param>
		/// <returns>v / s</returns>
		public static Vec3 operator /(Vec3 v, float s) => new Vec3(v.X / s, v.Y / s, v.Z / s);

		/// <summary>
		/// Operator == overload ; check vector equality
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 == v2</returns>
		public static bool operator ==(Vec3 v1, Vec3 v2) => v1.Equals(v2);

		/// <summary>
		/// Operator != overload ; check vector inequality
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 != v2</returns>
		public static bool operator !=(Vec3 v1, Vec3 v2) => !v1.Equals(v2);

		/// <summary>
		/// Compare vector and object and checks if they are equal
		/// </summary>
		/// <param name="obj">Object to check</param>
		/// <returns>Object and vector are equal</returns>
		public override bool Equals(object obj) => (obj is Vec3) && Equals((Vec3)obj);

		/// <summary>
		/// Compare two vectors and checks if they are equal
		/// </summary>
		/// <param name="other">Vector to check</param>
		/// <returns>Vectors are equal</returns>
		public bool Equals(Vec3 other) => (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z);

		/// <summary>
		/// 
		/// </summary>
		/// <returns>The length of the vector</returns>
		public float Magnitude() => Mathf.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);

		/// <summary>
		/// Makes this vector have a magnitude of 1
		/// </summary>
		/// <returns>Normalized vector</returns>
		public static Vec3 Normalize() => new Vec3(this.X / this.X.Magnitude(), this.Y / this.Y.Magnitude(), this.Z / this.Z.Magnitude());

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns>Dot Product of two vectors</returns>
		public static float DotProduct(Vec3 v) => this.X * v.X + this.Y * v.Y + this.Z * v.Z;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns>Cross Product of two vectors</returns>
		public static Vec3 CrossProduct(Vec3 v) => new Vec3(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X);

		/// <summary>
		/// Provide a string describing the object
		/// </summary>
		/// <returns>String description of the object</returns>
		public override string ToString() => $"[Vec3] X({ this.X }) Y({ this.Y }) Z({ this.Z })";
	}
}