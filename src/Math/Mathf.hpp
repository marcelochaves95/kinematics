#pragma once

#include <cmath>

// Port of src/Math/Mathf.cs
//
// PARITY NOTE: the C# Mathf wraps System.Math, whose Sin/Cos/Acos/Sqrt/Pow/Floor
// take and return `double`; the C# code then casts the result back to `float`.
// To reproduce the exact same rounding (and therefore the exact same simulation
// trajectory), each function below computes in `double` and casts to `float` —
// do NOT replace these with the float-native <cmath> overloads (sqrtf, sinf, ...),
// which would round differently and make the sim diverge after a few seconds.

namespace kinematics::mathf {

inline constexpr float PI      = static_cast<float>(3.14159265358979311599796346854); // (float)System.Math.PI
inline constexpr float Epsilon = 0.000001f;

inline float Sin(float value)  { return static_cast<float>(std::sin(static_cast<double>(value))); }
inline float Cos(float value)  { return static_cast<float>(std::cos(static_cast<double>(value))); }
inline float Acos(float value) { return static_cast<float>(std::acos(static_cast<double>(value))); }
inline float Sqrt(float value) { return static_cast<float>(std::sqrt(static_cast<double>(value))); }
inline float Pow(float value, float power) {
    return static_cast<float>(std::pow(static_cast<double>(value), static_cast<double>(power)));
}
inline float Floor(float value) { return static_cast<float>(std::floor(static_cast<double>(value))); }

// System.Math.Abs(float) / Min(float,float) / Max(float,float) operate in float
// directly. We mirror that with the float-native operations.
// (NaN edge-case semantics differ slightly from .NET, but a valid simulation
//  never produces NaN, so this does not affect parity on the golden scenes.)
inline float Abs(float value)        { return std::fabs(value); }
inline float Min(float a, float b)   { return a < b ? a : b; }
inline float Max(float a, float b)   { return a > b ? a : b; }

} // namespace kinematics::mathf
