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
        static std::vector<CollisionInfo> Intersects(Body& bodyA, Body& bodyB);
    };
}
