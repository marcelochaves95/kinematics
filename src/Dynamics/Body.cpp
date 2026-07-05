#include <Dynamics/Body.h>
#include <math/mathf.h>
#include <math/vector3.h>
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
            Vector2 baseNormal = Vector2::normalize(
                Vector2(_baseShape.points[i].x, _baseShape.points[i].y));
            Vector2 currentNormal = Vector2::normalize(
                Vector2(PointMassList[i]->position.x - Position.x,
                    PointMassList[i]->position.y - Position.y));

            float dot = Vector2::dot(baseNormal, currentNormal);
            if (dot > 1.0f)
            {
                dot = 1.0f;
            }
            if (dot < -1.0f)
            {
                dot = -1.0f;
            }

            float thisAngle = mathf::acos(dot);
            if (!baseNormal.is_counter_clockwise(currentNormal))
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
                if (mathf::abs(diff) > mathf::PI && thisSign != originalSign)
                {
                    thisAngle = thisSign == -1 ? thisAngle + mathf::PI * 2.0f
                                               : thisAngle - mathf::PI * 2.0f;
                }
            }

            angle += thisAngle;
        }

        angle /= Count;

        float cos = mathf::cos(angle);
        float sin = mathf::sin(angle);
        for (int i = 0; i < Count; i++)
        {
            float x = _baseShape.points[i].x;
            float y = _baseShape.points[i].y;
            CurrentShape.points[i].x = cos * x - sin * y + Position.x;
            CurrentShape.points[i].y = cos * y + sin * x + Position.y;
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
        Vector2 endPt(Aabb.max.x + 0.1f, point.y);
        bool inside = false;
        Vector2 edgeSt = PointMassList[0]->position;
        for (int i = 0; i < Count; i++)
        {
            Vector2 edgeEnd = i < Count - 1 ? PointMassList[i + 1]->position
                                            : PointMassList[0]->position;
            if ((edgeSt.y <= point.y && edgeEnd.y > point.y) ||
                (edgeSt.y > point.y && edgeEnd.y <= point.y))
            {
                float slope = (edgeEnd.x - edgeSt.x) / (edgeEnd.y - edgeSt.y);
                float hitX = edgeSt.x + ((point.y - edgeSt.y) * slope);
                if (hitX >= point.x && hitX <= endPt.x)
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
        closest = Vector2::ZERO;
        pointA = -1;
        pointB = -1;
        edgeD = 0.0f;
        normal = Vector2::ZERO;
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
        Vector2 toP(point.x - ptA.x, point.y - ptA.y);
        Vector2 e(ptB.x - ptA.x, ptB.y - ptA.y);

        float edgeLength = mathf::sqrt(e.x * e.x + e.y * e.y);
        if (edgeLength > mathf::EPSILON)
        {
            e.x /= edgeLength;
            e.y /= edgeLength;
        }

        Vector2 perpendicular = e.perpendicular();
        float dot = Vector2::dot(toP, e);
        if (dot <= mathf::EPSILON)
        {
            distance = Vector2::distance(point, ptA);
            hitPt = ptA;
            edgeD = 0.0f;
            normal = perpendicular;
        }
        else if (dot >= edgeLength)
        {
            distance = Vector2::distance(point, ptB);
            hitPt = ptB;
            edgeD = 1.0f;
            normal = perpendicular;
        }
        else
        {
            Vector3 toP3(toP.x, toP.y, 0.0f);
            Vector3 e3(e.x, e.y, 0.0f);
            e3 = Vector3::cross(toP3, e3);
            distance = mathf::abs(e3.z);
            hitPt.x = ptA.x + e.x * dot;
            hitPt.y = ptA.y + e.y * dot;
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
        Vector2 toP(point.x - ptA.x, point.y - ptA.y);
        Vector2 edge(ptB.x - ptA.x, ptB.y - ptA.y);

        float edgeLength = mathf::sqrt(edge.x * edge.x + edge.y * edge.y);
        if (edgeLength > mathf::EPSILON)
        {
            edge.x /= edgeLength;
            edge.y /= edgeLength;
        }

        Vector2 perpendicular = edge.perpendicular();
        float dot = Vector2::dot(toP, edge);
        if (dot <= mathf::EPSILON)
        {
            distanceSquared = Vector2::distance_squared(point, ptA);
            hitPt = ptA;
            edgeD = 0.0f;
            normal = perpendicular;
        }
        else if (dot >= edgeLength)
        {
            distanceSquared = Vector2::distance_squared(point, ptB);
            hitPt = ptB;
            edgeD = 1.0f;
            normal = perpendicular;
        }
        else
        {
            Vector3 toP3(toP.x, toP.y, 0.0f);
            Vector3 e3(edge.x, edge.y, 0.0f);
            e3 = Vector3::cross(toP3, e3);
            distanceSquared = mathf::abs(e3.z * e3.z);
            hitPt.x = ptA.x + edge.x * dot;
            hitPt.y = ptA.y + edge.y * dot;
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
            float thisD = (point - PointMassList[i]->position).length_squared();
            if (thisD < closestSQD)
            {
                closestSQD = thisD;
                closest = i;
            }
        }
        dist = mathf::sqrt(closestSQD);
        return PointMassList[closest];
    }

    void Body::ApplyForce(const Vector2& point, const Vector2& force)
    {
        Vector2 r = Position - point;
        float torqueForce = Vector3::cross(r.vector3_from_vector2(),
            force.vector3_from_vector2()).z;
        for (int i = 0; i < Count; i++)
        {
            Vector2 toPt = PointMassList[i]->position - Position;
            Vector2 torque(toPt.y, -toPt.x);
            PointMassList[i]->force += torque * torqueForce;
            PointMassList[i]->force += force;
        }
    }

    void Body::UpdatePointMasses(double elapsed)
    {
        for (int i = 0; i < Count; i++)
        {
            PointMassList[i]->velocity.x *= DAMPING;
            PointMassList[i]->velocity.y *= DAMPING;
            PointMassList[i]->update(elapsed);
        }
    }

    void Body::UpdateAABB(double elapsed)
    {
        Aabb.clear();
        for (int i = 0; i < Count; i++)
        {
            float x = PointMassList[i]->position.x;
            float y = PointMassList[i]->position.y;
            Aabb.add(x, y);
            x += static_cast<float>(PointMassList[i]->velocity.x * elapsed);
            y += static_cast<float>(PointMassList[i]->velocity.y * elapsed);
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
        position.x = 0.0f;
        position.y = 0.0f;
        velocity.x = 0.0f;
        velocity.y = 0.0f;
        force.x = 0.0f;
        force.y = 0.0f;
        for (int i = 0; i < Count; i++)
        {
            position.x += PointMassList[i]->position.x * inverse_count;
            position.y += PointMassList[i]->position.y * inverse_count;
            velocity.x += PointMassList[i]->velocity.x * inverse_count;
            velocity.y += PointMassList[i]->velocity.y * inverse_count;
            force.x += PointMassList[i]->force.x * inverse_count;
            force.y += PointMassList[i]->force.y * inverse_count;
        }
    }
}
