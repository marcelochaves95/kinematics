#include <test_framework.h>

#include <memory>

#include <kinematics.h>

using namespace kinematics;

TEST_CASE(umbrella_exposes_full_api) {

    Vector2 v(3.0f, 4.0f);
    CHECK_NEAR(v.length(), 5.0f, 1e-6);

    Shape s;
    s.begin(true);
    s.add(Vector2(-1, -1));
    s.add(Vector2(1, -1));
    s.add(Vector2(1, 1));
    s.add(Vector2(-1, 1));
    s.end();

    auto body = std::make_shared<SpringBody>(s, 1.0f, 150.0f, 10.0f, 200.0f, 15.0f);
    auto ball = std::make_shared<PressureBody>(s, 1.0f, 30.0f, 150.0f, 10.0f, 200.0f, 15.0f);

    KinematicsController ctrl;
    ctrl.add(body);
    ctrl.add(ball);
    CHECK(ctrl.body_list.size() == 2);
}
