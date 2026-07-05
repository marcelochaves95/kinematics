#pragma once

#include <collision/collision_info.h>
#include <vector>

namespace kinematics
{
    class Body;

    class Collision
    {
    public:
        static void intersects(Body& bodyA, Body& bodyB, std::vector<CollisionInfo>& out);
    };
}
