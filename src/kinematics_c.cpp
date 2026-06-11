#define KN_BUILD
#include <kinematics_c.h>
#include <kn_internal.h>
#include <Collision/Chain.h>
#include <Collision/PointMass.h>
#include <Collision/Shape.h>
#include <Dynamics/Body.h>
#include <Dynamics/PressureBody.h>
#include <Dynamics/SpringBody.h>
#include <Math/Vector2.h>
#include <Utils/KinematicsController.h>
#include <cmath>
#include <limits>
#include <memory>
#include <vector>

// Implementation of the Kinematics C ABI (see kinematics_c.h). Bridges the flat
// C surface to the C++ classes; built into the kinematics_c shared/static library.

using namespace kinematics;

namespace
{
    Shape makeShape(const float* xy, int count)
    {
        Shape s;
        s.Begin(true); // center on centroid; the body is then placed via posX/posY
        for (int i = 0; i < count; i++)
        {
            s.Add(Vector2(xy[2 * i], xy[2 * i + 1]));
        }
        s.End();
        return s;
    }

    bool validBody(KnWorld* w, int h)
    {
        return w && h >= 0 && static_cast<size_t>(h) < w->ctrl.BodyList.size();
    }

    bool validChain(KnWorld* w, int h)
    {
        return w && h >= 0 && static_cast<size_t>(h) < w->ctrl.ChainList.size();
    }

    int addBody(KnWorld* w, const std::shared_ptr<Body>& body, float posX, float posY,
        float gravX, float gravY, int isStatic)
    {
        body->Position = Vector2(posX, posY);
        body->Gravity = Vector2(gravX, gravY);
        body->IsStatic = isStatic != 0;
        w->ctrl.Add(body);
        return static_cast<int>(w->ctrl.BodyList.size()) - 1;
    }

