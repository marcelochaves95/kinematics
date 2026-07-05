#include <math/vector3.h>

namespace kinematics
{
    Vector3::Vector3(float x, float y, float z) : x(x), y(y), z(z)
    {
    }

    Vector3 Vector3::cross(const Vector3& a, const Vector3& b)
    {
        return Vector3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x);
    }
}
