using Kinematics.Math;

namespace Kinematics.Collision
{
    public class PointMass
    {
        // Anti-tunneling cap: the largest distance a point may travel in a single
        // integration (sub)step. A point moving faster than this would jump across
        // a thin collider between samples and never register as "inside" it, so we
        // clamp the velocity to bound the per-step displacement. 0.4 m is below half
        // the thinnest wall (1 m) in the demo arena. Set very large to disable.
        public static float MaxStep = 0.4f;

        public float Mass;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Force;

        public PointMass()
        {
            //
        }

        public PointMass(Vector2 position, float mass)
        {
            Mass = mass;
            Position = position;
            Velocity = Force = Vector2.Zero;
        }

        public override string ToString()
        {
            return $"{{Position: [{Position}] Velocity: [{Velocity}] Force: [{Force}]}}";
        }

        public void Update(double elapsed)
        {
            float dt = (float) elapsed;
            float k = dt / Mass;
            Velocity.X += Force.X * k;
            Velocity.Y += Force.Y * k;

            // Clamp displacement to MaxStep so a fast point can't tunnel through
            // a thin collider in one step (only does work when the cap is exceeded).
            float stepSq = (Velocity.X * Velocity.X + Velocity.Y * Velocity.Y) * dt * dt;
            if (stepSq > MaxStep * MaxStep)
            {
                float scale = MaxStep / (Mathf.Sqrt(stepSq));
                Velocity.X *= scale;
                Velocity.Y *= scale;
            }

            Position.X += Velocity.X * dt;
            Position.Y += Velocity.Y * dt;
            Force.X = 0f;
            Force.Y = 0f;
        }
    }
}