#include <Math/Vector2.h>
#include <Math/Mathf.h>

namespace kinematics
{
    const Vector2 Vector2::Zero = Vector2(0.0f, 0.0f);
    const Vector2 Vector2::One = Vector2(1.0f, 1.0f);

    Vector2::Vector2(float x, float y) : X(x), Y(y)
    {
    }

    float Vector2::Length() const
    {
        return mathf::Sqrt(X * X + Y * Y);
    }

    float Vector2::LengthSquared() const
    {
        return X * X + Y * Y;
    }

    void Vector2::Normalize()
    {
        float length = Length();
        if (length > mathf::Epsilon)
        {
            X /= length;
            Y /= length;
        }
    }

    float Vector2::Dot(const Vector2& a, const Vector2& b)
    {
        return a.X * b.X + a.Y * b.Y;
    }

    float Vector2::Distance(const Vector2& a, const Vector2& b)
    {
        float dx = a.X - b.X;
        float dy = a.Y - b.Y;
        return mathf::Sqrt(dx * dx + dy * dy);
    }

    float Vector2::DistanceSquared(const Vector2& a, const Vector2& b)
    {
        float dx = a.X - b.X;
        float dy = a.Y - b.Y;
        return dx * dx + dy * dy;
    }

    Vector2 Vector2::Normalize(const Vector2& value)
    {
        float length = value.Length();
        if (length > mathf::Epsilon)
        {
            return Vector2(value.X / length, value.Y / length);
        }
        return Vector2(value.X, value.Y);
    }

    Vector3 Vector2::Vector3FromVector2() const
    {
        return Vector3(X, Y, 0.0f);
    }

    Vector2 Vector2::Rotate(float value) const
    {
        float cos = mathf::Cos(value);
        float sin = mathf::Sin(value);
        return Vector2(cos * X - sin * Y, cos * Y + sin * X);
    }

    Vector2 Vector2::Perpendicular() const
    {
        return Vector2(-Y, X);
    }

    bool Vector2::IsCounterClockwise(const Vector2& value) const
    {
        Vector2 perpendicular = Perpendicular();
        float dot = Vector2::Dot(value, perpendicular);
        return dot >= 0.0f;
    }

    Vector2 operator+(const Vector2& a, const Vector2& b)
    {
        return Vector2(a.X + b.X, a.Y + b.Y);
    }

    Vector2 operator-(const Vector2& a, const Vector2& b)
    {
        return Vector2(a.X - b.X, a.Y - b.Y);
    }

    Vector2 operator-(const Vector2& a)
    {
        return Vector2(-a.X, -a.Y);
    }

    Vector2 operator*(const Vector2& a, float scalar)
    {
        return Vector2(a.X * scalar, a.Y * scalar);
    }

    Vector2 operator*(float scalar, const Vector2& a)
    {
        return Vector2(a.X * scalar, a.Y * scalar);
    }

    Vector2 operator/(const Vector2& a, float scalar)
    {
        return Vector2(a.X / scalar, a.Y / scalar);
    }

    bool operator==(const Vector2& a, const Vector2& b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    bool operator!=(const Vector2& a, const Vector2& b)
    {
        return a.X != b.X || a.Y != b.Y;
    }

    Vector2& operator+=(Vector2& a, const Vector2& b)
    {
        a.X += b.X;
        a.Y += b.Y;
        return a;
    }

    Vector2& operator-=(Vector2& a, const Vector2& b)
    {
        a.X -= b.X;
        a.Y -= b.Y;
        return a;
    }
}
