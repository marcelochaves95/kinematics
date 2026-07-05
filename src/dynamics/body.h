#pragma once

#include <collision/aabb.h>
#include <collision/point_mass.h>
#include <collision/shape.h>
#include <math/vector2.h>
#include <Utils/Bitmask.h>
#include <vector>

namespace kinematics
{
    class Body
    {
    public:
        int count = 0;
        bool is_static = false;
        Vector2 position;
        Vector2 velocity;
        Vector2 gravity = Vector2::ZERO;
        std::vector<PointMassPtr> point_mass_list;
        Shape current_shape;

        AABB aabb;
        Bitmask bitmask_x;
        Bitmask bitmask_y;

        Body(const Shape& shape, float mass);
        virtual ~Body() = default;

        virtual void apply_internal_forces(double elapsed);

        void update_body_position_velocity_force();
        void rotate_shape(double elapsed);
        void update(double elapsed);

        bool contains(const Vector2& point) const;
        float get_closest_point(Vector2 point, Vector2& closest, Vector2& normal,
            int& pointA, int& pointB, float& edgeD);
        float get_closest_point_on_edge(Vector2 point, int edgeNum, Vector2& hitPt,
            Vector2& normal, float& edgeD);
        float get_closest_point_on_edge_squared(Vector2 point, int edgeNum, Vector2& hitPt,
            Vector2& normal, float& edgeD);
        PointMassPtr get_closest_point_mass(Vector2 point, float& dist);
        void apply_force(const Vector2& point, const Vector2& force);

    protected:
        static constexpr float DAMPING = 0.999f;

    private:
        void update_point_masses(double elapsed);
        void update_aabb(double elapsed);
        void set_body_position_velocity_force(Vector2 position, Vector2 velocity, Vector2 force);
        void get_body_position_velocity_force(Vector2& position, Vector2& velocity, Vector2& force);

        bool _is_dirty = true;
        Vector2 _force;
        Shape _base_shape;
    };
}
