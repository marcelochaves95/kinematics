#pragma once

namespace kinematics
{
    struct Vector3
    {
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        Vector3() = default;
        Vector3(float x, float y, float z);

        static Vector3 cross(const Vector3& a, const Vector3& b);
    };
}
