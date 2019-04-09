using System;
using System.Threading;

namespace CoreModule
{
    /// <summary>
    /// A collection of common mathematics functions
    /// </summary>
    public struct Mathematics
    {
        /// <summary>
        /// The infamous ''3.14159265358979...'' value (RO)
        /// </summary>
        /// <returns>PI</returns>
        public const float PI = (float)Math.PI;

        /// <summary>
        /// Returns the sine of angle value in radians
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Sin(float value) => (float)Math.Sin(value);

        /// <summary>
        /// Returns the cosine of angle value in radians
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Cos(float value) => (float)Math.Cos(value);

        /// <summary>
        /// Returns the tangent of angle value in radians.
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Tan(float value) => (float)Math.Tan(value);

        /// <summary>
        /// Returns the arc-sine of value - the angle in radians whose sine is value
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Asin(float value) => (float)Math.Asin(value);

        /// <summary>
        /// Returns the arc-cosine of value - the angle in radians whose cosine is value
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Acos(float value) => (float)Math.Acos(value);

        /// <summary>
        /// Returns the arc-tangent of value - the angle in radians whose tangent is value
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Atan(float value) => (float)Math.Atan(value);

        /// <summary>
        /// Returns the angle in radians whose ::ref::Tan is @@y/x@@.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Atan2(float y, float x) => (float)Math.Atan2(y, x);

        /// <summary>
        /// Returns square root of value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static float Sqrt(float value) => (float)Math.Sqrt(value);

        /// <summary>
        /// Returns the absolute value of value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static float Abs(float value) => (float)Math.Abs(value);

        /// <summary>
        /// Returns the absolute value of value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static int Abs(int value) => Math.Abs(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>The lowest value</returns>
        public static float Min(float value1, float value2) => value1 < value2 ? value1 : value2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>The lowest value</returns>
        public static int Min(int value1, int value2) => value1 < value2 ? value1 : value2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>The lowest value</returns>
        public static float Max(float value1, float value2) => value1 > value2 ? value1 : value2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>The highest value</returns>
        public static int Max(int value1, int value2) => value1 > value2 ? value1 : value2;

        /// <summary>
        /// Returns value raised to power
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static float Pow(float value, float power) => (float)Math.Pow(value, power);

        /// <summary>
        /// Returns e raised to the specified power
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public static float Exp(float power) => (float)Math.Exp(power);

        /// <summary>
        /// Returns the logarithm of a specified number in a specified base
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="logBase">Base</param>
        /// <returns></returns>
        public static float Log(float value, float logBase) => (float)Math.Log(value, logBase);

        /// <summary>
        /// Returns the natural (base e) logarithm of a specified number
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Log on base e</returns>
        public static float Log(float value) => (float)Math.Log(value);

        /// <summary>
        /// Returns the base 10 logarithm of a specified number
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Log on base 10</returns>
        public static float Log10(float value) => (float)Math.Log10(value);

        /// <summary>
        /// Degrees-to-radians conversion constant (RO)
        /// </summary>
        /// <returns></returns>
        public static float DegToRad() => PI * 2f / 360f;

        /// <summary>
        /// Radians-to-degrees conversion constant (RO)
        /// </summary>
        /// <returns></returns>
        public static float RadToDeg() => 1f / DegToRad();
    }
}
