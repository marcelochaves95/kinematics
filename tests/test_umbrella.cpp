#include <test_framework.h>

#include <memory>

// The ONLY engine include — proves the umbrella header is self-sufficient and
// exposes the whole public API on its own.
#include <kinematics.h>

using namespace kinematics;

TEST_CASE(umbrella_exposes_full_api) {
    // Touch a type from each subsystem through the single umbrella include.
    Vector2 v(3.0f, 4.0f);
    CHECK_NEAR(v.Length(), 5.0f, 1e-6);

    Shape s;
    s.Begin(true);
    s.Add(Vector2(-1, -1));
    s.Add(Vector2(1, -1));
    s.Add(Vector2(1, 1));
    s.Add(Vector2(-1, 1));
    s.End();

    auto body = std::make_shared<SpringBody>(s, 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    auto ball = std::make_shared<PressureBody>(s, 1.0f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);

    KinematicsController ctrl;
    ctrl.Add(body);
    ctrl.Add(ball);
    CHECK(ctrl.BodyList.size() == 2);
}
