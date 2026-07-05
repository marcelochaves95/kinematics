#include <test_framework.h>

#include <limits>
#include <memory>

#include <collision/chain.h>
#include <collision/point_mass.h>
#include <collision/spring.h>

using namespace kinematics;

namespace {
constexpr double EPS = 1e-6;
constexpr float INF = std::numeric_limits<float>::infinity();

struct SettleResult { float maxSpeed; float midY; };

SettleResult settle(float springDamp, int steps) {
    auto anchorA = std::make_shared<PointMass>(Vector2(-5.0f, 2.0f), INF);
    auto anchorB = std::make_shared<PointMass>(Vector2(5.0f, 2.0f), INF);
    const float mass = 0.25f;
    Chain chain(anchorA, anchorB, /*count*/ 20, /*k*/ 120.0f, springDamp, mass);

    for (Spring& s : chain.spring_list) {
        s.d *= 1.5f;
    }
    chain.damping = 0.96f;

    const double dt = 1.0 / 60.0;
    const float g = 9.8f;
    for (int step = 0; step < steps; step++) {
        for (size_t i = 1; i + 1 < chain.point_mass_list.size(); i++) {
            chain.point_mass_list[i]->force.y += g * mass;
        }
        chain.update(dt);
    }

    float maxSpeed = 0.0f;
    for (size_t i = 1; i + 1 < chain.point_mass_list.size(); i++) {
        float v = chain.point_mass_list[i]->velocity.length();
        if (v > maxSpeed) maxSpeed = v;
    }
    return {maxSpeed, chain.point_mass_list[10]->position.y};
}
}

TEST_CASE(spring_rest_length_defaults_to_distance) {
    auto a = std::make_shared<PointMass>(Vector2(0, 0), 1.0f);
    auto b = std::make_shared<PointMass>(Vector2(3, 4), 1.0f);
    Spring s(a, b, 100.0f, 5.0f);
    CHECK_NEAR(s.d, 5.0f, EPS);
    CHECK_NEAR(s.k, 100.0f, EPS);
    CHECK_NEAR(s.damping, 5.0f, EPS);
}

TEST_CASE(spring_explicit_rest_length) {
    auto a = std::make_shared<PointMass>(Vector2(0, 0), 1.0f);
    auto b = std::make_shared<PointMass>(Vector2(3, 4), 1.0f);
    Spring s(a, b, 100.0f, 5.0f, /*length*/ 2.0f);
    CHECK_NEAR(s.d, 2.0f, EPS);
}

TEST_CASE(spring_stretched_pulls_together) {

    auto a = std::make_shared<PointMass>(Vector2(1, 0), 1.0f);
    auto b = std::make_shared<PointMass>(Vector2(-1, 0), 1.0f);
    Spring s(a, b, 10.0f, 0.0f, /*length*/ 1.0f);
    Vector2 f = Spring::spring_force(s);
    CHECK_NEAR(f.x, -10.0f, 1e-4);
    CHECK_NEAR(f.y, 0.0f, EPS);
}

TEST_CASE(spring_at_rest_zero_force) {
    auto a = std::make_shared<PointMass>(Vector2(2, 0), 1.0f);
    auto b = std::make_shared<PointMass>(Vector2(0, 0), 1.0f);
    Spring s(a, b, 50.0f, 3.0f, /*length*/ 2.0f);
    Vector2 f = Spring::spring_force(s);
    CHECK_NEAR(f.x, 0.0f, 1e-4);
    CHECK_NEAR(f.y, 0.0f, 1e-4);
}

TEST_CASE(chain_topology) {
    auto a = std::make_shared<PointMass>(Vector2(-5, 2), INF);
    auto b = std::make_shared<PointMass>(Vector2(5, 2), INF);
    Chain chain(a, b, 20, 120.0f, 6.0f, 0.25f);

    CHECK(chain.point_mass_list.size() == 21);
    CHECK(chain.spring_list.size() == 20);

    CHECK(chain.point_mass_list.front().get() == a.get());
    CHECK(chain.point_mass_list.back().get() == b.get());

    CHECK(chain.spring_list[0].point_mass_a.get() == chain.point_mass_list[0].get());
    CHECK(chain.spring_list[0].point_mass_b.get() == chain.point_mass_list[1].get());

    CHECK_NEAR(chain.damping, 0.99f, EPS);
}

TEST_CASE(chain_stable_at_springdamp_6) {

    SettleResult r = settle(/*springDamp*/ 6.0f, /*steps*/ 300);
    CHECK(r.maxSpeed < 0.5f);
    CHECK(r.midY > 4.0f);
}

TEST_CASE(chain_unstable_at_springdamp_8) {

    SettleResult r = settle(/*springDamp*/ 8.0f, /*steps*/ 300);
    CHECK(r.maxSpeed > 1.0f);
}
