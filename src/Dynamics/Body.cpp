#include <Dynamics/Body.h>
#include <Math/Mathf.h>
#include <Math/Vector3.h>
#include <cmath>
#include <limits>
#include <memory>

namespace kinematics
{
    Body::Body(const Shape& shape, float mass)
    {
        _baseShape = shape;
        CurrentShape = shape.clone();
        Count = shape.count;
        PointMassList.reserve(static_cast<size_t>(shape.count));
        for (int i = 0; i < shape.count; i++)
        {
            PointMassList.push_back(std::make_shared<PointMass>(shape.points[i], mass));
        }
    }

    void Body::ApplyInternalForces(double /*elapsed*/)
    {
        for (int i = 0; i < Count; i++)
        {
            float mass = PointMassList[i]->mass;
            if (!std::isinf(mass))
            {
                PointMassList[i]->force += Gravity * mass;
            }
        }
    }

    void Body::UpdateBodyPositionVelocityForce()
    {
        GetBodyPositionVelocityForce(Position, Velocity, _force);
    }

    void Body::RotateShape(double /*elapsed*/)
    {
        float angle = 0.0f;
        int originalSign = 1;
        float originalAngle = 0.0f;
        for (int i = 0; i < Count; i++)
        {
            Vector2 baseNormal = Vector2::Normalize(
                Vector2(_baseShape.points[i].X, _baseShape.points[i].Y));
            Vector2 currentNormal = Vector2::Normalize(
                Vector2(PointMassList[i]->position.X - Position.X,
                    PointMassList[i]->position.Y - Position.Y));

            float dot = Vector2::Dot(baseNormal, currentNormal);
            if (dot > 1.0f)
            {
                dot = 1.0f;
            }
            if (dot < -1.0f)
            {
                dot = -1.0f;
            }

            float thisAngle = mathf::Acos(dot);
            if (!baseNormal.IsCounterClockwise(currentNormal))
            {
                thisAngle = -thisAngle;
            }

            if (i == 0)
            {
                originalSign = thisAngle >= 0.0f ? 1 : -1;
                originalAngle = thisAngle;
            }
            else
            {
                float diff = thisAngle - originalAngle;
                int thisSign = thisAngle >= 0.0f ? 1 : -1;
                if (mathf::Abs(diff) > mathf::PI && thisSign != originalSign)
                {
                    thisAngle = thisSign == -1 ? thisAngle + mathf::PI * 2.0f
                                               : thisAngle - mathf::PI * 2.0f;
                }
            }

            angle += thisAngle;
        }

        angle /= Count;

        float cos = mathf::Cos(angle);
        float sin = mathf::Sin(angle);
        for (int i = 0; i < Count; i++)
        {
            float x = _baseShape.points[i].X;
            float y = _baseShape.points[i].Y;
            CurrentShape.points[i].X = cos * x - sin * y + Position.X;
            CurrentShape.points[i].Y = cos * y + sin * x + Position.Y;
        }
    }

    void Body::Update(double elapsed)
    {
        if (!_isDirty)
        {
            return;
        }

        SetBodyPositionVelocityForce(Position, Velocity, _force);
        RotateShape(elapsed);
        ApplyInternalForces(elapsed);
        UpdatePointMasses(elapsed);
        UpdateAABB(elapsed);
        UpdateBodyPositionVelocityForce();
        if (IsStatic)
        {
            _isDirty = false;
        }
    }

    bool Body::Contains(const Vector2& point) const
    {
        Vector2 endPt(Aabb.max.X + 0.1f, point.Y);
        bool inside = false;
        Vector2 edgeSt = PointMassList[0]->position;
        for (int i = 0; i < Count; i++)
        {
            Vector2 edgeEnd = i < Count - 1 ? PointMassList[i + 1]->position
                                            : PointMassList[0]->position;
            if ((edgeSt.Y <= point.Y && edgeEnd.Y > point.Y) ||
                (edgeSt.Y > point.Y && edgeEnd.Y <= point.Y))
            {
                float slope = (edgeEnd.X - edgeSt.X) / (edgeEnd.Y - edgeSt.Y);
                float hitX = edgeSt.X + ((point.Y - edgeSt.Y) * slope);
                if (hitX >= point.X && hitX <= endPt.X)
                {
                    inside = !inside;
                }
            }
            edgeSt = edgeEnd;
        }
        return inside;
    }

