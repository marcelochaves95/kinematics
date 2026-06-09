#pragma once

#include "Math/Mathf.hpp"
#include "Math/Vector3.hpp"

// Port of src/Math/Vector2.cs + src/Math/Vector2Extensions.cs
//
// The C# extension methods (Perpendicular, Rotate, IsCounterClockwise,
// Vector3FromVector2) are folded in as member functions so the call sites read
// identically to the C# (`v.Perpendicular()`, `v.Rotate(a)`, ...).

namespace kinematics {

struct Vector2 {
    float X = 0.0f;
    float Y = 0.0f;

    Vector2() = default;
    Vector2(float x, float y) : X(x), Y(y) {}

    static const Vector2 Zero;
    static const Vector2 One;

    float Length() const        { return mathf::Sqrt(X * X + Y * Y); }
    float LengthSquared() const { return X * X + Y * Y; }

    void Normalize() {
        float length = Length();
        if (length > mathf::Epsilon) {
            X /= length;
            Y /= length;
        }
    }

    static float Dot(const Vector2& a, const Vector2& b) {
        return a.X * b.X + a.Y * b.Y;
    }

    static float Distance(const Vector2& a, const Vector2& b) {
        float dx = a.X - b.X;
        float dy = a.Y - b.Y;
        return mathf::Sqrt(dx * dx + dy * dy);
    }

    static float DistanceSquared(const Vector2& a, const Vector2& b) {
        float dx = a.X - b.X;
        float dy = a.Y - b.Y;
        return dx * dx + dy * dy;
    }

    static Vector2 Normalize(const Vector2& value) {
        float length = value.Length();
        if (length > mathf::Epsilon) {
            return Vector2(value.X / length, value.Y / length);
        }
        return Vector2(value.X, value.Y);
    }

    // ── from Vector2Extensions ──────────────────────────────────
    Vector3 Vector3FromVector2() const { return Vector3(X, Y, 0.0f); }

    Vector2 Rotate(float value) const {
        float cos = mathf::Cos(value);
        float sin = mathf::Sin(value);
        return Vector2(cos * X - sin * Y, cos * Y + sin * X);
    }

    Vector2 Perpendicular() const { return Vector2(-Y, X); }

    bool IsCounterClockwise(const Vector2& value) const {
        Vector2 perpendicular = Perpendicular();
        float dot = Vector2::Dot(value, perpendicular);
        return dot >= 0.0f;
    }
};

// ── operators (C# static operator overloads) ────────────────────
inline Vector2 operator+(const Vector2& a, const Vector2& b) { return Vector2(a.X + b.X, a.Y + b.Y); }
inline Vector2 operator-(const Vector2& a, const Vector2& b) { return Vector2(a.X - b.X, a.Y - b.Y); }
inline Vector2 operator-(const Vector2& a)                   { return Vector2(-a.X, -a.Y); }
inline Vector2 operator*(const Vector2& a, float scalar)     { return Vector2(a.X * scalar, a.Y * scalar); }
inline Vector2 operator*(float scalar, const Vector2& a)     { return Vector2(a.X * scalar, a.Y * scalar); }
inline Vector2 operator/(const Vector2& a, float scalar)     { return Vector2(a.X / scalar, a.Y / scalar); }
inline bool    operator==(const Vector2& a, const Vector2& b){ return a.X == b.X && a.Y == b.Y; }
inline bool    operator!=(const Vector2& a, const Vector2& b){ return a.X != b.X || a.Y != b.Y; }

// Compound assignment (used heavily in the dynamics/collision code).
inline Vector2& operator+=(Vector2& a, const Vector2& b) { a.X += b.X; a.Y += b.Y; return a; }
inline Vector2& operator-=(Vector2& a, const Vector2& b) { a.X -= b.X; a.Y -= b.Y; return a; }

// Static member definitions (header-only: inline to avoid ODR issues).
inline const Vector2 Vector2::Zero = Vector2(0.0f, 0.0f);
inline const Vector2 Vector2::One  = Vector2(1.0f, 1.0f);

} // namespace kinematics
