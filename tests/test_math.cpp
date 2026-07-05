#include <test_framework.h>

#include <math/mathf.h>
#include <math/vector2.h>
#include <math/vector3.h>

using namespace kinematics;

namespace {
constexpr double EPS = 1e-6;
}

TEST_CASE(mathf_constants) {
    CHECK_NEAR(mathf::PI, 3.14159265358979, 1e-6);
    CHECK_NEAR(mathf::EPSILON, 0.000001f, 1e-12);
}

TEST_CASE(mathf_basic) {
    CHECK_NEAR(mathf::sqrt(16.0f), 4.0f, EPS);
    CHECK_NEAR(mathf::abs(-3.5f), 3.5f, EPS);
    CHECK_NEAR(mathf::min(2.0f, 5.0f), 2.0f, EPS);
    CHECK_NEAR(mathf::max(2.0f, 5.0f), 5.0f, EPS);
    CHECK_NEAR(mathf::floor(3.9f), 3.0f, EPS);
    CHECK_NEAR(mathf::pow(2.0f, 10.0f), 1024.0f, EPS);
    CHECK_NEAR(mathf::cos(0.0f), 1.0f, EPS);
    CHECK_NEAR(mathf::sin(0.0f), 0.0f, EPS);
    CHECK_NEAR(mathf::acos(1.0f), 0.0f, EPS);
}

TEST_CASE(vector2_length) {
    Vector2 v(3.0f, 4.0f);
    CHECK_NEAR(v.length(), 5.0f, EPS);
    CHECK_NEAR(v.length_squared(), 25.0f, EPS);
}

TEST_CASE(vector2_normalize_member) {
    Vector2 v(3.0f, 4.0f);
    v.normalize();
    CHECK_NEAR(v.x, 0.6f, EPS);
    CHECK_NEAR(v.y, 0.8f, EPS);
}

TEST_CASE(vector2_normalize_zero_is_safe) {

    Vector2 v(0.0f, 0.0f);
    v.normalize();
    CHECK(v == Vector2::ZERO);
    Vector2 n = Vector2::normalize(Vector2(0.0f, 0.0f));
    CHECK(n == Vector2::ZERO);
}

TEST_CASE(vector2_dot_distance) {
    CHECK_NEAR(Vector2::dot(Vector2(1, 2), Vector2(3, 4)), 11.0f, EPS);
    CHECK_NEAR(Vector2::distance(Vector2(0, 0), Vector2(3, 4)), 5.0f, EPS);
    CHECK_NEAR(Vector2::distance_squared(Vector2(0, 0), Vector2(3, 4)), 25.0f, EPS);
}

TEST_CASE(vector2_operators) {
    CHECK(Vector2(1, 2) + Vector2(3, 4) == Vector2(4, 6));
    CHECK(Vector2(3, 4) - Vector2(1, 2) == Vector2(2, 2));
    CHECK(-Vector2(1, -2) == Vector2(-1, 2));
    CHECK(Vector2(1, 2) * 2.0f == Vector2(2, 4));
    CHECK(3.0f * Vector2(1, 2) == Vector2(3, 6));
    CHECK(Vector2(2, 4) / 2.0f == Vector2(1, 2));
    CHECK(Vector2(1, 2) != Vector2(1, 3));

    Vector2 a(1, 1);
    a += Vector2(2, 3);
    CHECK(a == Vector2(3, 4));
    a -= Vector2(1, 1);
    CHECK(a == Vector2(2, 3));
}

TEST_CASE(vector2_perpendicular) {

    CHECK(Vector2(1, 0).perpendicular() == Vector2(0, 1));
    CHECK(Vector2(0, 1).perpendicular() == Vector2(-1, 0));
}

TEST_CASE(vector2_rotate_90deg) {
    Vector2 r = Vector2(1, 0).rotate(mathf::PI / 2.0f);
    CHECK_NEAR(r.x, 0.0f, 1e-6);
    CHECK_NEAR(r.y, 1.0f, 1e-6);
}

TEST_CASE(vector2_is_counter_clockwise) {

    CHECK(Vector2(1, 0).is_counter_clockwise(Vector2(0, 1)) == true);
    CHECK(Vector2(1, 0).is_counter_clockwise(Vector2(0, -1)) == false);
}

TEST_CASE(vector2_to_vector3) {
    Vector3 v = Vector2(2, 3).vector3_from_vector2();
    CHECK_NEAR(v.x, 2.0f, EPS);
    CHECK_NEAR(v.y, 3.0f, EPS);
    CHECK_NEAR(v.z, 0.0f, EPS);
}

TEST_CASE(vector3_cross) {
    Vector3 c = Vector3::cross(Vector3(1, 0, 0), Vector3(0, 1, 0));
    CHECK_NEAR(c.x, 0.0f, EPS);
    CHECK_NEAR(c.y, 0.0f, EPS);
    CHECK_NEAR(c.z, 1.0f, EPS);
}
