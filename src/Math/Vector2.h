#pragma once

#include <Math/Vector3.h>

// Port of Math/Vector2.cs + Math/Vector2Extensions.cs
//
// The C# extension methods (Perpendicular, Rotate, IsCounterClockwise,
// Vector3FromVector2) are folded in as member functions so the call sites read
// identically to the C#.

namespace kinematics
{
    struct Vector2
    {
        float X = 0.0f;
        float Y = 0.0f;

        static const Vector2 Zero;
        static const Vector2 One;

        Vector2() = default;
        Vector2(float x, float y);

        float Length() const;
        float LengthSquared() const;
        void Normalize();

        static float Dot(const Vector2& a, const Vector2& b);
        static float Distance(const Vector2& a, const Vector2& b);
        static float DistanceSquared(const Vector2& a, const Vector2& b);
        static Vector2 Normalize(const Vector2& value);

        Vector3 Vector3FromVector2() const;
        Vector2 Rotate(float value) const;
        Vector2 Perpendicular() const;
        bool IsCounterClockwise(const Vector2& value) const;
    };

    Vector2 operator+(const Vector2& a, const Vector2& b);
    Vector2 operator-(const Vector2& a, const Vector2& b);
    Vector2 operator-(const Vector2& a);
    Vector2 operator*(const Vector2& a, float scalar);
    Vector2 operator*(float scalar, const Vector2& a);
    Vector2 operator/(const Vector2& a, float scalar);
    bool operator==(const Vector2& a, const Vector2& b);
    bool operator!=(const Vector2& a, const Vector2& b);
    Vector2& operator+=(Vector2& a, const Vector2& b);
    Vector2& operator-=(Vector2& a, const Vector2& b);
}
