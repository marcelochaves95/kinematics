using System;
using System.Runtime.InteropServices;

namespace MathModule
{
	/// <summary>
	/// Vector3D is an utility class for manipulating 3 dimensional
	/// vectors with float components
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3D : IEquatable<Vector3D>
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
		public Vector3D(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Shorthand for writing Vector3D(0, 0, 1)
		/// </summary>
		/// <returns>Vector forward</returns>
		public static readonly Vector3D Forward => new Vector3D(0f, 0f, 1f);

		/// <summary>
		/// Shorthand for writing Vector3D(0, 0, -1)
		/// </summary>
		/// <returns>Vector back</returns>
		public static readonly Vector3D Back => new Vector3D(0f, 0f, -1f);

		/// <summary>
		/// Shorthand for writing Vector3D(0, 1, 0)
		/// </summary>
		/// <returns>Vector up</returns>
		public static readonly Vector3D Up => new Vector3D(0f, 1f, 0f);

		/// <summary>
		/// Shorthand for writing Vector3D(0, -1, 0)
		/// </summary>
		/// <returns>Vector down</returns>
		public static readonly Vector3D Down => new Vector3D(0f, -1f, 0f);

		/// <summary>
		/// Shorthand for writing Vector3D(-1, 0, 0)
		/// </summary>
		/// <returns>Vector left</returns>
		public static readonly Vector3D Left => new Vector3D(-1f, 0f, 0f);

		/// <summary>
		/// Shorthand for writing Vector3D(1, 0, 0)
		/// </summary>
		/// <returns>Vector right</returns>
		public static readonly Vector3D Right => new Vector3D(1f, 0f, 0f);

		/// <summary>
		/// Shorthand for writing Vector3D(1, 1, 1)
		/// </summary>
		/// <returns>Vector one</returns>
		public static readonly Vector3D One => new Vector3D(1f, 1f, 1f);

		/// <summary>
		/// Shorthand for writing Vector3D(0, 0, 0)
		/// </summary>
		/// <returns>Vector zero</returns>
		public static readonly Vector3D Zero => new Vector3D(0f, 0f, 0f);

		/// <summary>
		/// Operator + overload ; add two vectors
		/// </summary>
		/// <param name="vector1">First vector</param>
		/// <param name="vector2">Second vector</param>
		/// <returns>v1 + v2</returns>
		public static Vector3D operator +(Vector3D vector1, Vector3D vector2) => new Vector3D(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);

		/// <summary>
		/// Operator - overload ; subtracts two vectors
		/// </summary>
		/// <param name="vector1">First vector</param>
		/// <param name="vector2">Second vector</param>
		/// <returns>v1 - v2</returns>
		public static Vector3D operator -(Vector3D vector1, Vector3D vector2) => new Vector3D(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z);

		/// <summary>
		/// Operator - overload ; returns the opposite of a vector
		/// </summary>
		/// <param name="vector">Vector to negate</param>
		/// <returns>-v</returns>
		public static Vector3D operator -(Vector3D vector) => new Vector3D(-vector.X, -vector.Y, -vector.Z);

		/// <summary>
		/// Operator * overload ; multiply a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="s">Scalar value</param>
		/// <returns>v * s</returns>
		public static Vector3D operator *(Vector3D v, float s) => new Vector3D(v.X * s, v.Y * s, v.Z * s);

		/// <summary>
		/// Operator * overload ; multiply a scalar value by a vector
		/// </summary>
		/// <param name="scalar">Scalar value</param>
		/// <param name="vector">Vector</param>
		/// <returns>s * v</returns>
		public static Vector3D operator *(float scalar, Vector3D vector) => new Vector3D(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);

		/// <summary>
		/// Operator / overload ; divide a vector by a scalar value
		/// </summary>
		/// <param name="vector">Vector</param>
		/// <param name="scalar">Scalar value</param>
		/// <returns>v / s</returns>
		public static Vector3D operator /(Vector3D vector, float scalar) => new Vector3D(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);

		/// <summary>
		/// Operator == overload ; check vector equality
		/// </summary>
		/// <param name="vector1">First vector</param>
		/// <param name="vector2">Second vector</param>
		/// <returns>v1 == v2</returns>
		public static bool operator ==(Vector3D vector1, Vector3D vector2) => vector1.Equals(vector2);

		/// <summary>
		/// Operator != overload ; check vector inequality
		/// </summary>
		/// <param name="vector1">First vector</param>
		/// <param name="vector2">Second vector</param>
		/// <returns>v1 != v2</returns>
		public static bool operator !=(Vector3D vector1, Vector3D vector2) => !vector1.Equals(vector2);

		/// <summary>
		/// Compare vector and object and checks if they are equal
		/// </summary>
		/// <param name="obj">Object to check</param>
		/// <returns>Object and vector are equal</returns>
		public override bool Equals(object obj) => (obj is Vector3D) && Equals((Vector3D)obj);

		/// <summary>
		/// Compare two vectors and checks if they are equal
		/// </summary>
		/// <param name="other">Vector to check</param>
		/// <returns>Vectors are equal</returns>
		public bool Equals(Vector3D other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

		/// <summary>
		/// Calculates the length of the vector
		/// </summary>
		/// <returns>The length of the vector</returns>
		public static float Magnitude(Vector3D vector) => new Mathematics.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);

		/// <summary>
		/// Converts the vector into a unit vector
		/// </summary>
		/// <param name="vector">Vector</param>
		/// <returns>Normalized vector</returns>
		public static Vector3D Normalize(Vector3D vector) => vector / Magnitude(vector);

		/// <summary>
		/// Converts the vector into a unit vector
		/// </summary>
		/// <returns>Normalized vector</returns>
		public static Vector3D Normalize() => this / Magnitude(this);

		/// <summary>
		/// Calculates the dot product of two vectors
		/// </summary>
		/// <param name="vector">Vector</param>
		/// <returns>Dot Product of two vectors</returns>
		public static float DotProduct(Vector3D vector) => X * vector.X + Y * vector.Y + Z * vector.Z;

		/// <summary>
		/// Calculates the cross product of two vectors
		/// </summary>
		/// <param name="vector">Vector</param>
		/// <returns>Cross Product of two vectors</returns>
		public static Vector3D CrossProduct(Vector3D vector) => new Vector3D(Y * vector.Z - Z * vector.Y, Z * vector.X - X * vector.Z, X * vector.Y - Y * vector.X);

		/// <summary>
		/// Used to allow Vector3Ds to be used as keys in hash tables
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
		/// hash table.</returns>
		public override int GetHashCode() => X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2);

		/// <summary>
		/// Provide a string describing the object
		/// </summary>
		/// <returns>String description of the object</returns>
		public override string ToString() => $"[Vector3D] X({ X }) Y({ Y }) Z({ Z })";
	}
}