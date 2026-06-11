#pragma once

#include <Collision/AABB.h>
#include <Collision/Chain.h>
#include <Collision/CollisionInfo.h>
#include <Dynamics/Body.h>
#include <Math/Vector2.h>
#include <functional>
#include <memory>
#include <random>
#include <vector>

// Port of Utils/KinematicsController.cs
//
// List<Body>/List<Chain> become vector<shared_ptr<...>>. Action<...> callbacks
// become std::function. The C# Random (only used by MoveDistantBodies) becomes a
// fixed-seed std::mt19937 — deterministic, off the parity-critical path.

namespace kinematics
{
    class KinematicsController
    {
    public:
        std::vector<std::shared_ptr<Chain>> ChainList;
        std::vector<std::shared_ptr<Body>> BodyList;
        int PenetrationCount = 0;
        float PenetrationThreshold = 0.015f;
        float Friction = 1.9f;
        float Elasticity = 1.5f;
        AABB Aabb;
        Vector2 Size;
        Vector2 Cell;
        std::function<void(Body&, Body&)> OnAABBCollision;
        std::function<void(Body&, Body&, const CollisionInfo&)> OnCollision;
        std::function<void(float, Body&, Body&)> OnPenetration;

        KinematicsController() = default;

        void Add(const std::shared_ptr<Chain>& chain);
        void Remove(const std::shared_ptr<Chain>& chain);
        void Add(const std::shared_ptr<Body>& body);
        void Remove(const std::shared_ptr<Body>& body);

        void SetWorldLimits(Vector2 min, Vector2 max);
        void UpdateBitmask(Body& body);
        bool IsPointInsideAnyBody(Vector2 point);
        void Initialize();
        void MoveDistantBodies(Vector2 position, float near_, float far_);
        void Update(double elapsed);

    private:
        float nextFloat();

        std::vector<CollisionInfo> _collisions;
        bool _initialized = false;
        // While a step is running, Remove(body) is deferred: an in-flight
        // CollisionInfo holds a raw Body* into BodyList, so erasing the body's
        // shared_ptr mid-step (e.g. from a collision callback) could free it and
        // leave that pointer dangling. Deferred removals are applied once the
        // step finishes (the body stays alive in BodyList until then).
        bool _stepping = false;
        std::vector<std::shared_ptr<Body>> _pendingRemove;
        std::mt19937 _random{0u};
    };
}
