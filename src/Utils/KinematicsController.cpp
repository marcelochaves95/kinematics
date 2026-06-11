#include <Utils/KinematicsController.h>
#include <Collision/Collision.h>
#include <Math/Mathf.h>
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
        // UpdateBitmask divides by Cell.X/Cell.Y == 0 -> Inf/NaN -> UB on the int
        // cast. A unit cell just maps every body into the same broadphase column.
        if (Cell.X <= 0.0f)
        {
            Cell.X = 1.0f;
        }
        if (Cell.Y <= 0.0f)
        {
            Cell.Y = 1.0f;
        }
    }

    void KinematicsController::UpdateBitmask(Body& body)
    {
        kinematics::AABB box = body.Aabb;

        int minX = static_cast<int>(mathf::Floor((box.Min.X - Aabb.Min.X) / Cell.X));
        int maxX = static_cast<int>(mathf::Floor((box.Max.X - Aabb.Min.X) / Cell.X));
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

        int minY = static_cast<int>(mathf::Floor((box.Min.Y - Aabb.Min.Y) / Cell.Y));
        int maxY = static_cast<int>(mathf::Floor((box.Max.Y - Aabb.Min.Y) / Cell.Y));
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
            if (!body.Aabb.Contains(point.X, point.Y))
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
        Vector2 min = Vector2::Zero;
        Vector2 max = Vector2::Zero;

        for (size_t i = 0; i < BodyList.size(); i++)
        {
            if (!BodyList[i]->IsStatic)
            {
                continue;
            }

            BodyList[i]->RotateShape(0);
            BodyList[i]->Update(0);

            if (BodyList[i]->Aabb.Min.X < min.X)
            {
                min.X = BodyList[i]->Aabb.Min.X;
            }
            if (BodyList[i]->Aabb.Min.Y < min.Y)
            {
                min.Y = BodyList[i]->Aabb.Min.Y;
            }
            if (BodyList[i]->Aabb.Max.X > max.X)
            {
                max.X = BodyList[i]->Aabb.Max.X;
            }
            if (BodyList[i]->Aabb.Max.Y > max.Y)
            {
                max.Y = BodyList[i]->Aabb.Max.Y;
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

            float distance = (body.Position - position).Length();
            Vector2 point;
            if (distance > far_)
            {
                point.X = nextFloat() - 0.5f;
                point.Y = nextFloat() - 0.5f;
                point.Normalize();
                point = point * (near_ + (far_ - near_) * nextFloat());
                point += position;

                while (IsPointInsideAnyBody(point))
                {
                    point.X = nextFloat() - 0.5f;
                    point.Y = nextFloat() - 0.5f;
                    point.Normalize();
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
            ChainList[i]->Update(elapsed);
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

                if (!BodyList[i]->Aabb.Intersects(BodyList[j]->Aabb))
                {
                    continue;
                }

                if (OnAABBCollision)
                {
                    OnAABBCollision(*BodyList[i], *BodyList[j]);
                }
                Collision::Intersects(*BodyList[j], *BodyList[i], _collisions);
                Collision::Intersects(*BodyList[i], *BodyList[j], _collisions);
            }
        }

        for (size_t i = 0; i < _collisions.size(); i++)
        {
            const CollisionInfo& info = _collisions[i];
            const PointMassPtr& A = info.PointMassA;
            const PointMassPtr& B1 = info.PointMassB;
            const PointMassPtr& B2 = info.PointMassC;
            if (OnCollision)
            {
                OnCollision(*info.BodyA, *info.BodyB, info);
            }

            Vector2 bVel((B1->Velocity.X + B2->Velocity.X) * 0.5f,
                (B1->Velocity.Y + B2->Velocity.Y) * 0.5f);
            Vector2 relVel(A->Velocity.X - bVel.X, A->Velocity.Y - bVel.Y);
            float relDot = Vector2::Dot(relVel, info.Normal);

            if (OnPenetration)
            {
                OnPenetration(info.Penetration, *info.BodyA, *info.BodyB);
            }

            if (info.Penetration > 1.0f)
            {
                PenetrationCount++;
                continue;
            }

            float b1inf = 1.0f - info.EdgeDistance;
            float b2inf = info.EdgeDistance;
            float b2MassSum = (std::isinf(B1->Mass) || std::isinf(B2->Mass))
                                  ? std::numeric_limits<float>::infinity()
                                  : (B1->Mass + B2->Mass);
            float massSum = A->Mass + b2MassSum;
            float moveA;
            float moveB;
            // Cap single-frame position correction to 0.1 m (see the C# original).
            const float MaxCorrection = 0.1f;
            if (std::isinf(A->Mass))
            {
                moveA = 0.0f;
                moveB = mathf::Min(info.Penetration, MaxCorrection) + 0.001f;
            }
            else if (std::isinf(b2MassSum))
            {
                moveA = mathf::Min(info.Penetration, MaxCorrection) + 0.001f;
                moveB = 0.0f;
            }
            else
            {
                moveA = info.Penetration * (b2MassSum / massSum);
                moveB = info.Penetration * (A->Mass / massSum);
            }

            float B1move = moveB * b1inf;
            float B2move = moveB * b2inf;
            float invMassA = std::isinf(A->Mass) ? 0.0f : 1.0f / A->Mass;
            float invMassB = std::isinf(b2MassSum) ? 0.0f : 1.0f / b2MassSum;
            float jDenom = invMassA + invMassB;
            Vector2 numV;
            float elasticity = Elasticity;
            numV.X = relVel.X * elasticity;
            numV.Y = relVel.Y * elasticity;
            float jNumerator = Vector2::Dot(numV, info.Normal);
            jNumerator = -jNumerator;
            // jDenom == 0 means both colliding sides are effectively immovable
            // (infinite mass): there is no impulse to distribute. Guard the
            // divides so j/f stay finite instead of Inf/NaN. (Today the velocity
            // updates below are already mass-guarded, so these would be unused,
            // but this keeps the math defined if those guards ever change.)
            float j = jDenom > 0.0f ? jNumerator / jDenom : 0.0f;
            if (!std::isinf(A->Mass))
            {
                A->Position.X += info.Normal.X * moveA;
                A->Position.Y += info.Normal.Y * moveA;
            }

            if (!std::isinf(B1->Mass))
            {
                B1->Position.X -= info.Normal.X * B1move;
                B1->Position.Y -= info.Normal.Y * B1move;
            }

            if (!std::isinf(B2->Mass))
            {
                B2->Position.X -= info.Normal.X * B2move;
                B2->Position.Y -= info.Normal.Y * B2move;
            }

            Vector2 tangent = info.Normal.Perpendicular();
            float fNumerator = Vector2::Dot(relVel, tangent);
            fNumerator *= Friction;
            float f = jDenom > 0.0f ? fNumerator / jDenom : 0.0f;
            if (relDot <= mathf::Epsilon)
            {
                if (!std::isinf(A->Mass))
                {
                    A->Velocity.X += info.Normal.X * (j / A->Mass) - tangent.X * (f / A->Mass);
                    A->Velocity.Y += info.Normal.Y * (j / A->Mass) - tangent.Y * (f / A->Mass);
                }

                if (!std::isinf(b2MassSum))
                {
                    B1->Velocity.X -= info.Normal.X * (j / b2MassSum) * b1inf - tangent.X * (f / b2MassSum) * b1inf;
                    B1->Velocity.Y -= info.Normal.Y * (j / b2MassSum) * b1inf - tangent.Y * (f / b2MassSum) * b1inf;
                }

                if (!std::isinf(b2MassSum))
                {
                    B2->Velocity.X -= info.Normal.X * (j / b2MassSum) * b2inf - tangent.X * (f / b2MassSum) * b2inf;
                    B2->Velocity.Y -= info.Normal.Y * (j / b2MassSum) * b2inf - tangent.Y * (f / b2MassSum) * b2inf;
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
