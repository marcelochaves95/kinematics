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
        point = Vector2::Zero;
        normal = Vector2::Zero;
        edge_distance = 0.0f;
        penetration = 0.0f;
    }
}
