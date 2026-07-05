#pragma once

#include <math/vector3.h>

namespace kinematics
{
    struct Vector2
    {
        float x = 0.0f;
        float y = 0.0f;

        static const Vector2 ZERO;
        static const Vector2 ONE;

        Vector2() = default;
        Vector2(float x, float y);

        float length() const;
        float length_squared() const;
        void normalize();

        static float dot(const Vector2& a, const Vector2& b);
        static float distance(const Vector2& a, const Vector2& b);
        static float distance_squared(const Vector2& a, const Vector2& b);
        static Vector2 normalize(const Vector2& value);

        Vector3 vector3_from_vector2() const;
        Vector2 rotate(float value) const;
        Vector2 perpendicular() const;
        bool is_counter_clockwise(const Vector2& value) const;
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
