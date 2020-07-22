namespace Kinematics.Math
{
    public static class Mathf
    {
        #region Properties

        public const float PI = (float) System.Math.PI;
        public const float Epsilon = 0.000001f;
        public const float DegToRad = PI * 2f / 360f;
        public const float RadToDeg = 1f / DegToRad;

        #endregion

        #region Methods

        public static float Sin(float value)
        {
            return (float) System.Math.Sin(value); 
        }

        public static float Cos(float value)
        {
            return (float) System.Math.Cos(value);
        }

        public static float Tan(float value)
        {
            return (float) System.Math.Tan(value);
        }

        public static float Asin(float value)
        {
            return (float) System.Math.Asin(value);
        }

        public static float Acos(float value)
        {
            return (float) System.Math.Acos(value);
        }

        public static float Atan(float value)
        {
            return (float) System.Math.Atan(value);
        }

        public static float Atan2(float y, float x)
        {
            return (float) System.Math.Atan2(y, x);
        }

        public static float Sqrt(float value)
        {
            return (float) System.Math.Sqrt(value);
        }

        public static float Abs(float value)
        {
            return System.Math.Abs(value);
        }

        public static float Min(float value1, float value2)
        {
            return value1 < value2 ? value1 : value2;
        }

        public static int Min(int value1, int value2)
        {
            return value1 < value2 ? value1 : value2;
        }

        public static float Max(float value1, float value2)
        {
            return value1 > value2 ? value1 : value2;
        }

        public static int Max(int value1, int value2)
        {
            return value1 > value2 ? value1 : value2;
        }

        public static float Pow(float value, float power)
        {
            return (float) System.Math.Pow(value, power);
        }

        public static float Exp(float power)
        {
            return (float) System.Math.Exp(power);
        }

        public static float Log(float value, float logBase)
        {
            return (float) System.Math.Log(value, logBase);
        }

        public static float Log(float value)
        {
            return (float) System.Math.Log(value);
        }

        public static float Log10(float value)
        {
            return (float) System.Math.Log10(value);
        }

        public static float Sign(float value)
        {
            return value >= 0f ? 1f : -1f;
        }

        public static float Floor(float value)
        {
            return (float) System.Math.Floor(value);
        }

        public static float Round(float value)
        {
            return (float) System.Math.Round(value);
        }

        public static float Ceiling(float value)
        {
            return (float) System.Math.Ceiling(value);
        }

        public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
        {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
        {
            double amountSquared = amount * amount;
            double amountCubed = amountSquared * amount;
            return (float) (0.5 * (2.0 * value2 + (value3 - value1) * amount +
                                   (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
                                   (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed));
        }

        public static float Clamp(float value, float min, float max)
        {
            value = value > max ? max : value;
            value = value < min ? min : value;
            return value;
        }

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

        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        public static float LerpPrecise(float value1, float value2, float amount)
        {
            return (1 - amount) * value1 + value2 * amount;
        }

        public static float SmoothStep(float value1, float value2, float amount)
        {
            float result = Clamp(amount, 0f, 1f);
            result = Hermite(value1, 0f, value2, 0f, result);
            return result;
        }

        #endregion
    }
}