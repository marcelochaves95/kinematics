#include <Math/Mathf.h>
#include <cmath>

namespace kinematics::mathf
{
    float Sin(float value)
    {
        return static_cast<float>(std::sin(static_cast<double>(value)));
    }

    float Cos(float value)
    {
        return static_cast<float>(std::cos(static_cast<double>(value)));
    }

    float Acos(float value)
    {
        return static_cast<float>(std::acos(static_cast<double>(value)));
    }

    float Sqrt(float value)
    {
        return static_cast<float>(std::sqrt(static_cast<double>(value)));
    }

    float Pow(float value, float power)
    {
        return static_cast<float>(std::pow(static_cast<double>(value), static_cast<double>(power)));
    }

    float Floor(float value)
    {
        return static_cast<float>(std::floor(static_cast<double>(value)));
    }

    // System.Math.Abs/Min/Max for float operate in float directly; we mirror that.
    // (NaN edge-case semantics differ slightly from .NET, but a valid simulation
    //  never produces NaN, so this does not affect parity on the golden scenes.)
    float Abs(float value)
    {
        return std::fabs(value);
    }

    float Min(float a, float b)
    {
        return a < b ? a : b;
    }

    float Max(float a, float b)
    {
        return a > b ? a : b;
    }
}
