using Kinematics.Common;

namespace Kinematics.Collision
{
    public class PointMass
    {
        public float Mass;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Force;

        public PointMass() { }

        public PointMass(Vector2 position, float mass)
        {
            Mass = mass;
            Position = position;
            Velocity = Force = Vector2.Zero;
        }

        public override string ToString()
        {
            return $"{{position:[{Position}] velocity:[{Velocity}] force:[{Force}]}}";
        }

        public void Update(double elapsed)
        {
            float k = (float) elapsed / Mass;

            Velocity.X += (Force.X * k);
            Velocity.Y += (Force.Y * k);

            Position.X += (Velocity.X * k);
            Position.Y += (Velocity.Y * k);

            Force.X = 0f;
            Force.Y = 0f;
        }
    }
}