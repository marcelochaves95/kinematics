using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kinematics.Math
{ 
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M21;
        public float M22;
        public float M23;
        public float M24;
        public float M31;
        public float M32;
        public float M33;
        public float M34;
        public float M41;
        public float M42;
        public float M43;
        public float M44;

        public readonly Vector3 Forward => new Vector3(-M31, -M32, -M33);
        public readonly Vector3 Backward => new Vector3(M31, M32, M33);
        public readonly Vector3 Up => new Vector3(M21, M22, M23);
        public readonly Vector3 Down => new Vector3(-M21, -M22, -M23);
        public readonly Vector3 Left => new Vector3(-M11, -M12, -M13);
        public readonly Vector3 Right => new Vector3(M11, M12, M13);
        public readonly Vector3 Translation => new Vector3(M41, M42, M43);
        public static Matrix4x4 Identity => new Matrix4x4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31,
                      float m32, float m33, float m34, float m41, float m42, float m43, float m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4(Vector4 row1, Vector4 row2, Vector4 row3, Vector4 row4)
        {
            M11 = row1.X;
            M12 = row1.Y;
            M13 = row1.Z;
            M14 = row1.W;
            M21 = row2.X;
            M22 = row2.Y;
            M23 = row2.Z;
            M24 = row2.W;
            M31 = row3.X;
            M32 = row3.Y;
            M33 = row3.Z;
            M34 = row3.W;
            M41 = row4.X;
            M42 = row4.Y;
            M43 = row4.Z;
            M44 = row4.W;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3? cameraForwardVector)
        {
            Vector3 vector;
            vector.X = objectPosition.X - cameraPosition.X;
            vector.Y = objectPosition.Y - cameraPosition.Y;
            vector.Z = objectPosition.Z - cameraPosition.Z;
            float magnitudeSquared = vector.MagnitudeSquared();
            if (magnitudeSquared < Mathf.Epsilon)
            {
	            vector = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            }
            else
            {
	            vector *= 1f / Mathf.Sqrt(magnitudeSquared);
            }

            Vector3 vector3 = Vector3.Cross(cameraUpVector, vector);
            vector3.Normalize();
            Vector3 vector2 = Vector3.Cross(vector, vector3);

            Matrix4x4 result;
            result.M11 = vector3.X;
            result.M12 = vector3.Y;
            result.M13 = vector3.Z;
            result.M14 = 0f;
            result.M21 = vector2.X;
            result.M22 = vector2.Y;
            result.M23 = vector2.Z;
            result.M24 = 0f;
            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M34 = 0f;
            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1f;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateConstrainedBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector)
        {
            Vector3 vector;
		    Vector3 vector2;
		    Vector3 vector3;
		    vector2.X = objectPosition.X - cameraPosition.X;
		    vector2.Y = objectPosition.Y - cameraPosition.Y;
		    vector2.Z = objectPosition.Z - cameraPosition.Z;
		    float magnitudeSquared = vector2.MagnitudeSquared();
		    if (magnitudeSquared < Mathf.Epsilon)
		    {
		        vector2 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
		    }
		    else
		    {
			    vector2 *= 1f / Mathf.Sqrt(magnitudeSquared);
		    }

		    Vector3 vector4 = rotateAxis;
		    float num = Vector3.Dot(rotateAxis, vector2);
		    const float minAbs = 0.9982547f;
		    if (Mathf.Abs(num) > minAbs)
		    {
		        if (objectForwardVector.HasValue)
		        {
		            vector = objectForwardVector.Value;
		            num = Vector3.Dot(rotateAxis, vector);
		            if (Mathf.Abs(num) > minAbs)
		            {
		                num = rotateAxis.X * Vector3.Forward.X + rotateAxis.Y * Vector3.Forward.Y + rotateAxis.Z * Vector3.Forward.Z;
		                vector = Mathf.Abs(num) > minAbs ? Vector3.Right : Vector3.Forward;
		            }
		        }
		        else
		        {
		            num = rotateAxis.X * Vector3.Forward.X + rotateAxis.Y * Vector3.Forward.Y + rotateAxis.Z * Vector3.Forward.Z;
		            vector = Mathf.Abs(num) > minAbs ? Vector3.Right : Vector3.Forward;
		        }
		        vector3 = Vector3.Cross(rotateAxis, vector);
		        vector3.Normalize();
		        vector = Vector3.Cross(vector3, rotateAxis);
		        vector.Normalize();
		    }
		    else
		    {
			    vector3 = Vector3.Cross(rotateAxis, vector2);
		        vector3.Normalize();
		        vector = Vector3.Cross(vector3, vector4);
		        vector.Normalize();
		    }

		    Matrix4x4 result;
		    result.M11 = vector3.X;
		    result.M12 = vector3.Y;
		    result.M13 = vector3.Z;
		    result.M14 = 0f;
		    result.M21 = vector4.X;
		    result.M22 = vector4.Y;
		    result.M23 = vector4.Z;
		    result.M24 = 0f;
		    result.M31 = vector.X;
		    result.M32 = vector.Y;
		    result.M33 = vector.Z;
		    result.M34 = 0f;
		    result.M41 = objectPosition.X;
		    result.M42 = objectPosition.Y;
		    result.M43 = objectPosition.Z;
		    result.M44 = 1f;
		    return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateFromAxisAngle(Vector3 axis, float angle)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float num2 = Mathf.Sin(angle);
            float num = Mathf.Cos(angle);
            float num11 = x * x;
            float num10 = y * y;
            float num9 = z * z;
            float num8 = x * y;
            float num7 = x * z;
            float num6 = y * z;

            Matrix4x4 result;
            result.M11 = num11 + num * (1f - num11);
            result.M12 = num8 - num * num8 + num2 * z;
            result.M13 = num7 - num * num7 - num2 * y;
            result.M14 = 0f;
            result.M21 = num8 - num * num8 - num2 * z;
            result.M22 = num10 + num * (1f - num10);
            result.M23 = num6 - num * num6 + num2 * x;
            result.M24 = 0f;
            result.M31 = num7 - num * num7 + num2 * y;
            result.M32 = num6 - num * num6 - num2 * x;
            result.M33 = num9 + num * (1f - num9);
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateFromQuaternion(Quaternion value)
        {
            float num9 = value.X * value.X;
            float num8 = value.Y * value.Y;
            float num7 = value.Z * value.Z;
            float num6 = value.X * value.Y;
            float num5 = value.Z * value.W;
            float num4 = value.Z * value.X;
            float num3 = value.Y * value.W;
            float num2 = value.Y * value.Z;
            float num = value.X * value.W;

            Matrix4x4 result;
            result.M11 = 1f - 2f * (num8 + num7);
            result.M12 = 2f * (num6 + num5);
            result.M13 = 2f * (num4 - num3);
            result.M14 = 0f;
            result.M21 = 2f * (num6 - num5);
            result.M22 = 1f - 2f * (num7 + num9);
            result.M23 = 2f * (num2 + num);
            result.M24 = 0f;
            result.M31 = 2f * (num4 + num3);
            result.M32 = 2f * (num2 - num);
            result.M33 = 1f - 2f * (num8 + num9);
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix4x4 CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
			Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
			Matrix4x4 result = CreateFromQuaternion(quaternion);
		    return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            Vector3 vector = Vector3.Normalize(cameraPosition - cameraTarget);
            Vector3 vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
            Vector3 vector3 = Vector3.Cross(vector, vector2);

            Matrix4x4 result;
            result.M11 = vector2.X;
            result.M12 = vector3.X;
            result.M13 = vector.X;
            result.M14 = 0f;
            result.M21 = vector2.Y;
            result.M22 = vector3.Y;
            result.M23 = vector.Y;
            result.M24 = 0f;
            result.M31 = vector2.Z;
            result.M32 = vector3.Z;
            result.M33 = vector.Z;
            result.M34 = 0f;
            result.M41 = -Vector3.Dot(vector2, cameraPosition);
            result.M42 = -Vector3.Dot(vector3, cameraPosition);
            result.M43 = -Vector3.Dot(vector, cameraPosition);
            result.M44 = 1f;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
        {
            Matrix4x4 result;
            result.M11 = 2f / width;
            result.M12 = result.M13 = result.M14 = 0f;
            result.M22 = 2f / height;
            result.M21 = result.M23 = result.M24 = 0f;
            result.M33 = 1f / (zNearPlane - zFarPlane);
            result.M31 = result.M32 = result.M34 = 0f;
            result.M41 = result.M42 = 0f;
            result.M43 = zNearPlane / (zNearPlane - zFarPlane);
            result.M44 = 1f;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
        {
			Matrix4x4 result;
			result.M11 = 2f / (right - left);
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = 2f / (top - bottom);
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f / (zNearPlane - zFarPlane);
			result.M34 = 0f;
			result.M41 = (left + right) / (left - right);
			result.M42 = (top + bottom) / (bottom - top);
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1f;
			return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
        {
            if (nearPlaneDistance <= 0f)
            {
	            throw new ArgumentException("nearPlaneDistance <= 0");
            }

            if (farPlaneDistance <= 0f)
            {
	            throw new ArgumentException("farPlaneDistance <= 0");
            }

            if (nearPlaneDistance >= farPlaneDistance)
            {
	            throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }

            Matrix4x4 result;
            result.M11 = 2f * nearPlaneDistance / width;
            result.M12 = result.M13 = result.M14 = 0f;
            result.M22 = 2f * nearPlaneDistance / height;
            result.M21 = result.M23 = result.M24 = 0f;
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M31 = result.M32 = 0f;
            result.M34 = -1f;
            result.M41 = result.M42 = result.M44 = 0f;
            result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        {
            if (fieldOfView <= 0f || fieldOfView >= 3.141593f)
            {
	            throw new ArgumentException("fieldOfView <= 0 or >= PI");
            }

            if (nearPlaneDistance <= 0f)
            {
	            throw new ArgumentException("nearPlaneDistance <= 0");
            }
            if (farPlaneDistance <= 0f)
            {
	            throw new ArgumentException("farPlaneDistance <= 0");
            }

            if (nearPlaneDistance >= farPlaneDistance)
            {
	            throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }

            float num = 1f / Mathf.Tan(fieldOfView * 0.5f);
            float num9 = num / aspectRatio;
            Matrix4x4 result;
            result.M11 = num9;
            result.M12 = result.M13 = result.M14 = 0f;
            result.M22 = num;
            result.M21 = result.M23 = result.M24 = 0f;
            result.M31 = result.M32 = 0f;
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1f;
            result.M41 = result.M42 = result.M44 = 0f;
            result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
        {
            if (nearPlaneDistance <= 0f)
            {
	            throw new ArgumentException("nearPlaneDistance <= 0");
            }

            if (farPlaneDistance <= 0f)
            {
	            throw new ArgumentException("farPlaneDistance <= 0");
            }

            if (nearPlaneDistance >= farPlaneDistance)
            {
	            throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }

            Matrix4x4 result;
            result.M11 = 2f * nearPlaneDistance / (right - left);
            result.M12 = result.M13 = result.M14 = 0f;
            result.M22 = 2f * nearPlaneDistance / (top - bottom);
            result.M21 = result.M23 = result.M24 = 0f;
            result.M31 = (left + right) / (right - left);
            result.M32 = (top + bottom) / (top - bottom);
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1;
            result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M41 = result.M42 = result.M44 = 0f;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateRotationX(float radians)
        {
	        Matrix4x4 result = Identity;

            float val1 = Mathf.Cos(radians);
            float val2 = Mathf.Sin(radians);

            result.M22 = val1;
            result.M23 = val2;
            result.M32 = -val2;
            result.M33 = val1;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateRotationY(float radians)
        {
	        Matrix4x4 result = Identity;

            float val1 = Mathf.Cos(radians);
            float val2 = Mathf.Sin(radians);

            result.M11 = val1;
            result.M13 = -val2;
            result.M31 = val2;
            result.M33 = val1;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateRotationZ(float radians)
        {
	        Matrix4x4 result = Identity;

            float val1 = Mathf.Cos(radians);
            float val2 = Mathf.Sin(radians);

            result.M11 = val1;
            result.M12 = val2;
            result.M21 = -val2;
            result.M22 = val1;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateScale(float x, float y, float z)
        {
	        Matrix4x4 result;
	        result.M11 = x;
	        result.M12 = 0f;
	        result.M13 = 0f;
	        result.M14 = 0f;
	        result.M21 = 0f;
	        result.M22 = y;
	        result.M23 = 0f;
	        result.M24 = 0f;
	        result.M31 = 0f;
	        result.M32 = 0f;
	        result.M33 = z;
	        result.M34 = 0f;
	        result.M41 = 0f;
	        result.M42 = 0f;
	        result.M43 = 0f;
	        result.M44 = 1f;
	        return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateScale(Vector3 scales)
        {
            Matrix4x4 result;
            result.M11 = scales.X;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = scales.Y;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scales.Z;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTranslation(float x, float y, float z)
        {
            Matrix4x4 result;
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;
            result.M44 = 1f;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTranslation(Vector3 position)
        {
			return CreateTranslation(position.X, position.Y, position.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Decompose(Vector3 scale, Vector3 translation)
        {
            translation.X = M41;
            translation.Y = M42;
            translation.Z = M43;

            float xs = Mathf.Sign(M11 * M12 * M13 * M14) < 0f ? -1f : 1f;
            float ys = Mathf.Sign(M21 * M22 * M23 * M24) < 0f ? -1f : 1f;
            float zs = Mathf.Sign(M31 * M32 * M33 * M34) < 0f ? -1f : 1f;

            scale.X = xs * Mathf.Sqrt(M11 * M11 + M12 * M12 + M13 * M13);
            scale.Y = ys * Mathf.Sqrt(M21 * M21 + M22 * M22 + M23 * M23);
            scale.Z = zs * Mathf.Sqrt(M31 * M31 + M32 * M32 + M33 * M33);

            if (Mathf.Abs(scale.X) < Mathf.Epsilon || Mathf.Abs(scale.Y) < Mathf.Epsilon || Mathf.Abs(scale.Z) < Mathf.Epsilon)
            {
	            return false;
            }

            Matrix4x4 m1 = new Matrix4x4(M11 / scale.X, M12 / scale.X, M13 / scale.X, 0f,
                                   M21 / scale.Y, M22 / scale.Y, M23 / scale.Y, 0f,
                                   M31 / scale.Z, M32 / scale.Z, M33 / scale.Z, 0f,
                                   0f, 0f, 0f, 1f);

            Quaternion.CreateFromRotationMatrix(m1);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Determinant()
        {
            float num22 = M11;
		    float num21 = M12;
		    float num20 = M13;
		    float num19 = M14;
		    float num12 = M21;
		    float num11 = M22;
		    float num10 = M23;
		    float num9 = M24;
		    float num8 = M31;
		    float num7 = M32;
		    float num6 = M33;
		    float num5 = M34;
		    float num4 = M41;
		    float num3 = M42;
		    float num2 = M43;
		    float num = M44;
		    float num18 = num6 * num - num5 * num2;
		    float num17 = num7 * num - num5 * num3;
		    float num16 = num7 * num2 - num6 * num3;
		    float num15 = num8 * num - num5 * num4;
		    float num14 = num8 * num2 - num6 * num4;
		    float num13 = num8 * num3 - num7 * num4;
		    return num22 * (num11 * num18 - num10 * num17 + num9 * num16) - num21 * (num12 * num18 - num10 * num15 + num9 * num14) + num20 *
		           (num12 * num17 - num11 * num15 + num9 * num13) - num19 * (num12 * num16 - num11 * num14 + num10 * num13);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Invert(Matrix4x4 value)
        {
            float num1 = value.M11;
			float num2 = value.M12;
			float num3 = value.M13;
			float num4 = value.M14;
			float num5 = value.M21;
			float num6 = value.M22;
			float num7 = value.M23;
			float num8 = value.M24;
			float num9 = value.M31;
			float num10 = value.M32;
			float num11 = value.M33;
			float num12 = value.M34;
			float num13 = value.M41;
			float num14 = value.M42;
			float num15 = value.M43;
			float num16 = value.M44;
			float num17 = num11 * num16 - num12 * num15;
			float num18 = num10 * num16 - num12 * num14;
			float num19 = num10 * num15 - num11 * num14;
			float num20 = num9 * num16 - num12 * num13;
			float num21 = num9 * num15 - num11 * num13;
			float num22 = num9 * num14 - num10 * num13;
			float num23 = num6 * num17 - num7 * num18 + num8 * num19;
			float num24 = -(num5 * num17 - num7 * num20 + num8 * num21);
			float num25 = num5 * num18 - num6 * num20 + num8 *  num22;
			float num26 = -(num5 * num19 - num6 * num21 + num7 * num22);
			float num27 = 1f / (num1 * num23 + num2 * num24 + num3 * num25 + num4 * num26);

			Matrix4x4 result;
			result.M11 = num23 * num27;
			result.M21 = num24 * num27;
			result.M31 = num25 * num27;
			result.M41 = num26 * num27;
			result.M12 = -(num2 * num17 - num3 * num18 + num4 * num19) * num27;
			result.M22 = (num1 * num17 - num3 * num20 + num4 * num21) * num27;
			result.M32 = -(num1 * num18 - num2 * num20 + num4 * num22) * num27;
			result.M42 = (num1 * num19 - num2 * num21 + num3 * num22) * num27;
			float num28 = num7 * num16 - num8 * num15;
			float num29 = num6 * num16 - num8 * num14;
			float num30 = num6 * num15 - num7 * num14;
			float num31 = num5 * num16 - num8 * num13;
			float num32 = num5 * num15 - num7 * num13;
			float num33 = num5 * num14 - num6 * num13;
			result.M13 = (num2 * num28 - num3 * num29 + num4 * num30) * num27;
			result.M23 = -(num1 * num28 - num3 * num31 + num4 * num32) * num27;
			result.M33 = (num1 * num29 - num2 * num31 + num4 * num33) * num27;
			result.M43 = -(num1 * num30 - num2 * num32 + num3 * num33) * num27;
			float num34 = num7 * num12 - num8 * num11;
			float num35 = num6 * num12 - num8 * num10;
			float num36 = num6 * num11 - num7 * num10;
			float num37 = num5 * num12 - num8 * num9;
			float num38 = num5 * num11 - num7 * num9;
			float num39 = num5 * num10 - num6 * num9;
			result.M14 = -(num2 * num34 - num3 * num35 + num4 * num36) * num27;
			result.M24 = (num1 * num34 - num3 * num37 + num4 * num38) * num27;
			result.M34 = -(num1 * num35 - num2 * num37 + num4 * num39) * num27;
			result.M44 = (num1 * num36 - num2 * num38 + num3 * num39) * num27;
			return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Lerp(Matrix4x4 lhs, Matrix4x4 rhs, float amount)
        {
		    lhs.M11 += (rhs.M11 - lhs.M11) * amount;
		    lhs.M12 += (rhs.M12 - lhs.M12) * amount;
		    lhs.M13 += (rhs.M13 - lhs.M13) * amount;
		    lhs.M14 += (rhs.M14 - lhs.M14) * amount;
		    lhs.M21 += (rhs.M21 - lhs.M21) * amount;
		    lhs.M22 += (rhs.M22 - lhs.M22) * amount;
		    lhs.M23 += (rhs.M23 - lhs.M23) * amount;
		    lhs.M24 += (rhs.M24 - lhs.M24) * amount;
		    lhs.M31 += (rhs.M31 - lhs.M31) * amount;
		    lhs.M32 += (rhs.M32 - lhs.M32) * amount;
		    lhs.M33 += (rhs.M33 - lhs.M33) * amount;
		    lhs.M34 += (rhs.M34 - lhs.M34) * amount;
		    lhs.M41 += (rhs.M41 - lhs.M41) * amount;
		    lhs.M42 += (rhs.M42 - lhs.M42) * amount;
		    lhs.M43 += (rhs.M43 - lhs.M43) * amount;
		    lhs.M44 += (rhs.M44 - lhs.M44) * amount;
		    return lhs;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Transpose(Matrix4x4 value)
        {
            Matrix4x4 result;
            result.M11 = value.M11;
            result.M12 = value.M21;
            result.M13 = value.M31;
            result.M14 = value.M41;
            result.M21 = value.M12;
            result.M22 = value.M22;
            result.M23 = value.M32;
            result.M24 = value.M42;
            result.M31 = value.M13;
            result.M32 = value.M23;
            result.M33 = value.M33;
            result.M34 = value.M43;
            result.M41 = value.M14;
            result.M42 = value.M24;
            result.M43 = value.M34;
            result.M44 = value.M44;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator +(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            lhs.M11 += rhs.M11;
            lhs.M12 += rhs.M12;
            lhs.M13 += rhs.M13;
            lhs.M14 += rhs.M14;
            lhs.M21 += rhs.M21;
            lhs.M22 += rhs.M22;
            lhs.M23 += rhs.M23;
            lhs.M24 += rhs.M24;
            lhs.M31 += rhs.M31;
            lhs.M32 += rhs.M32;
            lhs.M33 += rhs.M33;
            lhs.M34 += rhs.M34;
            lhs.M41 += rhs.M41;
            lhs.M42 += rhs.M42;
            lhs.M43 += rhs.M43;
            lhs.M44 += rhs.M44;
            return lhs;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator -(Matrix4x4 lhs, Matrix4x4 rhs)
        {
	        lhs.M11 -= rhs.M11;
	        lhs.M12 -= rhs.M12;
	        lhs.M13 -= rhs.M13;
	        lhs.M14 -= rhs.M14;
	        lhs.M21 -= rhs.M21;
	        lhs.M22 -= rhs.M22;
	        lhs.M23 -= rhs.M23;
	        lhs.M24 -= rhs.M24;
	        lhs.M31 -= rhs.M31;
	        lhs.M32 -= rhs.M32;
	        lhs.M33 -= rhs.M33;
	        lhs.M34 -= rhs.M34;
	        lhs.M41 -= rhs.M41;
	        lhs.M42 -= rhs.M42;
	        lhs.M43 -= rhs.M43;
	        lhs.M44 -= rhs.M44;
	        return lhs;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator -(Matrix4x4 value)
        {
	        value.M11 = -value.M11;
	        value.M12 = -value.M12;
	        value.M13 = -value.M13;
	        value.M14 = -value.M14;
	        value.M21 = -value.M21;
	        value.M22 = -value.M22;
	        value.M23 = -value.M23;
	        value.M24 = -value.M24;
	        value.M31 = -value.M31;
	        value.M32 = -value.M32;
	        value.M33 = -value.M33;
	        value.M34 = -value.M34;
	        value.M41 = -value.M41;
	        value.M42 = -value.M42;
	        value.M43 = -value.M43;
	        value.M44 = -value.M44;
	        return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
        {
	        lhs.M11 = lhs.M11 * rhs.M11 + lhs.M12 * rhs.M21 + lhs.M13 * rhs.M31 + lhs.M14 * rhs.M41;
	        lhs.M12 = lhs.M11 * rhs.M12 + lhs.M12 * rhs.M22 + lhs.M13 * rhs.M32 + lhs.M14 * rhs.M42;
	        lhs.M13 = lhs.M11 * rhs.M13 + lhs.M12 * rhs.M23 + lhs.M13 * rhs.M33 + lhs.M14 * rhs.M43;
	        lhs.M14 = lhs.M11 * rhs.M14 + lhs.M12 * rhs.M24 + lhs.M13 * rhs.M34 + lhs.M14 * rhs.M44;
	        lhs.M21 = lhs.M21 * rhs.M11 + lhs.M22 * rhs.M21 + lhs.M23 * rhs.M31 + lhs.M24 * rhs.M41;
	        lhs.M22 = lhs.M21 * rhs.M12 + lhs.M22 * rhs.M22 + lhs.M23 * rhs.M32 + lhs.M24 * rhs.M42;
	        lhs.M23 = lhs.M21 * rhs.M13 + lhs.M22 * rhs.M23 + lhs.M23 * rhs.M33 + lhs.M24 * rhs.M43;
	        lhs.M24 = lhs.M21 * rhs.M14 + lhs.M22 * rhs.M24 + lhs.M23 * rhs.M34 + lhs.M24 * rhs.M44;
	        lhs.M31 = lhs.M31 * rhs.M11 + lhs.M32 * rhs.M21 + lhs.M33 * rhs.M31 + lhs.M34 * rhs.M41;
	        lhs.M32 = lhs.M31 * rhs.M12 + lhs.M32 * rhs.M22 + lhs.M33 * rhs.M32 + lhs.M34 * rhs.M42;
	        lhs.M33 = lhs.M31 * rhs.M13 + lhs.M32 * rhs.M23 + lhs.M33 * rhs.M33 + lhs.M34 * rhs.M43;
	        lhs.M34 = lhs.M31 * rhs.M14 + lhs.M32 * rhs.M24 + lhs.M33 * rhs.M34 + lhs.M34 * rhs.M44;
	        lhs.M41 = lhs.M41 * rhs.M11 + lhs.M42 * rhs.M21 + lhs.M43 * rhs.M31 + lhs.M44 * rhs.M41;
	        lhs.M42 = lhs.M41 * rhs.M12 + lhs.M42 * rhs.M22 + lhs.M43 * rhs.M32 + lhs.M44 * rhs.M42;
	        lhs.M43 = lhs.M41 * rhs.M13 + lhs.M42 * rhs.M23 + lhs.M43 * rhs.M33 + lhs.M44 * rhs.M43;
	        lhs.M44 = lhs.M41 * rhs.M14 + lhs.M42 * rhs.M24 + lhs.M43 * rhs.M34 + lhs.M44 * rhs.M44;
			return lhs;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator *(Matrix4x4 matrix, float scalar)
        {
		    matrix.M11 *= scalar;
		    matrix.M12 *= scalar;
		    matrix.M13 *= scalar;
		    matrix.M14 *= scalar;
		    matrix.M21 *= scalar;
		    matrix.M22 *= scalar;
		    matrix.M23 *= scalar;
		    matrix.M24 *= scalar;
		    matrix.M31 *= scalar;
		    matrix.M32 *= scalar;
		    matrix.M33 *= scalar;
		    matrix.M34 *= scalar;
		    matrix.M41 *= scalar;
		    matrix.M42 *= scalar;
		    matrix.M43 *= scalar;
		    matrix.M44 *= scalar;
		    return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator /(Matrix4x4 lhs, Matrix4x4 rhs)
        {
		    lhs.M11 /= rhs.M11;
		    lhs.M12 /= rhs.M12;
		    lhs.M13 /= rhs.M13;
		    lhs.M14 /= rhs.M14;
		    lhs.M21 /= rhs.M21;
		    lhs.M22 /= rhs.M22;
		    lhs.M23 /= rhs.M23;
		    lhs.M24 /= rhs.M24;
		    lhs.M31 /= rhs.M31;
		    lhs.M32 /= rhs.M32;
		    lhs.M33 /= rhs.M33;
		    lhs.M34 /= rhs.M34;
		    lhs.M41 /= rhs.M41;
		    lhs.M42 /= rhs.M42;
		    lhs.M43 /= rhs.M43;
		    lhs.M44 /= rhs.M44;
		    return lhs;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator /(Matrix4x4 matrix, float scalar)
        {
		    float num = 1f / scalar;
		    matrix.M11 *= num;
		    matrix.M12 *= num;
		    matrix.M13 *= num;
		    matrix.M14 *= num;
		    matrix.M21 *= num;
		    matrix.M22 *= num;
		    matrix.M23 *= num;
		    matrix.M24 *= num;
		    matrix.M31 *= num;
		    matrix.M32 *= num;
		    matrix.M33 *= num;
		    matrix.M34 *= num;
		    matrix.M41 *= num;
		    matrix.M42 *= num;
		    matrix.M43 *= num;
		    matrix.M44 *= num;
		    return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
        {
	        return lhs.Equals(rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
        {
	        return !lhs.Equals(rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			return obj is Matrix4x4 matrix && Equals(matrix);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Matrix4x4 other)
        {
	        return M11.Equals(other.M11) &&
	               M22.Equals(other.M22) &&
	               M33.Equals(other.M33) &&
	               M44.Equals(other.M44) &&
	               M12.Equals(other.M12) &&
	               M13.Equals(other.M13) &&
	               M14.Equals(other.M14) &&
	               M21.Equals(other.M21) &&
	               M23.Equals(other.M23) &&
	               M24.Equals(other.M24) &&
	               M31.Equals(other.M31) &&
	               M32.Equals(other.M32) &&
	               M34.Equals(other.M34) &&
	               M41.Equals(other.M41) &&
	               M42.Equals(other.M42) &&
	               M43.Equals(other.M43);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return M11.GetHashCode() +
                   M12.GetHashCode() +
                   M13.GetHashCode() +
                   M14.GetHashCode() +
                   M21.GetHashCode() +
                   M22.GetHashCode() +
                   M23.GetHashCode() +
                   M24.GetHashCode() +
                   M31.GetHashCode() +
                   M32.GetHashCode() +
                   M33.GetHashCode() +
                   M34.GetHashCode() +
                   M41.GetHashCode() +
                   M42.GetHashCode() +
                   M43.GetHashCode() +
                   M44.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
	        return $"[Matrix4x4] M11({M11}) M12({M12}) M13({M13}) M14({M14})" +
	               $"M21({M21}) M22({M22}) M23({M23}) M24({M24})" +
	               $"M31({M31}) M32({M32}) M33({M33}) M34({M34})" +
	               $"M41({M41}) M42({M42}) M43({M43}) M44({M44})";
        }
    }
}