    int copyPoints(const std::vector<PointMassPtr>& pml, float* outXY, int maxPoints)
    {
        int n = static_cast<int>(pml.size());
        if (n > maxPoints)
        {
            n = maxPoints;
        }
        for (int i = 0; i < n; i++)
        {
            outXY[2 * i] = pml[i]->Position.X;
            outXY[2 * i + 1] = pml[i]->Position.Y;
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
        world->ctrl.SetWorldLimits(Vector2(minX, minY), Vector2(maxX, maxY));
    }
}

void kn_world_set_friction(KnWorld* world, float friction)
{
    if (world)
    {
        world->ctrl.Friction = friction;
    }
}

void kn_world_set_elasticity(KnWorld* world, float elasticity)
{
    if (world)
    {
        world->ctrl.Elasticity = elasticity;
    }
}

void kn_set_max_step(float maxStep)
{
    PointMass::MaxStep = maxStep;
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
    for (auto& body : world->ctrl.BodyList)
    {
        if (body->IsStatic)
        {
            continue;
        }
        float dx = body->Position.X - wx;
        float dy = body->Position.Y - wy;
        float dist = std::sqrt(dx * dx + dy * dy);
        if (dist < 0.01f || dist > radius)
        {
            continue;
        }
        float s = strength * (1.0f - dist / radius);
        float vx = dx / dist * s;
        float vy = dy / dist * s;
        for (auto& pm : body->PointMassList)
        {
            pm->Velocity.X += vx;
            pm->Velocity.Y += vy;
        }
        body->UpdateBodyPositionVelocityForce();
    }
    for (auto& chain : world->ctrl.ChainList)
    {
        for (size_t i = 1; i + 1 < chain->PointMassList.size(); i++)
        {
            auto& pm = chain->PointMassList[i];
            float dx = pm->Position.X - wx;
            float dy = pm->Position.Y - wy;
            float dist = std::sqrt(dx * dx + dy * dy);
            if (dist < 0.01f || dist > radius)
            {
                continue;
            }
            float s = strength * (1.0f - dist / radius);
            pm->Velocity.X += dx / dist * s;
            pm->Velocity.Y += dy / dist * s;
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
    // Never let a C++ exception (e.g. from Shape building) cross the C boundary —
    // that is UB for FFI hosts. Report failure as -1 instead. (Native builds have
    // exceptions enabled; the WASM build only feeds valid shapes, so none throw.)
    try
    {
        auto body = std::make_shared<Body>(makeShape(xy, count), mass);
        return addBody(world, body, posX, posY, gravX, gravY, isStatic);
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
        auto body = std::make_shared<SpringBody>(makeShape(xy, count), mass,
            edgeK, edgeDamp, shapeK, shapeDamp);
        return addBody(world, body, posX, posY, gravX, gravY, isStatic);
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
        auto body = std::make_shared<PressureBody>(makeShape(xy, count), mass, gasPressure,
            edgeK, edgeDamp, shapeK, shapeDamp);
        return addBody(world, body, posX, posY, gravX, gravY, isStatic);
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
        world->ctrl.Add(chain);
        return static_cast<int>(world->ctrl.ChainList.size()) - 1;
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
        world->chainGravity = Vector2(gx, gy);
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
        // PreUpdate forces, re-applied each substep (mirrors the demo's scenes):
        // chain gravity + linear drag on non-static body point masses.
        for (auto& chain : world->ctrl.ChainList)
        {
            for (size_t i = 1; i + 1 < chain->PointMassList.size(); i++)
            {
                float m = chain->PointMassList[i]->Mass;
                if (!std::isinf(m))
                {
                    chain->PointMassList[i]->Force.X += world->chainGravity.X * m;
                    chain->PointMassList[i]->Force.Y += world->chainGravity.Y * m;
                }
            }
        }
        if (world->drag != 0.0f)
        {
            for (auto& body : world->ctrl.BodyList)
            {
                if (body->IsStatic)
                {
                    continue;
                }
                for (auto& pm : body->PointMassList)
                {
                    pm->Force.X -= world->drag * pm->Velocity.X;
                    pm->Force.Y -= world->drag * pm->Velocity.Y;
                }
            }
        }
        world->ctrl.Update(sub);
    }
}

int kn_world_penetration_count(KnWorld* world)
{
    return world ? world->ctrl.PenetrationCount : 0;
}

int kn_world_body_count(KnWorld* world)
{
    return world ? static_cast<int>(world->ctrl.BodyList.size()) : 0;
}

int kn_world_chain_count(KnWorld* world)
{
    return world ? static_cast<int>(world->ctrl.ChainList.size()) : 0;
}

int kn_body_is_static(KnWorld* world, int body)
{
    if (!validBody(world, body))
    {
        return 0;
    }
    return world->ctrl.BodyList[body]->IsStatic ? 1 : 0;
}

int kn_body_point_count(KnWorld* world, int body)
{
    if (!validBody(world, body))
    {
        return 0;
    }
    return static_cast<int>(world->ctrl.BodyList[body]->PointMassList.size());
}

int kn_body_get_points(KnWorld* world, int body, float* outXY, int maxPoints)
{
    if (!validBody(world, body) || !outXY || maxPoints < 0)
    {
        return 0;
    }
    return copyPoints(world->ctrl.BodyList[body]->PointMassList, outXY, maxPoints);
}

void kn_body_get_position(KnWorld* world, int body, float* outX, float* outY)
{
    if (!validBody(world, body))
    {
        return;
    }
    if (outX)
    {
        *outX = world->ctrl.BodyList[body]->Position.X;
    }
    if (outY)
    {
        *outY = world->ctrl.BodyList[body]->Position.Y;
    }
}

void kn_body_apply_force(KnWorld* world, int body, float px, float py, float fx, float fy)
{
    if (!validBody(world, body))
    {
        return;
    }
    world->ctrl.BodyList[body]->ApplyForce(Vector2(px, py), Vector2(fx, fy));
}

int kn_chain_point_count(KnWorld* world, int chain)
{
    if (!validChain(world, chain))
    {
        return 0;
    }
    return static_cast<int>(world->ctrl.ChainList[chain]->PointMassList.size());
}

int kn_chain_get_points(KnWorld* world, int chain, float* outXY, int maxPoints)
{
    if (!validChain(world, chain) || !outXY || maxPoints < 0)
    {
        return 0;
    }
    return copyPoints(world->ctrl.ChainList[chain]->PointMassList, outXY, maxPoints);
}
