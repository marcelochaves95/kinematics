using System.Runtime.CompilerServices;

namespace Kinematics.Math
{
    internal static class Mathf
    {
        internal const float PI = (float) System.Math.PI;
        internal const float Epsilon = 0.000001f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Sin(float value)
        {
            return (float) System.Math.Sin(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Cos(float value)
        {
            return (float) System.Math.Cos(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Acos(float value)
        {
            return (float) System.Math.Acos(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Sqrt(float value)
        {
            return (float) System.Math.Sqrt(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Abs(float value)
        {
            return System.Math.Abs(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Pow(float value, float power)
        {
            return (float) System.Math.Pow(value, power);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Floor(float value)
        {
            return (float) System.Math.Floor(value);
        }
    }
}
