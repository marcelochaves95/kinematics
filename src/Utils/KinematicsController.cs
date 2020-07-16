using System;
using System.Collections.Generic;
using Kinematics.Collision;
using Kinematics.Math;
using Kinematics.Dynamics;

namespace Kinematics.Utils
{
    public class KinematicsController
    {
        private static Random _random = new Random();
        public List<Chain> ChainList;
        public List<Body> BodyList;
        private List<CollisionInfo> _collisionList;
        public int PenetrationCount;
        public float PenetrationThreshold = 0.015f;
        public float Friction = 1.9f;
        public float Elasticity = 1.5f;

        public AABB AABB;
        public Vector2 Size;
        public Vector2 Cell;
        private readonly bool _initialized;

        private Action<Body, Body> _onAABBCollision;
        private Action<Body, Body, CollisionInfo> _onCollision;
        private Action<float, Body, Body> _onPenetration;

        public KinematicsController()
        {
            ChainList = new List<Chain>();
            BodyList = new List<Body>();
            _collisionList = new List<CollisionInfo>();
            _initialized = false;
        }

        public void Add(Chain chain)
        {
            if (!ChainList.Contains(chain))
            {
                ChainList.Add(chain);
            }
        }

        public void Remove(Chain chain)
        {
            if (ChainList.Contains(chain))
            {
                ChainList.Remove(chain);
            }
        }

        public void Add(Body body)
        {
            if (!BodyList.Contains(body))
            {
                BodyList.Add(body);
            }
        }

        public void Remove(Body body)
        {
            if (BodyList.Contains(body))
            {
                BodyList.Remove(body);
            }
        }

        public void SetWorldLimits(Vector2 min, Vector2 max)
        {
            AABB = new AABB(ref min, ref max);
            Size = max - min;
            Cell = Size / 32;
        }

