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
            if (x < min.X)
            {
                min.X = x;
            }
            else if (x > max.X)
            {
                max.X = x;
            }

            if (y < min.Y)
            {
                min.Y = y;
            }
            else if (y > max.Y)
            {
                max.Y = y;
            }
        }
        else
        {
            min.X = x;
            max.X = x;
            min.Y = y;
            max.Y = y;
            is_valid = true;
        }
    }

    void AABB::clear()
    {
        min.X = 0.0f;
        max.X = 0.0f;
        min.Y = 0.0f;
        max.Y = 0.0f;
        is_valid = false;
    }

    bool AABB::contains(float x, float y) const
    {
        if (is_valid)
        {
            if (x < min.X || x > max.X)
            {
                return false;
            }
            if (y < min.Y || y > max.Y)
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
        if (max.X < aabb.min.X || min.X > aabb.max.X)
        {
            return false;
        }
        if (max.Y < aabb.min.Y || min.Y > aabb.max.Y)
        {
            return false;
        }
        return true;
    }
}
