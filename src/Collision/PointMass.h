#pragma once

#include <Math/Vector2.h>
#include <memory>

// Port of Collision/PointMass.cs

namespace kinematics
{
    struct PointMass
    {
        // Anti-tunneling cap: the largest distance a point may travel in a single
        // integration (sub)step. A point moving faster than this would jump across
        // a thin collider between samples and never register as "inside" it, so the
        // velocity is clamped to bound the per-step displacement. 0.4 m is below
        // half the thinnest wall (1 m) in the demo arena. Set very large to disable.
        static float MaxStep;

        float Mass = 0.0f;
        Vector2 Position;
        Vector2 Velocity;
        Vector2 Force;

        PointMass() = default;
        PointMass(const Vector2& position, float mass);

        void Update(double elapsed);
    };

    // C# PointMass is a reference type: springs, chains, bodies and collision info
    // all share the SAME instances and mutate them in place. shared_ptr reproduces
    // that shared-identity semantics exactly.
    using PointMassPtr = std::shared_ptr<PointMass>;
}
