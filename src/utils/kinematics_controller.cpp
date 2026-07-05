#include <utils/kinematics_controller.h>
#include <collision/collision.h>
#include <math/mathf.h>
#include <algorithm>
#include <cmath>
#include <limits>

namespace kinematics
{
    void KinematicsController::add(const std::shared_ptr<Chain>& chain)
    {
        if (std::find(chain_list.begin(), chain_list.end(), chain) == chain_list.end())
        {
            chain_list.push_back(chain);
        }
    }

    void KinematicsController::remove(const std::shared_ptr<Chain>& chain)
    {
        auto it = std::find(chain_list.begin(), chain_list.end(), chain);
        if (it != chain_list.end())
        {
            chain_list.erase(it);
        }
    }

    void KinematicsController::add(const std::shared_ptr<Body>& body)
    {
        if (std::find(body_list.begin(), body_list.end(), body) == body_list.end())
        {
            body_list.push_back(body);
        }
    }

    void KinematicsController::remove(const std::shared_ptr<Body>& body)
    {
        if (_stepping)
        {

            _pending_remove.push_back(body);
            return;
        }
        auto it = std::find(body_list.begin(), body_list.end(), body);
        if (it != body_list.end())
        {
            body_list.erase(it);
        }
    }

    void KinematicsController::set_world_limits(Vector2 min, Vector2 max)
    {
        aabb = kinematics::AABB(min, max);
        size = max - min;
        cell = size / 32.0f;

        if (cell.x <= 0.0f)
        {
            cell.x = 1.0f;
        }
        if (cell.y <= 0.0f)
        {
            cell.y = 1.0f;
        }
    }

    void KinematicsController::update_bitmask(Body& body)
    {
        kinematics::AABB box = body.aabb;

        int minX = static_cast<int>(mathf::floor((box.min.x - aabb.min.x) / cell.x));
        int maxX = static_cast<int>(mathf::floor((box.max.x - aabb.min.x) / cell.x));
        if (minX < 0)
        {
            minX = 0;
        }
        else if (minX > 32)
        {
            minX = 32;
        }
        if (maxX < 0)
        {
            maxX = 0;
        }
        else if (maxX > 32)
        {
            maxX = 32;
        }

        int minY = static_cast<int>(mathf::floor((box.min.y - aabb.min.y) / cell.y));
        int maxY = static_cast<int>(mathf::floor((box.max.y - aabb.min.y) / cell.y));
        if (minY < 0)
        {
            minY = 0;
        }
        else if (minY > 32)
        {
            minY = 32;
        }
        if (maxY < 0)
        {
            maxY = 0;
        }
        else if (maxY > 32)
        {
            maxY = 32;
        }

        body.bitmask_x.clear();
        for (int i = minX; i <= maxX; i++)
        {
            body.bitmask_x.set_on(i);
        }

        body.bitmask_y.clear();
        for (int i = minY; i <= maxY; i++)
        {
            body.bitmask_y.set_on(i);
        }
    }

    bool KinematicsController::is_point_inside_any_body(Vector2 point)
    {
        for (size_t i = 0; i < body_list.size(); i++)
        {
            Body& body = *body_list[i];
            if (!body.aabb.contains(point.x, point.y))
            {
                continue;
            }
            if (body.contains(point))
            {
                return true;
            }
        }
        return false;
    }

    void KinematicsController::initialize()
    {
        Vector2 min = Vector2::ZERO;
        Vector2 max = Vector2::ZERO;

        for (size_t i = 0; i < body_list.size(); i++)
        {
            if (!body_list[i]->is_static)
            {
                continue;
            }

            body_list[i]->rotate_shape(0);
            body_list[i]->update(0);

            if (body_list[i]->aabb.min.x < min.x)
            {
                min.x = body_list[i]->aabb.min.x;
            }
            if (body_list[i]->aabb.min.y < min.y)
            {
                min.y = body_list[i]->aabb.min.y;
            }
            if (body_list[i]->aabb.max.x > max.x)
            {
                max.x = body_list[i]->aabb.max.x;
            }
            if (body_list[i]->aabb.max.y > max.y)
            {
                max.y = body_list[i]->aabb.max.y;
            }
        }

        set_world_limits(min, max);
        _initialized = true;
    }

    void KinematicsController::move_distant_bodies(Vector2 position, float near_, float far_)
    {
        for (size_t i = 0; i < body_list.size(); i++)
        {
            Body& body = *body_list[i];
            if (body.is_static)
            {
                continue;
            }

            float distance = (body.position - position).length();
            Vector2 point;
            if (distance > far_)
            {
                point.x = next_float() - 0.5f;
                point.y = next_float() - 0.5f;
                point.normalize();
                point = point * (near_ + (far_ - near_) * next_float());
                point += position;

                while (is_point_inside_any_body(point))
                {
                    point.x = next_float() - 0.5f;
                    point.y = next_float() - 0.5f;
                    point.normalize();
                    point = point * (near_ + (far_ - near_) * next_float());
                    point += position;
                }

                body_list[i]->position = point;
                body_list[i]->update(0);
            }
        }
    }

