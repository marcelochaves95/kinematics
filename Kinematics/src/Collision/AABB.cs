using System;
using Microsoft.Xna.Framework;

namespace Kinematics.Collision
{
    public struct AABB : IDisposable
    {
        public bool IsValid;
        public Vector2 Min;
        public Vector2 Max;

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
                Min.X = x;
                Max.X = x;
                Min.Y = y;
                Max.Y = y;
                IsValid = true;
            }
        }

        public void Clear()
        {
            Dispose();
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

        public void Dispose()
        {
            Min.X = 0f;
            Max.X = 0f;
            Min.Y = 0f;
            Max.Y = 0f;
            IsValid = false;
        }
    }
}
