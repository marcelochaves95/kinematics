using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kinematics.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion(Quaternion value)
        {
	        X = value.X;
	        Y = value.Y;
	        Z = value.Z;
	        W = value.W;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion(Vector3 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion(Vector4 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = value.W;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Concatenate(Quaternion lhs, Quaternion rhs)
		{
            float x1 = lhs.X;
            float y1 = lhs.Y;
            float z1 = lhs.Z;
            float w1 = lhs.W;

            float x2 = rhs.X;
		    float y2 = rhs.Y;
		    float z2 = rhs.Z;
		    float w2 = rhs.W;

		    float x = x2 * w1 + x1 * w2 + (y2 * z1 - z2 * y1);
		    float y = y2 * w1 + y1 * w2 + (z2 * x1 - x2 * z1);
		    float z = z2 * w1 + z1 * w2 + (x2 * y1 - y2 * x1);
		    float w = w2 * w1 - (x2 * x1 + y2 * y1 + z2 * z1);
		    return new Quaternion(x, y, z, w);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Conjugate()
		{
			X = -X;
			Y = -Y;
			Z = -Z;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Conjugate(Quaternion value)
		{
			return new Quaternion(-value.X, -value.Y, -value.Z, value.W);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
		    float half = angle * 0.5f;
		    float sin = Mathf.Sin(half);
		    float cos = Mathf.Cos(half);
		    return new Quaternion(axis.X * sin, axis.Y * sin, axis.Z * sin, cos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion CreateFromRotationMatrix(Matrix4x4 matrix4X4)
        {
            Quaternion quaternion;
            float sqrt;
            float half;
            float scale = matrix4X4.M11 + matrix4X4.M22 + matrix4X4.M33;
            if (scale > 0f)
		    {
                sqrt = Mathf.Sqrt(scale + 1f);
		        quaternion.W = sqrt * 0.5f;
                sqrt = 0.5f / sqrt;
                quaternion.X = (matrix4X4.M23 - matrix4X4.M32) * sqrt;
		        quaternion.Y = (matrix4X4.M31 - matrix4X4.M13) * sqrt;
		        quaternion.Z = (matrix4X4.M12 - matrix4X4.M21) * sqrt;
		        return quaternion;
		    }

		    if (matrix4X4.M11 >= matrix4X4.M22 && matrix4X4.M11 >= matrix4X4.M33)
		    {
                sqrt = Mathf.Sqrt(1f + matrix4X4.M11 - matrix4X4.M22 - matrix4X4.M33);
                half = 0.5f / sqrt;
                quaternion.X = 0.5f * sqrt;
		        quaternion.Y = (matrix4X4.M12 + matrix4X4.M21) * half;
		        quaternion.Z = (matrix4X4.M13 + matrix4X4.M31) * half;
		        quaternion.W = (matrix4X4.M23 - matrix4X4.M32) * half;
		        return quaternion;
		    }

		    if (matrix4X4.M22 > matrix4X4.M33)
		    {
                sqrt = Mathf.Sqrt(1f + matrix4X4.M22 - matrix4X4.M11 - matrix4X4.M33);
                half = 0.5f / sqrt;
                quaternion.X = (matrix4X4.M21 + matrix4X4.M12) * half;
		        quaternion.Y = 0.5f * sqrt;
		        quaternion.Z = (matrix4X4.M32 + matrix4X4.M23) * half;
		        quaternion.W = (matrix4X4.M31 - matrix4X4.M13) * half;
		        return quaternion;
		    }

            sqrt = Mathf.Sqrt(1f + matrix4X4.M33 - matrix4X4.M11 - matrix4X4.M22);
		    half = 0.5f / sqrt;
		    quaternion.X = (matrix4X4.M31 + matrix4X4.M13) * half;
		    quaternion.Y = (matrix4X4.M32 + matrix4X4.M23) * half;
		    quaternion.Z = 0.5f * sqrt;
		    quaternion.W = (matrix4X4.M12 - matrix4X4.M21) * half;
		    return quaternion;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
            float halfRoll = roll * 0.5f;
            float halfPitch = pitch * 0.5f;
            float halfYaw = yaw * 0.5f;

            float sinRoll = Mathf.Sin(halfRoll);
            float cosRoll = Mathf.Cos(halfRoll);
            float sinPitch = Mathf.Sin(halfPitch);
            float cosPitch = Mathf.Cos(halfPitch);
            float sinYaw = Mathf.Sin(halfYaw);
            float cosYaw = Mathf.Cos(halfYaw);

            float x = cosYaw * sinPitch * cosRoll + sinYaw * cosPitch * sinRoll;
            float y = sinYaw * cosPitch * cosRoll - cosYaw * sinPitch * sinRoll;
            float z = cosYaw * cosPitch * sinRoll - sinYaw * sinPitch * cosRoll;
            float w = cosYaw * cosPitch * cosRoll + sinYaw * sinPitch * sinRoll;
            return new Quaternion(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Quaternion lhs, Quaternion rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z + lhs.W * rhs.W;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Inverse(Quaternion value)
        {
		    float lengthSquared = 1f / LengthSquared(value);
		    float x = -value.X * lengthSquared;
		    float y = -value.Y * lengthSquared;
		    float z = -value.Z * lengthSquared;
		    float w = value.W * lengthSquared;
		    return new Quaternion(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Length()
        {
	        return Length(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Length(Quaternion value)
        {
	        float lengthSquared = LengthSquared(value);
	        return Mathf.Sqrt(lengthSquared);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float LengthSquared()
        {
            return LengthSquared(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LengthSquared(Quaternion value)
        {
	        return Mathf.Pow(value.X, 2) + Mathf.Pow(value.Y, 2) + Mathf.Pow(value.Z, 2) + Mathf.Pow(value.W, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Lerp(Quaternion lhs, Quaternion rhs, float amount)
        {
            float num = amount;
		    float num2 = 1f - num;
		    Quaternion quaternion;
		    float dot = Dot(lhs, rhs);
		    if (dot >= 0f)
		    {
		        quaternion.X = num2 * lhs.X + num * rhs.X;
		        quaternion.Y = num2 * lhs.Y + num * rhs.Y;
		        quaternion.Z = num2 * lhs.Z + num * rhs.Z;
		        quaternion.W = num2 * lhs.W + num * rhs.W;
		    }
		    else
		    {
		        quaternion.X = num2 * lhs.X - num * rhs.X;
		        quaternion.Y = num2 * lhs.Y - num * rhs.Y;
		        quaternion.Z = num2 * lhs.Z - num * rhs.Z;
		        quaternion.W = num2 * lhs.W - num * rhs.W;
		    }

		    return Normalize(quaternion);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Slerp(Quaternion lhs, Quaternion rhs, float amount)
        {
	        float num = amount;
            float num2;
		    float num3;
		    float dot = Dot(lhs, rhs);
		    bool flag = false;
		    if (dot < 0f)
		    {
		        flag = true;
		        dot = -dot;
		    }

		    if (dot > 0.999999f)
		    {
		        num3 = 1f - num;
		        num2 = flag ? -num : num;
		    }
		    else
		    {
		        float num5 = Mathf.Acos(dot);
		        float num6 = 1f / Mathf.Sin(num5);
		        num3 = Mathf.Sin((1f - num) * num5) * num6;
		        num2 = Mathf.Sin(num * num5) * num6;
		    }

		    float x = num3 * lhs.X + num2 * rhs.X;
		    float y = num3 * lhs.Y + num2 * rhs.Y;
		    float z = num3 * lhs.Z + num2 * rhs.Z;
		    float w = num3 * lhs.W + num2 * rhs.W;
		    return new Quaternion(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
	        Normalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Normalize(Quaternion value)
        {
	        float length = 1f / Length(value);
		    value.X *= length;
		    value.Y *= length;
		    value.Z *= length;
		    value.W *= length;
		    return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator +(Quaternion lhs, Quaternion rhs)
        {
	        return new Quaternion(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator -(Quaternion lhs, Quaternion rhs)
        {
	        return new Quaternion(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator -(Quaternion value)
        {
	        return new Quaternion(-value.X, -value.Y, -value.Z, -value.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
	        float x = lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y;
	        float y = lhs.W * rhs.Y + lhs.Y * rhs.W + lhs.Z * rhs.X - lhs.X * rhs.Z;
	        float z = lhs.W * rhs.Z + lhs.Z * rhs.W + lhs.X * rhs.Y - lhs.Y * rhs.X;
	        float w = lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z;
	        return new Quaternion(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator *(Quaternion quaternion, float scalar)
        {
	        return new Quaternion(quaternion.X * scalar, quaternion.Y * scalar, quaternion.Z * scalar, quaternion.W * scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator *(float scalar, Quaternion quaternion)
        {
	        return new Quaternion(quaternion.X * scalar, quaternion.Y * scalar, quaternion.Z * scalar, quaternion.W * scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator /(Quaternion lhs, Quaternion rhs)
        {
	        float x = lhs.W / rhs.X + lhs.X / rhs.W + lhs.Y / rhs.Z - lhs.Z / rhs.Y;
	        float y = lhs.W / rhs.Y + lhs.Y / rhs.W + lhs.Z / rhs.X - lhs.X / rhs.Z;
	        float z = lhs.W / rhs.Z + lhs.Z / rhs.W + lhs.X / rhs.Y - lhs.Y / rhs.X;
	        float w = lhs.W / rhs.W - lhs.X / rhs.X - lhs.Y / rhs.Y - lhs.Z / rhs.Z;
	        return new Quaternion(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator /(Quaternion quaternion, float scalar)
        {
	        return new Quaternion(quaternion.X / scalar, quaternion.Y / scalar, quaternion.Z / scalar, quaternion.W / scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Quaternion lhs, Quaternion rhs)
        {
	        return lhs.Equals(rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Quaternion lhs, Quaternion rhs)
        {
	        return !lhs.Equals(rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3(Quaternion value)
        {
	        return new Vector3(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector4(Quaternion value)
        {
	        return new Vector4(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
	        return obj is Quaternion quaternion && Equals(quaternion);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Quaternion other)
        {
	        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
	        return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
	        return $"[Quaternion] X({X}) Y({Y}) Z({Z}) W({W})";
        }
    }
}