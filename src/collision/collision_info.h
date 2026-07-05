#pragma once

#include <collision/point_mass.h>
#include <math/vector2.h>

namespace kinematics
{
    class Body;

    struct CollisionInfo
    {
        float edge_distance = 0.0f;
        float penetration = 0.0f;
        Vector2 normal;
        Vector2 point;
        Body* body_a = nullptr;
        Body* body_b = nullptr;
        PointMassPtr point_mass_a;
        PointMassPtr point_mass_b;
        PointMassPtr point_mass_c;

        void clear();
    };
}
