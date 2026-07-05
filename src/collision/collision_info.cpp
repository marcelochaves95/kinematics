#include <collision/collision_info.h>

namespace kinematics
{
    void CollisionInfo::clear()
    {
        body_a = nullptr;
        body_b = nullptr;
        point_mass_a = nullptr;
        point_mass_b = nullptr;
        point_mass_c = nullptr;
        point = Vector2::ZERO;
        normal = Vector2::ZERO;
        edge_distance = 0.0f;
        penetration = 0.0f;
    }
}
