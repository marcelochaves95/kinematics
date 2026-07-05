#pragma once

namespace kinematics::mathf
{
    constexpr float PI = static_cast<float>(3.14159265358979311599796346854);
    constexpr float EPSILON = 0.000001f;

    float sin(float value);
    float cos(float value);
    float acos(float value);
    float sqrt(float value);
    float pow(float value, float power);
    float floor(float value);
    float abs(float value);
    float min(float a, float b);
    float max(float a, float b);
}
