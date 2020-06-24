namespace Kinematics.Math
{
    /// <summary>
    /// A collection of common mathematics functions
    /// </summary>
    public static class Mathf
    {
        /// <summary>
        /// The infamous ''3.14159265358979...'' value (RO)
        /// </summary>
        /// <returns>PI</returns>
        public const float PI = (float) System.Math.PI;

        /// <summary>
        /// A tiny floating point value (Read Only)
        /// </summary>
        public const float Epsilon = 0.000001f;

        /// <summary>
        /// Degrees-to-radians conversion constant (RO)
        /// </summary>
        /// <returns></returns>
        public const float DegToRad = PI * 2f / 360f;

        /// <summary>
        /// Radians-to-degrees conversion constant (RO)
        /// </summary>
        /// <returns></returns>
        public const float RadToDeg = 1f / DegToRad;

        /// <summary>
        /// Returns the sine of angle value in radians
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Sin(float value)
        {
            return (float) System.Math.Sin(value); 
        }

        /// <summary>
        /// Returns the cosine of angle value in radians
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Cos(float value)
        {
            return (float) System.Math.Cos(value);
        }

        /// <summary>
        /// Returns the tangent of angle value in radians.
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Tan(float value)
        {
            return (float) System.Math.Tan(value);
        }

        /// <summary>
        /// Returns the arc-sine of value - the angle in radians whose sine is value
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Asin(float value)
        {
            return (float) System.Math.Asin(value);
        }

        /// <summary>
        /// Returns the arc-cosine of value - the angle in radians whose cosine is value
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Acos(float value)
        {
            return (float) System.Math.Acos(value);
        }

        /// <summary>
        /// Returns the arc-tangent of value - the angle in radians whose tangent is value
        /// </summary>
        /// <param name="value">Angle</param>
        /// <returns></returns>
        public static float Atan(float value)
        {
            return (float) System.Math.Atan(value);
        }

        /// <summary>
        /// Returns the angle in radians whose ::ref::Tan is @@y/x@@.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Atan2(float y, float x)
        {
            return (float) System.Math.Atan2(y, x);
        }

        /// <summary>
        /// Returns square root of value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static float Sqrt(float value)
        {
            return (float) System.Math.Sqrt(value);
        }

        /// <summary>
        /// Returns the absolute value of value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static float Abs(float value)
        {
            return System.Math.Abs(value);
        }

        /// <summary>
        /// Returns the smaller of two numbers.
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>The lowest value</returns>
        public static float Min(float value1, float value2)
        {
            return value1 < value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns the smaller of two numbers.
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>The lowest value</returns>
        public static int Min(int value1, int value2)
        {
            return value1 < value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns the larger of two specified numbers.
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>The lowest value</returns>
        public static float Max(float value1, float value2)
        {
            return value1 > value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns the larger of two specified numbers.
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>The highest value</returns>
        public static int Max(int value1, int value2)
        {
            return value1 > value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns value raised to power
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static float Pow(float value, float power)
        {
            return (float) System.Math.Pow(value, power);
        }

        /// <summary>
        /// Returns e raised to the specified power
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public static float Exp(float power)
        {
            return (float) System.Math.Exp(power);
        }

        /// <summary>
        /// Returns the logarithm of a specified number in a specified base
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="logBase">Base</param>
        /// <returns></returns>
        public static float Log(float value, float logBase)
        {
            return (float) System.Math.Log(value, logBase);
        }

        /// <summary>
        /// Returns the natural (base e) logarithm of a specified number
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Log on base e</returns>
        public static float Log(float value)
        {
            return (float) System.Math.Log(value);
        }

        /// <summary>
        /// Returns the base 10 logarithm of a specified number
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Log on base 10</returns>
        public static float Log10(float value)
        {
            return (float) System.Math.Log10(value);
        }

        /// <summary>
        /// Returns the sign of value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Sign(float value)
        {
            return value >= 0f ? 1f : -1f;
        }
    }
}