        public void UpdateBitmask(Body body)
        {
            AABB box = body.AABB;

            int minX = (int) Mathf.Floor((box.Min.X - AABB.Min.X) / Cell.X);
            int maxX = (int) Mathf.Floor((box.Max.X - AABB.Min.X) / Cell.X);

            if (minX < 0)
            {
                minX = 0;
            } else if (minX > 32)
            {
                minX = 32;
            }

            if (maxX < 0)
            {
                maxX = 0;
            } else if (maxX > 32)
            {
                maxX = 32;
            }

            int minY = (int) Mathf.Floor((box.Min.Y - AABB.Min.Y) / Cell.Y);
            int maxY = (int) Mathf.Floor((box.Max.Y - AABB.Min.Y) / Cell.Y);

            if (minY < 0)
            {
                minY = 0;
            } else if (minY > 32)
            {
                minY = 32;
            }

            if (maxY < 0)
            {
                maxY = 0;
            } else if (maxY > 32)
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

        public bool IsPointInsideAnyBody(Vector2 point)
        {
            for (int i = 0; i < BodyList.Count; i++)
            {
                Body body = BodyList[i];
                if (!body.AABB.Contains(point.X, point.Y))
                {
                    continue;
                }

                if (body.Contains(ref point))
                {
                    return true;
                }
            }

            return false;
        }

        public void Initialize()
        {
            Vector2 min = Vector2.Zero;
            Vector2 max = Vector2.Zero;

            for (int i = 0; i < BodyList.Count; i++)
            {
                if (!BodyList[i].IsStatic)
                {
                    continue;
                }

                BodyList[i].RotateShape(0);
                BodyList[i].Update(0);

                if (BodyList[i].AABB.Min.X < min.X)
                {
                    min.X = BodyList[i].AABB.Min.X;
                }

                if (BodyList[i].AABB.Min.Y < min.Y)
                {
                    min.Y = BodyList[i].AABB.Min.Y;
                }

                if (BodyList[i].AABB.Max.X > max.X)
                {
                    max.X = BodyList[i].AABB.Max.X;
                }

                if (BodyList[i].AABB.Max.Y > max.Y)
                {
                    max.Y = BodyList[i].AABB.Max.Y;
                }
            }

            SetWorldLimits(min, max);
        }

        public void MoveDistantBodies(Vector2 position, float near, float far)
        {
            for (int i = 0; i < BodyList.Count; i++)
            {
                Body body = BodyList[i];
                if (body.IsStatic)
                {
                    continue;
                }

                float distance = (body.Position - position).Length();
                Vector2 point = new Vector2();
                if (distance > far)
                {
                    point.X = (float) _random.NextDouble() - 0.5f;
                    point.Y = (float) _random.NextDouble() - 0.5f;
                    point.Normalize();

                    point *= near + (far - near) * (float) _random.NextDouble();
                    point += position;

                    while (IsPointInsideAnyBody(point))
                    {
                        point.X = (float) _random.NextDouble() - 0.5f;
                        point.Y = (float) _random.NextDouble() - 0.5f;
                        point.Normalize();

                        point *= near + (far - near) * (float) _random.NextDouble();
                        point += position;
                    }

                    BodyList[i].Position = point;
                    BodyList[i].Update(0);
                }
            }
        }

        public void Update(double elapsed)
        {
            if (!_initialized)
            {
                Initialize();
            }

            PenetrationCount = 0;
            _collisionList.Clear();

            for (int i = 0; i < BodyList.Count; i++)
            {
                BodyList[i].Update(elapsed);
                UpdateBitmask(BodyList[i]);
            }

            for (int i = 0; i < ChainList.Count; i++)
            {
                ChainList[i].Update(elapsed);
            }

            for (int i = 0; i < BodyList.Count; i++)
            {
                for (int j = i + 1; j < BodyList.Count; j++)
                {
                    if (BodyList[i].IsStatic && BodyList[j].IsStatic)
                    {
                        continue;
                    }

                    if ((BodyList[i].BitmaskX.Mask & BodyList[j].BitmaskX.Mask) == 0 && ((BodyList[i].BitmaskY.Mask & BodyList[j].BitmaskY.Mask) == 0))
                    {
                        continue;
                    }

                    if (!BodyList[i].AABB.Intersects(ref BodyList[j].AABB))
                    {
                        continue;
                    }

                    _onAABBCollision?.Invoke(BodyList[i], BodyList[j]);

                    _collisionList.AddRange(Collision.Collision.Intersects(BodyList[j], BodyList[i]));
                    _collisionList.AddRange(Collision.Collision.Intersects(BodyList[i], BodyList[j]));
                }
            }

            for (int i = 0; i < _collisionList.Count; i++)
            {
                CollisionInfo info = _collisionList[i];

                PointMass A = info.PointMassA;
                PointMass B1 = info.PointMassB;
                PointMass B2 = info.PointMassC;

                _onCollision?.Invoke(info.BodyA, info.BodyB, info);

                Vector2 bVel = new Vector2
                {
                    X = (B1.Velocity.X + B2.Velocity.X) * 0.5f,
                    Y = (B1.Velocity.Y + B2.Velocity.Y) * 0.5f
                };

                Vector2 relVel = new Vector2
                {
                    X = A.Velocity.X - bVel.X,
                    Y = A.Velocity.Y - bVel.Y
                };

                float relDot = Vector2.Dot(relVel, info.Normal);

                _onPenetration?.Invoke(info.Penetration, info.BodyA, info.BodyB);

                if (info.Penetration > 0.3f)
                {
                    PenetrationCount++;
                    continue;
                }

                float b1inf = 1.0f - info.EdgeDistance;
                float b2inf = info.EdgeDistance;

                float b2MassSum = float.IsPositiveInfinity(B1.Mass) || float.IsPositiveInfinity(B2.Mass) ? float.PositiveInfinity : (B1.Mass + B2.Mass);

                float massSum = A.Mass + b2MassSum;

                float moveA;
                float moveB;
                if (float.IsPositiveInfinity(A.Mass))
                {
                    moveA = 0f;
                    moveB = info.Penetration + 0.001f;
                }
                else if (float.IsPositiveInfinity(b2MassSum))
                {
                    moveA = info.Penetration + 0.001f;
                    moveB = 0f;
                }
                else
                {
                    moveA = info.Penetration * (b2MassSum / massSum);
                    moveB = info.Penetration * (A.Mass / massSum);
                }

                float B1move = moveB * b1inf;
                float B2move = moveB * b2inf;

                float invMassA = float.IsPositiveInfinity(A.Mass) ? 0f : 1f / A.Mass;
                float invMassB = float.IsPositiveInfinity(b2MassSum) ? 0f : 1f / b2MassSum;

                float jDenom = invMassA + invMassB;
                Vector2 numV = new Vector2();
                float elasticity = Elasticity;
                numV.X = relVel.X * elasticity;
                numV.Y = relVel.Y * elasticity;

                float jNumerator = Vector2.Dot(numV, info.Normal);
                jNumerator = -jNumerator;

                float j = jNumerator / jDenom;

                if (!float.IsPositiveInfinity(A.Mass))
                {
                    A.Position.X += info.Normal.X * moveA;
                    A.Position.Y += info.Normal.Y * moveA;
                }

                if (!float.IsPositiveInfinity(B1.Mass))
                {
                    B1.Position.X -= info.Normal.X * B1move;
                    B1.Position.Y -= info.Normal.Y * B1move;
                }

                if (!float.IsPositiveInfinity(B2.Mass))
                {
                    B2.Position.X -= info.Normal.X * B2move;
                    B2.Position.Y -= info.Normal.Y * B2move;
                }

                Vector2 tangent = Vector2.Perpendicular(info.Normal);
                float fNumerator = Vector2.Dot(relVel, tangent);
                fNumerator *= Friction;
                float f = fNumerator / jDenom;

                if (relDot <= 0.0001f)
                {
                    if (!float.IsPositiveInfinity(A.Mass))
                    {
                        A.Velocity.X += info.Normal.X * (j / A.Mass) - tangent.X * (f / A.Mass);
                        A.Velocity.Y += info.Normal.Y * (j / A.Mass) - tangent.Y * (f / A.Mass);
                    }

                    if (!float.IsPositiveInfinity(b2MassSum))
                    {
                        B1.Velocity.X -= info.Normal.X * (j / b2MassSum) * b1inf - tangent.X * (f / b2MassSum) * b1inf;
                        B1.Velocity.Y -= info.Normal.Y * (j / b2MassSum) * b1inf - tangent.Y * (f / b2MassSum) * b1inf;
                    }

                    if (!float.IsPositiveInfinity(b2MassSum))
                    {
                        B2.Velocity.X -= info.Normal.X * (j / b2MassSum) * b2inf - tangent.X * (f / b2MassSum) * b2inf;
                        B2.Velocity.Y -= info.Normal.Y * (j / b2MassSum) * b2inf - tangent.Y * (f / b2MassSum) * b2inf;
                    }
                }
            }

            for (int i = 0; i < BodyList.Count; i++)
            {
                BodyList[i].UpdateBodyPositionVelocityForce();
            }
        }
    }
}