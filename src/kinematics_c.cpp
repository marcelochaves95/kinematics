#define KN_BUILD
#include <kinematics_c.h>
#include <kn_internal.h>
#include <collision/chain.h>
#include <collision/point_mass.h>
#include <collision/shape.h>
#include <dynamics/body.h>
#include <dynamics/pressure_body.h>
#include <dynamics/spring_body.h>
#include <math/vector2.h>
#include <utils/kinematics_controller.h>
#include <cmath>
#include <limits>
#include <memory>
#include <vector>

using namespace kinematics;

namespace
{
    Shape make_shape(const float* xy, int count)
    {
        Shape s;
        s.begin(true);
        for (int i = 0; i < count; i++)
        {
            s.add(Vector2(xy[2 * i], xy[2 * i + 1]));
        }
        s.end();
        return s;
    }

    bool valid_body(KnWorld* w, int h)
    {
        return w && h >= 0 && static_cast<size_t>(h) < w->ctrl.body_list.size();
    }

    bool valid_chain(KnWorld* w, int h)
    {
        return w && h >= 0 && static_cast<size_t>(h) < w->ctrl.chain_list.size();
    }

    int add_body(KnWorld* w, const std::shared_ptr<Body>& body, float posX, float posY,
        float gravX, float gravY, int isStatic)
    {
        body->position = Vector2(posX, posY);
        body->gravity = Vector2(gravX, gravY);
        body->is_static = isStatic != 0;
        w->ctrl.add(body);
        return static_cast<int>(w->ctrl.body_list.size()) - 1;
    }

    int copy_points(const std::vector<PointMassPtr>& pml, float* outXY, int maxPoints)
    {
        int n = static_cast<int>(pml.size());
        if (n > maxPoints)
        {
            n = maxPoints;
        }
        for (int i = 0; i < n; i++)
        {
            outXY[2 * i] = pml[i]->position.x;
            outXY[2 * i + 1] = pml[i]->position.y;
        }
        return n;
    }
}

KnWorld* kn_world_create(void)
{
    return new KnWorld();
}

void kn_world_destroy(KnWorld* world)
{
    delete world;
}

void kn_world_set_limits(KnWorld* world, float minX, float minY, float maxX, float maxY)
{
    if (world)
    {
        world->ctrl.set_world_limits(Vector2(minX, minY), Vector2(maxX, maxY));
    }
}

void kn_world_set_friction(KnWorld* world, float friction)
{
    if (world)
    {
        world->ctrl.friction = friction;
    }
}

void kn_world_set_elasticity(KnWorld* world, float elasticity)
{
    if (world)
    {
        world->ctrl.elasticity = elasticity;
    }
}

void kn_set_max_step(float maxStep)
{
    PointMass::MAX_STEP = maxStep;
}

void kn_world_set_drag(KnWorld* world, float drag)
{
    if (world)
    {
        world->drag = drag;
    }
}

void kn_world_apply_impulse(KnWorld* world, float wx, float wy, float radius, float strength)
{
    if (!world)
    {
        return;
    }
    for (auto& body : world->ctrl.body_list)
    {
        if (body->is_static)
        {
            continue;
        }
        float dx = body->position.x - wx;
        float dy = body->position.y - wy;
        float dist = std::sqrt(dx * dx + dy * dy);
        if (dist < 0.01f || dist > radius)
        {
            continue;
        }
        float s = strength * (1.0f - dist / radius);
        float vx = dx / dist * s;
        float vy = dy / dist * s;
        for (auto& pm : body->point_mass_list)
        {
            pm->velocity.x += vx;
            pm->velocity.y += vy;
        }
        body->update_body_position_velocity_force();
    }
    for (auto& chain : world->ctrl.chain_list)
    {
        for (size_t i = 1; i + 1 < chain->point_mass_list.size(); i++)
        {
            auto& pm = chain->point_mass_list[i];
            float dx = pm->position.x - wx;
            float dy = pm->position.y - wy;
            float dist = std::sqrt(dx * dx + dy * dy);
            if (dist < 0.01f || dist > radius)
            {
                continue;
            }
            float s = strength * (1.0f - dist / radius);
            pm->velocity.x += dx / dist * s;
            pm->velocity.y += dy / dist * s;
        }
    }
}

int kn_world_add_rigid_body(KnWorld* world, const float* xy, int count, float mass,
    float posX, float posY, float gravX, float gravY, int isStatic)
{
    if (!world || !xy || count < 3)
    {
        return -1;
    }

    try
    {
        auto body = std::make_shared<Body>(make_shape(xy, count), mass);
        return add_body(world, body, posX, posY, gravX, gravY, isStatic);
    }
    catch (...)
    {
        return -1;
    }
}

int kn_world_add_spring_body(KnWorld* world, const float* xy, int count, float mass,
    float edgeK, float edgeDamp, float shapeK, float shapeDamp,
    float posX, float posY, float gravX, float gravY, int isStatic)
{
    if (!world || !xy || count < 3)
    {
        return -1;
    }
    try
    {
        auto body = std::make_shared<SpringBody>(make_shape(xy, count), mass,
            edgeK, edgeDamp, shapeK, shapeDamp);
        return add_body(world, body, posX, posY, gravX, gravY, isStatic);
    }
    catch (...)
    {
        return -1;
    }
}

