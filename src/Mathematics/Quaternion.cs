using System;
using System.Runtime.InteropServices;

namespace Kinematics.Mathematics
{
    /// <summary>
    /// Represents a four dimensional mathematical quaternion
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        /// <summary>X component of the Quaternion. Don't modify this directly unless you know quaternions inside out</summary>
        public float X { get; set; }

        /// <summary>Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out</summary>
        public float Y { get; set; }

        /// <summary>Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out</summary>
        public float Z { get; set; }

        /// <summary>W component of the Quaternion. Don't modify this directly unless you know quaternions inside out</summary>
        public float W { get; set; }

        /// <summary>
        /// Construct the quaternion from it's coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <param name="w">The width</param>
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Construct the quaternion from it's coordinates
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the components</param>
        public Quaternion(Vector4 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = value.W;
        }

        /// <summary>
        /// Construct the quaternion from it's coordinates
        /// </summary>
        /// <param name="values">The values to assign to the X, Y, Z, and W components of the quaternion. This must be an array with four elements</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements</exception>
        public Quaternion(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            if (values.Length != 4)
                throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Quaternion.");

            X = values[0];
            Y = values[1];
            Z = values[2];
            W = values[3];
        }

        /// <summary>
        /// Initializes a new instance of the struct
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components</param>
        /// <param name="angle">Initial value for the angle of the quaternion</param>
        public Quaternion(Vector3 axis, float angle)
        {
            float angle2 = angle * 0.5f;
            float s = Math.Sin(angle2) / Vector3.Magnitude(axis);
            X = axis.X * s;
            Y = axis.Y * s;
            Z = axis.Z * s;
            W = Math.Cos(angle2);
        }

        /// <summary>
        /// Shorthand for writing Quaternion(1, 1, 1, 1)
        /// </summary>
        public static readonly Quaternion One = new Quaternion(1f, 1f, 1f, 1f);

        /// <summary>
        /// Shorthand for writing Quaternion(0, 0, 0, 0)
        /// </summary>
        public static readonly Quaternion Zero = new Quaternion(0f, 0f, 0f, 0f);

        /// <summary>
        /// The identity rotation (RO). This quaternion corresponds to "no rotation": the object
        /// </summary>
        /// <value>The identity matrix</value>
        public static readonly Quaternion Identity = new Quaternion(0f, 0f, 0f, 1f);

        /// <summary>
        /// Get's a value indicating whether this instance is equivalent to the identity quaternion
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an identity quaternion; otherwise, <c>false</c>
        /// </value>
        public bool IsIdentity => this.Equals(Identity);

        /// <summary>
        /// Gets a value indicting whether this instance is normalized
        /// </summary>
        public bool IsNormalized => Math.Abs(X * X + Y * Y + Z * Z + W * W - 1f) < 0f;

        /// <summary>
        /// Adds two quaternions
        /// </summary>
        /// <param name="value1">The first quaternion to add</param>
        /// <param name="value2">The second quaternion to add</param>
        /// <returns>The sum of the two quaternions</returns>
        public static Quaternion operator +(Quaternion value1, Quaternion value2) => new Quaternion(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z, value1.W + value2.W);

        /// <summary>
        /// Subtracts two quaternions
        /// </summary>
        /// <param name="left">The first quaternion to subtract</param>
        /// <param name="right">The second quaternion to subtract</param>
        /// <returns>The difference of the two quaternions</returns>
        public static Quaternion operator -(Quaternion value1, Quaternion value2) => new Quaternion(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z, value1.W - value2.W);

        /// <summary>
        /// Reverses the direction of a given quaternion.
        /// </summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>A quaternion facing in the opposite direction.</returns>
        public static Quaternion operator -(Quaternion value) => new Quaternion(-value.X, -value.Y, -value.Z, -value.W);

        /// <summary>
        /// Scales a quaternion by the given value
        /// </summary>
        /// <param name="value">The quaternion to scale</param>
        /// <param name="scalar">The amount by which to scale the quaternion</param>
        /// <returns>The scaled quaternion</returns>
        public static Quaternion operator *(Quaternion value, float scalar) => new Quaternion(value.X * scalar, value.Y * scalar, value.Z * scalar, value.W * scalar);

