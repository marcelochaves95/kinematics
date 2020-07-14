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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Floor(float value)
        {
            return (float) System.Math.Floor(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Round(float value)
        {
            return (float) System.Math.Round(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Ceiling(float value)
        {
            return (float) System.Math.Ceiling(value);
        }

        /// <summary>
        /// Returns the Cartesian coordinate for one axis of a point that is defined by a given triangle and two normalized barycentric (areal) coordinates
        /// </summary>
        /// <param name="value1">The coordinate on one axis of vertex 1 of the defining triangle</param>
        /// <param name="value2">The coordinate on the same axis of vertex 2 of the defining triangle</param>
        /// <param name="value3">The coordinate on the same axis of vertex 3 of the defining triangle</param>
        /// <param name="amount1">The normalized barycentric (areal) coordinate b2, equal to the weighting factor for vertex 2, the coordinate of which is specified in value2</param>
        /// <param name="amount2">The normalized barycentric (areal) coordinate b3, equal to the weighting factor for vertex 3, the coordinate of which is specified in value3</param>
        /// <returns>Cartesian coordinate of the specified point with respect to the axis being used</returns>
        public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
        {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions
        /// </summary>
        /// <param name="value1">The first position in the interpolation</param>
        /// <param name="value2">The second position in the interpolation</param>
        /// <param name="value3">The third position in the interpolation</param>
        /// <param name="value4">The fourth position in the interpolation</param>
        /// <param name="amount">Weighting factor</param>
        /// <returns>A position that is the result of the Catmull-Rom interpolation</returns>
        public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
        {
            double amountSquared = amount * amount;
            double amountCubed = amountSquared * amount;
            return (float) (0.5 * (2.0 * value2 + (value3 - value1) * amount + (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared + (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed));
        }

        /// <summary>
        /// Restricts a value to be within a specified range
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value. If <c>value</c> is less than <c>min</c>, <c>min</c> will be returned</param>
        /// <param name="max">The maximum value. If <c>value</c> is greater than <c>max</c>, <c>max</c> will be returned</param>
        /// <returns>The clamped value</returns>
        public static float Clamp(float value, float min, float max)
        {
            value = value > max ? max : value;
            value = value < min ? min : value;

            return value;
        }

        /// <summary>
        /// Performs a Hermite spline interpolation
        /// </summary>
        /// <param name="value1">Source position</param>
        /// <param name="tangent1">Source tangent</param>
        /// <param name="value2">Source position</param>
        /// <param name="tangent2">Source tangent</param>
        /// <param name="amount">Weighting factor</param>
        /// <returns>The result of the Hermite spline interpolation</returns>
        public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
        {
            double v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
            double sCubed = s * s * s;
            double sSquared = s * s;

            if (Abs(amount) < Epsilon)
            {
                result = value1;
            }
            else if (Abs(amount - 1f) < Epsilon)
            {
                result = value2;
            }
            else
            {
                result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed + (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared + t1 * s + v1;
            }

            return (float) result;
        }


        /// <summary>
        /// Linearly interpolates between two values
        /// </summary>
        /// <param name="value1">Source value</param>
        /// <param name="value2">Destination value</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2</param>
        /// <returns>Interpolated value</returns>
        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// Linearly interpolates between two values
        /// This method is a less efficient, more precise version of <see cref="Mathf.Lerp"/>.
        /// </summary>
        /// <param name="value1">Source value</param>
        /// <param name="value2">Destination value</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2</param>
        /// <returns>Interpolated value</returns>
        public static float LerpPrecise(float value1, float value2, float amount)
        {
            return (1 - amount) * value1 + value2 * amount;
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value</param>
        /// <param name="value2">Source value</param>
        /// <param name="amount">Weighting value</param>
        /// <returns>Interpolated value</returns>
        public static float SmoothStep(float value1, float value2, float amount)
        {
            float result = Clamp(amount, 0f, 1f);
            result = Hermite(value1, 0f, value2, 0f, result);

            return result;
        }
    }
}