int kn_world_add_pressure_body(KnWorld* world, const float* xy, int count, float mass,
    float gasPressure, float edgeK, float edgeDamp, float shapeK, float shapeDamp,
    float posX, float posY, float gravX, float gravY, int isStatic)
{
    if (!world || !xy || count < 3)
    {
        return -1;
    }
    try
    {
        auto body = std::make_shared<PressureBody>(make_shape(xy, count), mass, gasPressure,
            edgeK, edgeDamp, shapeK, shapeDamp);
        return add_body(world, body, posX, posY, gravX, gravY, isStatic);
    }
    catch (...)
    {
        return -1;
    }
}

int kn_world_add_chain(KnWorld* world, float ax, float ay, float bx, float by,
    int count, float k, float damping, float mass)
{
    if (!world || count < 1)
    {
        return -1;
    }
    try
    {
        auto from = std::make_shared<PointMass>(Vector2(ax, ay), std::numeric_limits<float>::infinity());
        auto to = std::make_shared<PointMass>(Vector2(bx, by), std::numeric_limits<float>::infinity());
        auto chain = std::make_shared<Chain>(from, to, count, k, damping, mass);
        world->ctrl.add(chain);
        return static_cast<int>(world->ctrl.chain_list.size()) - 1;
    }
    catch (...)
    {
        return -1;
    }
}

void kn_world_set_chain_gravity(KnWorld* world, float gx, float gy)
{
    if (world)
    {
        world->chain_gravity = Vector2(gx, gy);
    }
}

void kn_world_step(KnWorld* world, double dt, int substeps)
{
    if (!world)
    {
        return;
    }
    if (substeps < 1)
    {
        substeps = 1;
    }
    double sub = dt / substeps;
    for (int s = 0; s < substeps; s++)
    {

        for (auto& chain : world->ctrl.chain_list)
        {
            for (size_t i = 1; i + 1 < chain->point_mass_list.size(); i++)
            {
                float m = chain->point_mass_list[i]->mass;
                if (!std::isinf(m))
                {
                    chain->point_mass_list[i]->force.x += world->chain_gravity.x * m;
                    chain->point_mass_list[i]->force.y += world->chain_gravity.y * m;
                }
            }
        }
        if (world->drag != 0.0f)
        {
            for (auto& body : world->ctrl.body_list)
            {
                if (body->is_static)
                {
                    continue;
                }
                for (auto& pm : body->point_mass_list)
                {
                    pm->force.x -= world->drag * pm->velocity.x;
                    pm->force.y -= world->drag * pm->velocity.y;
                }
            }
        }
        world->ctrl.update(sub);
    }
}

int kn_world_penetration_count(KnWorld* world)
{
    return world ? world->ctrl.penetration_count : 0;
}

int kn_world_body_count(KnWorld* world)
{
    return world ? static_cast<int>(world->ctrl.body_list.size()) : 0;
}

int kn_world_chain_count(KnWorld* world)
{
    return world ? static_cast<int>(world->ctrl.chain_list.size()) : 0;
}

int kn_body_is_static(KnWorld* world, int body)
{
    if (!valid_body(world, body))
    {
        return 0;
    }
    return world->ctrl.body_list[body]->is_static ? 1 : 0;
}

int kn_body_point_count(KnWorld* world, int body)
{
    if (!valid_body(world, body))
    {
        return 0;
    }
    return static_cast<int>(world->ctrl.body_list[body]->point_mass_list.size());
}

int kn_body_get_points(KnWorld* world, int body, float* outXY, int maxPoints)
{
    if (!valid_body(world, body) || !outXY || maxPoints < 0)
    {
        return 0;
    }
    return copy_points(world->ctrl.body_list[body]->point_mass_list, outXY, maxPoints);
}

void kn_body_get_position(KnWorld* world, int body, float* outX, float* outY)
{
    if (!valid_body(world, body))
    {
        return;
    }
    if (outX)
    {
        *outX = world->ctrl.body_list[body]->position.x;
    }
    if (outY)
    {
        *outY = world->ctrl.body_list[body]->position.y;
    }
}

void kn_body_apply_force(KnWorld* world, int body, float px, float py, float fx, float fy)
{
    if (!valid_body(world, body))
    {
        return;
    }
    world->ctrl.body_list[body]->apply_force(Vector2(px, py), Vector2(fx, fy));
}

int kn_chain_point_count(KnWorld* world, int chain)
{
    if (!valid_chain(world, chain))
    {
        return 0;
    }
    return static_cast<int>(world->ctrl.chain_list[chain]->point_mass_list.size());
}

int kn_chain_get_points(KnWorld* world, int chain, float* outXY, int maxPoints)
{
    if (!valid_chain(world, chain) || !outXY || maxPoints < 0)
    {
        return 0;
    }
    return copy_points(world->ctrl.chain_list[chain]->point_mass_list, outXY, maxPoints);
}
