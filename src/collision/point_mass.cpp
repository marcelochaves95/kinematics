#include <collision/point_mass.h>
#include <math/mathf.h>

namespace kinematics
{
    float PointMass::MAX_STEP = 0.4f;

    PointMass::PointMass(const Vector2& position, float mass) : mass(mass), position(position)
    {
        velocity = Vector2::ZERO;
        force = Vector2::ZERO;
    }

    void PointMass::update(double elapsed)
    {
        float dt = static_cast<float>(elapsed);
        float k = mass > 0.0f ? dt / mass : 0.0f;
        velocity.x += force.x * k;
        velocity.y += force.y * k;

        float stepSq = (velocity.x * velocity.x + velocity.y * velocity.y) * dt * dt;
        if (stepSq > MAX_STEP * MAX_STEP)
        {
            float scale = MAX_STEP / mathf::sqrt(stepSq);
            velocity.x *= scale;
            velocity.y *= scale;
        }

        position.x += velocity.x * dt;
        position.y += velocity.y * dt;
        force.x = 0.0f;
        force.y = 0.0f;
    }
}
