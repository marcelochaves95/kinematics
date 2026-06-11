#pragma once

#include <Collision/CollisionInfo.h>
#include <vector>

// Port of Collision/Collision.cs — narrowphase: for each vertex of bodyA inside
// bodyB, find the closest edge of bodyB and emit a CollisionInfo.

namespace kinematics
{
    class Body;

    class Collision
    {
    public:
        // Appends the contacts (vertices of bodyA inside bodyB) onto `out`,
        // writing directly into the caller's buffer to avoid a per-pair heap
        // allocation + copy each frame.
        static void Intersects(Body& bodyA, Body& bodyB, std::vector<CollisionInfo>& out);
    };
}
