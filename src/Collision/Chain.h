#pragma once

#include <Collision/PointMass.h>
#include <Collision/Spring.h>
#include <vector>

// Port of Collision/Chain.cs
//
// NOTE: the ctor's `damping` arg is the SPRING damping coefficient (passed to
// every Spring). The per-frame velocity multiplier is the separate `Damping`
// field, which the ctor hardcodes to 0.99 (overriding the arg), matching the C#.

namespace kinematics
{
    class Chain
    {
    public:
        float Damping;
        std::vector<PointMassPtr> PointMassList;
        std::vector<Spring> SpringList;

        Chain(PointMassPtr from, PointMassPtr to, int count, float k, float damping, float mass);

        void Update(double elapsed);
    };
}
