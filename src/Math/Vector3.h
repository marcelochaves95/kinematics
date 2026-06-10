#pragma once

// Port of Math/Vector3.cs

namespace kinematics
{
    struct Vector3
    {
        float X = 0.0f;
        float Y = 0.0f;
        float Z = 0.0f;

        Vector3() = default;
        Vector3(float x, float y, float z);

        static Vector3 Cross(const Vector3& a, const Vector3& b);
    };
}
