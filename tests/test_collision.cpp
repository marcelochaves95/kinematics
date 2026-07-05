#include <test_framework.h>

#include <cmath>
#include <stdexcept>

#include <collision/aabb.h>
#include <collision/point_mass.h>
#include <collision/shape.h>
#include <math/mathf.h>

using namespace kinematics;

namespace {
constexpr double EPS = 1e-6;
}

TEST_CASE(aabb_default_is_invalid) {
    AABB box;
    CHECK(box.is_valid == false);
    CHECK(box.contains(0.0f, 0.0f) == false);
}

TEST_CASE(aabb_add_grows_box) {
    AABB box;
    box.add(1.0f, 2.0f);
    CHECK(box.is_valid == true);
    CHECK_NEAR(box.min.x, 1.0f, EPS);
    CHECK_NEAR(box.max.y, 2.0f, EPS);

    box.add(5.0f, -3.0f);
    CHECK_NEAR(box.min.x, 1.0f, EPS);
    CHECK_NEAR(box.max.x, 5.0f, EPS);
    CHECK_NEAR(box.min.y, -3.0f, EPS);
    CHECK_NEAR(box.max.y, 2.0f, EPS);
}

TEST_CASE(aabb_contains_and_clear) {
    AABB box(Vector2(0, 0), Vector2(10, 10));
    CHECK(box.contains(5, 5) == true);
    CHECK(box.contains(-1, 5) == false);
    CHECK(box.contains(5, 11) == false);

    box.clear();
    CHECK(box.is_valid == false);
    CHECK(box.contains(5, 5) == false);
}

TEST_CASE(aabb_intersects) {
    AABB a(Vector2(0, 0), Vector2(4, 4));
    AABB b(Vector2(3, 3), Vector2(6, 6));
    AABB c(Vector2(5, 5), Vector2(7, 7));
    CHECK(a.intersects(b) == true);
    CHECK(a.intersects(c) == false);
    CHECK(b.intersects(c) == true);
}

TEST_CASE(pointmass_gravity_integration) {

    PointMass pm(Vector2(0, 0), 1.0f);
    const double dt = 1.0 / 60.0;
    const float g = 9.8f;

    pm.force.y = g * pm.mass;
    pm.update(dt);

    float expectedV = g * (float)dt;
    float expectedY = expectedV * (float)dt;
    CHECK_NEAR(pm.velocity.y, expectedV, EPS);
    CHECK_NEAR(pm.position.y, expectedY, EPS);
    CHECK_NEAR(pm.force.y, 0.0f, EPS);
}

TEST_CASE(pointmass_maxstep_clamp) {
    float savedMax = PointMass::MAX_STEP;
    PointMass::MAX_STEP = 0.4f;

    PointMass pm(Vector2(0, 0), 1.0f);
    pm.velocity.x = 1000.0f;
    pm.force = Vector2::ZERO;
    const double dt = 1.0 / 60.0;
    pm.update(dt);

    CHECK_NEAR(pm.position.x, 0.4f, 1e-5);
    CHECK_NEAR(pm.velocity.x, 0.4f / (float)dt, 1e-3);

    PointMass::MAX_STEP = savedMax;
}

TEST_CASE(pointmass_zero_mass_is_safe) {

    PointMass pm(Vector2(0.0f, 0.0f), 0.0f);
    pm.force = Vector2(0.0f, 9.8f);
    pm.update(1.0 / 60.0);
    CHECK(std::isfinite(pm.velocity.y));
    CHECK(std::isfinite(pm.position.y));
    CHECK_NEAR(pm.velocity.y, 0.0f, 1e-6);
}

TEST_CASE(pointmass_below_cap_unclamped) {
    float savedMax = PointMass::MAX_STEP;
    PointMass::MAX_STEP = 0.4f;

    PointMass pm(Vector2(0, 0), 1.0f);
    pm.velocity.x = 6.0f;
    pm.force = Vector2::ZERO;
    const double dt = 1.0 / 60.0;
    pm.update(dt);

    CHECK_NEAR(pm.velocity.x, 6.0f, EPS);
    CHECK_NEAR(pm.position.x, 0.1f, 1e-5);

    PointMass::MAX_STEP = savedMax;
}

TEST_CASE(shape_build_uncentered) {
    Shape s;
    s.begin(false);
    s.add(Vector2(0, 0));
    s.add(Vector2(2, 0));
    s.add(Vector2(2, 2));
    s.add(Vector2(0, 2));
    s.end();

    CHECK(s.count == 4);
    Vector2 c = s.get_center();
    CHECK_NEAR(c.x, 1.0f, EPS);
    CHECK_NEAR(c.y, 1.0f, EPS);
}

TEST_CASE(shape_build_centered) {
    Shape s;
    s.begin(true);
    s.add(Vector2(0, 0));
    s.add(Vector2(2, 0));
    s.add(Vector2(2, 2));
    s.add(Vector2(0, 2));
    s.end();

    Vector2 c = s.get_center();
    CHECK_NEAR(c.x, 0.0f, 1e-5);
    CHECK_NEAR(c.y, 0.0f, 1e-5);
}

TEST_CASE(shape_begin_twice_throws) {
    Shape s;
    s.begin(false);
    bool threw = false;
    try {
        s.begin(false);
    } catch (const std::logic_error&) {
        threw = true;
    }
    CHECK(threw == true);
}

TEST_CASE(shape_clone_is_independent) {
    Shape s;
    s.begin(false);
    s.add(Vector2(1, 1));
    s.add(Vector2(3, 5));
    s.end();

    Shape clone = s.clone();
    CHECK(clone.count == 2);
    clone.points[0].x = 99.0f;
    CHECK_NEAR(s.points[0].x, 1.0f, EPS);
}

TEST_CASE(shape_transform_translate_only) {
    std::vector<Vector2> pts = {Vector2(1, 0), Vector2(0, 1)};
    auto out = Shape::transform(pts, Vector2(10, 20), 0.0f, Vector2::ONE);
    CHECK_NEAR(out[0].x, 11.0f, EPS);
    CHECK_NEAR(out[0].y, 20.0f, EPS);
    CHECK_NEAR(out[1].x, 10.0f, EPS);
    CHECK_NEAR(out[1].y, 21.0f, EPS);
}

TEST_CASE(shape_transform_rotate_90) {
    std::vector<Vector2> pts = {Vector2(1, 0)};
    auto out = Shape::transform(pts, Vector2::ZERO, mathf::PI / 2.0f, Vector2::ONE);
    CHECK_NEAR(out[0].x, 0.0f, 1e-6);
    CHECK_NEAR(out[0].y, 1.0f, 1e-6);
}
