#include <test_framework.h>

#include <Math/Mathf.h>
#include <Math/Vector2.h>
#include <Math/Vector3.h>

using namespace kinematics;

namespace {
constexpr double EPS = 1e-6;
}

// ── Mathf ────────────────────────────────────────────────────────
TEST_CASE(mathf_constants) {
    CHECK_NEAR(mathf::PI, 3.14159265358979, 1e-6);
    CHECK_NEAR(mathf::Epsilon, 0.000001f, 1e-12);
}

TEST_CASE(mathf_basic) {
    CHECK_NEAR(mathf::Sqrt(16.0f), 4.0f, EPS);
    CHECK_NEAR(mathf::Abs(-3.5f), 3.5f, EPS);
    CHECK_NEAR(mathf::Min(2.0f, 5.0f), 2.0f, EPS);
    CHECK_NEAR(mathf::Max(2.0f, 5.0f), 5.0f, EPS);
    CHECK_NEAR(mathf::Floor(3.9f), 3.0f, EPS);
    CHECK_NEAR(mathf::Pow(2.0f, 10.0f), 1024.0f, EPS);
    CHECK_NEAR(mathf::Cos(0.0f), 1.0f, EPS);
    CHECK_NEAR(mathf::Sin(0.0f), 0.0f, EPS);
    CHECK_NEAR(mathf::Acos(1.0f), 0.0f, EPS);
}

// ── Vector2: core ────────────────────────────────────────────────
TEST_CASE(vector2_length) {
    Vector2 v(3.0f, 4.0f);
    CHECK_NEAR(v.Length(), 5.0f, EPS);
    CHECK_NEAR(v.LengthSquared(), 25.0f, EPS);
}

TEST_CASE(vector2_normalize_member) {
    Vector2 v(3.0f, 4.0f);
    v.Normalize();
    CHECK_NEAR(v.X, 0.6f, EPS);
    CHECK_NEAR(v.Y, 0.8f, EPS);
}

TEST_CASE(vector2_normalize_zero_is_safe) {
    // Below Epsilon the C# Normalize leaves the vector unchanged.
    Vector2 v(0.0f, 0.0f);
    v.Normalize();
    CHECK(v == Vector2::Zero);
    Vector2 n = Vector2::Normalize(Vector2(0.0f, 0.0f));
    CHECK(n == Vector2::Zero);
}

TEST_CASE(vector2_dot_distance) {
    CHECK_NEAR(Vector2::Dot(Vector2(1, 2), Vector2(3, 4)), 11.0f, EPS);
    CHECK_NEAR(Vector2::Distance(Vector2(0, 0), Vector2(3, 4)), 5.0f, EPS);
    CHECK_NEAR(Vector2::DistanceSquared(Vector2(0, 0), Vector2(3, 4)), 25.0f, EPS);
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

// ── Vector2: extension methods ──────────────────────────────────
TEST_CASE(vector2_perpendicular) {
    // C#: Perpendicular() => new Vector2(-Y, X)
    CHECK(Vector2(1, 0).Perpendicular() == Vector2(0, 1));
    CHECK(Vector2(0, 1).Perpendicular() == Vector2(-1, 0));
}

TEST_CASE(vector2_rotate_90deg) {
    Vector2 r = Vector2(1, 0).Rotate(mathf::PI / 2.0f);
    CHECK_NEAR(r.X, 0.0f, 1e-6);
    CHECK_NEAR(r.Y, 1.0f, 1e-6);
}

TEST_CASE(vector2_is_counter_clockwise) {
    // perpendicular of (1,0) is (0,1); dot((0,1),(0,1)) = 1 >= 0 -> true
    CHECK(Vector2(1, 0).IsCounterClockwise(Vector2(0, 1)) == true);
    CHECK(Vector2(1, 0).IsCounterClockwise(Vector2(0, -1)) == false);
}

TEST_CASE(vector2_to_vector3) {
    Vector3 v = Vector2(2, 3).Vector3FromVector2();
    CHECK_NEAR(v.X, 2.0f, EPS);
    CHECK_NEAR(v.Y, 3.0f, EPS);
    CHECK_NEAR(v.Z, 0.0f, EPS);
}

// ── Vector3 ──────────────────────────────────────────────────────
TEST_CASE(vector3_cross) {
    Vector3 c = Vector3::Cross(Vector3(1, 0, 0), Vector3(0, 1, 0));
    CHECK_NEAR(c.X, 0.0f, EPS);
    CHECK_NEAR(c.Y, 0.0f, EPS);
    CHECK_NEAR(c.Z, 1.0f, EPS);
}
