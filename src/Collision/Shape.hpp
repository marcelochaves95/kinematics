#pragma once

#include <stdexcept>
#include <vector>

#include "Math/Mathf.hpp"
#include "Math/Vector2.hpp"

// Port of src/Collision/Shape.cs
//
// C# arrays (Vector2[]) become std::vector<Vector2>. `Count` is kept alongside
// Points.size() purely to mirror the C# API. C# exceptions become std::logic_error.

namespace kinematics {

class Shape {
public:
    int Count = 0;
    std::vector<Vector2> Points;

    Shape() { _pointList.reserve(128); }

    void Begin(bool center) {
        if (_hasBegun) {
            throw std::logic_error("You must call End() before calling Begin()");
        }
        _hasBegun = true;
        _pointList.clear();
        _center = center;
    }

    void Add(const Vector2& point) {
        if (!_hasBegun) {
            throw std::logic_error("You must call Begin() before adding points");
        }
        _pointList.push_back(point);
    }

    void End() {
        if (!_hasBegun) {
            throw std::logic_error("You must call Begin() before calling End()");
        }
        _hasBegun = false;
        Points = _pointList;
        Count = static_cast<int>(Points.size());

        if (_center) {
            CenterAtZero();
        }
    }

    Shape Clone() const {
        Shape clone;
        clone.Count = static_cast<int>(Points.size());
        clone.Points = Points;
        return clone;
    }

    Vector2 GetCenter() const {
        float x = 0.0f;
        float y = 0.0f;
        for (int i = 0; i < Count; i++) {
            x += Points[i].X;
            y += Points[i].Y;
        }
        x /= Count;
        y /= Count;
        return Vector2(x, y);
    }

    void CenterAtZero() {
        float x = 0.0f;
        float y = 0.0f;
        for (int i = 0; i < Count; i++) {
            x += Points[i].X;
            y += Points[i].Y;
        }
        x /= Count;
        y /= Count;

        for (int i = 0; i < Count; i++) {
            Points[i].X -= x;
            Points[i].Y -= y;
        }
    }

    // C# signature used ref/out params; in C++ we return the transformed list.
    // cos/sin are hoisted out of the loop (loop-invariant) — bit-identical result.
    static std::vector<Vector2> Transform(const std::vector<Vector2>& points,
                                          const Vector2& position, float angle,
                                          const Vector2& scale) {
        float cos = mathf::Cos(angle);
        float sin = mathf::Sin(angle);
        std::vector<Vector2> out(points.size());
        for (size_t i = 0; i < points.size(); i++) {
            float x = points[i].X * scale.X;
            float y = points[i].Y * scale.Y;
            out[i].X = cos * x - sin * y + position.X;
            out[i].Y = cos * y + sin * x + position.Y;
        }
        return out;
    }

private:
    bool _hasBegun = false;
    bool _center = false;
    std::vector<Vector2> _pointList;
};

} // namespace kinematics
