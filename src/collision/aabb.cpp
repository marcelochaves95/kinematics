#include <collision/aabb.h>

namespace kinematics
{
    AABB::AABB(const Vector2& min, const Vector2& max) : is_valid(true), min(min), max(max)
    {
    }

    void AABB::add(float x, float y)
    {
        if (is_valid)
        {
            if (x < min.x)
            {
                min.x = x;
            }
            else if (x > max.x)
            {
                max.x = x;
            }

            if (y < min.y)
            {
                min.y = y;
            }
            else if (y > max.y)
            {
                max.y = y;
            }
        }
        else
        {
            min.x = x;
            max.x = x;
            min.y = y;
            max.y = y;
            is_valid = true;
        }
    }

    void AABB::clear()
    {
        min.x = 0.0f;
        max.x = 0.0f;
        min.y = 0.0f;
        max.y = 0.0f;
        is_valid = false;
    }

    bool AABB::contains(float x, float y) const
    {
        if (is_valid)
        {
            if (x < min.x || x > max.x)
            {
                return false;
            }
            if (y < min.y || y > max.y)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    bool AABB::intersects(const AABB& aabb) const
    {
        if (max.x < aabb.min.x || min.x > aabb.max.x)
        {
            return false;
        }
        if (max.y < aabb.min.y || min.y > aabb.max.y)
        {
            return false;
        }
        return true;
    }
}
