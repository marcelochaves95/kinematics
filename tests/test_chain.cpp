#include "test_framework.hpp"

#include <limits>
#include <memory>

#include "Collision/Chain.hpp"
#include "Collision/PointMass.hpp"
#include "Collision/Spring.hpp"

using namespace kinematics;

namespace {
constexpr double EPS = 1e-6;
constexpr float INF = std::numeric_limits<float>::infinity();

// Build the demo chain and pre-simulate it under gravity, mirroring ChainScene.
// Returns the max interior speed and the midpoint Y after `steps` frames.
struct SettleResult { float maxSpeed; float midY; };

SettleResult settle(float springDamp, int steps) {
    auto anchorA = std::make_shared<PointMass>(Vector2(-5.0f, 2.0f), INF);
    auto anchorB = std::make_shared<PointMass>(Vector2(5.0f, 2.0f), INF);
    const float mass = 0.25f;
    Chain chain(anchorA, anchorB, /*count*/ 20, /*k*/ 120.0f, springDamp, mass);

    for (Spring& s : chain.SpringList) {
        s.D *= 1.5f; // rest length 1.5x spacing -> sags into a catenary
    }
    chain.Damping = 0.96f;

    const double dt = 1.0 / 60.0;
    const float g = 9.8f;
    for (int step = 0; step < steps; step++) {
        for (size_t i = 1; i + 1 < chain.PointMassList.size(); i++) {
            chain.PointMassList[i]->Force.Y += g * mass;
        }
        chain.Update(dt);
    }

    float maxSpeed = 0.0f;
    for (size_t i = 1; i + 1 < chain.PointMassList.size(); i++) {
        float v = chain.PointMassList[i]->Velocity.Length();
        if (v > maxSpeed) maxSpeed = v;
    }
    return {maxSpeed, chain.PointMassList[10]->Position.Y};
}
} // namespace

// ── Spring ───────────────────────────────────────────────────────
TEST_CASE(spring_rest_length_defaults_to_distance) {
    auto a = std::make_shared<PointMass>(Vector2(0, 0), 1.0f);
    auto b = std::make_shared<PointMass>(Vector2(3, 4), 1.0f);
    Spring s(a, b, 100.0f, 5.0f); // 4-arg -> Reset() sets D = distance = 5
    CHECK_NEAR(s.D, 5.0f, EPS);
    CHECK_NEAR(s.K, 100.0f, EPS);
    CHECK_NEAR(s.Damping, 5.0f, EPS);
}

TEST_CASE(spring_explicit_rest_length) {
    auto a = std::make_shared<PointMass>(Vector2(0, 0), 1.0f);
    auto b = std::make_shared<PointMass>(Vector2(3, 4), 1.0f);
    Spring s(a, b, 100.0f, 5.0f, /*length*/ 2.0f); // 5-arg -> no Reset
    CHECK_NEAR(s.D, 2.0f, EPS);
}

TEST_CASE(spring_stretched_pulls_together) {
    // A at (1,0), B at (-1,0): distance 2, rest length 1 -> stretched.
    // Force on A should point toward B (negative X), magnitude K*(rest-dist)=K*(-1).
    auto a = std::make_shared<PointMass>(Vector2(1, 0), 1.0f);
    auto b = std::make_shared<PointMass>(Vector2(-1, 0), 1.0f);
    Spring s(a, b, 10.0f, 0.0f, /*length*/ 1.0f);
    Vector2 f = Spring::SpringForce(s);
    CHECK_NEAR(f.X, -10.0f, 1e-4);
    CHECK_NEAR(f.Y, 0.0f, EPS);
}

TEST_CASE(spring_at_rest_zero_force) {
    auto a = std::make_shared<PointMass>(Vector2(2, 0), 1.0f);
    auto b = std::make_shared<PointMass>(Vector2(0, 0), 1.0f);
    Spring s(a, b, 50.0f, 3.0f, /*length*/ 2.0f); // distance == rest length
    Vector2 f = Spring::SpringForce(s);
    CHECK_NEAR(f.X, 0.0f, 1e-4);
    CHECK_NEAR(f.Y, 0.0f, 1e-4);
}

// ── Chain construction ───────────────────────────────────────────
TEST_CASE(chain_topology) {
    auto a = std::make_shared<PointMass>(Vector2(-5, 2), INF);
    auto b = std::make_shared<PointMass>(Vector2(5, 2), INF);
    Chain chain(a, b, 20, 120.0f, 6.0f, 0.25f);

    CHECK(chain.PointMassList.size() == 21);
    CHECK(chain.SpringList.size() == 20);
    // Endpoints share the caller's anchor instances.
    CHECK(chain.PointMassList.front().get() == a.get());
    CHECK(chain.PointMassList.back().get() == b.get());
    // Springs reference the shared list instances (not copies).
    CHECK(chain.SpringList[0].PointMassA.get() == chain.PointMassList[0].get());
    CHECK(chain.SpringList[0].PointMassB.get() == chain.PointMassList[1].get());
    // ctor hardcodes per-frame Damping to 0.99 regardless of the spring-damp arg.
    CHECK_NEAR(chain.Damping, 0.99f, EPS);
}

// ── The critical stability test ──────────────────────────────────
TEST_CASE(chain_stable_at_springdamp_6) {
    // Inside the explicit-Euler stability bound (<7): settles to a near-motionless
    // catenary that has visibly sagged below the anchors (y grows downward).
    SettleResult r = settle(/*springDamp*/ 6.0f, /*steps*/ 300);
    CHECK(r.maxSpeed < 0.5f);  // settled
    CHECK(r.midY > 4.0f);      // sagged well below the anchors at y=2
}

TEST_CASE(chain_unstable_at_springdamp_8) {
    // Above the bound: the damping force flips sign and injects energy, so the
    // chain never settles (the MaxStep clamp prevents a true blow-up, but the
    // residual speed stays high vs the stable case).
    SettleResult r = settle(/*springDamp*/ 8.0f, /*steps*/ 300);
    CHECK(r.maxSpeed > 1.0f);  // not settled
}
