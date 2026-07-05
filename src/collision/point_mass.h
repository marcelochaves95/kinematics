#pragma once

#include <math/vector2.h>
#include <memory>

namespace kinematics
{
    struct PointMass
    {
        static float MAX_STEP;

        float mass = 0.0f;
        Vector2 position;
        Vector2 velocity;
        Vector2 force;

        PointMass() = default;
        PointMass(const Vector2& position, float mass);

        void update(double elapsed);
    };

    using PointMassPtr = std::shared_ptr<PointMass>;
}
