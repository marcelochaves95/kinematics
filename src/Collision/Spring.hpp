#pragma once

#include <utility>

#include "Collision/PointMass.hpp"
#include "Math/Mathf.hpp"
#include "Math/Vector2.hpp"

// Port of src/Collision/Spring.cs
//
// NOTE: `D` is the spring's REST LENGTH, not a damping coefficient (the damping
// coefficient is the separate `Damping` field). The C# `ref Spring`/`out Vector2`
// API becomes value-returning here.
//
// PARITY: the C# computes the segment length with Mathf.Pow(x, 2) (which routes
// through double pow()), NOT x*x. We keep mathf::Pow to match the rounding.

namespace kinematics {

class Spring {
public:
    float Damping;
    float K;
    float D;
    PointMassPtr PointMassA;
    PointMassPtr PointMassB;

    // 4-arg: rest length defaults to the current A–B distance (via Reset()).
    Spring(PointMassPtr a, PointMassPtr b, float k, float damping)
        : Spring(std::move(a), std::move(b), k, damping, 0.0f) {
        Reset();
    }

    // 5-arg: rest length given explicitly.
    Spring(PointMassPtr a, PointMassPtr b, float k, float damping, float length)
        : Damping(damping), K(k), D(length),
          PointMassA(std::move(a)), PointMassB(std::move(b)) {}

    void Reset() {
        D = (PointMassA->Position - PointMassB->Position).Length();
    }

    static Vector2 SpringForce(const Spring& spring) {
        return SpringForce(spring.PointMassA->Position, spring.PointMassA->Velocity,
                           spring.PointMassB->Position, spring.PointMassB->Velocity,
                           spring.D, spring.K, spring.Damping);
    }

    static Vector2 SpringForce(const Vector2& posA, const Vector2& velA,
                               const Vector2& posB, const Vector2& velB,
                               float springD, float springK, float damping) {
        float BtoAX = posA.X - posB.X;
        float BtoAY = posA.Y - posB.Y;
        float distance = mathf::Sqrt(mathf::Pow(BtoAX, 2.0f) + mathf::Pow(BtoAY, 2.0f));
        Vector2 forceOut;
        if (distance > mathf::Epsilon) {
            BtoAX /= distance;
            BtoAY /= distance;
        } else {
            forceOut.X = 0.0f;
            forceOut.Y = 0.0f;
            return forceOut;
        }

        distance = springD - distance;
        float relVelX = velA.X - velB.X;
        float relVelY = velA.Y - velB.Y;
        float totalRelVel = relVelX * BtoAX + relVelY * BtoAY;
        forceOut.X = BtoAX * (distance * springK - totalRelVel * damping);
        forceOut.Y = BtoAY * (distance * springK - totalRelVel * damping);
        return forceOut;
    }
};

} // namespace kinematics
