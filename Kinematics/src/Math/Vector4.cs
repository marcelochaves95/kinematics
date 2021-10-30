using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kinematics.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 : IEquatable<Vector4>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public static readonly Vector4 UnitX = new Vector4(1f, 0f, 0f, 0f);
        public static readonly Vector4 UnitY = new Vector4(0f, 1f, 0f, 0f);
        public static readonly Vector4 UnitZ = new Vector4(0f, 0f, 1f, 0f);
        public static readonly Vector4 UnitW = new Vector4(0f, 0f, 0f, 1f);
        public static readonly Vector4 One = new Vector4(1f, 1f, 1f, 1f);
        public static readonly Vector4 Zero = new Vector4(0f, 0f, 0f, 0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(Vector4 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = value.W;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(Vector3 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(Quaternion value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = value.W;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Barycentric(Vector4 value1, Vector4 value2, Vector4 value3, float amount1, float amount2)
        {
            float x = Mathf.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            float y = Mathf.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
            float z = Mathf.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
            float w = Mathf.Barycentric(value1.W, value2.W, value3.W, amount1, amount2);
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 CatmullRom(Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount)
        {
            float x = Mathf.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            float y = Mathf.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
            float z = Mathf.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
            float w = Mathf.CatmullRom(value1.W, value2.W, value3.W, value4.W, amount);
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Ceiling()
        {
            X = Mathf.Ceiling(X);
            Y = Mathf.Ceiling(Y);
            Z = Mathf.Ceiling(Z);
            W = Mathf.Ceiling(W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Ceiling(Vector4 value)
        {
            value.X = Mathf.Ceiling(value.X);
            value.Y = Mathf.Ceiling(value.Y);
            value.Z = Mathf.Ceiling(value.Z);
            value.W = Mathf.Ceiling(value.W);
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max)
        {
            float x = Mathf.Clamp(value.X, min.X, max.X);
            float y = Mathf.Clamp(value.Y, min.Y, max.Y);
            float z = Mathf.Clamp(value.Z, min.Z, max.Z);
            float w = Mathf.Clamp(value.W, min.W, max.W);
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector4 lhs, Vector4 rhs)
        {
            float distanceSquared = DistanceSquared(lhs, rhs);
            return Mathf.Sqrt(distanceSquared);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Vector4 lhs, Vector4 rhs)
        {
              return (lhs.W - rhs.W) * (lhs.W - rhs.W) + (lhs.X - rhs.X) * (lhs.X - rhs.X) + (lhs.Y - rhs.Y) * (lhs.Y - rhs.Y) + (lhs.Z - rhs.Z) * (lhs.Z - rhs.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector4 value1, Vector4 value2)
        {
            return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z + value1.W * value2.W;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Floor()
        {
            X = Mathf.Floor(X);
            Y = Mathf.Floor(Y);
            Z = Mathf.Floor(Z);
            W = Mathf.Floor(W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Floor(Vector4 value)
        {
            value.X = Mathf.Floor(value.X);
            value.Y = Mathf.Floor(value.Y);
            value.Z = Mathf.Floor(value.Z);
            value.W = Mathf.Floor(value.W);
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Hermite(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
        {
            float x = Mathf.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            float y = Mathf.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
            float z = Mathf.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
            float w = Mathf.Hermite(value1.W, tangent1.W, value2.W, tangent2.W, amount);
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Magnitude()
        {
            float lengthSquared = MagnitudeSquared(this);
            return Mathf.Sqrt(lengthSquared);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Magnitude(Vector4 value)
        {
            float lengthSquared = MagnitudeSquared(value);
            return Mathf.Sqrt(lengthSquared);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float MagnitudeSquared()
        {
            return MagnitudeSquared(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MagnitudeSquared(Vector4 value)
        {
            return Mathf.Pow(value.X, 2) + Mathf.Pow(value.Y, 2) + Mathf.Pow(value.Z, 2) + Mathf.Pow(value.W, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Lerp(Vector4 lhs, Vector4 rhs, float amount)
        {
            float x = Mathf.Lerp(lhs.X, rhs.X, amount);
            float y = Mathf.Lerp(lhs.Y, rhs.Y, amount);
            float z = Mathf.Lerp(lhs.Z, rhs.Z, amount);
            float w = Mathf.Lerp(lhs.W, rhs.W, amount);
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 LerpPrecise(Vector4 lhs, Vector4 rhs, float amount)
        {
            float x = Mathf.LerpPrecise(lhs.X, rhs.X, amount);
            float y = Mathf.LerpPrecise(lhs.Y, rhs.Y, amount);
            float z = Mathf.LerpPrecise(lhs.Z, rhs.Z, amount);
            float w = Mathf.LerpPrecise(lhs.W, rhs.W, amount);
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Max(Vector4 lhs, Vector4 rhs)
        {
            float x = Mathf.Max(lhs.X, rhs.X);
            float y = Mathf.Max(lhs.Y, rhs.Y);
            float z = Mathf.Max(lhs.Z, rhs.Z);
            float w = Mathf.Max(lhs.W, rhs.W);
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Min(Vector4 lhs, Vector4 rhs)
        {
            float x = Mathf.Min(lhs.X, rhs.X);
            float y = Mathf.Min(lhs.Y, rhs.Y);
            float z = Mathf.Min(lhs.Z, rhs.Z);
            float w = Mathf.Min(lhs.W, rhs.W);
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            this = Normalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Normalize(Vector4 value)
        {
            float magnitude = Magnitude(value);
            if (magnitude > Mathf.Epsilon)
            {
                return value / magnitude;
            }

            return Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Round()
        {
            X = Mathf.Round(X);
            Y = Mathf.Round(Y);
            Z = Mathf.Round(Z);
            W = Mathf.Round(W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Round(Vector4 value)
        {
            value.X = Mathf.Round(value.X);
            value.Y = Mathf.Round(value.Y);
            value.Z = Mathf.Round(value.Z);
            value.W = Mathf.Round(value.W);
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SmoothStep(Vector4 lhs, Vector4 rhs, float amount)
        {
            float x = Mathf.SmoothStep(lhs.X, rhs.X, amount);
            float y = Mathf.SmoothStep(lhs.Y, rhs.Y, amount);
            float z = Mathf.SmoothStep(lhs.Z, rhs.Z, amount);
            float w = Mathf.SmoothStep(lhs.W, rhs.W, amount);
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Transform(Vector2 value, Matrix4x4 matrix4X4)
        {
            float x = value.X * matrix4X4.M11 + value.Y * matrix4X4.M21 + matrix4X4.M41;
            float y = value.X * matrix4X4.M12 + value.Y * matrix4X4.M22 + matrix4X4.M42;
            float z = value.X * matrix4X4.M13 + value.Y * matrix4X4.M23 + matrix4X4.M43;
            float w = value.X * matrix4X4.M14 + value.Y * matrix4X4.M24 + matrix4X4.M44;
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Transform(Vector3 value, Matrix4x4 matrix4X4)
        {
            float x = value.X * matrix4X4.M11 + value.Y * matrix4X4.M21 + value.Z * matrix4X4.M31 + matrix4X4.M41;
            float y = value.X * matrix4X4.M12 + value.Y * matrix4X4.M22 + value.Z * matrix4X4.M32 + matrix4X4.M42;
            float z = value.X * matrix4X4.M13 + value.Y * matrix4X4.M23 + value.Z * matrix4X4.M33 + matrix4X4.M43;
            float w = value.X * matrix4X4.M14 + value.Y * matrix4X4.M24 + value.Z * matrix4X4.M34 + matrix4X4.M44;
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Transform(Vector4 value, Matrix4x4 matrix4X4)
        {
            float x = value.X * matrix4X4.M11 + value.Y * matrix4X4.M21 + value.Z * matrix4X4.M31 + value.W * matrix4X4.M41;
            float y = value.X * matrix4X4.M12 + value.Y * matrix4X4.M22 + value.Z * matrix4X4.M32 + value.W * matrix4X4.M42;
            float z = value.X * matrix4X4.M13 + value.Y * matrix4X4.M23 + value.Z * matrix4X4.M33 + value.W * matrix4X4.M43;
            float w = value.X * matrix4X4.M14 + value.Y * matrix4X4.M24 + value.Z * matrix4X4.M34 + value.W * matrix4X4.M44;
            return new Vector4(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator +(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator -(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator -(Vector4 value)
        {
            return new Vector4(-value.X, -value.Y, -value.Z, -value.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z, lhs.W * rhs.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(Vector4 vector, float scalar)
        {
            return new Vector4(vector.X * scalar, vector.Y * scalar, vector.Z * scalar, vector.W * scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(float scalar, Vector4 vector)
        {
            return new Vector4(vector.X * scalar, vector.Y * scalar, vector.Z * scalar, vector.W * scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator /(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z, lhs.W / rhs.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator /(Vector4 vector, float scalar)
        {
            return new Vector4(vector.X / scalar, vector.Y / scalar, vector.Z / scalar, vector.W / scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4 lhs, Vector4 rhs)
        {
            return lhs.Equals(rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4 lhs, Vector4 rhs)
        {
            return !lhs.Equals(rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3(Vector4 value)
        {
            return new Vector3(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Quaternion(Vector4 value)
        {
            return new Quaternion(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Vector4 vector && Equals(vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4 other)
        {
            return W.Equals(other.W) && X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"[Vector4] X({X}) Y({Y}) Z({Z}) W({W})";
        }

#if XNA || MONOGAME
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 FromXna(Microsoft.Xna.Framework.Vector4 value)
        {
            return new Vector4(value.X, value.Y, value.Z, value.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Microsoft.Xna.Framework.Vector4 ToXna()
        {
            return new Microsoft.Xna.Framework.Vector4(X, Y, Z, W);
        }
#endif
    }
}
