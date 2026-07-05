#pragma once

#include <collision/point_mass.h>
#include <math/vector2.h>

namespace kinematics
{
    class Spring
    {
    public:
        float damping;
        float k;
        float d;
        PointMassPtr point_mass_a;
        PointMassPtr point_mass_b;

        Spring(PointMassPtr a, PointMassPtr b, float k, float damping);
        Spring(PointMassPtr a, PointMassPtr b, float k, float damping, float length);

        void reset();

        static Vector2 spring_force(const Spring& spring);
        static Vector2 spring_force(const Vector2& posA, const Vector2& velA,
            const Vector2& posB, const Vector2& velB,
            float springD, float springK, float damping);
    };
}
