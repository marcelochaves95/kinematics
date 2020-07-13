using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public struct AABB
    {
        public Vector2 min;
        public Vector2 max;
        public bool valid;

        public AABB(ref Vector2 min, ref Vector2 max)
        {
            this.min = min;
            this.max = max;
            valid = true;
        }

        public AABB(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
            valid = true;
        }

        public void Add(float x, float y)
        {
            if (valid)
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
                min.X = max.X = x;
                min.Y = max.Y = y;
                valid = true;
            }
        }

        public void Clear()
        {
            min.X = max.X = min.Y = max.Y = 0;
            valid = false;
        }

        public bool Contains(float x, float y)
        {
            if (valid)
            {
                if ((x < min.X) || (x > max.X))
                {
                    return false;
                }

                if ((y < min.Y) || (y > max.Y))
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
}