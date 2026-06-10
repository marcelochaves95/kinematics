#pragma once

#include <Math/Vector2.h>
#include <vector>

// Port of Collision/Shape.cs (Vector2[] -> std::vector<Vector2>; C# exceptions
// -> std::logic_error). `Count` mirrors the C# API alongside Points.size().

namespace kinematics
{
    class Shape
    {
    public:
        int Count = 0;
        std::vector<Vector2> Points;

        Shape();

        void Begin(bool center);
        void Add(const Vector2& point);
        void End();
        Shape Clone() const;
        Vector2 GetCenter() const;
        void CenterAtZero();

        static std::vector<Vector2> Transform(const std::vector<Vector2>& points,
            const Vector2& position, float angle, const Vector2& scale);

    private:
        bool _hasBegun = false;
        bool _center = false;
        std::vector<Vector2> _pointList;
    };
}
