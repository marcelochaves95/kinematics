#pragma once

// Umbrella header: a single include that pulls in the whole public C++ API.
//
//     #include "kinematics.hpp"
//     using namespace kinematics;
//
// (Prefer this for application code. Individual headers can still be included
// directly if you want to keep translation-unit dependencies tight.)

// Math
#include "Math/Mathf.hpp"
#include "Math/Vector2.hpp"
#include "Math/Vector3.hpp"

// Collision primitives
#include "Collision/AABB.hpp"
#include "Collision/Shape.hpp"
#include "Collision/PointMass.hpp"
#include "Collision/Spring.hpp"
#include "Collision/Chain.hpp"
#include "Collision/Collision.hpp"
#include "Collision/CollisionInfo.hpp"

// Dynamics
#include "Dynamics/Body.hpp"
#include "Dynamics/SpringBody.hpp"
#include "Dynamics/PressureBody.hpp"

// Controller
#include "Utils/KinematicsController.hpp"
