#include <Collision/CollisionInfo.h>

namespace kinematics
{
    // Reset to an empty record. The point-mass pointers are null (not throwaway
    // PointMass instances): Collision::Intersects only emits a CollisionInfo once
    // it has assigned the real shared point masses, and the controller never reads
    // an unpopulated record, so nothing dereferences these nulls.
    void CollisionInfo::Clear()
    {
        BodyA = nullptr;
        BodyB = nullptr;
        PointMassA = nullptr;
        PointMassB = nullptr;
        PointMassC = nullptr;
        Point = Vector2::Zero;
        Normal = Vector2::Zero;
        EdgeDistance = 0.0f;
        Penetration = 0.0f;
    }
}
