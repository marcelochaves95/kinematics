#include <Collision/AABB.h>

namespace kinematics
{
    AABB::AABB(const Vector2& min, const Vector2& max) : IsValid(true), Min(min), Max(max)
    {
    }

    void AABB::Add(float x, float y)
    {
        if (IsValid)
        {
            if (x < Min.X)
            {
                Min.X = x;
            }
            else if (x > Max.X)
            {
                Max.X = x;
            }

            if (y < Min.Y)
            {
                Min.Y = y;
            }
            else if (y > Max.Y)
            {
                Max.Y = y;
            }
        }
        else
        {
            Min.X = x;
            Max.X = x;
            Min.Y = y;
            Max.Y = y;
            IsValid = true;
        }
    }

    void AABB::Clear()
    {
        Min.X = 0.0f;
        Max.X = 0.0f;
        Min.Y = 0.0f;
        Max.Y = 0.0f;
        IsValid = false;
    }

    bool AABB::Contains(float x, float y) const
    {
        if (IsValid)
        {
            if (x < Min.X || x > Max.X)
            {
                return false;
            }
            if (y < Min.Y || y > Max.Y)
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

    bool AABB::Intersects(const AABB& aabb) const
    {
        if (Max.X < aabb.Min.X || Min.X > aabb.Max.X)
        {
            return false;
        }
        if (Max.Y < aabb.Min.Y || Min.Y > aabb.Max.Y)
        {
            return false;
        }
        return true;
    }
}