    float Body::GetClosestPoint(Vector2 point, Vector2& closest, Vector2& normal,
        int& pointA, int& pointB, float& edgeD)
    {
        closest = Vector2::Zero;
        pointA = -1;
        pointB = -1;
        edgeD = 0.0f;
        normal = Vector2::Zero;
        float closestD = std::numeric_limits<float>::max();
        for (int i = 0; i < Count; i++)
        {
            Vector2 tempHit, tempNorm;
            float tempEdgeD;
            float dist = GetClosestPointOnEdge(point, i, tempHit, tempNorm, tempEdgeD);
            if (dist < closestD)
            {
                closestD = dist;
                pointA = i;
                pointB = i < Count - 1 ? i + 1 : 0;
                edgeD = tempEdgeD;
                normal = tempNorm;
                closest = tempHit;
            }
        }
        return closestD;
    }

    float Body::GetClosestPointOnEdge(Vector2 point, int edgeNum, Vector2& hitPt,
        Vector2& normal, float& edgeD)
    {
        hitPt = Vector2(0.0f, 0.0f);
        normal = Vector2(0.0f, 0.0f);
        edgeD = 0.0f;
        float distance;
        Vector2 ptA = PointMassList[edgeNum]->position;
        Vector2 ptB = edgeNum < Count - 1 ? PointMassList[edgeNum + 1]->position
                                          : PointMassList[0]->position;
        Vector2 toP(point.X - ptA.X, point.Y - ptA.Y);
        Vector2 e(ptB.X - ptA.X, ptB.Y - ptA.Y);

        float edgeLength = mathf::Sqrt(e.X * e.X + e.Y * e.Y);
        if (edgeLength > mathf::Epsilon)
        {
            e.X /= edgeLength;
            e.Y /= edgeLength;
        }

        Vector2 perpendicular = e.Perpendicular();
        float dot = Vector2::Dot(toP, e);
        if (dot <= mathf::Epsilon)
        {
            distance = Vector2::Distance(point, ptA);
            hitPt = ptA;
            edgeD = 0.0f;
            normal = perpendicular;
        }
        else if (dot >= edgeLength)
        {
            distance = Vector2::Distance(point, ptB);
            hitPt = ptB;
            edgeD = 1.0f;
            normal = perpendicular;
        }
        else
        {
            Vector3 toP3(toP.X, toP.Y, 0.0f);
            Vector3 e3(e.X, e.Y, 0.0f);
            e3 = Vector3::Cross(toP3, e3);
            distance = mathf::Abs(e3.Z);
            hitPt.X = ptA.X + e.X * dot;
            hitPt.Y = ptA.Y + e.Y * dot;
            edgeD = dot / edgeLength;
            normal = perpendicular;
        }

        return distance;
    }

    float Body::GetClosestPointOnEdgeSquared(Vector2 point, int edgeNum, Vector2& hitPt,
        Vector2& normal, float& edgeD)
    {
        hitPt = Vector2(0.0f, 0.0f);
        normal = Vector2(0.0f, 0.0f);
        edgeD = 0.0f;
        float distanceSquared;
        Vector2 ptA = PointMassList[edgeNum]->position;
        Vector2 ptB = edgeNum < Count - 1 ? PointMassList[edgeNum + 1]->position
                                          : PointMassList[0]->position;
        Vector2 toP(point.X - ptA.X, point.Y - ptA.Y);
        Vector2 edge(ptB.X - ptA.X, ptB.Y - ptA.Y);

        float edgeLength = mathf::Sqrt(edge.X * edge.X + edge.Y * edge.Y);
        if (edgeLength > mathf::Epsilon)
        {
            edge.X /= edgeLength;
            edge.Y /= edgeLength;
        }

        Vector2 perpendicular = edge.Perpendicular();
        float dot = Vector2::Dot(toP, edge);
        if (dot <= mathf::Epsilon)
        {
            distanceSquared = Vector2::DistanceSquared(point, ptA);
            hitPt = ptA;
            edgeD = 0.0f;
            normal = perpendicular;
        }
        else if (dot >= edgeLength)
        {
            distanceSquared = Vector2::DistanceSquared(point, ptB);
            hitPt = ptB;
            edgeD = 1.0f;
            normal = perpendicular;
        }
        else
        {
            Vector3 toP3(toP.X, toP.Y, 0.0f);
            Vector3 e3(edge.X, edge.Y, 0.0f);
            e3 = Vector3::Cross(toP3, e3);
            distanceSquared = mathf::Abs(e3.Z * e3.Z);
            hitPt.X = ptA.X + edge.X * dot;
            hitPt.Y = ptA.Y + edge.Y * dot;
            edgeD = dot / edgeLength;
            normal = perpendicular;
        }

        return distanceSquared;
    }

