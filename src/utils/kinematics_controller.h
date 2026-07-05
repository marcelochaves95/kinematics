#pragma once

#include <collision/aabb.h>
#include <collision/chain.h>
#include <collision/collision_info.h>
#include <dynamics/body.h>
#include <math/vector2.h>
#include <functional>
#include <memory>
#include <random>
#include <vector>

namespace kinematics
{
    class KinematicsController
    {
    public:
        std::vector<std::shared_ptr<Chain>> chain_list;
        std::vector<std::shared_ptr<Body>> body_list;
        int penetration_count = 0;
        float penetration_threshold = 0.015f;
        float friction = 1.9f;
        float elasticity = 1.5f;
        AABB aabb;
        Vector2 size;
        Vector2 cell;
        std::function<void(Body&, Body&)> on_aabb_collision;
        std::function<void(Body&, Body&, const CollisionInfo&)> on_collision;
        std::function<void(float, Body&, Body&)> on_penetration;

        KinematicsController() = default;

        void add(const std::shared_ptr<Chain>& chain);
        void remove(const std::shared_ptr<Chain>& chain);
        void add(const std::shared_ptr<Body>& body);
        void remove(const std::shared_ptr<Body>& body);

        void set_world_limits(Vector2 min, Vector2 max);
        void update_bitmask(Body& body);
        bool is_point_inside_any_body(Vector2 point);
        void initialize();
        void move_distant_bodies(Vector2 position, float near_, float far_);
        void update(double elapsed);

    private:
        float next_float();

        std::vector<CollisionInfo> _collisions;
        bool _initialized = false;
        bool _stepping = false;
        std::vector<std::shared_ptr<Body>> _pending_remove;
        std::mt19937 _random{0u};
    };
}
