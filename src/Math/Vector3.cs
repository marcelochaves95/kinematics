using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kinematics.Math
{
    /// <summary>
    /// Vector3 is an utility class for manipulating 3 dimensional
    /// vectors with float components
    /// </summary>
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

        public Vector3(Vector3 vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
        }

        public Vector3(Vector2 vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = 0.0f;
        }

        public Vector3(Vector4 vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
        }

        public Vector3(Quaternion quaternion)
        {
            X = quaternion.X;
            Y = quaternion.Y;
            Z = quaternion.Z;
        }

        #endregion

        #region Operators

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Vector3 operator *(Vector3 v, float s)
        {
            return new Vector3(v.X * s, v.Y * s, v.Z * s);
        }

        public static Vector3 operator *(float s, Vector3 v)
        {
            return new Vector3(v.X * s, v.Y * s, v.Z * s);
        }

        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static Vector3 operator /(Vector3 v, float s)
        {
            return new Vector3(v.X / s, v.Y / s, v.Z / s);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !v1.Equals(v2);
        }

        public static explicit operator Vector2(Vector3 v)
        {
            return new Vector2(v);
        }

        public static explicit operator Vector4(Vector3 v)
        {
            return new Vector4(v);
        }

        public static explicit operator Quaternion(Vector3 v)
        {
            return new Quaternion(v);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            return obj is Vector3 v && Equals(v);
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

        #region Public Methods

        public static float Length(Vector3 v)
        {
            return Mathf.Sqrt(Mathf.Pow(v.X, 2) + Mathf.Pow(v.Y, 2) + Mathf.Pow(v.Z, 2));
        }

        public float LengthSquared()
        {
            return Mathf.Pow(X, 2) + Mathf.Pow(Y, 2) + Mathf.Pow(Z, 2);
        }

        public void Normalize()
        {
            float val = 1.0f / Length(this);
            X *= val;
            Y *= val;
            Z *= val;
        }

        public static Vector3 Normalize(Vector3 v)
        {
            float val = 1.0f / Length(v);
            v.X *= val;
            v.Y *= val;
            v.Z *= val;
            return v;
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        public static Vector3 Barycentric(Vector3 v1, Vector3 v2, Vector3 v3, float a1, float a2)
        {
            return new Vector3(Mathf.Barycentric(v1.X, v2.X, v3.X, a1, a2),
                Mathf.Barycentric(v1.Y, v2.Y, v3.Y, a1, a2),
                Mathf.Barycentric(v1.Z, v2.Z, v3.Z, a1, a2));
        }

        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            return new Vector3(Mathf.Clamp(value.X, min.X, max.X), Mathf.Clamp(value.Y, min.Y, max.Y), Mathf.Clamp(value.Z, min.Z, max.Z));
        }

        public static float Distance(Vector3 v1, Vector3 v2)
        {
            float x = v1.X - v2.X;
            float y = v1.Y - v2.Y;
            float z = v1.Z - v2.Z;
            Vector3 v3 = new Vector3(x, y, z);

            return Length(v3);
        }

        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
        {
            return new Vector3(Mathf.Lerp(value1.X, value2.X, amount), Mathf.Lerp(value1.Y, value2.Y, amount), Mathf.Lerp(value1.Z, value2.Z, amount));
        }

        public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
        {
            return new Vector3(Mathf.SmoothStep(value1.X, value2.X, amount),
                Mathf.SmoothStep(value1.Y, value2.Y, amount),
                Mathf.SmoothStep(value1.Z, value2.Z, amount));
        }

        public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
        {
            return new Vector3(Mathf.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount),
                Mathf.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount),
                Mathf.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount));
        }

        public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
        {
            return new Vector3(Mathf.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                Mathf.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount),
                Mathf.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount));
        }

        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            return new Vector3(Mathf.Max(value1.X, value2.X),
                Mathf.Max(value1.Y, value2.Y),
                Mathf.Max(value1.Z, value2.Z));
        }

        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            return new Vector3(Mathf.Min(value1.X, value2.X),
                Mathf.Min(value1.Y, value2.Y),
                Mathf.Min(value1.Z, value2.Z));
        }

        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            float dot = Dot(vector, normal);
            return new Vector3(vector.X - 2f * dot * normal.X, vector.Y - 2f * dot * normal.Y, vector.Z - 2f * dot * normal.Z);
        }

        public static Vector3 Refract(Vector3 vector, Vector3 normal, float i)
        {
            float cos1 = Dot(vector, normal);
            float radians = 1f - i * i * (1f - cos1 * cos1);

            if (radians < 0f)
            {
                return Zero;
            }

            float cos2 = Mathf.Sqrt(radians);
            return i * vector + (cos2 - i * cos1) * normal;
        }

        #endregion
    }
}
