#pragma once

// Port of src/Math/Vector3.cs

namespace kinematics {

struct Vector3 {
    float X = 0.0f;
    float Y = 0.0f;
    float Z = 0.0f;

    Vector3() = default;
    Vector3(float x, float y, float z) : X(x), Y(y), Z(z) {}

    static Vector3 Cross(const Vector3& a, const Vector3& b) {
        return Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);
    }
};

} // namespace kinematics