    void KinematicsController::update(double elapsed)
    {
        if (!_initialized)
        {
            initialize();
        }

        penetration_count = 0;
        _collisions.clear();
        _stepping = true;

        for (size_t i = 0; i < body_list.size(); i++)
        {
            body_list[i]->update(elapsed);
            update_bitmask(*body_list[i]);
        }

        for (size_t i = 0; i < chain_list.size(); i++)
        {
            chain_list[i]->update(elapsed);
        }

        for (size_t i = 0; i < body_list.size(); i++)
        {
            for (size_t j = i + 1; j < body_list.size(); j++)
            {
                if (body_list[i]->is_static && body_list[j]->is_static)
                {
                    continue;
                }

                if ((body_list[i]->bitmask_x.mask & body_list[j]->bitmask_x.mask) == 0 ||
                    (body_list[i]->bitmask_y.mask & body_list[j]->bitmask_y.mask) == 0)
                {
                    continue;
                }

                if (!body_list[i]->aabb.intersects(body_list[j]->aabb))
                {
                    continue;
                }

                if (on_aabb_collision)
                {
                    on_aabb_collision(*body_list[i], *body_list[j]);
                }
                Collision::intersects(*body_list[j], *body_list[i], _collisions);
                Collision::intersects(*body_list[i], *body_list[j], _collisions);
            }
        }

        for (size_t i = 0; i < _collisions.size(); i++)
        {
            const CollisionInfo& info = _collisions[i];
            const PointMassPtr& A = info.point_mass_a;
            const PointMassPtr& B1 = info.point_mass_b;
            const PointMassPtr& B2 = info.point_mass_c;
            if (on_collision)
            {
                on_collision(*info.body_a, *info.body_b, info);
            }

            Vector2 bVel((B1->velocity.x + B2->velocity.x) * 0.5f,
                (B1->velocity.y + B2->velocity.y) * 0.5f);
            Vector2 relVel(A->velocity.x - bVel.x, A->velocity.y - bVel.y);
            float relDot = Vector2::dot(relVel, info.normal);

            if (on_penetration)
            {
                on_penetration(info.penetration, *info.body_a, *info.body_b);
            }

            if (info.penetration > 1.0f)
            {
                penetration_count++;
                continue;
            }

            float b1inf = 1.0f - info.edge_distance;
            float b2inf = info.edge_distance;
            float b2MassSum = (std::isinf(B1->mass) || std::isinf(B2->mass))
                                  ? std::numeric_limits<float>::infinity()
                                  : (B1->mass + B2->mass);
            float massSum = A->mass + b2MassSum;
            float moveA;
            float moveB;

            const float MaxCorrection = 0.1f;
            if (std::isinf(A->mass))
            {
                moveA = 0.0f;
                moveB = mathf::min(info.penetration, MaxCorrection) + 0.001f;
            }
            else if (std::isinf(b2MassSum))
            {
                moveA = mathf::min(info.penetration, MaxCorrection) + 0.001f;
                moveB = 0.0f;
            }
            else
            {
                moveA = info.penetration * (b2MassSum / massSum);
                moveB = info.penetration * (A->mass / massSum);
            }

            float B1move = moveB * b1inf;
            float B2move = moveB * b2inf;
            float invMassA = std::isinf(A->mass) ? 0.0f : 1.0f / A->mass;
            float invMassB = std::isinf(b2MassSum) ? 0.0f : 1.0f / b2MassSum;
            float jDenom = invMassA + invMassB;
            Vector2 numV;
            float elasticity = this->elasticity;
            numV.x = relVel.x * elasticity;
            numV.y = relVel.y * elasticity;
            float jNumerator = Vector2::dot(numV, info.normal);
            jNumerator = -jNumerator;

            float j = jDenom > 0.0f ? jNumerator / jDenom : 0.0f;
            if (!std::isinf(A->mass))
            {
                A->position.x += info.normal.x * moveA;
                A->position.y += info.normal.y * moveA;
            }

            if (!std::isinf(B1->mass))
            {
                B1->position.x -= info.normal.x * B1move;
                B1->position.y -= info.normal.y * B1move;
            }

            if (!std::isinf(B2->mass))
            {
                B2->position.x -= info.normal.x * B2move;
                B2->position.y -= info.normal.y * B2move;
            }

            Vector2 tangent = info.normal.perpendicular();
            float fNumerator = Vector2::dot(relVel, tangent);
            fNumerator *= friction;
            float f = jDenom > 0.0f ? fNumerator / jDenom : 0.0f;
            if (relDot <= mathf::EPSILON)
            {
                if (!std::isinf(A->mass))
                {
                    A->velocity.x += info.normal.x * (j / A->mass) - tangent.x * (f / A->mass);
                    A->velocity.y += info.normal.y * (j / A->mass) - tangent.y * (f / A->mass);
                }

                if (!std::isinf(b2MassSum))
                {
                    B1->velocity.x -= info.normal.x * (j / b2MassSum) * b1inf - tangent.x * (f / b2MassSum) * b1inf;
                    B1->velocity.y -= info.normal.y * (j / b2MassSum) * b1inf - tangent.y * (f / b2MassSum) * b1inf;
                }

                if (!std::isinf(b2MassSum))
                {
                    B2->velocity.x -= info.normal.x * (j / b2MassSum) * b2inf - tangent.x * (f / b2MassSum) * b2inf;
                    B2->velocity.y -= info.normal.y * (j / b2MassSum) * b2inf - tangent.y * (f / b2MassSum) * b2inf;
                }
            }
        }

        for (size_t i = 0; i < body_list.size(); i++)
        {
            body_list[i]->update_body_position_velocity_force();
        }

        _stepping = false;
        for (const std::shared_ptr<Body>& body : _pending_remove)
        {
            auto it = std::find(body_list.begin(), body_list.end(), body);
            if (it != body_list.end())
            {
                body_list.erase(it);
            }
        }
        _pending_remove.clear();
    }

    float KinematicsController::next_float()
    {
        return std::uniform_real_distribution<float>(0.0f, 1.0f)(_random);
    }
}
