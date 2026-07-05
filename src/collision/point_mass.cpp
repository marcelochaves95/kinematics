#include <collision/point_mass.h>
#include <Math/Mathf.h>

namespace kinematics
{
    float PointMass::MAX_STEP = 0.4f;

    PointMass::PointMass(const Vector2& position, float mass) : mass(mass), position(position)
    {
        velocity = Vector2::Zero;
        force = Vector2::Zero;
    }

    void PointMass::update(double elapsed)
    {
        float dt = static_cast<float>(elapsed);
        float k = mass > 0.0f ? dt / mass : 0.0f;
        velocity.X += force.X * k;
        velocity.Y += force.Y * k;

        float stepSq = (velocity.X * velocity.X + velocity.Y * velocity.Y) * dt * dt;
        if (stepSq > MAX_STEP * MAX_STEP)
        {
            float scale = MAX_STEP / mathf::Sqrt(stepSq);
            velocity.X *= scale;
            velocity.Y *= scale;
        }

        position.X += velocity.X * dt;
        position.Y += velocity.Y * dt;
        force.X = 0.0f;
        force.Y = 0.0f;
    }
}
