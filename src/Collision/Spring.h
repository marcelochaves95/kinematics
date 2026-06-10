#pragma once

#include <Collision/PointMass.h>
#include <Math/Vector2.h>

// Port of Collision/Spring.cs
//
// NOTE: `D` is the spring's REST LENGTH, not a damping coefficient (the damping
// coefficient is the separate `Damping` field). The C# `ref/out` API returns by
// value here. PARITY: segment length uses mathf::Pow(x, 2) (routes through
// double pow()), NOT x*x — to match the C# rounding.

namespace kinematics
{
    class Spring
    {
    public:
        float Damping;
        float K;
        float D;
        PointMassPtr PointMassA;
        PointMassPtr PointMassB;

        Spring(PointMassPtr a, PointMassPtr b, float k, float damping);
        Spring(PointMassPtr a, PointMassPtr b, float k, float damping, float length);

        void Reset();

        static Vector2 SpringForce(const Spring& spring);
        static Vector2 SpringForce(const Vector2& posA, const Vector2& velA,
            const Vector2& posB, const Vector2& velB,
            float springD, float springK, float damping);
    };
}
