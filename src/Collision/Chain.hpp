#pragma once

#include <vector>

#include "Collision/PointMass.hpp"
#include "Collision/Spring.hpp"
#include "Math/Vector2.hpp"

// Port of src/Collision/Chain.cs
//
// NOTE: the ctor's `damping` arg is the SPRING damping coefficient (passed to
// every Spring). The per-frame velocity multiplier is the separate `Damping`
// field, which the ctor hardcodes to 0.99 (overriding the arg), matching the C#.

namespace kinematics {

class Chain {
public:
    float Damping;
    std::vector<PointMassPtr> PointMassList;
    std::vector<Spring> SpringList;

    Chain(PointMassPtr from, PointMassPtr to, int count, float k, float damping, float mass) {
        Damping = 0.99f;
        float length = Vector2::Distance(from->Position, to->Position) / static_cast<float>(count);
        Vector2 direction = to->Position - from->Position;
        direction.Normalize();

        PointMassList.reserve(static_cast<size_t>(count) + 1);
        for (int i = 0; i < count + 1; i++) {
            PointMassList.push_back(std::make_shared<PointMass>(
                Vector2(from->Position.X + direction.X * length * i,
                        from->Position.Y + direction.Y * length * i),
                mass));
        }

        // Share the caller's anchor instances at the endpoints.
        PointMassList[0] = from;
        PointMassList[count] = to;

        SpringList.reserve(static_cast<size_t>(count));
        for (int i = 1; i < count + 1; i++) {
            SpringList.emplace_back(PointMassList[i - 1], PointMassList[i], k, damping);
        }
    }

    void Update(double elapsed) {
        for (size_t i = 0; i < SpringList.size(); i++) {
            Spring& spring = SpringList[i];
            Vector2 force = Spring::SpringForce(spring);
            spring.PointMassA->Force.X += force.X;
            spring.PointMassA->Force.Y += force.Y;
            spring.PointMassB->Force.X -= force.X;
            spring.PointMassB->Force.Y -= force.Y;
        }

        // Integrate interior masses only (endpoints 0 and N are fixed anchors).
        for (size_t i = 1; i + 1 < PointMassList.size(); i++) {
            PointMassList[i]->Velocity.X *= Damping;
            PointMassList[i]->Velocity.Y *= Damping;
            PointMassList[i]->Update(elapsed);
        }
    }
};

} // namespace kinematics
