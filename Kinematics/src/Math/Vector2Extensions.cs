using Microsoft.Xna.Framework;

namespace Kinematics.Math
{
    public static class Vector2Extensions
    {
        public static Vector3 Vector3FromVector2(this Vector2 source)
        {
            return new Vector3(source.X, source.Y, 0f);
        }

        public static Vector2 Rotate(this Vector2 source, float value)
        {
            float cos = Mathf.Cos(value);
            float sin = Mathf.Sin(value);

            float x = cos * source.X - sin * source.Y;
            float y = cos * source.Y + sin * source.X;

            return new Vector2(x, y);
        }

        public static Vector2 Perpendicular(this Vector2 source)
        {
            return new Vector2(-source.Y, source.X);
        }

        public static bool IsCounterClockwise(this Vector2 source, Vector2 value)
        {
            Vector2 perpendicular = source.Perpendicular();
            float dot = Vector2.Dot(value, perpendicular);
            return dot >= 0f;
        }
    }
}
