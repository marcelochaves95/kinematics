using UnityEngine;

using Quaternion = PhysicsEngine.MathModule.Quaternion;
using Vector3 = PhysicsEngine.MathModule.Vector3;
using Vector4 = PhysicsEngine.MathModule.Vector4;

public static class PhysicsEngineExtension
{
    public static Quaternion ToPhysics(this UnityEngine.Quaternion value) => new Quaternion(value.x, value.y, value.z, value.w);

    public static UnityEngine.Quaternion ToUnity(this Quaternion value) => new UnityEngine.Quaternion(value.X, value.Y, value.Z, value.W);

    public static Vector3 ToPhysics(this UnityEngine.Vector3 value) => new Vector3(value.x, value.y, value.z);

    public static UnityEngine.Vector3 ToUnity(this Vector3 value) => new UnityEngine.Vector3(value.X, value.Y, value.Z);

    public static Vector4 ToPhysics(this UnityEngine.Vector4 value) => new Vector4(value.x, value.y, value.z, value.w);

    public static UnityEngine.Vector4 ToUnity(this Vector4 value) => new UnityEngine.Vector4(value.X, value.Y, value.Z, value.W);
}
