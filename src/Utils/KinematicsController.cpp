#include <Utils/KinematicsController.h>
#include <collision/collision.h>
#include <math/mathf.h>
#include <algorithm>
#include <cmath>
#include <limits>

namespace kinematics
{
    void KinematicsController::Add(const std::shared_ptr<Chain>& chain)
    {
        if (std::find(ChainList.begin(), ChainList.end(), chain) == ChainList.end())
        {
            ChainList.push_back(chain);
        }
    }

    void KinematicsController::Remove(const std::shared_ptr<Chain>& chain)
    {
        auto it = std::find(ChainList.begin(), ChainList.end(), chain);
        if (it != ChainList.end())
        {
            ChainList.erase(it);
        }
    }

    void KinematicsController::Add(const std::shared_ptr<Body>& body)
    {
        if (std::find(BodyList.begin(), BodyList.end(), body) == BodyList.end())
        {
            BodyList.push_back(body);
        }
    }

    void KinematicsController::Remove(const std::shared_ptr<Body>& body)
    {
        if (_stepping)
        {
            // Defer: collision records still reference this body this frame.
            _pendingRemove.push_back(body);
            return;
        }
        auto it = std::find(BodyList.begin(), BodyList.end(), body);
        if (it != BodyList.end())
        {
            BodyList.erase(it);
        }
    }

    void KinematicsController::SetWorldLimits(Vector2 min, Vector2 max)
    {
        Aabb = kinematics::AABB(min, max);
        Size = max - min;
        Cell = Size / 32.0f;

        // Guard against a degenerate (zero-extent) world — e.g. no static bodies
        // and no explicit limits, which leaves min == max == (0,0). Without this,
        // UpdateBitmask divides by Cell.x/Cell.y == 0 -> Inf/NaN -> UB on the int
        // cast. A unit cell just maps every body into the same broadphase column.
        if (Cell.x <= 0.0f)
        {
            Cell.x = 1.0f;
        }
        if (Cell.y <= 0.0f)
        {
            Cell.y = 1.0f;
        }
    }

    void KinematicsController::UpdateBitmask(Body& body)
    {
        kinematics::AABB box = body.Aabb;

        int minX = static_cast<int>(mathf::floor((box.min.x - Aabb.min.x) / Cell.x));
        int maxX = static_cast<int>(mathf::floor((box.max.x - Aabb.min.x) / Cell.x));
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

        int minY = static_cast<int>(mathf::floor((box.min.y - Aabb.min.y) / Cell.y));
        int maxY = static_cast<int>(mathf::floor((box.max.y - Aabb.min.y) / Cell.y));
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

        body.BitmaskX.Clear();
        for (int i = minX; i <= maxX; i++)
        {
            body.BitmaskX.SetOn(i);
        }

        body.BitmaskY.Clear();
        for (int i = minY; i <= maxY; i++)
        {
            body.BitmaskY.SetOn(i);
        }
    }

    bool KinematicsController::IsPointInsideAnyBody(Vector2 point)
    {
        for (size_t i = 0; i < BodyList.size(); i++)
        {
            Body& body = *BodyList[i];
            if (!body.Aabb.contains(point.x, point.y))
            {
                continue;
            }
            if (body.Contains(point))
            {
                return true;
            }
        }
        return false;
    }

    void KinematicsController::Initialize()
    {
        Vector2 min = Vector2::ZERO;
        Vector2 max = Vector2::ZERO;

        for (size_t i = 0; i < BodyList.size(); i++)
        {
            if (!BodyList[i]->IsStatic)
            {
                continue;
            }

            BodyList[i]->RotateShape(0);
            BodyList[i]->Update(0);

            if (BodyList[i]->Aabb.min.x < min.x)
            {
                min.x = BodyList[i]->Aabb.min.x;
            }
            if (BodyList[i]->Aabb.min.y < min.y)
            {
                min.y = BodyList[i]->Aabb.min.y;
            }
            if (BodyList[i]->Aabb.max.x > max.x)
            {
                max.x = BodyList[i]->Aabb.max.x;
            }
            if (BodyList[i]->Aabb.max.y > max.y)
            {
                max.y = BodyList[i]->Aabb.max.y;
            }
        }

        SetWorldLimits(min, max);
        _initialized = true;
    }

