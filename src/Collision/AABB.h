#pragma once

#include <Math/Vector2.h>

// Port of Collision/AABB.cs (IDisposable dropped; Clear() does the reset).

namespace kinematics
{
    struct AABB
    {
        bool IsValid = false;
        Vector2 Min;
        Vector2 Max;

        AABB() = default;
        AABB(const Vector2& min, const Vector2& max);

        void Add(float x, float y);
        void Clear();
        bool Contains(float x, float y) const;
        bool Intersects(const AABB& aabb) const;
    };
}
