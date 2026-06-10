#include <Math/Vector3.h>

namespace kinematics
{
    Vector3::Vector3(float x, float y, float z) : X(x), Y(y), Z(z)
    {
    }

    Vector3 Vector3::Cross(const Vector3& a, const Vector3& b)
    {
        return Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);
    }
}
