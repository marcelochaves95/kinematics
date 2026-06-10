#include <Collision/PointMass.h>
#include <Math/Mathf.h>

namespace kinematics
{
    float PointMass::MaxStep = 0.4f;

    PointMass::PointMass(const Vector2& position, float mass) : Mass(mass), Position(position)
    {
        Velocity = Vector2::Zero;
        Force = Vector2::Zero;
    }

    void PointMass::Update(double elapsed)
    {
        float dt = static_cast<float>(elapsed);
        float k = dt / Mass;
        Velocity.X += Force.X * k;
        Velocity.Y += Force.Y * k;

        // Clamp displacement to MaxStep so a fast point can't tunnel through a thin
        // collider in one step (only does work when the cap is exceeded).
        float stepSq = (Velocity.X * Velocity.X + Velocity.Y * Velocity.Y) * dt * dt;
        if (stepSq > MaxStep * MaxStep)
        {
            float scale = MaxStep / mathf::Sqrt(stepSq);
            Velocity.X *= scale;
            Velocity.Y *= scale;
        }

        Position.X += Velocity.X * dt;
        Position.Y += Velocity.Y * dt;
        Force.X = 0.0f;
        Force.Y = 0.0f;
    }
}
