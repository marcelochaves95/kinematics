#include "test_framework.hpp"

#include "Collision/AABB.hpp"
#include "Collision/PointMass.hpp"
#include "Collision/Shape.hpp"

using namespace kinematics;

namespace {
constexpr double EPS = 1e-6;
}

// ── AABB ─────────────────────────────────────────────────────────
TEST_CASE(aabb_default_is_invalid) {
    AABB box;
    CHECK(box.IsValid == false);
    CHECK(box.Contains(0.0f, 0.0f) == false);
}

TEST_CASE(aabb_add_grows_box) {
    AABB box;
    box.Add(1.0f, 2.0f);   // first Add seeds Min=Max=(1,2)
    CHECK(box.IsValid == true);
    CHECK_NEAR(box.Min.X, 1.0f, EPS);
    CHECK_NEAR(box.Max.Y, 2.0f, EPS);

    box.Add(5.0f, -3.0f);
    CHECK_NEAR(box.Min.X, 1.0f, EPS);
    CHECK_NEAR(box.Max.X, 5.0f, EPS);
    CHECK_NEAR(box.Min.Y, -3.0f, EPS);
    CHECK_NEAR(box.Max.Y, 2.0f, EPS);
}

TEST_CASE(aabb_contains_and_clear) {
    AABB box(Vector2(0, 0), Vector2(10, 10));
    CHECK(box.Contains(5, 5) == true);
    CHECK(box.Contains(-1, 5) == false);
    CHECK(box.Contains(5, 11) == false);

    box.Clear();
    CHECK(box.IsValid == false);
    CHECK(box.Contains(5, 5) == false);
}

TEST_CASE(aabb_intersects) {
    AABB a(Vector2(0, 0), Vector2(4, 4));
    AABB b(Vector2(3, 3), Vector2(6, 6));
    AABB c(Vector2(5, 5), Vector2(7, 7));
    CHECK(a.Intersects(b) == true);
    CHECK(a.Intersects(c) == false);
    CHECK(b.Intersects(c) == true);
}

// ── PointMass ────────────────────────────────────────────────────
TEST_CASE(pointmass_gravity_integration) {
    // Semi-implicit Euler with the engine's order: v += F/m*dt, then x += v*dt.
    PointMass pm(Vector2(0, 0), 1.0f);
    const double dt = 1.0 / 60.0;
    const float g = 9.8f;

    pm.Force.Y = g * pm.Mass; // external gravity force for this step
    pm.Update(dt);

    float expectedV = g * (float)dt;          // 9.8 / 60
    float expectedY = expectedV * (float)dt;  // v * dt
    CHECK_NEAR(pm.Velocity.Y, expectedV, EPS);
    CHECK_NEAR(pm.Position.Y, expectedY, EPS);
    CHECK_NEAR(pm.Force.Y, 0.0f, EPS); // force cleared after Update
}

TEST_CASE(pointmass_maxstep_clamp) {
    float savedMax = PointMass::MaxStep;
    PointMass::MaxStep = 0.4f;

    PointMass pm(Vector2(0, 0), 1.0f);
    pm.Velocity.X = 1000.0f; // |v|*dt = 1000/60 ≈ 16.7 m >> 0.4 m cap
    pm.Force = Vector2::Zero;
    const double dt = 1.0 / 60.0;
    pm.Update(dt);

    // Clamp forces displacement to exactly MaxStep, and velocity to MaxStep/dt.
    CHECK_NEAR(pm.Position.X, 0.4f, 1e-5);
    CHECK_NEAR(pm.Velocity.X, 0.4f / (float)dt, 1e-3);

    PointMass::MaxStep = savedMax;
}

TEST_CASE(pointmass_below_cap_unclamped) {
    float savedMax = PointMass::MaxStep;
    PointMass::MaxStep = 0.4f;

    PointMass pm(Vector2(0, 0), 1.0f);
    pm.Velocity.X = 6.0f; // |v|*dt = 0.1 m < 0.4 m -> untouched
    pm.Force = Vector2::Zero;
    const double dt = 1.0 / 60.0;
    pm.Update(dt);

    CHECK_NEAR(pm.Velocity.X, 6.0f, EPS);
    CHECK_NEAR(pm.Position.X, 0.1f, 1e-5);

    PointMass::MaxStep = savedMax;
}

// ── Shape ────────────────────────────────────────────────────────
TEST_CASE(shape_build_uncentered) {
    Shape s;
    s.Begin(false);
    s.Add(Vector2(0, 0));
    s.Add(Vector2(2, 0));
    s.Add(Vector2(2, 2));
    s.Add(Vector2(0, 2));
    s.End();

    CHECK(s.Count == 4);
    Vector2 c = s.GetCenter();
    CHECK_NEAR(c.X, 1.0f, EPS);
    CHECK_NEAR(c.Y, 1.0f, EPS);
}

TEST_CASE(shape_build_centered) {
    Shape s;
    s.Begin(true); // centers at zero on End()
    s.Add(Vector2(0, 0));
    s.Add(Vector2(2, 0));
    s.Add(Vector2(2, 2));
    s.Add(Vector2(0, 2));
    s.End();

    Vector2 c = s.GetCenter();
    CHECK_NEAR(c.X, 0.0f, 1e-5);
    CHECK_NEAR(c.Y, 0.0f, 1e-5);
}

TEST_CASE(shape_begin_twice_throws) {
    Shape s;
    s.Begin(false);
    bool threw = false;
    try {
        s.Begin(false);
    } catch (const std::logic_error&) {
        threw = true;
    }
    CHECK(threw == true);
}

TEST_CASE(shape_clone_is_independent) {
    Shape s;
    s.Begin(false);
    s.Add(Vector2(1, 1));
    s.Add(Vector2(3, 5));
    s.End();

    Shape clone = s.Clone();
    CHECK(clone.Count == 2);
    clone.Points[0].X = 99.0f;            // mutate clone
    CHECK_NEAR(s.Points[0].X, 1.0f, EPS); // original unaffected
}

TEST_CASE(shape_transform_translate_only) {
    std::vector<Vector2> pts = {Vector2(1, 0), Vector2(0, 1)};
    auto out = Shape::Transform(pts, Vector2(10, 20), 0.0f, Vector2::One);
    CHECK_NEAR(out[0].X, 11.0f, EPS);
    CHECK_NEAR(out[0].Y, 20.0f, EPS);
    CHECK_NEAR(out[1].X, 10.0f, EPS);
    CHECK_NEAR(out[1].Y, 21.0f, EPS);
}

TEST_CASE(shape_transform_rotate_90) {
    std::vector<Vector2> pts = {Vector2(1, 0)};
    auto out = Shape::Transform(pts, Vector2::Zero, mathf::PI / 2.0f, Vector2::One);
    CHECK_NEAR(out[0].X, 0.0f, 1e-6);
    CHECK_NEAR(out[0].Y, 1.0f, 1e-6);
}
