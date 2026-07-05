#define KN_BUILD
#include <kinematics_c.h>
#include <kn_internal.h>
#include <collision/chain.h>
#include <collision/point_mass.h>
#include <collision/shape.h>
#include <dynamics/body.h>
#include <dynamics/pressure_body.h>
#include <dynamics/spring_body.h>
#include <math/mathf.h>
#include <math/vector2.h>
#include <cmath>
#include <limits>
#include <memory>

using namespace kinematics;

namespace
{
    constexpr float INF = std::numeric_limits<float>::infinity();

    void add_static_body(KnWorld* w, const Shape& s, float cx, float cy)
    {
        auto b = std::make_shared<Body>(s, INF);
        b->position = Vector2(cx, cy);
        b->is_static = true;
        w->ctrl.add(b);
    }

    Shape h_wall()
    {
        Shape s;
        s.begin(true);
        s.add(Vector2(10.0f, -0.5f));
        s.add(Vector2(-10.0f, -0.5f));
        s.add(Vector2(-10.0f, 0.5f));
        s.add(Vector2(10.0f, 0.5f));
        s.end();
        return s;
    }

    Shape v_wall()
    {
        Shape s;
        s.begin(true);
        s.add(Vector2(-1.5f, -7.0f));
        s.add(Vector2(-1.5f, 7.0f));
        s.add(Vector2(1.5f, 7.0f));
        s.add(Vector2(1.5f, -7.0f));
        s.end();
        return s;
    }

    void add_arena(KnWorld* w)
    {
        add_static_body(w, h_wall(), 0.0f, 13.5f);
        add_static_body(w, h_wall(), 0.0f, 0.5f);
        add_static_body(w, v_wall(), -10.5f, 7.0f);
        add_static_body(w, v_wall(), 10.5f, 7.0f);
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
        w->chain_gravity = Vector2::ZERO;

        Shape box;
        box.begin(true);
        box.add(Vector2(-1.0f, -1.0f));
        box.add(Vector2(-1.0f, 1.0f));
        box.add(Vector2(1.0f, 1.0f));
        box.add(Vector2(1.0f, -1.0f));
        box.end();

        auto body = std::make_shared<SpringBody>(box, 1.0f, 200.0f, 15.0f, 300.0f, 20.0f);
        body->position = Vector2(0.0f, 3.0f);
        body->gravity = Vector2(0.0f, 9.8f);
        w->ctrl.add(body);

        add_arena(w);
    }

    KN_API void kn_demo_pressure(KnWorld* w)
    {
        if (!w)
        {
            return;
        }
        w->drag = 2.0f;
        w->chain_gravity = Vector2::ZERO;

        const int N = 12;
        const float radius = 1.5f;
        Shape ball;
        ball.begin(true);
        for (int i = 0; i < N; i++)
        {
            float a = 2.0f * mathf::PI * i / N;
            ball.add(Vector2(radius * mathf::cos(a), -radius * mathf::sin(a)));
        }
        ball.end();

        auto body = std::make_shared<PressureBody>(ball, 0.5f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
        body->position = Vector2(0.0f, 4.5f);
        body->gravity = Vector2(0.0f, 9.8f);
        w->ctrl.add(body);

        add_arena(w);
    }

    KN_API void kn_demo_chain(KnWorld* w)
    {
        if (!w)
        {
            return;
        }
        w->drag = 0.0f;
        w->chain_gravity = Vector2(0.0f, 9.8f);

        auto anchorA = std::make_shared<PointMass>(Vector2(-5.0f, 2.0f), INF);
        auto anchorB = std::make_shared<PointMass>(Vector2(5.0f, 2.0f), INF);
        auto chain = std::make_shared<Chain>(anchorA, anchorB, 20, 120.0f, 6.0f, 0.25f);

        for (Spring& s : chain->spring_list)
        {
            s.d *= 1.5f;
        }

        chain->damping = 0.96f;
        const double dt = 1.0 / 60.0;
        const float g = 9.8f;
        for (int step = 0; step < 300; step++)
        {
            for (size_t i = 1; i + 1 < chain->point_mass_list.size(); i++)
            {
                chain->point_mass_list[i]->force.y += g * chain->point_mass_list[i]->mass;
            }
            chain->update(dt);
        }

        chain->damping = 0.974f;

        w->ctrl.add(chain);
        add_arena(w);
    }
}
