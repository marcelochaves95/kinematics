#include <dynamics/body.h>
#include <math/mathf.h>
#include <math/vector3.h>
#include <cmath>
#include <limits>
#include <memory>

namespace kinematics
{
    Body::Body(const Shape& shape, float mass)
    {
        _base_shape = shape;
        current_shape = shape.clone();
        count = shape.count;
        point_mass_list.reserve(static_cast<size_t>(shape.count));
        for (int i = 0; i < shape.count; i++)
        {
            point_mass_list.push_back(std::make_shared<PointMass>(shape.points[i], mass));
        }
    }

    void Body::apply_internal_forces(double)
    {
        for (int i = 0; i < count; i++)
        {
            float mass = point_mass_list[i]->mass;
            if (!std::isinf(mass))
            {
                point_mass_list[i]->force += gravity * mass;
            }
        }
    }

    void Body::update_body_position_velocity_force()
    {
        get_body_position_velocity_force(position, velocity, _force);
    }

    void Body::rotate_shape(double)
    {
        float angle = 0.0f;
        int originalSign = 1;
        float originalAngle = 0.0f;
        for (int i = 0; i < count; i++)
        {
            Vector2 baseNormal = Vector2::normalize(
                Vector2(_base_shape.points[i].x, _base_shape.points[i].y));
            Vector2 currentNormal = Vector2::normalize(
                Vector2(point_mass_list[i]->position.x - position.x,
                    point_mass_list[i]->position.y - position.y));

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

        angle /= count;

        float cos = mathf::cos(angle);
        float sin = mathf::sin(angle);
        for (int i = 0; i < count; i++)
        {
            float x = _base_shape.points[i].x;
            float y = _base_shape.points[i].y;
            current_shape.points[i].x = cos * x - sin * y + position.x;
            current_shape.points[i].y = cos * y + sin * x + position.y;
        }
    }

    void Body::update(double elapsed)
    {
        if (!_is_dirty)
        {
            return;
        }

        set_body_position_velocity_force(position, velocity, _force);
        rotate_shape(elapsed);
        apply_internal_forces(elapsed);
        update_point_masses(elapsed);
        update_aabb(elapsed);
        update_body_position_velocity_force();
        if (is_static)
        {
            _is_dirty = false;
        }
    }

    bool Body::contains(const Vector2& point) const
    {
        Vector2 endPt(aabb.max.x + 0.1f, point.y);
        bool inside = false;
        Vector2 edgeSt = point_mass_list[0]->position;
        for (int i = 0; i < count; i++)
        {
            Vector2 edgeEnd = i < count - 1 ? point_mass_list[i + 1]->position
                                            : point_mass_list[0]->position;
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

    float Body::get_closest_point(Vector2 point, Vector2& closest, Vector2& normal,
        int& pointA, int& pointB, float& edgeD)
    {
        closest = Vector2::ZERO;
        pointA = -1;
        pointB = -1;
        edgeD = 0.0f;
        normal = Vector2::ZERO;
        float closestD = std::numeric_limits<float>::max();
        for (int i = 0; i < count; i++)
        {
            Vector2 tempHit, tempNorm;
            float tempEdgeD;
            float dist = get_closest_point_on_edge(point, i, tempHit, tempNorm, tempEdgeD);
            if (dist < closestD)
            {
                closestD = dist;
                pointA = i;
                pointB = i < count - 1 ? i + 1 : 0;
                edgeD = tempEdgeD;
                normal = tempNorm;
                closest = tempHit;
            }
        }
        return closestD;
    }

    float Body::get_closest_point_on_edge(Vector2 point, int edgeNum, Vector2& hitPt,
        Vector2& normal, float& edgeD)
    {
        hitPt = Vector2(0.0f, 0.0f);
        normal = Vector2(0.0f, 0.0f);
        edgeD = 0.0f;
        float distance;
        Vector2 ptA = point_mass_list[edgeNum]->position;
        Vector2 ptB = edgeNum < count - 1 ? point_mass_list[edgeNum + 1]->position
                                          : point_mass_list[0]->position;
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

    float Body::get_closest_point_on_edge_squared(Vector2 point, int edgeNum, Vector2& hitPt,
        Vector2& normal, float& edgeD)
    {
        hitPt = Vector2(0.0f, 0.0f);
        normal = Vector2(0.0f, 0.0f);
        edgeD = 0.0f;
        float distanceSquared;
        Vector2 ptA = point_mass_list[edgeNum]->position;
        Vector2 ptB = edgeNum < count - 1 ? point_mass_list[edgeNum + 1]->position
                                          : point_mass_list[0]->position;
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

    PointMassPtr Body::get_closest_point_mass(Vector2 point, float& dist)
    {
        float closestSQD = 100000.0f;
        int closest = -1;
        for (int i = 0; i < count; i++)
        {
            float thisD = (point - point_mass_list[i]->position).length_squared();
            if (thisD < closestSQD)
            {
                closestSQD = thisD;
                closest = i;
            }
        }
        dist = mathf::sqrt(closestSQD);
        return point_mass_list[closest];
    }

    void Body::apply_force(const Vector2& point, const Vector2& force)
    {
        Vector2 r = position - point;
        float torqueForce = Vector3::cross(r.vector3_from_vector2(),
            force.vector3_from_vector2()).z;
        for (int i = 0; i < count; i++)
        {
            Vector2 toPt = point_mass_list[i]->position - position;
            Vector2 torque(toPt.y, -toPt.x);
            point_mass_list[i]->force += torque * torqueForce;
            point_mass_list[i]->force += force;
        }
    }

    void Body::update_point_masses(double elapsed)
    {
        for (int i = 0; i < count; i++)
        {
            point_mass_list[i]->velocity.x *= DAMPING;
            point_mass_list[i]->velocity.y *= DAMPING;
            point_mass_list[i]->update(elapsed);
        }
    }

    void Body::update_aabb(double elapsed)
    {
        aabb.clear();
        for (int i = 0; i < count; i++)
        {
            float x = point_mass_list[i]->position.x;
            float y = point_mass_list[i]->position.y;
            aabb.add(x, y);
            x += static_cast<float>(point_mass_list[i]->velocity.x * elapsed);
            y += static_cast<float>(point_mass_list[i]->velocity.y * elapsed);
            aabb.add(x, y);
        }
    }

    void Body::set_body_position_velocity_force(Vector2 position, Vector2 velocity, Vector2 force)
    {
        Vector2 currentPosition, currentVelocity, currentForce;
        get_body_position_velocity_force(currentPosition, currentVelocity, currentForce);
        for (int i = 0; i < count; i++)
        {
            point_mass_list[i]->position -= currentPosition;
            point_mass_list[i]->position += position;
            point_mass_list[i]->velocity -= currentVelocity;
            point_mass_list[i]->velocity += velocity;
            point_mass_list[i]->force -= currentForce;
            point_mass_list[i]->force += force;
        }
    }

    void Body::get_body_position_velocity_force(Vector2& position, Vector2& velocity, Vector2& force)
    {
        float inverse_count = 1.0f / count;
        position.x = 0.0f;
        position.y = 0.0f;
        velocity.x = 0.0f;
        velocity.y = 0.0f;
        force.x = 0.0f;
        force.y = 0.0f;
        for (int i = 0; i < count; i++)
        {
            position.x += point_mass_list[i]->position.x * inverse_count;
            position.y += point_mass_list[i]->position.y * inverse_count;
            velocity.x += point_mass_list[i]->velocity.x * inverse_count;
            velocity.y += point_mass_list[i]->velocity.y * inverse_count;
            force.x += point_mass_list[i]->force.x * inverse_count;
            force.y += point_mass_list[i]->force.y * inverse_count;
        }
    }
}
