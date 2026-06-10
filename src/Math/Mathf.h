#pragma once

// Port of Math/Mathf.cs
//
// PARITY NOTE: the C# Mathf wraps System.Math, whose Sin/Cos/Acos/Sqrt/Pow/Floor
// take and return `double`; the C# then casts the result back to `float`. The
// .cpp reproduces that exactly (compute in double, cast to float) so the sim
// matches bit-for-bit — do NOT switch to the float-native <cmath> overloads.

namespace kinematics::mathf
{
    constexpr float PI = static_cast<float>(3.14159265358979311599796346854);
    constexpr float Epsilon = 0.000001f;

    float Sin(float value);
    float Cos(float value);
    float Acos(float value);
    float Sqrt(float value);
    float Pow(float value, float power);
    float Floor(float value);
    float Abs(float value);
    float Min(float a, float b);
    float Max(float a, float b);
}
