#pragma once

#include <Math/Vector2.h>

namespace kinematics
{
    struct AABB
    {
        bool is_valid = false;
        Vector2 min;
        Vector2 max;

        AABB() = default;
        AABB(const Vector2& min, const Vector2& max);

        void add(float x, float y);
        void clear();
        bool contains(float x, float y) const;
        bool intersects(const AABB& aabb) const;
    };
}
