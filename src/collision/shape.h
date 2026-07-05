#pragma once

#include <math/vector2.h>
#include <vector>

namespace kinematics
{
    class Shape
    {
    public:
        int count = 0;
        std::vector<Vector2> points;

        Shape();

        void begin(bool center);
        void add(const Vector2& point);
        void end();
        Shape clone() const;
        Vector2 get_center() const;
        void center_at_zero();

        static std::vector<Vector2> transform(const std::vector<Vector2>& points,
            const Vector2& position, float angle, const Vector2& scale);

    private:
        bool _has_begun = false;
        bool _center = false;
        std::vector<Vector2> _point_list;
    };
}
