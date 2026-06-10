#pragma once

#include <Collision/PointMass.h>
#include <Math/Vector2.h>

// Port of Collision/CollisionInfo.cs (IDisposable dropped; Clear() resets).
//
// BodyA/BodyB are non-owning raw pointers into the controller's BodyList — valid
// for the duration of a frame's collision resolution. PointMass* are shared.

namespace kinematics
{
    class Body;

    struct CollisionInfo
    {
        float EdgeDistance = 0.0f;
        float Penetration = 0.0f;
        Vector2 Normal;
        Vector2 Point;
        Body* BodyA = nullptr;
        Body* BodyB = nullptr;
        PointMassPtr PointMassA;
        PointMassPtr PointMassB;
        PointMassPtr PointMassC;

        void Clear();
    };
}
