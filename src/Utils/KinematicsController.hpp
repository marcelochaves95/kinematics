#pragma once

#include <algorithm>
#include <cmath>
#include <functional>
#include <limits>
#include <memory>
#include <random>
#include <vector>

#include "Collision/AABB.hpp"
#include "Collision/Chain.hpp"
#include "Collision/Collision.hpp"
#include "Collision/CollisionInfo.hpp"
#include "Collision/PointMass.hpp"
#include "Dynamics/Body.hpp"
#include "Math/Mathf.hpp"
#include "Math/Vector2.hpp"

// Port of src/Utils/KinematicsController.cs
//
// List<Body>/List<Chain> become vector<shared_ptr<...>> (ownership + polymorphism).
// Action<...> callbacks become std::function. The C# Random (time-seeded, only
// used by MoveDistantBodies) becomes a fixed-seed std::mt19937 — deterministic,
// which is preferable for physics; MoveDistantBodies is off the parity-critical path.

namespace kinematics {

class KinematicsController {
public:
    std::vector<std::shared_ptr<Chain>> ChainList;
    std::vector<std::shared_ptr<Body>> BodyList;
    int PenetrationCount = 0;
    float PenetrationThreshold = 0.015f;
    float Friction = 1.9f;
    float Elasticity = 1.5f;
    AABB AABB;
    Vector2 Size;
    Vector2 Cell;
    std::function<void(Body&, Body&)> OnAABBCollision;
    std::function<void(Body&, Body&, const CollisionInfo&)> OnCollision;
    std::function<void(float, Body&, Body&)> OnPenetration;

    KinematicsController() = default;

    void Add(const std::shared_ptr<Chain>& chain) {
        if (std::find(ChainList.begin(), ChainList.end(), chain) == ChainList.end()) {
            ChainList.push_back(chain);
        }
    }

    void Remove(const std::shared_ptr<Chain>& chain) {
        auto it = std::find(ChainList.begin(), ChainList.end(), chain);
        if (it != ChainList.end()) {
            ChainList.erase(it);
        }
    }

    void Add(const std::shared_ptr<Body>& body) {
        if (std::find(BodyList.begin(), BodyList.end(), body) == BodyList.end()) {
            BodyList.push_back(body);
        }
    }

    void Remove(const std::shared_ptr<Body>& body) {
        auto it = std::find(BodyList.begin(), BodyList.end(), body);
        if (it != BodyList.end()) {
            BodyList.erase(it);
        }
    }

    void SetWorldLimits(Vector2 min, Vector2 max) {
        AABB = kinematics::AABB(min, max);
        Size = max - min;
        Cell = Size / 32.0f;
    }

    void UpdateBitmask(Body& body) {
        kinematics::AABB box = body.AABB;

        int minX = static_cast<int>(mathf::Floor((box.Min.X - AABB.Min.X) / Cell.X));
        int maxX = static_cast<int>(mathf::Floor((box.Max.X - AABB.Min.X) / Cell.X));
        if (minX < 0) { minX = 0; } else if (minX > 32) { minX = 32; }
        if (maxX < 0) { maxX = 0; } else if (maxX > 32) { maxX = 32; }

        int minY = static_cast<int>(mathf::Floor((box.Min.Y - AABB.Min.Y) / Cell.Y));
        int maxY = static_cast<int>(mathf::Floor((box.Max.Y - AABB.Min.Y) / Cell.Y));
        if (minY < 0) { minY = 0; } else if (minY > 32) { minY = 32; }
        if (maxY < 0) { maxY = 0; } else if (maxY > 32) { maxY = 32; }

        body.BitmaskX.Clear();
        for (int i = minX; i <= maxX; i++) {
            body.BitmaskX.SetOn(i);
        }

        body.BitmaskY.Clear();
        for (int i = minY; i <= maxY; i++) {
            body.BitmaskY.SetOn(i);
        }
    }

    bool IsPointInsideAnyBody(Vector2 point) {
        for (size_t i = 0; i < BodyList.size(); i++) {
            Body& body = *BodyList[i];
            if (!body.AABB.Contains(point.X, point.Y)) {
                continue;
            }
            if (body.Contains(point)) {
                return true;
            }
        }
        return false;
    }

    void Initialize() {
        Vector2 min = Vector2::Zero;
        Vector2 max = Vector2::Zero;

        for (size_t i = 0; i < BodyList.size(); i++) {
            if (!BodyList[i]->IsStatic) {
                continue;
            }

            BodyList[i]->RotateShape(0);
            BodyList[i]->Update(0);

            if (BodyList[i]->AABB.Min.X < min.X) min.X = BodyList[i]->AABB.Min.X;
            if (BodyList[i]->AABB.Min.Y < min.Y) min.Y = BodyList[i]->AABB.Min.Y;
            if (BodyList[i]->AABB.Max.X > max.X) max.X = BodyList[i]->AABB.Max.X;
            if (BodyList[i]->AABB.Max.Y > max.Y) max.Y = BodyList[i]->AABB.Max.Y;
        }

        SetWorldLimits(min, max);
        _initialized = true;
    }

