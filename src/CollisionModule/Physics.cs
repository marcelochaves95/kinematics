using System;
using System.Collections.Generic;
using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class Physics
    {
        private static Random rand = new Random();
        public List<Chain> chain_list;
        public List<Body> body_list;
        public List<CollisionInfo> collision_list;
        public int penetration_count;
        public float penetration_threshold = 0.015f;
        public float friction = 1.9f;
        public float elasticity = 1.5f;

        public AxisAlignedBoundingBox aabb;
        public Vector2 size;
        public Vector2 cell;
        private bool initialized;

        public Action<Body, Body> on_aabb_collision;
        public Action<Body, Body, CollisionInfo> on_collision;
        public Action<float, Body, Body> on_penetration;

        public Physics()
        {
            chain_list = new List<Chain>();
            body_list = new List<Body>();
            collision_list = new List<CollisionInfo>();
            initialized = false;
        }

        public void Add(Chain chain)
        {
            if (!chain_list.Contains(chain))
            {
                chain_list.Add(chain);
            }
        }

        public void Remove(Chain chain)
        {
            if (chain_list.Contains(chain))
            {
                chain_list.Remove(chain);
            }
        }

        public void Add(Body body)
        {
            if (!body_list.Contains(body))
            {
                body_list.Add(body);
            }
        }

        public void Remove(Body body)
        {
            if (body_list.Contains(body))
            {
                body_list.Remove(body);
            }
        }

        public void SetWorldLimits(Vector2 min, Vector2 max)
        {
            aabb = new AxisAlignedBoundingBox(ref min, ref max);
            size = max - min;
            cell = size / 32;
        }

        public void UpdateBitmask(Body body)
        {
            AxisAlignedBoundingBox box = body.AABB;

            int minX = (int) Mathf.Floor((box.min.X - aabb.min.X) / cell.X);
            int maxX = (int) Mathf.Floor((box.max.X - aabb.min.X) / cell.X);

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

            int minY = (int) Mathf.Floor((box.min.Y - aabb.min.Y) / cell.Y);
            int maxY = (int) Mathf.Floor((box.max.Y - aabb.min.Y) / cell.Y);

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

            body.BitmaskX.clear();
            for (int i = minX; i <= maxX; i++)
            {
                body.BitmaskX.setOn(i);
            }

            body.BitmaskY.clear();
            for (int i = minY; i <= maxY; i++)
            {
                body.BitmaskY.setOn(i);
            }
        }

        public bool IsPointInsideAnyBody(Vector2 point)
        {
            for (int i = 0; i < body_list.Count; i++)
            {
                Body body = body_list[i];
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

            for (int i = 0; i < body_list.Count; i++)
            {
                if (!body_list[i].IsStatic)
                {
                    continue;
                }

                body_list[i].RotateShape(0);
                body_list[i].Update(0);

                if (body_list[i].AABB.min.X < min.X)
                {
                    min.X = body_list[i].AABB.min.X;
                }

                if (body_list[i].AABB.min.Y < min.Y)
                {
                    min.Y = body_list[i].AABB.min.Y;
                }

                if (body_list[i].AABB.max.X > max.X)
                {
                    max.X = body_list[i].AABB.max.X;
                }

                if (body_list[i].AABB.max.Y > max.Y)
                {
                    max.Y = body_list[i].AABB.max.Y;
                }
            }

            SetWorldLimits(min, max);
        }

        public void MoveDistantBodies(Vector2 position, float near, float far)
        {
            for (int i = 0; i < body_list.Count; i++)
            {
                Body body = body_list[i];

                if (body.IsStatic)
                {
                    continue;
                }

                float distance = (body.Position - position).Length();
                Vector2 point = new Vector2();
                if (distance > far)
                {
                    point.X = ((float)rand.NextDouble() - 0.5f);
                    point.Y = ((float)rand.NextDouble() - 0.5f);
                    point.Normalize();

                    point *= near + (far - near) * ((float)rand.NextDouble());
                    point += position;

                    while (IsPointInsideAnyBody(point))
                    {
                        point.X = ((float)rand.NextDouble() - 0.5f);
                        point.Y = ((float)rand.NextDouble() - 0.5f);
                        point.Normalize();

                        point *= near + (far - near) * ((float)rand.NextDouble());
                        point += position;
                    }

                    body_list[i].Position = point;
                    body_list[i].Update(0);
                }
            }
        }

        public void Update(double elapsed)
        {
            if (!initialized)
            {
                Initialize();
            }

            penetration_count = 0;
            collision_list.Clear();

            for (int i = 0; i < body_list.Count; i++)
            {
                body_list[i].Update(elapsed);
                UpdateBitmask(body_list[i]);
            }

            for (int i = 0; i < chain_list.Count; i++)
            {
                chain_list[i].Update(elapsed);
            }

            for (int i = 0; i < body_list.Count; i++)
            {
                for (int j = i + 1; j < body_list.Count; j++)
                {
                    if (body_list[i].IsStatic && body_list[j].IsStatic)
                    {
                        continue;
                    }

                    if (((body_list[i].BitmaskX.mask & body_list[j].BitmaskX.mask) == 0) && ((body_list[i].BitmaskY.mask & body_list[j].BitmaskY.mask) == 0))
                    {
                        continue;
                    }

                    if (!(body_list[i].AABB).Intersects(ref (body_list[j].AABB)))
                    {
                        continue;
                    }

                    if (on_aabb_collision != null)
                    {
                        this.on_aabb_collision(body_list[i], body_list[j]);
                    }

                    collision_list.AddRange(Collision.Intersects(body_list[j], body_list[i]));
                    collision_list.AddRange(Collision.Intersects(body_list[i], body_list[j]));
                }
            }

            // now handle all collisions found during the update at once.
            // handle all collisions!
            for (int i = 0; i < collision_list.Count; i++)
            {
                CollisionInfo info = collision_list[i];

                PointMass A = info.pointmass_a;
                PointMass B1 = info.pointmass_b;
                PointMass B2 = info.pointmass_c;

                if (on_collision != null)
                {
                    this.on_collision(info.body_a, info.body_b, info);
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

                Vector2.Dot(ref relVel, ref info.normal, out float relDot);

                if (on_penetration != null)
                {
                    this.on_penetration(info.penetration, info.body_a, info.body_b);
                }

                if (info.penetration > 0.3f)
                {
                    penetration_count++;
                    continue;
                }

                float b1inf = 1.0f - info.edge_distance;
                float b2inf = info.edge_distance;

                float b2MassSum = ((float.IsPositiveInfinity(B1.mass)) || (float.IsPositiveInfinity(B2.mass))) ? float.PositiveInfinity : (B1.mass + B2.mass);

                float massSum = A.mass + b2MassSum;

                float Amove;
                float Bmove;
                if (float.IsPositiveInfinity(A.mass))
                {
                    Amove = 0f;
                    Bmove = (info.penetration) + 0.001f;
                }
                else if (float.IsPositiveInfinity(b2MassSum))
                {
                    Amove = (info.penetration) + 0.001f;
                    Bmove = 0f;
                }
                else
                {
                    Amove = (info.penetration * (b2MassSum / massSum));
                    Bmove = (info.penetration * (A.mass / massSum));
                }

                float B1move = Bmove * b1inf;
                float B2move = Bmove * b2inf;

                float AinvMass = (float.IsPositiveInfinity(A.mass)) ? 0f : 1f / A.mass;
                float BinvMass = (float.IsPositiveInfinity(b2MassSum)) ? 0f : 1f / b2MassSum;

                float jDenom = AinvMass + BinvMass;
                Vector2 numV = new Vector2();
                float elas = elasticity;
                numV.X = relVel.X * elas;
                numV.Y = relVel.Y * elas;

                Vector2.Dot(ref numV, ref info.normal, out float jNumerator);
                jNumerator = -jNumerator;

                float j = jNumerator / jDenom;

                if (!float.IsPositiveInfinity(A.mass))
                {
                    A.position.X += info.normal.X * Amove;
                    A.position.Y += info.normal.Y * Amove;
                }

                if (!float.IsPositiveInfinity(B1.mass))
                {
                    B1.position.X -= info.normal.X * B1move;
                    B1.position.Y -= info.normal.Y * B1move;
                }

                if (!float.IsPositiveInfinity(B2.mass))
                {
                    B2.position.X -= info.normal.X * B2move;
                    B2.position.Y -= info.normal.Y * B2move;
                }

                Vector2 tangent = new Vector2();
                VectorHelper.Perpendicular(ref info.normal, ref tangent);
                Vector2.Dot(ref relVel, ref tangent, out float fNumerator);
                fNumerator *= friction;
                float f = fNumerator / jDenom;

                if (relDot <= 0.0001f)
                {
                    if (!float.IsPositiveInfinity(A.mass))
                    {
                        A.velocity.X += (info.normal.X * (j / A.mass)) - (tangent.X * (f / A.mass));
                        A.velocity.Y += (info.normal.Y * (j / A.mass)) - (tangent.Y * (f / A.mass));
                    }

                    if (!float.IsPositiveInfinity(b2MassSum))
                    {
                        B1.velocity.X -= (info.normal.X * (j / b2MassSum) * b1inf) - (tangent.X * (f / b2MassSum) * b1inf);
                        B1.velocity.Y -= (info.normal.Y * (j / b2MassSum) * b1inf) - (tangent.Y * (f / b2MassSum) * b1inf);
                    }

                    if (!float.IsPositiveInfinity(b2MassSum))
                    {
                        B2.velocity.X -= (info.normal.X * (j / b2MassSum) * b2inf) - (tangent.X * (f / b2MassSum) * b2inf);
                        B2.velocity.Y -= (info.normal.Y * (j / b2MassSum) * b2inf) - (tangent.Y * (f / b2MassSum) * b2inf);
                    }
                }
            }

            for (int i = 0; i < body_list.Count; i++)
            {
                body_list[i].UpdateBodyPositionVelocityForce(elapsed);
            }
        }
    }
}