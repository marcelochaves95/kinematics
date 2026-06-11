#include <test_framework.h>

#include <cmath>

#include <Collision/Shape.h>
#include <Dynamics/Body.h>
#include <Dynamics/PressureBody.h>
#include <Dynamics/SpringBody.h>
#include <Math/Vector2.h>

using namespace kinematics;

namespace {
constexpr double EPS = 1e-6;

// Unit square centered at the origin (4 vertices).
Shape unitSquare() {
    Shape s;
    s.Begin(true);
    s.Add(Vector2(-0.5f, -0.5f));
    s.Add(Vector2(0.5f, -0.5f));
    s.Add(Vector2(0.5f, 0.5f));
    s.Add(Vector2(-0.5f, 0.5f));
    s.End();
    return s;
}

bool allFinite(const Body& b) {
    for (const auto& pm : b.PointMassList) {
        if (!std::isfinite(pm->Position.X) || !std::isfinite(pm->Position.Y)) {
            return false;
        }
    }
    return true;
}

// Largest distance from any point mass to the body centroid.
float maxRadius(const Body& b) {
    float m = 0.0f;
    for (const auto& pm : b.PointMassList) {
        float d = (pm->Position - b.Position).Length();
        if (d > m) m = d;
    }
    return m;
}
} // namespace

// ── Body construction ────────────────────────────────────────────
TEST_CASE(body_construction) {
    Shape s = unitSquare();
    Body b(s, 1.0f);
    CHECK(b.Count == 4);
    CHECK(b.PointMassList.size() == 4);
    CHECK(b.CurrentShape.Count == 4);
}

TEST_CASE(springbody_construction) {
    Shape s = unitSquare();
    SpringBody b(s, 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    CHECK(b.Count == 4);
    CHECK(b.PointMassList.size() == 4);
}

// ── Free fall (no floor in Phase 4 — pure dynamics) ──────────────
TEST_CASE(springbody_falls_under_gravity) {
    Shape s = unitSquare();
    SpringBody b(s, 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.Position = Vector2(0.0f, 0.0f);
    b.Gravity = Vector2(0.0f, 9.8f); // +Y is down

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 60; i++) {
        b.Update(dt);
    }

    CHECK(allFinite(b));
    CHECK(b.Position.Y > 1.0f); // fell several metres in 1 s
}

// ── Soft body stays cohesive (no explosion) ──────────────────────
TEST_CASE(springbody_stays_cohesive) {
    Shape s = unitSquare();
    SpringBody b(s, 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.Position = Vector2(0.0f, 0.0f);
    b.Gravity = Vector2(0.0f, 9.8f);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 120; i++) {
        b.Update(dt);
    }

    CHECK(allFinite(b));
    // Initial half-diagonal ≈ 0.707; the edge/shape springs keep it bounded.
    float r = maxRadius(b);
    CHECK(r > 0.3f);
    CHECK(r < 2.0f);
}

// ── Pressure body inflates and holds together ────────────────────
TEST_CASE(pressurebody_holds_together) {
    Shape s = unitSquare();
    PressureBody b(s, 1.0f, /*gasPressure*/ 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.Position = Vector2(0.0f, 0.0f);
    b.Gravity = Vector2(0.0f, 9.8f);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 120; i++) {
        b.Update(dt);
    }

    CHECK(allFinite(b));
    float r = maxRadius(b);
    CHECK(r > 0.3f);
    CHECK(r < 2.5f); // pressure may inflate slightly, but no blow-up
}

TEST_CASE(pressurebody_stable_without_gravity) {
    Shape s = unitSquare();
    PressureBody b(s, 1.0f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.Position = Vector2(0.0f, 0.0f);
    // no gravity: should sit essentially still

    const double dt = 1.0 / 60.0;
    float r0 = maxRadius(b);
    for (int i = 0; i < 120; i++) {
        b.Update(dt);
    }

    CHECK(allFinite(b));
    float r = maxRadius(b);
    CHECK_NEAR(r, r0, 0.5); // stays close to its initial size
}

// ── Degenerate (collapsed) pressure body must not produce NaN ────
TEST_CASE(pressurebody_degenerate_volume_is_safe) {
    // Coincident vertices -> ~zero estimated volume; the 1/_volume pressure term
    // must be skipped instead of spreading Inf/NaN through the body.
    Shape s;
    s.Begin(true);
    s.Add(Vector2(0.0f, 0.0f));
    s.Add(Vector2(0.0f, 0.0f));
    s.Add(Vector2(0.0f, 0.0f));
    s.End();
    PressureBody b(s, 1.0f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    b.Gravity = Vector2(0.0f, 9.8f);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 60; i++) {
        b.Update(dt);
    }
    CHECK(allFinite(b));
}

// ── Virtual dispatch: PressureBody::ApplyInternalForces is reached ──
TEST_CASE(virtual_dispatch_through_base_ref) {
    Shape s = unitSquare();
    PressureBody pb(s, 1.0f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    Body& base = pb;
    base.Gravity = Vector2(0.0f, 9.8f);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 30; i++) {
        base.Update(dt); // dispatches to PressureBody::ApplyInternalForces
    }
    CHECK(allFinite(pb));
    (void) EPS;
}
