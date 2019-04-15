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
		/// Construct the vector from it's coordinates
		/// </summary>
		/// <param name="values">The values to assign to the X, Y, and Z components of the vector. This must be an array with three elements</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c></exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than three elements</exception>
		public Vector3D(float[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}

			if (values.Length != 3)
			{
				throw new ArgumentOutOfRangeException("values", "There must be three and only three input values for Vector3D.");
			}

			X = values[0];
			Y = values[1];
			Z = values[2];
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
		public static Vector3D operator +(Vector3D value1, Vector3D value2) => new Vector3D(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);

		/// <summary>
		/// Operator - overload ; subtracts two vectors
		/// </summary>
		/// <param name="value1">First vector</param>
		/// <param name="value2">Second vector</param>
		/// <returns>v1 - v2</returns>
		public static Vector3D operator -(Vector3D value1, Vector3D value2) => new Vector3D(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);

		/// <summary>
		/// Operator - overload ; returns the opposite of a vector
		/// </summary>
		/// <param name="value">Vector to negate</param>
		/// <returns>-v</returns>
		public static Vector3D operator -(Vector3D value) => new Vector3D(-value.X, -value.Y, -value.Z);

		/// <summary>
		/// Scales a vector by the given value
		/// </summary>
		/// <param name="value1">The vector to scale</param>
		/// <param name="value2">The amount by which to scale the vector</param>
		/// <returns>The scaled vector</returns>
		public static Vector3D operator *(Vector3D value1, Vector3D value2) => new Vector3D(value1.X * value2.X, value1.Y * value2.Y, value1.Z * value2.Z);

		/// <summary>
		/// Operator * overload ; multiply a vector by a scalar value
		/// </summary>
		/// <param name="value">Vector</param>
		/// <param name="scalar">Scalar value</param>
		/// <returns>v * s</returns>
		public static Vector3D operator *(Vector3D value, float scalar) => new Vector3D(value.X * scalar, value.Y * scalar, value.Z * scalar);

		/// <summary>
		/// Operator * overload ; multiply a scalar value by a vector
		/// </summary>
		/// <param name="scalar">Scalar value</param>
		/// <param name="value">Vector</param>
		/// <returns>s * v</returns>
		public static Vector3D operator *(float scalar, Vector3D value) => new Vector3D(value.X * scalar, value.Y * scalar, value.Z * scalar);

		/// <summary>
		/// Scales a vector by the given value
		/// </summary>
		/// <param name="value1">The vector to scale</param>
		/// <param name="value2">The amount by which to scale the vector</param>
		/// <returns>The scaled vector</returns>
		public static Vector3D operator /(Vector3D value1, Vector3D value2) => new Vector3D(value1.X / value2.X, value1.Y / value2.Y, value1.Z / value2.Z);

		/// <summary>
		/// Operator / overload ; divide a vector by a scalar value
		/// </summary>
		/// <param name="value">Vector</param>
		/// <param name="scalar">Scalar value</param>
		/// <returns>v / s</returns>
		public static Vector3D operator /(Vector3D value, float scalar) => new Vector3D(value.X / scalar, value.Y / scalar, value.Z / scalar);

		/// <summary>
		/// Performs an explicit conversion from Vector3D to Vector4D
		/// </summary>
		/// <param name="value">The value</param>
		/// <returns>The result of the conversion./returns>
		public static explicit operator Vector4D(Vector3D value) => new Vector4D(value, 0f);

		/// <summary>
		/// Operator == overload ; check vector equality
		/// </summary>
		/// <param name="value1">First vector</param>
		/// <param name="value2">Second vector</param>
		/// <returns>v1 == v2</returns>
		public static bool operator ==(Vector3D value1, Vector3D value2) => value1.Equals(value2);

		/// <summary>
		/// Operator != overload ; check vector inequality
		/// </summary>
		/// <param name="value1">First vector</param>
		/// <param name="value2">Second vector</param>
		/// <returns>v1 != v2</returns>
		public static bool operator !=(Vector3D value1, Vector3D value2) => !value1.Equals(value2);

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
		public static float Magnitude(Vector3D value) => Mathematics.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);

		/// <summary>
        /// Calculates the squared length of the vector
        /// </summary>
		/// <returns>The length of the vector</returns>
        public static float MagnitudeSquared => X * X + Y * Y + Z * Z;

		/// <summary>
		/// Converts the vector into a unit vector
		/// </summary>
		/// <param name="value">Vector</param>
		/// <returns>Normalized vector</returns>
		public static Vector3D Normalize(Vector3D value) => value / Magnitude(value);

		/// <summary>
		/// Converts the vector into a unit vector
		/// </summary>
		/// <returns>Normalized vector</returns>
		public static Vector3D Normalize() => Normalize(this);

		/// <summary>
		/// Calculates the dot product of two vectors
		/// </summary>
		/// <param name="value1">Vector</param>
		/// <returns>Dot Product of two vectors</returns>
		public static float DotProduct(Vector3D value1, Vector3D value2) => value1.X + value2.X + value1.Y * value2.Y + value1.Z * value2.Z;

		/// <summary>
		/// Calculates the cross product of two vectors
		/// </summary>
		/// <param name="value2">Vector</param>
		/// <returns>Cross Product of two vectors</returns>
		public static Vector3D CrossProduct(Vector3D value1, Vector3D value2) => new Vector3D(value1.Y * value2.Z - value1.Z * value2.Y, value1.Z * value2.X - value1.X * value2.Z, value1.X * value2.Y - value1.Y * value2.X);

		/// <summary>
		/// Returns a <see cref="Vector3D"/> containing the 3D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 3D triangle
		/// </summary>
		/// <param name="value1">A <see cref="Vector3D"/> containing the 3D Cartesian coordinates of vertex 1 of the triangle</param>
		/// <param name="value2">A <see cref="Vector3D"/> containing the 3D Cartesian coordinates of vertex 2 of the triangle</param>
		/// <param name="value3">A <see cref="Vector3D"/> containing the 3D Cartesian coordinates of vertex 3 of the triangle</param>
		/// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>)</param>
		/// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>)</param>
		/// <returns>A new <see cref="Vector3D"/> containing the 3D Cartesian coordinates of the specified point</returns>
		public static Vector3D Barycentric(Vector3D value1, Vector3D value2, Vector3D value3, float amount1, float amount2)
		{
			return new Vector3D((value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X)),
				(value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y)),
				(value1.Z + (amount1 * (value2.Z - value1.Z))) + (amount2 * (value3.Z - value1.Z)));
		}

		/// <summary>
		/// Restricts a value to be within a specified range
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		/// <returns>The clamped value</returns>
		public static Vector3D Clamp(Vector3D value, Vector3D min, Vector3D max)
		{
			float x = value.X;
			x = (x > max.X) ? max.X : x;
			x = (x < min.X) ? min.X : x;

			float y = value.Y;
			y = (y > max.Y) ? max.Y : y;
			y = (y < min.Y) ? min.Y : y;

			float z = value.Z;
			z = (z > max.Z) ? max.Z : z;
			z = (z < min.Z) ? min.Z : z;

			return new Vector3D(x, y, z);
		}

		/// <summary>
		/// Calculates the tripple cross product of three vectors
		/// </summary>
		/// <param name="value1">First source vector</param>
		/// <param name="value2">Second source vector</param>
		/// <param name="value3">Third source vector</param>
		/// <returns>The tripple cross product of the three vectors</returns>
		public static float TripleProduct(Vector3D value1, Vector3D value2, Vector3D value3) => DotProduct(ref value1, ref CrossProduct(ref value2, ref value3));

		/// <summary>
		/// Calculates the distance between two vectors
		/// </summary>
		/// <param name="value1">The first vector</param>
		/// <param name="value2">The second vector</param>
		/// <returns>The distance between the two vectors</returns>
		public static float Distance(Vector3D value1, Vector3D value2)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;
			float z = value1.Z - value2.Z;

			return Mathematics.Sqrt((x * x) + (y * y) + (z * z));
		}

		/// <summary>
		/// Performs a linear interpolation between two vectors
		/// </summary>
		/// <param name="start">Start vector</param>
		/// <param name="end">End vector</param>
		/// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/></param>
		/// <returns>The linear interpolation of the two vectors</returns>
		public static Vector3D Lerp(Vector3D start, Vector3D end, float amount)
		{
			return new Vector3D(
				start.X + ((end.X - start.X) * amount),
				start.Y + ((end.Y - start.Y) * amount),
				start.Z + ((end.Z - start.Z) * amount)
			);
		}

		/// <summary>
		/// Performs a cubic interpolation between two vectors
		/// </summary>
		/// <param name="start">Start vector</param>
		/// <param name="end">End vector</param>
		/// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/></param>
		/// <returns>The cubic interpolation of the two vectors</returns>
		public static Vector3D SmoothStep(Vector3D start, Vector3D end, float amount)
		{
			float newAmount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);

			return new Vector3D(
				start.X + ((end.X - start.X) * (newAmount * newAmount) * (3f - (2f * newAmount))),
				start.Y + ((end.Y - start.Y) * (newAmount * newAmount) * (3f - (2f * newAmount))),
				start.Z + ((end.Z - start.Z) * (newAmount * newAmount) * (3f - (2f * newAmount)))
			);
		}

		/// <summary>
		/// Performs a Hermite spline interpolation
		/// </summary>
		/// <param name="value1">First source position vector</param>
		/// <param name="tangent1">First source tangent vector</param>
		/// <param name="value2">Second source position vector</param>
		/// <param name="tangent2">Second source tangent vector</param>
		/// <param name="amount">Weighting factor</param>
		/// <returns>The result of the Hermite spline interpolation</returns>
		public static Vector3D Hermite(Vector3D value1, Vector3D tangent1, Vector3D value2, Vector3D tangent2, float amount)
		{
			float squared = amount * amount;
			float cubed = amount * squared;
			float part1 = ((2f * cubed) - (3f * squared)) + 1f;
			float part2 = (-2f * cubed) + (3f * squared);
			float part3 = (cubed - (2f * squared)) + amount;
			float part4 = cubed - squared;

			return new Vector3D(
				(((value1.X * part1) + (value2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4),
				(((value1.Y * part1) + (value2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4),
				(((value1.Z * part1) + (value2.Z * part2)) + (tangent1.Z * part3)) + (tangent2.Z * part4)
			);
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions
		/// </summary>
		/// <param name="value1">The first position in the interpolation</param>
		/// <param name="value2">The second position in the interpolation</param>
		/// <param name="value3">The third position in the interpolation</param>
		/// <param name="value4">The fourth position in the interpolation</param>
		/// <param name="amount">Weighting factor</param>
		/// <returns>A vector that is the result of the Catmull-Rom interpolation</returns>
		public static Vector3D CatmullRom(Vector3D value1, Vector3D value2, Vector3D value3, Vector3D value4, float amount)
		{
			float squared = amount * amount;
			float cubed = amount * squared;

			return new Vector3D(
				0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) +
						(((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * squared)) +
					((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * cubed)),
				0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) +
						(((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * squared)) +
					((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * cubed)),
				0.5f * ((((2f * value2.Z) + ((-value1.Z + value3.Z) * amount)) +
						(((((2f * value1.Z) - (5f * value2.Z)) + (4f * value3.Z)) - value4.Z) * squared)) +
					((((-value1.Z + (3f * value2.Z)) - (3f * value3.Z)) + value4.Z) * cubed))
			);
		}

		/// <summary>
		/// Returns a vector containing the largest components of the specified vectors
		/// </summary>
		/// <param name="value1">The first source vector</param>
		/// <param name="value2">The second source vector</param>
		/// <returns>A vector containing the largest components of the source vectors</returns>
		public static Vector3D Max(Vector3D value1, Vector3D value2)
		{
			return new Vector3D(
				(value1.X > value2.X) ? value1.X : value2.X,
				(value1.Y > value2.Y) ? value1.Y : value2.Y,
				(value1.Z > value2.Z) ? value1.Z : value2.Z
			);
		}

		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors
		/// </summary>
		/// <param name="value1">The first source vector</param>
		/// <param name="value2">The second source vector</param>
		/// <returns>A vector containing the smallest components of the source vectors</returns>
		public static Vector3D Min(Vector3D value1, Vector3D value2)
		{
			return new Vector3D(
				(value1.X < value2.X) ? value1.X : value2.X,
				(value1.Y < value2.Y) ? value1.Y : value2.Y,
				(value1.Z < value2.Z) ? value1.Z : value2.Z
			);
		}

		/// <summary>
		/// Projects a 3D vector from object space into screen space
		/// </summary>
		/// <param name="vector">The vector to project</param>
		/// <param name="x">The X position of the viewport</param>
		/// <param name="y">The Y position of the viewport</param>
		/// <param name="width">The width of the viewport</param>
		/// <param name="height">The height of the viewport</param>
		/// <param name="minZ">The minimum depth of the viewport</param>
		/// <param name="maxZ">The maximum depth of the viewport</param>
		/// <param name="worldViewProjection">The combined world-view-projection matrix</param>
		/// <returns>The vector in screen space</returns>
		public static Vector3D Project(Vector3D vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix worldViewProjection)
		{
			Vector3D v = TransformCoordinate(ref vector, ref worldViewProjection);

			return new Vector3D(((1f + v.X) * 0.5f * width) + x, ((1f - v.Y) * 0.5f * height) + y, (v.Z * (maxZ - minZ)) + minZ);
		}

		/// <summary>
		/// Projects a 3D vector from screen space into object space
		/// </summary>
		/// <param name="vector">The vector to project</param>
		/// <param name="x">The X position of the viewport</param>
		/// <param name="y">The Y position of the viewport</param>
		/// <param name="width">The width of the viewport</param>
		/// <param name="height">The height of the viewport</param>
		/// <param name="minZ">The minimum depth of the viewport</param>
		/// <param name="maxZ">The maximum depth of the viewport</param>
		/// <param name="worldViewProjection">The combined world-view-projection matrix</param>
		/// <returns>The vector in object space</returns>
		public static Vector3D Unproject(Vector3D vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix worldViewProjection)
		{
			Vector3D v = new Vector3D();
			Matrix matrix = Matrix.Invert(worldViewProjection);

			v.X = (((vector.X - x) / width) * 2f) - 1f;
			v.Y = -((((vector.Y - y) / height) * 2f) - 1f);
			v.Z = (vector.Z - minZ) / (maxZ - minZ);

			return TransformCoordinate(v, matrix);
		}

		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified normal
		/// </summary>
		/// <param name="vector">The source vector</param>
		/// <param name="normal">Normal of the surface</param>
		/// <returns>The reflected vector</returns>
		public static Vector3D Reflect(Vector3D vector, Vector3D normal)
		{
			float dot = DotProduct(vector, normal);

			return new Vector3D(
				vector.X - ((2f * dot) * normal.X),
				vector.Y - ((2f * dot) * normal.Y),
				vector.Z - ((2f * dot) * normal.Z)
			);
		}

		/// <summary>
		/// Returns the fraction of a vector off a surface that has the specified normal and index
		/// </summary>
		/// <param name="vector">The source vector</param>
		/// <param name="normal">Normal of the surface</param>
		/// <param name="index">Index of refraction</param>
		/// <returns>The refracted vector</returns>
		public static Vector3D Refract(Vector3D vector, Vector3D normal, float index)
		{
			float cos1;

			cos1 = DotProduct(vector, normal);

			float radicand = 1f - (index * index) * (1f - (cos1 * cos1));

			if (radicand < 0f)
			{
				return Zero;
			}
			else
			{
				float cos2 = Mathematics.Sqrt(radicand);
				return (index * vector) + ((cos2 - index * cos1) * normal);
			}
		}

		/// <summary>
		/// Transforms a 3D vector by the given <see cref="Quaternion"/> rotation
		/// </summary>
		/// <param name="vector">The vector to rotate</param>
		/// <param name="rotation">The <see cref="Quaternion"/> rotation to apply</param>
		/// <returns>The transformed <see cref="Vector4D"/></returns>
		public static Vector3D Transform(Vector3D vector, Quaternion rotation)
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

			float num1 = ((1f - yy) - zz);
			float num2 = (xy - wz);
			float num3 = (xz + wy);
			float num4 = (xy + wz);
			float num5 = ((1f - xx) - zz);
			float num6 = (yz - wx);
			float num7 = (xz - wy);
			float num8 = (yz + wx);
			float num9 = ((1f - xx) - yy);

			return new Vector3D(
				((vector.X * num1) + (vector.Y * num2)) + (vector.Z * num3),
				((vector.X * num4) + (vector.Y * num5)) + (vector.Z * num6),
				((vector.X * num7) + (vector.Y * num8)) + (vector.Z * num9));
		}

		/// <summary>
		/// Transforms a 3D vector by the given <see cref="Matrix"/>
		/// </summary>
		/// <param name="vector">The source vector</param>
		/// <param name="transform">The transformation <see cref="Matrix"/></param>
		/// <returns>The transformed <see cref="Vector4D"/></returns>
		public static Vector4D Transform(Vector3D vector, Matrix transform)
		{
			return new Vector4D(
				(vector.X * transform.M11) + (vector.Y * transform.M21) + (vector.Z * transform.M31) + transform.M41,
				(vector.X * transform.M12) + (vector.Y * transform.M22) + (vector.Z * transform.M32) + transform.M42,
				(vector.X * transform.M13) + (vector.Y * transform.M23) + (vector.Z * transform.M33) + transform.M43,
				(vector.X * transform.M14) + (vector.Y * transform.M24) + (vector.Z * transform.M34) + transform.M44);
		}

		/// <summary>
		/// Performs a coordinate transformation using the given <see cref="Matrix"/>
		/// </summary>
		/// <param name="coordinate">The coordinate vector to transform</param>
		/// <param name="transform">The transformation <see cref="Matrix"/></param>
		/// <returns>The transformed coordinates</returns>
		public static Vector3D TransformCoordinate(Vector3D coordinate, Matrix transform)
		{
			float w = 1f / ((coordinate.X * transform.M14) + (coordinate.Y * transform.M24) + (coordinate.Z * transform.M34) + transform.M44);

			return new Vector3D(
				w * ((coordinate.X * transform.M11) + (coordinate.Y * transform.M21) + (coordinate.Z * transform.M31) + transform.M41),
				w * ((coordinate.X * transform.M12) + (coordinate.Y * transform.M22) + (coordinate.Z * transform.M32) + transform.M42),
				w * ((coordinate.X * transform.M13) + (coordinate.Y * transform.M23) + (coordinate.Z * transform.M33) + transform.M43));
		}

		/// <summary>
		/// Performs a normal transformation using the given <see cref="Matrix"/>
		/// </summary>
		/// <param name="normal">The normal vector to transform</param>
		/// <param name="transform">The transformation <see cref="Matrix"/></param>
		/// <returns>The transformed normal</returns>
		public static Vector3D TransformNormal(Vector3D normal, Matrix transform)
		{
			return new Vector3D(
				(normal.X * transform.M11) + (normal.Y * transform.M21) + (normal.Z * transform.M31),
				(normal.X * transform.M12) + (normal.Y * transform.M22) + (normal.Z * transform.M32),
				(normal.X * transform.M13) + (normal.Y * transform.M23) + (normal.Z * transform.M33));
		}

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