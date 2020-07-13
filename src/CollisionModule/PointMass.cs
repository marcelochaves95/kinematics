using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class PointMass
    {
        public float mass;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 force;

        public PointMass() { }

        public PointMass(Vector2 position, float mass)
        {
            this.mass = mass;
            this.position = position;
            velocity = force = Vector2.Zero;
        }

        public override string ToString()
        {
            return $"{{position:[{position}] velocity:[{velocity}] force:[{force}]}}";
        }

        public void Update(double elapsed)
        {
            float k = (float) elapsed / mass;

            velocity.X += (force.X * k);
            velocity.Y += (force.Y * k);

            position.X += (velocity.X * k);
            position.Y += (velocity.Y * k);

            force.X = 0f;
            force.Y = 0f;
        }
    }
}