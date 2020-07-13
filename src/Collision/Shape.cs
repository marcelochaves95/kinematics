using System;
using System.Collections.Generic;
using Kinematics.Common;

namespace Kinematics.Collision
{
    public class Shape
    {
        public Vector2[] Points;
        public int Count;
        private bool _hasBegun;
        private readonly List<Vector2> _pointList;
        private bool _center;

        public Shape()
        {
            _pointList = new List<Vector2>(128);
        }

        public void Begin(bool center)
        {
            if (_hasBegun)
            {
                throw new Exception("You must call End() before calling Begin()");
            }

            _hasBegun = true;
            _pointList.Clear();
            _center = center;
        }

        public void Add(Vector2 point)
        {
            if (!_hasBegun)
            {
                throw new Exception("You must call Begin() before adding points");
            }

            _pointList.Add(point);
        }

        public void End()
        {
            if (!_hasBegun)
            {
                throw new Exception("You must call Begin() before calling End()");
            }

            _hasBegun = false;
            Points = _pointList.ToArray();
            Count = Points.Length;

            if (_center)
            {
                CenterAtZero();
            }
        }

        public Shape Clone()
        {
            Shape clone = new Shape
            {
                Count = Points.Length,
                Points = new Vector2[Points.Length]
            };

            Points.CopyTo(clone.Points, 0);
            return clone; 
        }

        public Vector2 GetCenter()
        {
            float x = 0;
            float y = 0;

            for (int i = 0; i < Count; i++)
            {
                x += Points[i].X;
                y += Points[i].Y;
            }

            x /= Count;
            y /= Count;

            return new Vector2(x, y);
        }

        public void CenterAtZero()
        {
            float x = 0;
            float y = 0;

            for (int i = 0; i < Count; i++)
            {
                x += Points[i].X;
                y += Points[i].Y;
            }

            x /= Count;
            y /= Count;

            for (int i = 0; i < Count; i++)
            {
                Points[i].X -= x;
                Points[i].Y -= y;
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
                float c = Mathf.Cos(angle);
                float s = Mathf.Sin(angle);
                array[i].X = (c * x) - (s * y) + position.X;
                array[i].Y = (c * y) + (s * x) + position.Y;
            }

            list = array;
        }
    }
}