    PointMassPtr Body::GetClosestPointMass(Vector2 point, float& dist)
    {
        float closestSQD = 100000.0f;
        int closest = -1;
        for (int i = 0; i < Count; i++)
        {
            float thisD = (point - PointMassList[i]->position).LengthSquared();
            if (thisD < closestSQD)
            {
                closestSQD = thisD;
                closest = i;
            }
        }
        dist = mathf::Sqrt(closestSQD);
        return PointMassList[closest];
    }

    void Body::ApplyForce(const Vector2& point, const Vector2& force)
    {
        Vector2 r = Position - point;
        float torqueForce = Vector3::Cross(r.Vector3FromVector2(),
            force.Vector3FromVector2()).Z;
        for (int i = 0; i < Count; i++)
        {
            Vector2 toPt = PointMassList[i]->position - Position;
            Vector2 torque(toPt.Y, -toPt.X);
            PointMassList[i]->force += torque * torqueForce;
            PointMassList[i]->force += force;
        }
    }

    void Body::UpdatePointMasses(double elapsed)
    {
        for (int i = 0; i < Count; i++)
        {
            PointMassList[i]->velocity.X *= DAMPING;
            PointMassList[i]->velocity.Y *= DAMPING;
            PointMassList[i]->update(elapsed);
        }
    }

    void Body::UpdateAABB(double elapsed)
    {
        Aabb.clear();
        for (int i = 0; i < Count; i++)
        {
            float x = PointMassList[i]->position.X;
            float y = PointMassList[i]->position.Y;
            Aabb.add(x, y);
            x += static_cast<float>(PointMassList[i]->velocity.X * elapsed);
            y += static_cast<float>(PointMassList[i]->velocity.Y * elapsed);
            Aabb.add(x, y);
        }
    }

    void Body::SetBodyPositionVelocityForce(Vector2 position, Vector2 velocity, Vector2 force)
    {
        Vector2 currentPosition, currentVelocity, currentForce;
        GetBodyPositionVelocityForce(currentPosition, currentVelocity, currentForce);
        for (int i = 0; i < Count; i++)
        {
            PointMassList[i]->position -= currentPosition;
            PointMassList[i]->position += position;
            PointMassList[i]->velocity -= currentVelocity;
            PointMassList[i]->velocity += velocity;
            PointMassList[i]->force -= currentForce;
            PointMassList[i]->force += force;
        }
    }

    void Body::GetBodyPositionVelocityForce(Vector2& position, Vector2& velocity, Vector2& force)
    {
        float inverse_count = 1.0f / Count;
        position.X = 0.0f;
        position.Y = 0.0f;
        velocity.X = 0.0f;
        velocity.Y = 0.0f;
        force.X = 0.0f;
        force.Y = 0.0f;
        for (int i = 0; i < Count; i++)
        {
            position.X += PointMassList[i]->position.X * inverse_count;
            position.Y += PointMassList[i]->position.Y * inverse_count;
            velocity.X += PointMassList[i]->velocity.X * inverse_count;
            velocity.Y += PointMassList[i]->velocity.Y * inverse_count;
            force.X += PointMassList[i]->force.X * inverse_count;
            force.Y += PointMassList[i]->force.Y * inverse_count;
        }
    }
}
