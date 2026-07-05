#include <math/vector2.h>
#include <math/mathf.h>

namespace kinematics
{
    const Vector2 Vector2::ZERO = Vector2(0.0f, 0.0f);
    const Vector2 Vector2::ONE = Vector2(1.0f, 1.0f);

    Vector2::Vector2(float x, float y) : x(x), y(y)
    {
    }

    float Vector2::length() const
    {
        return mathf::sqrt(x * x + y * y);
    }

    float Vector2::length_squared() const
    {
        return x * x + y * y;
    }

    void Vector2::normalize()
    {
        float length = this->length();
        if (length > mathf::EPSILON)
        {
            x /= length;
            y /= length;
        }
    }

    float Vector2::dot(const Vector2& a, const Vector2& b)
    {
        return a.x * b.x + a.y * b.y;
    }

    float Vector2::distance(const Vector2& a, const Vector2& b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        return mathf::sqrt(dx * dx + dy * dy);
    }

    float Vector2::distance_squared(const Vector2& a, const Vector2& b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        return dx * dx + dy * dy;
    }

    Vector2 Vector2::normalize(const Vector2& value)
    {
        float length = value.length();
        if (length > mathf::EPSILON)
        {
            return Vector2(value.x / length, value.y / length);
        }
        return Vector2(value.x, value.y);
    }

    Vector3 Vector2::vector3_from_vector2() const
    {
        return Vector3(x, y, 0.0f);
    }

    Vector2 Vector2::rotate(float value) const
    {
        float cos = mathf::cos(value);
        float sin = mathf::sin(value);
        return Vector2(cos * x - sin * y, cos * y + sin * x);
    }

    Vector2 Vector2::perpendicular() const
    {
        return Vector2(-y, x);
    }

    bool Vector2::is_counter_clockwise(const Vector2& value) const
    {
        Vector2 perpendicular = this->perpendicular();
        float dot = Vector2::dot(value, perpendicular);
        return dot >= 0.0f;
    }

    Vector2 operator+(const Vector2& a, const Vector2& b)
    {
        return Vector2(a.x + b.x, a.y + b.y);
    }

    Vector2 operator-(const Vector2& a, const Vector2& b)
    {
        return Vector2(a.x - b.x, a.y - b.y);
    }

    Vector2 operator-(const Vector2& a)
    {
        return Vector2(-a.x, -a.y);
    }

    Vector2 operator*(const Vector2& a, float scalar)
    {
        return Vector2(a.x * scalar, a.y * scalar);
    }

    Vector2 operator*(float scalar, const Vector2& a)
    {
        return Vector2(a.x * scalar, a.y * scalar);
    }

    Vector2 operator/(const Vector2& a, float scalar)
    {
        return Vector2(a.x / scalar, a.y / scalar);
    }

    bool operator==(const Vector2& a, const Vector2& b)
    {
        return a.x == b.x && a.y == b.y;
    }

    bool operator!=(const Vector2& a, const Vector2& b)
    {
        return a.x != b.x || a.y != b.y;
    }

    Vector2& operator+=(Vector2& a, const Vector2& b)
    {
        a.x += b.x;
        a.y += b.y;
        return a;
    }

    Vector2& operator-=(Vector2& a, const Vector2& b)
    {
        a.x -= b.x;
        a.y -= b.y;
        return a;
    }
}
