#include <Collision/Shape.h>
#include <Math/Mathf.h>
#include <stdexcept>

namespace kinematics
{
    Shape::Shape()
    {
        _pointList.reserve(128);
    }

    void Shape::Begin(bool center)
    {
        if (_hasBegun)
        {
            throw std::logic_error("You must call End() before calling Begin()");
        }
        _hasBegun = true;
        _pointList.clear();
        _center = center;
    }

    void Shape::Add(const Vector2& point)
    {
        if (!_hasBegun)
        {
            throw std::logic_error("You must call Begin() before adding points");
        }
        _pointList.push_back(point);
    }

    void Shape::End()
    {
        if (!_hasBegun)
        {
            throw std::logic_error("You must call Begin() before calling End()");
        }
        _hasBegun = false;
        Points = _pointList;
        Count = static_cast<int>(Points.size());
        if (_center)
        {
            CenterAtZero();
        }
    }

    Shape Shape::Clone() const
    {
        Shape clone;
        clone.Count = static_cast<int>(Points.size());
        clone.Points = Points;
        return clone;
    }

    Vector2 Shape::GetCenter() const
    {
        float x = 0.0f;
        float y = 0.0f;
        for (int i = 0; i < Count; i++)
        {
            x += Points[i].X;
            y += Points[i].Y;
        }
        x /= Count;
        y /= Count;
        return Vector2(x, y);
    }

    void Shape::CenterAtZero()
    {
        float x = 0.0f;
        float y = 0.0f;
        for (int i = 0; i < Count; i++)
        {
            x += Points[i].X;
            y += Points[i].Y;
        }
        x /= Count;
        y /= Count;
        for (int i = 0; i < Count; i++)
        {
            Points[i].X -= x;
            Points[i].Y -= y;
        }
    }

    std::vector<Vector2> Shape::Transform(const std::vector<Vector2>& points,
        const Vector2& position, float angle, const Vector2& scale)
    {
        float cos = mathf::Cos(angle);
        float sin = mathf::Sin(angle);
        std::vector<Vector2> out(points.size());
        for (size_t i = 0; i < points.size(); i++)
        {
            float x = points[i].X * scale.X;
            float y = points[i].Y * scale.Y;
            out[i].X = cos * x - sin * y + position.X;
            out[i].Y = cos * y + sin * x + position.Y;
        }
        return out;
    }
}
