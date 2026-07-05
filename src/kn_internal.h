#pragma once

#include <math/vector2.h>
#include <utils/kinematics_controller.h>

struct KnWorld
{
    kinematics::KinematicsController ctrl;
    kinematics::Vector2 chain_gravity;
    float drag = 0.0f;
};