        /// <summary>
        /// Scales a quaternion by the given value
        /// </summary>
        /// <param name="scalar">The amount by which to scale the quaternion</param>
        /// <param name="value">The quaternion to scale</param>
        /// <returns>The scaled quaternion</returns>
        public static Quaternion operator *(float scalar, Quaternion value) => new Quaternion(value.X * scalar, value.Y * scalar, value.Z * scalar, value.W * scalar);

        /// <summary>
        /// Multiplies a quaternion by another
        /// </summary>
        /// <param name="value1">The first quaternion to multiply</param>
        /// <param name="value2">The second quaternion to multiply</param>
        /// <returns></returns>
        public static Quaternion operator *(Quaternion value1, Quaternion value2)
        {
            return new Quaternion(
                value1.W * value2.X + value1.X * value2.W + value1.Y * value2.Z - value1.Z * value2.Y,
                value1.W * value2.Y + value1.Y * value2.W + value1.Z * value2.X - value1.X * value2.Z,
                value1.W * value2.Z + value1.Z * value2.W + value1.X * value2.Y - value1.Y * value2.X,
                value1.W * value2.W - value1.X * value2.X - value1.Y * value2.Y - value1.Z * value2.Z);
        }

        /// <summary>
        /// Multiplies a quaternion by a vector.
        /// </summary>
        /// <param name="value">The quaternion to multiply.</param>
        /// <param name="vector">The vector to multiply.</param>
        /// <returns>The multiplied quaternion.</returns>
        public static Quaternion operator *(Quaternion value, Vector3 vector)
        {
            return new Quaternion(
                value.W * vector.X + value.Y * vector.Z - value.Z * vector.Y,
                value.W * vector.Y + value.Z * vector.X - value.X * vector.Z,
                value.W * vector.Z + value.X * vector.Y - value.Y * vector.X,
                -value.X * vector.X - value.Y * vector.Y - value.Z * vector.Z);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale</param>
        /// <param name="scalar">The amount by which to scale the vector</param>
        /// <returns>The scaled vector</returns>
        public static Quaternion operator /(Quaternion value, float scalar) => new Quaternion(value.X / scalar, value.Y / scalar, value.Z / scalar, value.W / scalar);

        /// <summary>
        /// Are two quaternions equal to each other?
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool operator ==(Quaternion value1, Quaternion value2) => value1.Equals(value2);

        /// <summary>
        /// Are two quaternions different from each other?
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns>The presence of NaN values</returns>
        public static bool operator !=(Quaternion value1, Quaternion value2) => !value1.Equals(value2);

        /// <summary>
        /// The dot product between two rotations
        /// </summary>
        /// <param name="value1">First quaternion</param>
        /// <param name="value2">Second quaternion</param>
        /// <returns>Dot Product of two quaternions</returns>
        public static float DotProduct(Quaternion value1, Quaternion value2) => value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z + value1.W * value2.W;

        /// <summary>
        /// Gets the angle of the quaternion
        /// </summary>
        /// <value>The quaternion's angle</value>
        public float Angle
        {
            get
            {
                float magnitude = X * X + Y * Y + Z * Z;
                if (magnitude < 0f)
                    return 0f;

                return 2f * Math.Acos(W);
            }
        }

        /// <summary>
        /// Gets the axis components of the quaternion
        /// </summary>
        /// <value>The axis components of the quaternion</value>
        public Vector3 Axis
        {
            get
            {
                float inverse = 1f / (W * W);
                if (inverse < 1f * 0f)
                    return Vector3.Right;

                return new Vector3(X * inverse, Y * inverse, Z * inverse);
            }
        }

        /// <summary>
        /// Calculates the length of the quaternion
        /// </summary>
        /// <returns>The length of the vector</returns>
        public static float Magnitude(Quaternion value) => Math.Sqrt(Math.Pow(value.X, 2)) + Math.Sqrt(Math.Pow(value.Y, 2)) + Math.Sqrt(Math.Pow(value.Z, 2)) + Math.Sqrt(Math.Pow(value.W, 2));

        /// <summary>
        /// Makes this vector have a magnitude of 1
        /// </summary>
        /// <param name="value">Quaternion</param>
        /// <returns>Normalized quaternion</returns>
        public static Quaternion Normalize(Quaternion value)
        {
            float magnitude = Magnitude(value);

            if (magnitude < 0f)
                return Identity;

            return value / magnitude;
        }

        /// <summary>
        /// Makes this vector have a magnitude of 1
        /// </summary>
        /// <returns>Normalized quaternion</returns>
        public Quaternion Normalize() => Normalize(this);

        /// <summary>
        /// Conjugates and renormalizes the quaternion
        /// </summary>
        /// <param name="value">The quaternion to conjugate and renormalize</param>
        /// <returns>The conjugated and renormalized quaternion</returns>
        public static Quaternion Invert(Quaternion value)
        {
            float magnitude = Magnitude(value);

            if (magnitude > 0f)
                magnitude = 1f / magnitude;

            return -value * magnitude;
        }

        /// <summary>
        /// Compare quaternion and object and checks if they are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>Object and quaternion are equal</returns>
        public override bool Equals(object obj) => (obj is Quaternion) && Equals((Quaternion)obj);

        /// <summary>
        /// Compare two quaternions and checks if they are equal
        /// </summary>
        /// <param name="other">Quaternion to check</param>
        /// <returns>Quaternions are equal</returns>
        public bool Equals(Quaternion other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);

        /// <summary>
        /// Returns a containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 2D triangle
        /// </summary>
        /// <param name="value1">A containing the 4D Cartesian coordinates of vertex 1 of the triangle</param>
        /// <param name="value2">A containing the 4D Cartesian coordinates of vertex 2 of the triangle</param>
        /// <param name="value3">A containing the 4D Cartesian coordinates of vertex 3 of the triangle</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>)</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>)</param>
        /// <returns>A new containing the 4D Cartesian coordinates of the specified point</returns>
        public static Quaternion Barycentric(Quaternion value1, Quaternion value2, Quaternion value3, float amount1, float amount2)
        {
            Quaternion start, end;

            start = Slerp(value1, value2, amount1 + amount2);
            end = Slerp(value1, value3, amount1 + amount2);

            return Slerp(start, end, amount2 / (amount1 + amount2));
        }

        /// <summary>
        /// Exponentiates a quaternion
        /// </summary>
        /// <param name="value">The quaternion to exponentiate</param>
        /// <returns>The exponentiated quaternion</returns>
        public static Quaternion Exponential(Quaternion value)
        {
            Quaternion result = new Quaternion();

            float angle = Math.Sqrt((value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z));
            float sin = Math.Sin(angle);

            if (Math.Abs(sin) >= 0f)
            {
                float coeff = sin / angle;
                result.X = coeff * value.X;
                result.Y = coeff * value.Y;
                result.Z = coeff * value.Z;
            }
            else
                result = value;

            result.W = Math.Cos(angle);

            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between two quaternion
        /// </summary>
        /// <param name="start">Start quaternion</param>
        /// <param name="end">End quaternion</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/></param>
        /// <returns>The linear interpolation of the two quaternions</returns>
        public static Quaternion Lerp(Quaternion start, Quaternion end, float amount)
        {
            Quaternion result = new Quaternion();

            float inverse = 1f - amount;

            if (DotProduct(start, end) >= 0f)
            {
                result.X = (inverse * start.X) + (amount * end.X);
                result.Y = (inverse * start.Y) + (amount * end.Y);
                result.Z = (inverse * start.Z) + (amount * end.Z);
                result.W = (inverse * start.W) + (amount * end.W);
            }
            else
            {
                result.X = (inverse * start.X) - (amount * end.X);
                result.Y = (inverse * start.Y) - (amount * end.Y);
                result.Z = (inverse * start.Z) - (amount * end.Z);
                result.W = (inverse * start.W) - (amount * end.W);
            }

            return result.Normalize();
        }

        /// <summary>
        /// Calculates the natural logarithm of the specified quaternion
        /// </summary>
        /// <param name="value">The quaternion whose logarithm will be calculated</param>
        /// <returns>The natural logarithm of the quaternion</returns>
        public static Quaternion Logarithm(Quaternion value)
        {
            Quaternion result = new Quaternion();

            if (Math.Abs(value.W) < 1f)
            {
                float angle = Math.Acos(value.W);
                float sin = Math.Sin(angle);

                if (Math.Abs(sin) >= 0f)
                {
                    float coeff = angle / sin;
                    result.X = value.X * coeff;
                    result.Y = value.Y * coeff;
                    result.Z = value.Z * coeff;
                }
                else
                    result = value;
            }
            else
                result = value;

            result.W = 0f;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Vector</param>
        /// <returns></returns>
        public Vector3 Rotate(Vector3 value)
        {
            Quaternion rotation = this;
            Quaternion quaternion = rotation * value;
            quaternion *= Invert(rotation);
            return new Vector3(quaternion.X, quaternion.Y, quaternion.Z);
        }

        /// <summary>
        /// Creates a quaternion given a rotation and an axis
        /// </summary>
        /// <param name="axis">The axis of rotation</param>
        /// <param name="angle">The angle of rotation</param>
        /// <returns>The newly created quaternion</returns>
        public static Quaternion RotationAxis(Vector3 axis, float angle)
        {
            Vector3 normalized;
            normalized = Vector3.Normalize(axis);

            float half = angle * 0.5f;
            float sin = Math.Sin(half);
            float cos = Math.Cos(half);

            return new Quaternion(normalized.X * sin, normalized.Y * sin, normalized.Z * sin, cos);
        }

        /// <summary>
        /// Creates a quaternion given a yaw, pitch, and roll value
        /// </summary>
        /// <param name="yaw">The yaw of rotation</param>
        /// <param name="pitch">The pitch of rotation</param>
        /// <param name="roll">The roll of rotation</param>
        /// <returns>The newly created quaternion</returns>
        public static Quaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            float halfRoll = roll * 0.5f;
            float halfPitch = pitch * 0.5f;
            float halfYaw = yaw * 0.5f;

            float sinRoll = Math.Sin(halfRoll);
            float cosRoll = Math.Cos(halfRoll);
            float sinPitch = Math.Sin(halfPitch);
            float cosPitch = Math.Cos(halfPitch);
            float sinYaw = Math.Sin(halfYaw);
            float cosYaw = Math.Cos(halfYaw);

            return new Quaternion(
                (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll),
                (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll),
                (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll),
                (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll)
            );
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation
        /// </summary>
        /// <param name="start">Start quaternion</param>
        /// <param name="end">End quaternion</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/></param>
        /// <returns>The spherical linear interpolation of the two quaternions</returns>
        public static Quaternion Slerp(Quaternion start, Quaternion end, float amount)
        {
            float opposite;
            float inverse;
            float dot = DotProduct(start, end);

            if (Math.Abs(dot) > 1f - 0f)
            {
                inverse = 1f - amount;
                opposite = amount * Math.Sign(dot);
            }
            else
            {
                float acos = Math.Acos(Math.Abs(dot));
                float invSin = (float)(1f / Math.Sin(acos));

                inverse = Math.Sin((1f - amount) * acos) * invSin;
                opposite = Math.Sin(amount * acos) * invSin * Math.Sign(dot);
            }

            return new Quaternion(
                (inverse * start.X) + (opposite * end.X),
                (inverse * start.Y) + (opposite * end.Y),
                (inverse * start.Z) + (opposite * end.Z),
                (inverse * start.W) + (opposite * end.W)
            );
        }

        /// <summary>
        /// Interpolates between quaternions, using spherical quadrangle interpolation
        /// </summary>
        /// <param name="value1">First source quaternion</param>
        /// <param name="value2">Second source quaternion</param>
        /// <param name="value3">Thrid source quaternion</param>
        /// <param name="value4">Fourth source quaternion</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of interpolation</param>
        /// <returns>The spherical quadrangle interpolation of the quaternions</returns>
        public static Quaternion Squad(Quaternion value1, Quaternion value2, Quaternion value3, Quaternion value4, float amount)
        {
            Quaternion start, end;

            start = Slerp(value1, value4, amount);
            end = Slerp(value2, value3, amount);

            return Slerp(start, end, 2f * amount * (1f - amount));
        }

        /// <summary>
        /// Used to allow Quaternions to be used as keys in hash tables
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();

        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString() => $"[Quaternion] X({ X }) Y({ Y }) Z({ Z }) W({ W })";

        #region Kinematics/Unity
        public static implicit operator Quaternion(UnityEngine.Quaternion value)
        {
            return new Quaternion(value.x, value.y, value.z, value.w);
        }

        public static implicit operator UnityEngine.Quaternion(Quaternion value)
        {
            return new UnityEngine.Quaternion(value.X, value.Y, value.Z, value.W);
        }
        #endregion
    }
}
