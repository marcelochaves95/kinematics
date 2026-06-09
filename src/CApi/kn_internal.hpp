#pragma once

// Internal definition of the opaque KnWorld (shared between the C ABI
// implementation and the demo scene builders). NOT part of the public C header.

#include "Math/Vector2.hpp"
#include "Utils/KinematicsController.hpp"

struct KnWorld {
    kinematics::KinematicsController ctrl;
    kinematics::Vector2 chainGravity;        // applied to chain interior masses each step
    float drag = 0.0f;                       // linear drag on non-static body PMs each step
};
