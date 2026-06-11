#include <test_framework.h>

#include <cmath>
#include <limits>
#include <memory>

#include <Collision/Collision.h>
#include <Dynamics/Body.h>
#include <Dynamics/SpringBody.h>
#include <Utils/KinematicsController.h>

using namespace kinematics;

namespace {
constexpr float INF = std::numeric_limits<float>::infinity();

Shape box(float hw, float hh) {
    Shape s;
    s.Begin(true);
    s.Add(Vector2(-hw, -hh));
    s.Add(Vector2(hw, -hh));
    s.Add(Vector2(hw, hh));
    s.Add(Vector2(-hw, hh));
    s.End();
    return s;
}

std::shared_ptr<Body> makeFloor() {
    // Thin static slab spanning x[-10,10], y[5,6] (top face at y=5).
    auto floor = std::make_shared<Body>(box(10.0f, 0.5f), INF);
    floor->IsStatic = true;
    floor->Position = Vector2(0.0f, 5.5f);
    return floor;
}
} // namespace

// ── Add/Remove dedup ─────────────────────────────────────────────
TEST_CASE(controller_add_remove_dedup) {
    KinematicsController ctrl;
    auto b = std::make_shared<Body>(box(0.5f, 0.5f), 1.0f);
    ctrl.Add(b);
    ctrl.Add(b); // duplicate ignored
    CHECK(ctrl.BodyList.size() == 1);
    ctrl.Remove(b);
    CHECK(ctrl.BodyList.size() == 0);
}

// ── Narrowphase directly ─────────────────────────────────────────
TEST_CASE(collision_intersects_finds_contacts) {
    auto a = std::make_shared<Body>(box(0.5f, 0.5f), 1.0f);
    auto b = std::make_shared<Body>(box(0.5f, 0.5f), 1.0f);
    a->Position = Vector2(0.0f, 0.0f);
    b->Position = Vector2(0.3f, 0.0f); // overlaps A on the +X side
    const double dt = 1.0 / 60.0;
    a->Update(dt); // populate AABB / CurrentShape
    b->Update(dt);

    auto contacts = Collision::Intersects(*a, *b);
    CHECK(contacts.size() >= 1);
    if (!contacts.empty()) {
        CHECK(contacts[0].BodyA == a.get());
        CHECK(contacts[0].BodyB == b.get());
        // Emitted contacts are always fully populated (no null/dummy point masses).
        CHECK(contacts[0].PointMassA != nullptr);
        CHECK(contacts[0].PointMassB != nullptr);
        CHECK(contacts[0].PointMassC != nullptr);
    }
}

// ── Degenerate world (no static bodies) must not divide by zero ──
TEST_CASE(controller_no_static_bodies_is_safe) {
    KinematicsController ctrl;
    auto blob = std::make_shared<SpringBody>(box(0.5f, 0.5f), 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    blob->Gravity = Vector2(0.0f, 9.8f);
    ctrl.Add(blob); // no static bodies, no SetWorldLimits -> world would be zero-size

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 60; i++) {
        ctrl.Update(dt); // must not divide by zero / produce NaN
    }
    CHECK(std::isfinite(blob->Position.Y));
    CHECK(blob->Position.Y > 0.0f); // it fell (no floor to catch it)
}

// ── Integration: a falling body is caught by the floor ───────────
TEST_CASE(body_caught_by_floor) {
    KinematicsController ctrl;
    ctrl.Add(makeFloor());
    auto blob = std::make_shared<SpringBody>(box(0.5f, 0.5f), 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    blob->Position = Vector2(0.0f, 2.0f);
    blob->Gravity = Vector2(0.0f, 9.8f);
    ctrl.Add(blob);

    const double dt = 1.0 / 60.0;
    float yAt180 = 0.0f;
    for (int i = 0; i < 300; i++) {
        if (i == 180) yAt180 = blob->Position.Y;
        ctrl.Update(dt);
    }

    CHECK(std::isfinite(blob->Position.Y));
    CHECK(blob->Position.Y > 5.0f);              // reached the floor (fell from y=2)
    CHECK(blob->Position.Y < 7.0f);              // caught, didn't tunnel through
    CHECK(std::fabs(blob->Position.Y - yAt180) < 0.5f); // settled / stable
    CHECK(ctrl.PenetrationCount == 0);           // no deep blow-through with a thin slab
}

// ── Contrast: a body that misses the floor keeps falling ─────────
TEST_CASE(body_misses_floor_keeps_falling) {
    KinematicsController ctrl;
    ctrl.Add(makeFloor());
    auto blob = std::make_shared<SpringBody>(box(0.5f, 0.5f), 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    blob->Position = Vector2(50.0f, 2.0f); // far to the right of the slab (x[-10,10])
    blob->Gravity = Vector2(0.0f, 9.8f);
    ctrl.Add(blob);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 300; i++) {
        ctrl.Update(dt);
    }
    // No collider underneath -> free fall well past where the floor would catch it.
    CHECK(blob->Position.Y > 50.0f);
}

// ── Callbacks fire during contact ────────────────────────────────
TEST_CASE(collision_callbacks_fire) {
    KinematicsController ctrl;
    ctrl.Add(makeFloor());
    auto blob = std::make_shared<SpringBody>(box(0.5f, 0.5f), 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    blob->Position = Vector2(0.0f, 2.0f);
    blob->Gravity = Vector2(0.0f, 9.8f);
    ctrl.Add(blob);

    int aabbHits = 0;
    int contactHits = 0;
    ctrl.OnAABBCollision = [&](Body&, Body&) { aabbHits++; };
    ctrl.OnCollision = [&](Body&, Body&, const CollisionInfo&) { contactHits++; };

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 300; i++) {
        ctrl.Update(dt);
    }

    CHECK(aabbHits > 0);
    CHECK(contactHits > 0);
}
