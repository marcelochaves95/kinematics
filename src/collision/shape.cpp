#include <collision/shape.h>
#include <Math/Mathf.h>
#include <stdexcept>

namespace kinematics
{
    Shape::Shape()
    {
        _point_list.reserve(128);
    }

    void Shape::begin(bool center)
    {
        if (_has_begun)
        {
            throw std::logic_error("You must call end() before calling begin()");
        }
        _has_begun = true;
        _point_list.clear();
        _center = center;
    }

    void Shape::add(const Vector2& point)
    {
        if (!_has_begun)
        {
            throw std::logic_error("You must call begin() before adding points");
        }
        _point_list.push_back(point);
    }

    void Shape::end()
    {
        if (!_has_begun)
        {
            throw std::logic_error("You must call begin() before calling end()");
        }
        _has_begun = false;
        points = _point_list;
        count = static_cast<int>(points.size());
        if (_center)
        {
            center_at_zero();
        }
    }

    Shape Shape::clone() const
    {
        Shape clone;
        clone.count = static_cast<int>(points.size());
        clone.points = points;
        return clone;
    }

    Vector2 Shape::get_center() const
    {
        float x = 0.0f;
        float y = 0.0f;
        for (int i = 0; i < count; i++)
        {
            x += points[i].X;
            y += points[i].Y;
        }
        x /= count;
        y /= count;
        return Vector2(x, y);
    }

    void Shape::center_at_zero()
    {
        float x = 0.0f;
        float y = 0.0f;
        for (int i = 0; i < count; i++)
        {
            x += points[i].X;
            y += points[i].Y;
        }
        x /= count;
        y /= count;
        for (int i = 0; i < count; i++)
        {
            points[i].X -= x;
            points[i].Y -= y;
        }
    }

    std::vector<Vector2> Shape::transform(const std::vector<Vector2>& points,
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