    void KinematicsController::MoveDistantBodies(Vector2 position, float near_, float far_)
    {
        for (size_t i = 0; i < BodyList.size(); i++)
        {
            Body& body = *BodyList[i];
            if (body.IsStatic)
            {
                continue;
            }

            float distance = (body.Position - position).length();
            Vector2 point;
            if (distance > far_)
            {
                point.x = nextFloat() - 0.5f;
                point.y = nextFloat() - 0.5f;
                point.normalize();
                point = point * (near_ + (far_ - near_) * nextFloat());
                point += position;

                while (IsPointInsideAnyBody(point))
                {
                    point.x = nextFloat() - 0.5f;
                    point.y = nextFloat() - 0.5f;
                    point.normalize();
                    point = point * (near_ + (far_ - near_) * nextFloat());
                    point += position;
                }

                BodyList[i]->Position = point;
                BodyList[i]->Update(0);
            }
        }
    }

    void KinematicsController::Update(double elapsed)
    {
        if (!_initialized)
        {
            Initialize();
        }

        PenetrationCount = 0;
        _collisions.clear();
        _stepping = true;

        for (size_t i = 0; i < BodyList.size(); i++)
        {
            BodyList[i]->Update(elapsed);
            UpdateBitmask(*BodyList[i]);
        }

        for (size_t i = 0; i < ChainList.size(); i++)
        {
            ChainList[i]->update(elapsed);
        }

        for (size_t i = 0; i < BodyList.size(); i++)
        {
            for (size_t j = i + 1; j < BodyList.size(); j++)
            {
                if (BodyList[i]->IsStatic && BodyList[j]->IsStatic)
                {
                    continue;
                }

                if ((BodyList[i]->BitmaskX.Mask & BodyList[j]->BitmaskX.Mask) == 0 ||
                    (BodyList[i]->BitmaskY.Mask & BodyList[j]->BitmaskY.Mask) == 0)
                {
                    continue;
                }

                if (!BodyList[i]->Aabb.intersects(BodyList[j]->Aabb))
                {
                    continue;
                }

                if (OnAABBCollision)
                {
                    OnAABBCollision(*BodyList[i], *BodyList[j]);
                }
                Collision::intersects(*BodyList[j], *BodyList[i], _collisions);
                Collision::intersects(*BodyList[i], *BodyList[j], _collisions);
            }
        }

        for (size_t i = 0; i < _collisions.size(); i++)
        {
            const CollisionInfo& info = _collisions[i];
            const PointMassPtr& A = info.point_mass_a;
            const PointMassPtr& B1 = info.point_mass_b;
            const PointMassPtr& B2 = info.point_mass_c;
            if (OnCollision)
            {
                OnCollision(*info.body_a, *info.body_b, info);
            }

            Vector2 bVel((B1->velocity.x + B2->velocity.x) * 0.5f,
                (B1->velocity.y + B2->velocity.y) * 0.5f);
            Vector2 relVel(A->velocity.x - bVel.x, A->velocity.y - bVel.y);
            float relDot = Vector2::dot(relVel, info.normal);

            if (OnPenetration)
            {
                OnPenetration(info.penetration, *info.body_a, *info.body_b);
            }

            if (info.penetration > 1.0f)
            {
                PenetrationCount++;
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
            // Cap single-frame position correction to 0.1 m (see the C# original).
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
            float elasticity = Elasticity;
            numV.x = relVel.x * elasticity;
            numV.y = relVel.y * elasticity;
            float jNumerator = Vector2::dot(numV, info.normal);
            jNumerator = -jNumerator;
            // jDenom == 0 means both colliding sides are effectively immovable
            // (infinite mass): there is no impulse to distribute. Guard the
            // divides so j/f stay finite instead of Inf/NaN. (Today the velocity
            // updates below are already mass-guarded, so these would be unused,
            // but this keeps the math defined if those guards ever change.)
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
            fNumerator *= Friction;
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

        for (size_t i = 0; i < BodyList.size(); i++)
        {
            BodyList[i]->UpdateBodyPositionVelocityForce();
        }

        // Step finished: apply any removals deferred from callbacks above.
        _stepping = false;
        for (const std::shared_ptr<Body>& body : _pendingRemove)
        {
            auto it = std::find(BodyList.begin(), BodyList.end(), body);
            if (it != BodyList.end())
            {
                BodyList.erase(it);
            }
        }
        _pendingRemove.clear();
    }

    float KinematicsController::nextFloat()
    {
        return std::uniform_real_distribution<float>(0.0f, 1.0f)(_random);
    }
}