    void MoveDistantBodies(Vector2 position, float near_, float far_) {
        for (size_t i = 0; i < BodyList.size(); i++) {
            Body& body = *BodyList[i];
            if (body.IsStatic) {
                continue;
            }

            float distance = (body.Position - position).Length();
            Vector2 point;
            if (distance > far_) {
                point.X = nextFloat() - 0.5f;
                point.Y = nextFloat() - 0.5f;
                point.Normalize();
                point = point * (near_ + (far_ - near_) * nextFloat());
                point += position;

                while (IsPointInsideAnyBody(point)) {
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

    void Update(double elapsed) {
        if (!_initialized) {
            Initialize();
        }

        PenetrationCount = 0;
        _collisions.clear();

        for (size_t i = 0; i < BodyList.size(); i++) {
            BodyList[i]->Update(elapsed);
            UpdateBitmask(*BodyList[i]);
        }

        for (size_t i = 0; i < ChainList.size(); i++) {
            ChainList[i]->Update(elapsed);
        }

        for (size_t i = 0; i < BodyList.size(); i++) {
            for (size_t j = i + 1; j < BodyList.size(); j++) {
                if (BodyList[i]->IsStatic && BodyList[j]->IsStatic) {
                    continue;
                }

                if ((BodyList[i]->BitmaskX.Mask & BodyList[j]->BitmaskX.Mask) == 0 ||
                    (BodyList[i]->BitmaskY.Mask & BodyList[j]->BitmaskY.Mask) == 0) {
                    continue;
                }

                if (!BodyList[i]->AABB.Intersects(BodyList[j]->AABB)) {
                    continue;
                }

                if (OnAABBCollision) {
                    OnAABBCollision(*BodyList[i], *BodyList[j]);
                }
                auto a = Collision::Intersects(*BodyList[j], *BodyList[i]);
                _collisions.insert(_collisions.end(), a.begin(), a.end());
                auto b = Collision::Intersects(*BodyList[i], *BodyList[j]);
                _collisions.insert(_collisions.end(), b.begin(), b.end());
            }
        }

        for (size_t i = 0; i < _collisions.size(); i++) {
            CollisionInfo info = _collisions[i];
            PointMassPtr A = info.PointMassA;
            PointMassPtr B1 = info.PointMassB;
            PointMassPtr B2 = info.PointMassC;
            if (OnCollision) {
                OnCollision(*info.BodyA, *info.BodyB, info);
            }

            Vector2 bVel((B1->Velocity.X + B2->Velocity.X) * 0.5f,
                         (B1->Velocity.Y + B2->Velocity.Y) * 0.5f);
            Vector2 relVel(A->Velocity.X - bVel.X, A->Velocity.Y - bVel.Y);
            float relDot = Vector2::Dot(relVel, info.Normal);

            if (OnPenetration) {
                OnPenetration(info.Penetration, *info.BodyA, *info.BodyB);
            }

            if (info.Penetration > 1.0f) {
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
            // Cap single-frame position correction to 0.1 m (see KinematicsController.cs).
            const float MaxCorrection = 0.1f;
            if (std::isinf(A->Mass)) {
                moveA = 0.0f;
                moveB = mathf::Min(info.Penetration, MaxCorrection) + 0.001f;
            } else if (std::isinf(b2MassSum)) {
                moveA = mathf::Min(info.Penetration, MaxCorrection) + 0.001f;
                moveB = 0.0f;
            } else {
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
            float j = jNumerator / jDenom;
            if (!std::isinf(A->Mass)) {
                A->Position.X += info.Normal.X * moveA;
                A->Position.Y += info.Normal.Y * moveA;
            }

            if (!std::isinf(B1->Mass)) {
                B1->Position.X -= info.Normal.X * B1move;
                B1->Position.Y -= info.Normal.Y * B1move;
            }

            if (!std::isinf(B2->Mass)) {
                B2->Position.X -= info.Normal.X * B2move;
                B2->Position.Y -= info.Normal.Y * B2move;
            }

            Vector2 tangent = info.Normal.Perpendicular();
            float fNumerator = Vector2::Dot(relVel, tangent);
            fNumerator *= Friction;
            float f = fNumerator / jDenom;
            if (relDot <= mathf::Epsilon) {
                if (!std::isinf(A->Mass)) {
                    A->Velocity.X += info.Normal.X * (j / A->Mass) - tangent.X * (f / A->Mass);
                    A->Velocity.Y += info.Normal.Y * (j / A->Mass) - tangent.Y * (f / A->Mass);
                }

                if (!std::isinf(b2MassSum)) {
                    B1->Velocity.X -= info.Normal.X * (j / b2MassSum) * b1inf - tangent.X * (f / b2MassSum) * b1inf;
                    B1->Velocity.Y -= info.Normal.Y * (j / b2MassSum) * b1inf - tangent.Y * (f / b2MassSum) * b1inf;
                }

                if (!std::isinf(b2MassSum)) {
                    B2->Velocity.X -= info.Normal.X * (j / b2MassSum) * b2inf - tangent.X * (f / b2MassSum) * b2inf;
                    B2->Velocity.Y -= info.Normal.Y * (j / b2MassSum) * b2inf - tangent.Y * (f / b2MassSum) * b2inf;
                }
            }
        }

        for (size_t i = 0; i < BodyList.size(); i++) {
            BodyList[i]->UpdateBodyPositionVelocityForce();
        }
    }

private:
    float nextFloat() {
        return std::uniform_real_distribution<float>(0.0f, 1.0f)(_random);
    }

    std::vector<CollisionInfo> _collisions;
    bool _initialized = false;
    std::mt19937 _random{0u};
};

} // namespace kinematics
