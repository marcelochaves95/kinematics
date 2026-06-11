#pragma once

#include <Collision/AABB.h>
#include <Collision/PointMass.h>
#include <Collision/Shape.h>
#include <Math/Vector2.h>
#include <Utils/Bitmask.h>
#include <vector>

// Port of Dynamics/Body.cs
//
// C# `internal` members (AABB, BitmaskX/Y) are public here. `out` params become
// reference params; List<PointMass> becomes vector<PointMassPtr> (shared).

namespace kinematics
{
    class Body
    {
    public:
        int Count = 0;
        bool IsStatic = false;
        Vector2 Position;
        Vector2 Velocity;
        Vector2 Gravity = Vector2::Zero;
        std::vector<PointMassPtr> PointMassList;
        Shape CurrentShape;

        AABB Aabb;
        Bitmask BitmaskX;
        Bitmask BitmaskY;

        Body(const Shape& shape, float mass);
        virtual ~Body() = default;

        virtual void ApplyInternalForces(double elapsed);

        void UpdateBodyPositionVelocityForce();
        void RotateShape(double elapsed);
        void Update(double elapsed);

        bool Contains(const Vector2& point) const;
        float GetClosestPoint(Vector2 point, Vector2& closest, Vector2& normal,
            int& pointA, int& pointB, float& edgeD);
        float GetClosestPointOnEdge(Vector2 point, int edgeNum, Vector2& hitPt,
            Vector2& normal, float& edgeD);
        float GetClosestPointOnEdgeSquared(Vector2 point, int edgeNum, Vector2& hitPt,
            Vector2& normal, float& edgeD);
        PointMassPtr GetClosestPointMass(Vector2 point, float& dist);
        void ApplyForce(const Vector2& point, const Vector2& force);

    protected:
        static constexpr float DAMPING = 0.999f;

    private:
        void UpdatePointMasses(double elapsed);
        void UpdateAABB(double elapsed);
        void SetBodyPositionVelocityForce(Vector2 position, Vector2 velocity, Vector2 force);
        void GetBodyPositionVelocityForce(Vector2& position, Vector2& velocity, Vector2& force);

        float _previousAngle = 0.0f;
        bool _isDirty = true;
        bool _isMerging = false;
        Vector2 _force;
        Vector2 _scale = Vector2::One;
        Shape _baseShape;
    };
}
