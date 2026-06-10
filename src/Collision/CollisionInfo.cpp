#include <Collision/CollisionInfo.h>
#include <memory>

namespace kinematics
{
    void CollisionInfo::Clear()
    {
        BodyA = nullptr;
        BodyB = nullptr;
        PointMassA = std::make_shared<PointMass>();
        PointMassB = std::make_shared<PointMass>();
        PointMassC = std::make_shared<PointMass>();
        Point = Vector2::Zero;
        Normal = Vector2::Zero;
        EdgeDistance = 0.0f;
        Penetration = 0.0f;
    }
}
