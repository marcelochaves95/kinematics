using System;
using System.Runtime.InteropServices;

namespace SesanoEngine.Core
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
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		/// <summary>
		/// Shorthand for writing Vector3D(0, 0, 1)
		/// </summary>
		/// <returns>Vector forward</returns>
		public static Vector3D Forward() => new Vector3D(0, 0, 1);

		/// <summary>
		/// Shorthand for writing Vector3D(0, 0, -1)
		/// </summary>
		/// <returns>Vector back</returns>
		public static Vector3D Back() => new Vector3D(0, 0, -1);
		
		/// <summary>
		/// Shorthand for writing Vector3D(0, 1, 0)
		/// </summary>
		/// <returns>Vector up</returns>
		public static Vector3D Up() => new Vector3D(0, 1, 0);

		/// <summary>
		/// Shorthand for writing Vector3D(0, -1, 0)
		/// </summary>
		/// <returns>Vector down</returns>
		public static Vector3D Down() => new Vector3D(0, -1, 0);

		/// <summary>
		/// Shorthand for writing Vector3D(-1, 0, 0)
		/// </summary>
		/// <returns>Vector left</returns>
		public static Vector3D Left() => new Vector3D(-1, 0, 0);

		/// <summary>
		/// Shorthand for writing Vector3D(1, 0, 0)
		/// </summary>
		/// <returns>Vector right</returns>
		public static Vector3D Right() => new Vector3D(1, 0, 0);

		/// <summary>
		/// Shorthand for writing Vector3D(1, 1, 1)
		/// </summary>
		/// <returns>Vector one</returns>
		public static Vector3D One() => new Vector3D(1, 1, 1);

		/// <summary>
		/// Shorthand for writing Vector3D(0, 0, 0)
		/// </summary>
		/// <returns>Vector zero</returns>
		public static Vector3D Zero() => new Vector3D(0, 0, 0);

		/// <summary>
		/// Operator + overload ; add two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 + v2</returns>
		public static Vector3D operator +(Vector3D v1, Vector3D v2) => new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

		/// <summary>
		/// Operator - overload ; subtracts two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 - v2</returns>
		public static Vector3D operator -(Vector3D v1, Vector3D v2) => new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

		/// <summary>
		/// Operator - overload ; returns the opposite of a vector
		/// </summary>
		/// <param name="v">Vector to negate</param>
		/// <returns>-v</returns>
		public static Vector3D operator -(Vector3D v) => new Vector3D(-v.X, -v.Y, -v.Z);

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
		/// <param name="s">Scalar value</param>
		/// <param name="v">Vector</param>
		/// <returns>s * v</returns>
		public static Vector3D operator *(float s, Vector3D v) => new Vector3D(v.X * s, v.Y * s, v.Z * s);

		/// <summary>
		/// Operator / overload ; divide a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="s">Scalar value</param>
		/// <returns>v / s</returns>
		public static Vector3D operator /(Vector3D v, float s) => new Vector3D(v.X / s, v.Y / s, v.Z / s);

		/// <summary>
		/// Operator == overload ; check vector equality
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 == v2</returns>
		public static bool operator ==(Vector3D v1, Vector3D v2) => v1.Equals(v2);

		/// <summary>
		/// Operator != overload ; check vector inequality
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 != v2</returns>
		public static bool operator !=(Vector3D v1, Vector3D v2) => !v1.Equals(v2);

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
		public bool Equals(Vector3D other) => (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z);

		/// <summary>
		/// 
		/// </summary>
		/// <returns>The length of the vector</returns>
		public static float Magnitude(Vector3D v) { return Mathf.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z); }

		/// <summary>
		/// Makes this vector have a magnitude of 1
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns>Normalized vector</returns>
		public static Vector3D Normalize(Vector3D v)
        {
            float magnitude = this.Magnitude(v);
            if (magnitude > 0)
                return v / magnitude;
            else
                return Vector3D.Zero();
        }

		/// <summary>
		/// Makes this vector have a magnitude of 1
		/// </summary>
		/// <returns>Normalized vector</returns>
		public static Vector3D Normalize()
		{
			float magnitude = this.Magnitude(this);
            if (magnitude > 0)
                this = this / magnitude;
            else
                this = Vector3D.Zero();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns>Dot Product of two vectors</returns>
		public static float DotProduct(Vector3D v) => this.X * v.X + this.Y * v.Y + this.Z * v.Z;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns>Cross Product of two vectors</returns>
		public static Vector3D CrossProduct(Vector3D v) => new Vector3D(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X);

		/// <summary>
		/// Provide a string describing the object
		/// </summary>
		/// <returns>String description of the object</returns>
		public override string ToString() => $"[Vector3D] X({ this.X }) Y({ this.Y }) Z({ this.Z })";
	}
}