using Kinematics.MathModule;

namespace Kinematics.Modules.ExtensionModule
{
    public static class KinematicsExtension
    {
        public static Quaternion ToPhysics(this UnityEngine.Quaternion value)
        {
            return new Quaternion(value.x, value.y, value.z, value.w);
        }

        public static UnityEngine.Quaternion ToUnity(this Quaternion value)
        {
            return new UnityEngine.Quaternion(value.X, value.Y, value.Z, value.W);
        }

        public static Vector3 ToPhysics(this UnityEngine.Vector3 value)
        {
            return new Vector3(value.x, value.y, value.z);
        }

        public static UnityEngine.Vector3 ToUnity(this Vector3 value)
        {
            return new UnityEngine.Vector3(value.X, value.Y, value.Z);
        }

        public static Vector4 ToPhysics(this UnityEngine.Vector4 value)
        {
            return new Vector4(value.x, value.y, value.z, value.w);
        }

        public static UnityEngine.Vector4 ToUnity(this Vector4 value)
        {
            return new UnityEngine.Vector4(value.X, value.Y, value.Z, value.W);
        }
    }
}
