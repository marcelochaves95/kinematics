#define KN_BUILD
#include <kinematics_c.h>
#include <kn_internal.h>
#include <Collision/Chain.h>
#include <Collision/PointMass.h>
#include <Collision/Shape.h>
#include <Dynamics/Body.h>
#include <Dynamics/PressureBody.h>
#include <Dynamics/SpringBody.h>
#include <Math/Mathf.h>
#include <Math/Vector2.h>
#include <cmath>
#include <limits>
#include <memory>

// Demo scenes for the C++/WASM build — built in C++ and exposed via extern "C"
// so the JS shell only renders and forwards input: ALL physics AND scene
// construction live in C++.

using namespace kinematics;

namespace
{
    constexpr float INF = std::numeric_limits<float>::infinity();

    void addStaticBody(KnWorld* w, const Shape& s, float cx, float cy)
    {
        auto b = std::make_shared<Body>(s, INF);
        b->Position = Vector2(cx, cy);
        b->IsStatic = true;
        w->ctrl.Add(b);
    }

    // 20 m wide x 1 m tall box, CCW in screen-space (Y-down) for outward normals.
    Shape hWall()
    {
        Shape s;
        s.Begin(true);
        s.Add(Vector2(10.0f, -0.5f));
        s.Add(Vector2(-10.0f, -0.5f));
        s.Add(Vector2(-10.0f, 0.5f));
        s.Add(Vector2(10.0f, 0.5f));
        s.End();
        return s;
    }

    // 3 m wide x 14 m tall box, CCW. Extra thickness keeps the inner face closest.
    Shape vWall()
    {
        Shape s;
        s.Begin(true);
        s.Add(Vector2(-1.5f, -7.0f));
        s.Add(Vector2(-1.5f, 7.0f));
        s.Add(Vector2(1.5f, 7.0f));
        s.Add(Vector2(1.5f, -7.0f));
        s.End();
        return s;
    }

    void addArena(KnWorld* w)
    {
        addStaticBody(w, hWall(), 0.0f, 13.5f);  // bottom
        addStaticBody(w, hWall(), 0.0f, 0.5f);   // top
        addStaticBody(w, vWall(), -10.5f, 7.0f); // left
        addStaticBody(w, vWall(), 10.5f, 7.0f);  // right
    }
}

extern "C"
{
    KN_API void kn_demo_softbody(KnWorld* w)
    {
        if (!w)
        {
            return;
        }
        w->drag = 0.0f;
        w->chainGravity = Vector2::Zero;

        Shape box;
        box.Begin(true);
        box.Add(Vector2(-1.0f, -1.0f));
        box.Add(Vector2(-1.0f, 1.0f));
        box.Add(Vector2(1.0f, 1.0f));
        box.Add(Vector2(1.0f, -1.0f));
        box.End();

        auto body = std::make_shared<SpringBody>(box, 1.0f, 200.0f, 15.0f, 300.0f, 20.0f);
        body->Position = Vector2(0.0f, 3.0f);
        body->Gravity = Vector2(0.0f, 9.8f);
        w->ctrl.Add(body);

        addArena(w);
    }

    KN_API void kn_demo_pressure(KnWorld* w)
    {
        if (!w)
        {
            return;
        }
        w->drag = 2.0f; // linear drag (the C# PreUpdate) so the balloon settles
        w->chainGravity = Vector2::Zero;

        const int N = 12;
        const float radius = 1.5f;
        Shape ball;
        ball.Begin(true);
        for (int i = 0; i < N; i++)
        {
            float a = 2.0f * mathf::PI * i / N;
            ball.Add(Vector2(radius * mathf::Cos(a), -radius * mathf::Sin(a)));
        }
        ball.End();

        auto body = std::make_shared<PressureBody>(ball, 0.5f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
        body->Position = Vector2(0.0f, 4.5f);
        body->Gravity = Vector2(0.0f, 9.8f);
        w->ctrl.Add(body);

        addArena(w);
    }

    KN_API void kn_demo_chain(KnWorld* w)
    {
        if (!w)
        {
            return;
        }
        w->drag = 0.0f;
        w->chainGravity = Vector2(0.0f, 9.8f); // chains have no built-in gravity

        auto anchorA = std::make_shared<PointMass>(Vector2(-5.0f, 2.0f), INF);
        auto anchorB = std::make_shared<PointMass>(Vector2(5.0f, 2.0f), INF);
        auto chain = std::make_shared<Chain>(anchorA, anchorB, 20, 120.0f, 6.0f, 0.25f);

        for (Spring& s : chain->SpringList)
        {
            s.D *= 1.5f; // rest length 1.5x spacing -> sags into a catenary
        }

        // Pre-simulate to settle the chain into its catenary before display.
        chain->Damping = 0.96f;
        const double dt = 1.0 / 60.0;
        const float g = 9.8f;
        for (int step = 0; step < 300; step++)
        {
            for (size_t i = 1; i + 1 < chain->PointMassList.size(); i++)
            {
                chain->PointMassList[i]->Force.Y += g * chain->PointMassList[i]->Mass;
            }
            chain->Update(dt);
        }
        // Runtime damping tuned for 4 substeps/frame (0.90^(1/4)).
        chain->Damping = 0.974f;

        w->ctrl.Add(chain);
        addArena(w);
    }
}
