#include <math/mathf.h>
#include <cmath>

namespace kinematics::mathf
{
    float sin(float value)
    {
        return std::sin(value);
    }

    float cos(float value)
    {
        return std::cos(value);
    }

    float acos(float value)
    {
        return std::acos(value);
    }

    float sqrt(float value)
    {
        return std::sqrt(value);
    }

    float pow(float value, float power)
    {
        return std::pow(value, power);
    }

    float floor(float value)
    {
        return std::floor(value);
    }

    float abs(float value)
    {
        return std::fabs(value);
    }

    float min(float a, float b)
    {
        return a < b ? a : b;
    }

    float max(float a, float b)
    {
        return a > b ? a : b;
    }
}
