#include <test_framework.h>

#include <cmath>
#include <vector>

#include <kinematics_c.h>

// Exercises the extern "C" surface (here compiled as C++, but a real C program
// links the shared library the same way — validated separately in the build).

namespace {
constexpr double EPS = 1e-6;
}

TEST_CASE(capi_world_lifecycle) {
    KnWorld* w = kn_world_create();
    CHECK(w != nullptr);
    kn_world_destroy(w);
    (void) EPS;
}

TEST_CASE(capi_invalid_handle_is_safe) {
    KnWorld* w = kn_world_create();
    CHECK(kn_body_point_count(w, 999) == 0);
    float x = -1.0f, y = -1.0f;
    kn_body_get_position(w, 999, &x, &y); // no-op, must not crash
    kn_world_destroy(w);
}

TEST_CASE(capi_body_caught_by_floor) {
    KnWorld* w = kn_world_create();
    float floorpts[] = {-10, -0.5f, 10, -0.5f, 10, 0.5f, -10, 0.5f};
    int fl = kn_world_add_rigid_body(w, floorpts, 4, INFINITY, 0, 5.5f, 0, 0, 1);
    CHECK(fl == 0);
    float blob[] = {-0.5f, -0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f, 0.5f};
    int b = kn_world_add_spring_body(w, blob, 4, 1.0f, 150, 10, 200, 15, 0, 2.0f, 0, 9.8f, 0);
    CHECK(b == 1);

    for (int i = 0; i < 300; i++) {
        kn_world_step(w, 1.0 / 60.0, 4);
    }

    float x = 0, y = 0;
    kn_body_get_position(w, b, &x, &y);
    CHECK(std::isfinite(y));
    CHECK(y > 5.0f);
    CHECK(y < 7.0f);
    CHECK(kn_world_penetration_count(w) == 0);

    int n = kn_body_point_count(w, b);
    CHECK(n == 4);
    std::vector<float> buf(static_cast<size_t>(2 * n), 0.0f);
    int written = kn_body_get_points(w, b, buf.data(), n);
    CHECK(written == 4);
    CHECK(std::isfinite(buf[0]));

    kn_world_destroy(w);
}

TEST_CASE(capi_chain_readback) {
    KnWorld* w = kn_world_create();
    int ch = kn_world_add_chain(w, -5, 2, 5, 2, 20, 120, 6, 0.25f);
    CHECK(ch == 0);
    kn_world_set_chain_gravity(w, 0, 9.8f);
    // a static body is needed so the world derives its limits on the first step
    float fp[] = {-10, -0.5f, 10, -0.5f, 10, 0.5f, -10, 0.5f};
    kn_world_add_rigid_body(w, fp, 4, INFINITY, 0, 20, 0, 0, 1);

    for (int i = 0; i < 120; i++) {
        kn_world_step(w, 1.0 / 60.0, 1);
    }

    int n = kn_chain_point_count(w, ch);
    CHECK(n == 21);
    std::vector<float> buf(static_cast<size_t>(2 * n), 0.0f);
    int written = kn_chain_get_points(w, ch, buf.data(), n);
    CHECK(written == 21);
    CHECK(std::isfinite(buf[2 * 10 + 1])); // midpoint finite
    CHECK(buf[2 * 10 + 1] > 2.0f);         // sagged below the anchors (y down)

    kn_world_destroy(w);
}
