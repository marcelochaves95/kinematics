using System;
using System.Runtime.InteropServices;

namespace Kinematics.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 : IEquatable<Vector3>
    {
        #region Properties

        public float X;
        public float Y;
        public float Z;

        public static readonly Vector3 Forward = new Vector3(0f, 0f, 1f);
        public static readonly Vector3 Backward = new Vector3(0f, 0f, -1f);
        public static readonly Vector3 Up = new Vector3(0f, 1f, 0f);
        public static readonly Vector3 Down = new Vector3(0f, -1f, 0f);
        public static readonly Vector3 Left = new Vector3(-1f, 0f, 0f);
        public static readonly Vector3 Right = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 One = new Vector3(1f, 1f, 1f);
        public static readonly Vector3 Zero = new Vector3(0f, 0f, 0f);

        #endregion

        #region Constructors

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector3 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }

        public Vector3(Vector2 value)
        {
            X = value.X;
            Y = value.Y;
            Z = 0f;
        }

        public Vector3(Vector4 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }

        public Vector3(Quaternion value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }

        #endregion

        #region Operators

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        public static Vector3 operator -(Vector3 value)
        {
            return new Vector3(-value.X, -value.Y, -value.Z);
        }

        public static Vector3 operator *(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);
        }

        public static Vector3 operator *(Vector3 vector, float scalar)
        {
            return new Vector3(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        public static Vector3 operator *(float scalar, Vector3 vector)
        {
            return new Vector3(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        public static Vector3 operator /(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z);
        }

        public static Vector3 operator /(Vector3 vector, float scalar)
        {
            return new Vector3(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);
        }

        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static explicit operator Vector2(Vector3 value)
        {
            return new Vector2(value);
        }

        public static explicit operator Vector4(Vector3 value)
        {
            return new Vector4(value);
        }

        public static explicit operator Quaternion(Vector3 value)
        {
            return new Quaternion(value);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            return obj is Vector3 vector && Equals(vector);
        }

        public bool Equals(Vector3 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        public override string ToString()
        {
            return $"[Vector3] X({X}) Y({Y}) Z({Z})";
        }

        #endregion

        #region Methods
        
        public float Length()
        {
            return Length(this);
        }
        public static float Length(Vector3 value)
        {
            float lengthSquared = LengthSquared(value);
            return Mathf.Sqrt(lengthSquared);
        }

        public float LengthSquared()
        {
            return LengthSquared(this);
        }
        
        public static float LengthSquared(Vector3 value)
        {
            return Mathf.Pow(value.X, 2) + Mathf.Pow(value.Y, 2) + Mathf.Pow(value.Z, 2);
        }

        public void Normalize()
        {
            Normalize(this);
        }

        public static Vector3 Normalize(Vector3 value)
        {
            float length = 1f / Length(value);
            value.X *= length;
            value.Y *= length;
            value.Z *= length;

            return value;
        }

        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
        }

        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            float x = lhs.Y * rhs.Z - lhs.Z * rhs.Y;
            float y = lhs.Z * rhs.X - lhs.X * rhs.Z;
            float z = lhs.X * rhs.Y - lhs.Y * rhs.X;
            return new Vector3(x, y, z);
        }

        public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
        {
            float x = Mathf.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            float y = Mathf.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
            float z = Mathf.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
            return new Vector3(x, y, z);
        }

        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            float x = Mathf.Clamp(value.X, min.X, max.X);
            float y = Mathf.Clamp(value.Y, min.Y, max.Y);
            float z = Mathf.Clamp(value.Z, min.Z, max.Z);
            return new Vector3(x, y, z);
        }

        public static float Distance(Vector3 lhs, Vector3 rhs)
        {
            float x = lhs.X - rhs.X;
            float y = lhs.Y - rhs.Y;
            float z = lhs.Z - rhs.Z;
            Vector3 result = new Vector3(x, y, z);
            return Length(result);
        }

        public static Vector3 Lerp(Vector3 lhs, Vector3 rhs, float amount)
        {
            float x = Mathf.Lerp(lhs.X, rhs.X, amount);
            float y = Mathf.Lerp(lhs.Y, rhs.Y, amount);
            float z = Mathf.Lerp(lhs.Z, rhs.Z, amount);
            return new Vector3(x, y, z);
        }

        public static Vector3 SmoothStep(Vector3 lhs, Vector3 rhs, float amount)
        {
            float x = Mathf.SmoothStep(lhs.X, rhs.X, amount);
            float y = Mathf.SmoothStep(lhs.Y, rhs.Y, amount);
            float z = Mathf.SmoothStep(lhs.Z, rhs.Z, amount);
            return new Vector3(x, y, z);
        }

        public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
        {
            float x = Mathf.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            float y = Mathf.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            float z = Mathf.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            return new Vector3(x, y, z);
        }

        public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
        {
            float x = Mathf.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            float y = Mathf.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
            float z = Mathf.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
            return new Vector3(x, y, z);
        }

        public static Vector3 Max(Vector3 lhs, Vector3 rhs)
        {
            float x = Mathf.Max(lhs.X, rhs.X);
            float y = Mathf.Max(lhs.Y, rhs.Y);
            float z = Mathf.Max(lhs.Z, rhs.Z);
            return new Vector3(x, y, z);
        }

        public static Vector3 Min(Vector3 lhs, Vector3 rhs)
        {
            float x = Mathf.Min(lhs.X, rhs.X);
            float y = Mathf.Min(lhs.Y, rhs.Y);
            float z = Mathf.Min(lhs.Z, rhs.Z);
            return new Vector3(x, y, z);
        }

        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            float dot = Dot(vector, normal);
            float x = vector.X - 2f * dot * normal.X;
            float y = vector.Y - 2f * dot * normal.Y;
            float z = vector.Z - 2f * dot * normal.Z;
            return new Vector3(x, y, z);
        }

        #endregion
    }
}