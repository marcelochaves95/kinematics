using System;
using System.Collections.Generic;
using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class Shape
    {
        public Vector2[] points;
        public int count;
        private bool hasBegun;
        private List<Vector2> pointList;
        private bool center;

        public Shape()
        {
            pointList = new List<Vector2>(128);
        }

        public void Begin(bool center)
        {
            if (hasBegun)
            {
                throw new Exception("You must call End() before calling Begin()");
            }

            hasBegun = true;
            pointList.Clear();
            this.center = center;
        }

        public void Add(Vector2 point)
        {
            if (!hasBegun)
            {
                throw new Exception("You must call Begin() before adding points");
            }

            pointList.Add(point);
        }

        public void End()
        {
            if (!hasBegun)
            {
                throw new Exception("You must call Begin() before calling End()");
            }

            hasBegun = false;
            points = pointList.ToArray();
            count = points.Length;

            if (center)
            {
                CenterAtZero();
            }
        }

        public Shape Clone()
        {
            Shape clone = new Shape
            {
                count = points.Length,
                points = new Vector2[points.Length]
            };

            points.CopyTo(clone.points, 0);
            return clone; 
        }

        public Vector2 GetCenter()
        {
            float x = 0;
            float y = 0;

            for (int i = 0; i < count; i++)
            {
                x += points[i].X;
                y += points[i].Y;
            }

            x /= count;
            y /= count;

            return new Vector2(x, y);
        }

        public void CenterAtZero()
        {
            float x = 0;
            float y = 0;
           
            for (int i = 0; i < count; i++)
            {
                x += points[i].X;
                y += points[i].Y;
            }

            x /= count;
            y /= count;

            for (int i = 0; i < count; i++)
            {
                points[i].X -= x;
                points[i].Y -= y;
            }
        }

        public static void Transform(ref Vector2[] points, ref Vector2 position, float angle, ref Vector2 scale, out Vector2[] list)
        {
            int count = points.Length;
            Vector2[] array = new Vector2[count];

            for (int i = 0; i < count; i++)
            {
                float x = points[i].X * scale.X;
                float y = points[i].Y * scale.Y;
                float c = (float)Math.Cos(angle);
                float s = (float)Math.Sin(angle);
                array[i].X = (c * x) - (s * y) + position.X;
                array[i].Y = (c * y) + (s * x) + position.Y;
            }

            list = array;
        }
    }
}