#include <test_framework.h>

#include <cmath>
#include <limits>
#include <memory>

#include <collision/collision.h>
#include <dynamics/body.h>
#include <dynamics/spring_body.h>
#include <utils/bitmask.h>
#include <utils/kinematics_controller.h>

using namespace kinematics;

namespace {
constexpr float INF = std::numeric_limits<float>::infinity();

Shape box(float hw, float hh) {
    Shape s;
    s.begin(true);
    s.add(Vector2(-hw, -hh));
    s.add(Vector2(hw, -hh));
    s.add(Vector2(hw, hh));
    s.add(Vector2(-hw, hh));
    s.end();
    return s;
}

std::shared_ptr<Body> make_floor() {

    auto floor = std::make_shared<Body>(box(10.0f, 0.5f), INF);
    floor->is_static = true;
    floor->position = Vector2(0.0f, 5.5f);
    return floor;
}
}

TEST_CASE(bitmask_high_bit_is_defined) {
    Bitmask m;
    m.set_on(32);
    CHECK(m.mask != 0u);
    m.set_off(32);
    CHECK(m.mask == 0u);
    m.clear();
    CHECK(m.mask == 0u);
}

TEST_CASE(controller_add_remove_dedup) {
    KinematicsController ctrl;
    auto b = std::make_shared<Body>(box(0.5f, 0.5f), 1.0f);
    ctrl.add(b);
    ctrl.add(b);
    CHECK(ctrl.body_list.size() == 1);
    ctrl.remove(b);
    CHECK(ctrl.body_list.size() == 0);
}

TEST_CASE(collision_intersects_finds_contacts) {
    auto a = std::make_shared<Body>(box(0.5f, 0.5f), 1.0f);
    auto b = std::make_shared<Body>(box(0.5f, 0.5f), 1.0f);
    a->position = Vector2(0.0f, 0.0f);
    b->position = Vector2(0.3f, 0.0f);
    const double dt = 1.0 / 60.0;
    a->update(dt);
    b->update(dt);

    std::vector<CollisionInfo> contacts;
    Collision::intersects(*a, *b, contacts);
    CHECK(contacts.size() >= 1);
    if (!contacts.empty()) {
        CHECK(contacts[0].body_a == a.get());
        CHECK(contacts[0].body_b == b.get());

        CHECK(contacts[0].point_mass_a != nullptr);
        CHECK(contacts[0].point_mass_b != nullptr);
        CHECK(contacts[0].point_mass_c != nullptr);
    }
}

TEST_CASE(controller_no_static_bodies_is_safe) {
    KinematicsController ctrl;
    auto blob = std::make_shared<SpringBody>(box(0.5f, 0.5f), 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    blob->gravity = Vector2(0.0f, 9.8f);
    ctrl.add(blob);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 60; i++) {
        ctrl.update(dt);
    }
    CHECK(std::isfinite(blob->position.y));
    CHECK(blob->position.y > 0.0f);
}

TEST_CASE(body_caught_by_floor) {
    KinematicsController ctrl;
    ctrl.add(make_floor());
    auto blob = std::make_shared<SpringBody>(box(0.5f, 0.5f), 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    blob->position = Vector2(0.0f, 2.0f);
    blob->gravity = Vector2(0.0f, 9.8f);
    ctrl.add(blob);

    const double dt = 1.0 / 60.0;
    float yAt180 = 0.0f;
    for (int i = 0; i < 300; i++) {
        if (i == 180) yAt180 = blob->position.y;
        ctrl.update(dt);
    }

    CHECK(std::isfinite(blob->position.y));
    CHECK(blob->position.y > 5.0f);
    CHECK(blob->position.y < 7.0f);
    CHECK(std::fabs(blob->position.y - yAt180) < 0.5f);
    CHECK(ctrl.penetration_count == 0);
}

TEST_CASE(body_misses_floor_keeps_falling) {
    KinematicsController ctrl;
    ctrl.add(make_floor());
    auto blob = std::make_shared<SpringBody>(box(0.5f, 0.5f), 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    blob->position = Vector2(50.0f, 2.0f);
    blob->gravity = Vector2(0.0f, 9.8f);
    ctrl.add(blob);

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 300; i++) {
        ctrl.update(dt);
    }

    CHECK(blob->position.y > 50.0f);
}

TEST_CASE(remove_body_from_callback_is_safe) {
    KinematicsController ctrl;
    ctrl.add(make_floor());
    auto blob = std::make_shared<SpringBody>(box(0.5f, 0.5f), 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    blob->position = Vector2(0.0f, 2.0f);
    blob->gravity = Vector2(0.0f, 9.8f);
    ctrl.add(blob);

    bool removed = false;
    ctrl.on_collision = [&](Body&, Body&, const CollisionInfo&) {
        if (!removed) {
            removed = true;
            ctrl.remove(blob);
            blob.reset();
        }
    };

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 300; i++) {
        ctrl.update(dt);
    }
    CHECK(removed);
    CHECK(ctrl.body_list.size() == 1);
}

TEST_CASE(collision_callbacks_fire) {
    KinematicsController ctrl;
    ctrl.add(make_floor());
    auto blob = std::make_shared<SpringBody>(box(0.5f, 0.5f), 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    blob->position = Vector2(0.0f, 2.0f);
    blob->gravity = Vector2(0.0f, 9.8f);
    ctrl.add(blob);

    int aabbHits = 0;
    int contactHits = 0;
    ctrl.on_aabb_collision = [&](Body&, Body&) { aabbHits++; };
    ctrl.on_collision = [&](Body&, Body&, const CollisionInfo&) { contactHits++; };

    const double dt = 1.0 / 60.0;
    for (int i = 0; i < 300; i++) {
        ctrl.update(dt);
    }

    CHECK(aabbHits > 0);
    CHECK(contactHits > 0);
}
