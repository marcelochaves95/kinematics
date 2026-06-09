#pragma once

#include <memory>

#include "Collision/PointMass.hpp"
#include "Math/Vector2.hpp"

// Port of src/Collision/CollisionInfo.cs (IDisposable dropped; Clear() resets).
//
// BodyA/BodyB are non-owning raw pointers into the controller's BodyList — valid
// for the duration of a frame's collision resolution. PointMass* are shared.

namespace kinematics {

class Body; // forward declaration (only used by pointer here)

struct CollisionInfo {
    float EdgeDistance = 0.0f;
    float Penetration = 0.0f;
    Vector2 Normal;
    Vector2 Point;
    Body* BodyA = nullptr;
    Body* BodyB = nullptr;
    PointMassPtr PointMassA;
    PointMassPtr PointMassB;
    PointMassPtr PointMassC;

    void Clear() {
        BodyA = nullptr;
        BodyB = nullptr;
        PointMassA = std::make_shared<PointMass>();
        PointMassB = std::make_shared<PointMass>();
        PointMassC = std::make_shared<PointMass>();
        Point = Vector2::Zero;
        Normal = Vector2::Zero;
        EdgeDistance = 0.0f;
        Penetration = 0.0f;
    }
};

} // namespace kinematics
