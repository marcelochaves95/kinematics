using System;
using System.Collections.Generic;
using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class Physics
    {
        private static Random _random = new Random();
        public List<Chain> ChainList;
        public List<Body> BodyList;
        public List<CollisionInfo> CollisionList;
        public int PenetrationCount;
        public float PenetrationThreshold = 0.015f;
        public float Friction = 1.9f;
        public float Elasticity = 1.5f;

        public AxisAlignedBoundingBox AABB;
        public Vector2 Size;
        public Vector2 Cell;
        private readonly bool _initialized;

        public Action<Body, Body> OnAABBCollision;
        public Action<Body, Body, CollisionInfo> OnCollision;
        public Action<float, Body, Body> OnPenetration;

        public Physics()
        {
            ChainList = new List<Chain>();
            BodyList = new List<Body>();
            CollisionList = new List<CollisionInfo>();
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
            AABB = new AxisAlignedBoundingBox(ref min, ref max);
            Size = max - min;
            Cell = Size / 32;
        }

        public void UpdateBitmask(Body body)
        {
            AxisAlignedBoundingBox box = body.AABB;

            int minX = (int) Mathf.Floor((box.min.X - AABB.min.X) / Cell.X);
            int maxX = (int) Mathf.Floor((box.max.X - AABB.min.X) / Cell.X);

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

            int minY = (int) Mathf.Floor((box.min.Y - AABB.min.Y) / Cell.Y);
            int maxY = (int) Mathf.Floor((box.max.Y - AABB.min.Y) / Cell.Y);

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
                body.BitmaskX.setOn(i);
            }

            body.BitmaskY.Clear();
            for (int i = minY; i <= maxY; i++)
            {
                body.BitmaskY.setOn(i);
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

                if (BodyList[i].AABB.min.X < min.X)
                {
                    min.X = BodyList[i].AABB.min.X;
                }

                if (BodyList[i].AABB.min.Y < min.Y)
                {
                    min.Y = BodyList[i].AABB.min.Y;
                }

                if (BodyList[i].AABB.max.X > max.X)
                {
                    max.X = BodyList[i].AABB.max.X;
                }

                if (BodyList[i].AABB.max.Y > max.Y)
                {
                    max.Y = BodyList[i].AABB.max.Y;
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
                    point.X = ((float)_random.NextDouble() - 0.5f);
                    point.Y = ((float)_random.NextDouble() - 0.5f);
                    point.Normalize();

                    point *= near + (far - near) * ((float)_random.NextDouble());
                    point += position;

                    while (IsPointInsideAnyBody(point))
                    {
                        point.X = ((float)_random.NextDouble() - 0.5f);
                        point.Y = ((float)_random.NextDouble() - 0.5f);
                        point.Normalize();

                        point *= near + (far - near) * ((float)_random.NextDouble());
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
            CollisionList.Clear();

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

                    if (((BodyList[i].BitmaskX.Mask & BodyList[j].BitmaskX.Mask) == 0) && ((BodyList[i].BitmaskY.Mask & BodyList[j].BitmaskY.Mask) == 0))
                    {
                        continue;
                    }

                    if (!(BodyList[i].AABB).Intersects(ref (BodyList[j].AABB)))
                    {
                        continue;
                    }

                    OnAABBCollision?.Invoke(BodyList[i], BodyList[j]);

                    CollisionList.AddRange(Collision.Intersects(BodyList[j], BodyList[i]));
                    CollisionList.AddRange(Collision.Intersects(BodyList[i], BodyList[j]));
                }
            }

            for (int i = 0; i < CollisionList.Count; i++)
            {
                CollisionInfo info = CollisionList[i];

                PointMass A = info.PointMassA;
                PointMass B1 = info.PointMassB;
                PointMass B2 = info.PointMassC;

                if (OnCollision != null)
                {
                    OnCollision(info.BodyA, info.BodyB, info);
                }

                Vector2 bVel = new Vector2
                {
                    X = (B1.velocity.X + B2.velocity.X) * 0.5f,
                    Y = (B1.velocity.Y + B2.velocity.Y) * 0.5f
                };

                Vector2 relVel = new Vector2
                {
                    X = A.velocity.X - bVel.X,
                    Y = A.velocity.Y - bVel.Y
                };

                Vector2.Dot(ref relVel, ref info.Normal, out float relDot);

                if (OnPenetration != null)
                {
                    OnPenetration(info.Penetration, info.BodyA, info.BodyB);
                }

                if (info.Penetration > 0.3f)
                {
                    PenetrationCount++;
                    continue;
                }

                float b1inf = 1.0f - info.EdgeDistance;
                float b2inf = info.EdgeDistance;

                float b2MassSum = float.IsPositiveInfinity(B1.mass) || float.IsPositiveInfinity(B2.mass) ? float.PositiveInfinity : (B1.mass + B2.mass);

                float massSum = A.mass + b2MassSum;

                float moveA;
                float moveB;
                if (float.IsPositiveInfinity(A.mass))
                {
                    moveA = 0f;
                    moveB = (info.Penetration) + 0.001f;
                }
                else if (float.IsPositiveInfinity(b2MassSum))
                {
                    moveA = (info.Penetration) + 0.001f;
                    moveB = 0f;
                }
                else
                {
                    moveA = (info.Penetration * (b2MassSum / massSum));
                    moveB = (info.Penetration * (A.mass / massSum));
                }

                float B1move = moveB * b1inf;
                float B2move = moveB * b2inf;

                float invMassA = (float.IsPositiveInfinity(A.mass)) ? 0f : 1f / A.mass;
                float invMassB = (float.IsPositiveInfinity(b2MassSum)) ? 0f : 1f / b2MassSum;

                float jDenom = invMassA + invMassB;
                Vector2 numV = new Vector2();
                float elasticity = Elasticity;
                numV.X = relVel.X * elasticity;
                numV.Y = relVel.Y * elasticity;

                Vector2.Dot(ref numV, ref info.Normal, out float jNumerator);
                jNumerator = -jNumerator;

                float j = jNumerator / jDenom;

                if (!float.IsPositiveInfinity(A.mass))
                {
                    A.position.X += info.Normal.X * moveA;
                    A.position.Y += info.Normal.Y * moveA;
                }

                if (!float.IsPositiveInfinity(B1.mass))
                {
                    B1.position.X -= info.Normal.X * B1move;
                    B1.position.Y -= info.Normal.Y * B1move;
                }

                if (!float.IsPositiveInfinity(B2.mass))
                {
                    B2.position.X -= info.Normal.X * B2move;
                    B2.position.Y -= info.Normal.Y * B2move;
                }

                Vector2 tangent = new Vector2();
                VectorHelper.Perpendicular(ref info.Normal, ref tangent);
                Vector2.Dot(ref relVel, ref tangent, out float fNumerator);
                fNumerator *= Friction;
                float f = fNumerator / jDenom;

                if (relDot <= 0.0001f)
                {
                    if (!float.IsPositiveInfinity(A.mass))
                    {
                        A.velocity.X += (info.Normal.X * (j / A.mass)) - (tangent.X * (f / A.mass));
                        A.velocity.Y += (info.Normal.Y * (j / A.mass)) - (tangent.Y * (f / A.mass));
                    }

                    if (!float.IsPositiveInfinity(b2MassSum))
                    {
                        B1.velocity.X -= (info.Normal.X * (j / b2MassSum) * b1inf) - (tangent.X * (f / b2MassSum) * b1inf);
                        B1.velocity.Y -= (info.Normal.Y * (j / b2MassSum) * b1inf) - (tangent.Y * (f / b2MassSum) * b1inf);
                    }

                    if (!float.IsPositiveInfinity(b2MassSum))
                    {
                        B2.velocity.X -= (info.Normal.X * (j / b2MassSum) * b2inf) - (tangent.X * (f / b2MassSum) * b2inf);
                        B2.velocity.Y -= (info.Normal.Y * (j / b2MassSum) * b2inf) - (tangent.Y * (f / b2MassSum) * b2inf);
                    }
                }
            }

            for (int i = 0; i < BodyList.Count; i++)
            {
                BodyList[i].UpdateBodyPositionVelocityForce(elapsed);
            }
        }
    }
}