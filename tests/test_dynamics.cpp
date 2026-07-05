#include <test_framework.h>

#include <cmath>

#include <collision/shape.h>
#include <dynamics/body.h>
#include <dynamics/pressure_body.h>
#include <dynamics/spring_body.h>
#include <math/vector2.h>

using namespace kinematics;

namespace {
constexpr double EPS = 1e-6;

Shape unit_square() {
    Shape s;
    s.begin(true);
    s.add(Vector2(-0.5f, -0.5f));
    s.add(Vector2(0.5f, -0.5f));
    s.add(Vector2(0.5f, 0.5f));
    s.add(Vector2(-0.5f, 0.5f));
    s.end();
    return s;
}

bool all_finite(const Body& b) {
    for (const auto& pm : b.point_mass_list) {
        if (!std::isfinite(pm->position.x) || !std::isfinite(pm->position.y)) {
            return false;
        }
    }
    return true;
}

float max_radius(const Body& b) {
    float m = 0.0f;
    for (const auto& pm : b.point_mass_list) {
        float d = (pm->position - b.position).length();
        if (d > m) m = d;
    }
    return m;
}
}

TEST_CASE(body_construction) {
    Shape s = unit_square();
    Body b(s, 1.0f);
    CHECK(b.count == 4);
    CHECK(b.point_mass_list.size() == 4);
    CHECK(b.current_shape.count == 4);
}

TEST_CASE(springbody_construction) {
    Shape s = unit_square();
    SpringBody b(s, 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    CHECK(b.count == 4);
    CHECK(b.point_mass_list.size() == 4);
}

TEST_CASE(springbody_falls_under_gravity) {
    Shape s = unit_square();
    SpringBody b(s, 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.position = Vector2(0.0f, 0.0f);
    b.gravity = Vector2(0.0f, 9.8f);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 60; i++) {
        b.update(dt);
    }

    CHECK(all_finite(b));
    CHECK(b.position.y > 1.0f);
}

TEST_CASE(springbody_stays_cohesive) {
    Shape s = unit_square();
    SpringBody b(s, 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.position = Vector2(0.0f, 0.0f);
    b.gravity = Vector2(0.0f, 9.8f);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 120; i++) {
        b.update(dt);
    }

    CHECK(all_finite(b));

    float r = max_radius(b);
    CHECK(r > 0.3f);
    CHECK(r < 2.0f);
}

TEST_CASE(pressurebody_holds_together) {
    Shape s = unit_square();
    PressureBody b(s, 1.0f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.position = Vector2(0.0f, 0.0f);
    b.gravity = Vector2(0.0f, 9.8f);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 120; i++) {
        b.update(dt);
    }

    CHECK(all_finite(b));
    float r = max_radius(b);
    CHECK(r > 0.3f);
    CHECK(r < 2.5f);
}

TEST_CASE(pressurebody_stable_without_gravity) {
    Shape s = unit_square();
    PressureBody b(s, 1.0f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.position = Vector2(0.0f, 0.0f);

    const double dt = 1.0 / 60.0;
    float r0 = max_radius(b);
    for (int i = 0; i < 120; i++) {
        b.update(dt);
    }

    CHECK(all_finite(b));
    float r = max_radius(b);
    CHECK_NEAR(r, r0, 0.5);
}

TEST_CASE(pressurebody_degenerate_volume_is_safe) {

    Shape s;
    s.begin(true);
    s.add(Vector2(0.0f, 0.0f));
    s.add(Vector2(0.0f, 0.0f));
    s.add(Vector2(0.0f, 0.0f));
    s.end();
    PressureBody b(s, 1.0f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.gravity = Vector2(0.0f, 9.8f);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 60; i++) {
        b.update(dt);
    }
    CHECK(all_finite(b));
}

TEST_CASE(virtual_dispatch_through_base_ref) {
    Shape s = unit_square();
    PressureBody pb(s, 1.0f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    Body& base = pb;
    base.gravity = Vector2(0.0f, 9.8f);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 30; i++) {
        base.update(dt);
    }
    CHECK(all_finite(pb));
    (void) EPS;
}
