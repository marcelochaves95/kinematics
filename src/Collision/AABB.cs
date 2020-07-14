using Kinematics.Math;

namespace Kinematics.Collision
{
    public struct AABB
    {
        public Vector2 Min;
        public Vector2 Max;
        public bool IsValid;

        public AABB(ref Vector2 min, ref Vector2 max)
        {
            Min = min;
            Max = max;
            IsValid = true;
        }

        public AABB(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
            IsValid = true;
        }

        public void Add(float x, float y)
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
                Min.X = Max.X = x;
                Min.Y = Max.Y = y;
                IsValid = true;
            }
        }

        public void Clear()
        {
            Min.X = Max.X = Min.Y = Max.Y = 0;
            IsValid = false;
        }

        public bool Contains(float x, float y)
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

        public bool Intersects(ref AABB aabb)
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
}