using System.Collections.Generic;
using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class Body
    {
        public Shape BaseShape;
        public Shape CurrentShape;
        public List<PointMass> PointMassList;
        public AxisAlignedBoundingBox AABB;
        public Vector2 Scale = Vector2.One;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Force;
        public int Count;
        public float CurrentAngle;
        public float PreviousAngle;
        public float Omega;
        public float Damping = 0.999f;
        public bool IsStatic = false;
        public bool IsDirty = true;
        public bool IsMerging = false;
        public Bitmask BitmaskX;
        public Bitmask BitmaskY;

        public Body(Shape shape, float mass)
        {
            BaseShape = shape;
            CurrentShape = shape.Clone();
            Count = shape.Count;

            PointMassList = new List<PointMass>(shape.Count);
            for (int i = 0; i < shape.Count; i++)
            {
                PointMassList[i] = new PointMass(shape.Points[i], mass);
            }

            BitmaskX = new Bitmask();
            BitmaskY = new Bitmask();
        }

        private void UpdatePointMasses(double elapsed)
        {
            for (int i = 0; i < Count; i++)
            {
                PointMassList[i].velocity.X *= Damping;
                PointMassList[i].velocity.Y *= Damping;
                PointMassList[i].Update(elapsed);
            }
        }

        private void UpdataAABB(double elapsed)
        {
            AABB.Clear();

            for (int i = 0; i < Count; i++)
            {
                float x = PointMassList[i].position.X;
                float y = PointMassList[i].position.Y;

                AABB.Add(x, y);

                x += (float)(PointMassList[i].velocity.X * elapsed);
                y += (float)(PointMassList[i].velocity.Y * elapsed);

                AABB.Add(x, y);
            }
        }

        private void SetBodyPositionVelocityForce(Vector2 position, Vector2 velocity, Vector2 force)
        {
            GetBodyPositionVelocityForce(out Vector2 currentPosition, out Vector2 currentVelocity, out Vector2 currentForce);

            for (int i = 0; i < Count; i++)
            {
                PointMassList[i].position -= currentPosition;
                PointMassList[i].position += position;

                PointMassList[i].velocity -= currentVelocity;
                PointMassList[i].velocity += velocity;

                PointMassList[i].force -= currentForce;
                PointMassList[i].force += force;
            }
        }

        private void GetBodyPositionVelocityForce(out Vector2 position, out Vector2 velocity, out Vector2 force)
        {
            position = default;
            velocity = default;
            force = default;
            float inverse_count = 1.0f / Count;

            position.X = 0;
            position.Y = 0;

            velocity.X = 0;
            velocity.Y = 0;

            force.X = 0;
            force.Y = 0;

            for (int i = 0; i < Count; i++)
            {
                position.X += PointMassList[i].position.X * inverse_count;
                position.Y += PointMassList[i].position.Y * inverse_count;

                velocity.X += PointMassList[i].velocity.X * inverse_count;
                velocity.Y += PointMassList[i].velocity.Y * inverse_count;

                force.X += PointMassList[i].force.X * inverse_count;
                force.Y += PointMassList[i].force.Y * inverse_count;
            }
        }

        public void UpdateBodyPositionVelocityForce(double elapsed)
        {
            GetBodyPositionVelocityForce(out Position, out Velocity, out Force);
        }

        public void RotateShape(double elapsed)
        {
            float angle = 0;
            int originalSign = 1;
            float originalAngle = 0;
            for (int i = 0; i < Count; i++)
            {
                Vector2 baseNorm = new Vector2
                {
                    X = BaseShape.Points[i].X,
                    Y = BaseShape.Points[i].Y
                };
                Vector2.Normalize(ref baseNorm, out baseNorm);

                Vector2 curNorm = new Vector2
                {
                    X = PointMassList[i].position.X - Position.X,
                    Y = PointMassList[i].position.Y - Position.Y
                };
                Vector2.Normalize(ref curNorm, out curNorm);

                Vector2.Dot(ref baseNorm, ref curNorm, out float dot);
                if (dot > 1.0f)
                {
                    dot = 1.0f;
                }

                if (dot < -1.0f)
                {
                    dot = -1.0f;
                }

                float thisAngle = Mathf.Acos(dot);
                if (!VectorHelper.IsCCW(ref baseNorm, ref curNorm)) { thisAngle = -thisAngle; }

                if (i == 0)
                {
                    originalSign = (thisAngle >= 0.0f) ? 1 : -1;
                    originalAngle = thisAngle;
                }
                else
                {
                    float diff = (thisAngle - originalAngle);
                    int thisSign = (thisAngle >= 0.0f) ? 1 : -1;

                    if ((Mathf.Abs(diff) > Mathf.PI) && (thisSign != originalSign))
                    {
                        thisAngle = (thisSign == -1) ? (Mathf.PI + (Mathf.PI + thisAngle)) : ((Mathf.PI - thisAngle) - Mathf.PI);
                    }
                }

                angle += thisAngle;
            }

            angle /= Count;

            float angleChange = (angle - PreviousAngle);
            if (Mathf.Abs(angleChange) >= Mathf.PI)
            {
                if (angleChange < 0f)
                {
                    angleChange += Mathf.PI * 2;
                }
                else
                {
                    angleChange -= Mathf.PI * 2;
                }
            }

            Omega = angleChange / (float)elapsed;
            PreviousAngle = angle;

            for (int i = 0; i < Count; i++)
            {
                float x = BaseShape.Points[i].X * Scale.X;
                float y = BaseShape.Points[i].Y * Scale.Y;
                float c = Mathf.Cos(angle);
                float s = Mathf.Sin(angle);
                CurrentShape.Points[i].X = (c * x) - (s * y) + Position.X;
                CurrentShape.Points[i].Y = (c * y) + (s * x) + Position.Y;
            }
        }

        public virtual void ApplyInternalForces(double elapsed) { }

        public void Update(double elapsed)
        {
            if (!IsDirty)
            {
                return;
            }

            if (IsMerging)
            {
                return;
            }

            SetBodyPositionVelocityForce(Position, Velocity, Force);

            RotateShape(elapsed);
            
            ApplyInternalForces(elapsed);

            UpdatePointMasses(elapsed);
            UpdataAABB(elapsed);

            UpdateBodyPositionVelocityForce(elapsed);

            if (IsStatic)
            {
                IsDirty = false;
            }
        }

        public override string ToString()
        {
            return $"{{position:[{Position}] velocity:[{Velocity}] force[{Force}]}}";
        }

        public string ToStringSimple()
        {
            return $"{{position:[{{{Position.X:0.0}, {Position.Y:0.0}}}] velocity:[{{{Velocity.X:0.0}, {Velocity.Y:0.0}}}]}}";
        }

        public bool Contains(ref Vector2 point)
        {
            Vector2 endPt = new Vector2
            {
                X = AABB.max.X + 0.1f,
                Y = point.Y
            };

            bool inside = false;
            Vector2 edgeSt = PointMassList[0].position;
            for (int i = 0; i < Count; i++)
            {
                Vector2 edgeEnd = i < (Count - 1) ? PointMassList[i + 1].position : PointMassList[0].position;

                if (((edgeSt.Y <= point.Y) && (edgeEnd.Y > point.Y)) || ((edgeSt.Y > point.Y) && (edgeEnd.Y <= point.Y)))
                {
                    float slope = (edgeEnd.X - edgeSt.X) / (edgeEnd.Y - edgeSt.Y);
                    float hitX = edgeSt.X + ((point.Y - edgeSt.Y) * slope);

                    if ((hitX >= point.X) && (hitX <= endPt.X))
                    {
                        inside = !inside;
                    }
                }
                edgeSt = edgeEnd;
            }

            return inside;
        }


        public float GetClosestPoint(Vector2 point, out Vector2 closest, out Vector2 normal, out int pointA, out int pointB, out float edgeD)
        {
            closest = Vector2.Zero;
            pointA = -1;
            pointB = -1;
            edgeD = 0f;
            normal = Vector2.Zero;

            float closestD = 1000.0f;

            for (int i = 0; i < Count; i++)
            {
                float dist = GetClosestPointOnEdge(point, i, out Vector2 tempHit, out Vector2 tempNorm, out float tempEdgeD);
                if (dist < closestD)
                {
                    closestD = dist;
                    pointA = i;
                    if (i < (Count - 1))
                    {
                        pointB = i + 1;
                    }
                    else
                    {
                        pointB = 0;
                    }
                    edgeD = tempEdgeD;
                    normal = tempNorm;
                    closest = tempHit;
                }
            }

            return closestD;
        }

        public float GetClosestPointOnEdge(Vector2 point, int edgeNum, out Vector2 hitPt, out Vector2 normal, out float edgeD)
        {
            hitPt = new Vector2
            {
                X = 0f, Y = 0f
            };

            normal = new Vector2
            {
                X = 0f, Y = 0f
            };

            edgeD = 0f;
            float distance;

            Vector2 ptA = PointMassList[edgeNum].position;
            Vector2 ptB;

            if (edgeNum < (Count - 1))
            {
                ptB = PointMassList[edgeNum + 1].position;
            }
            else
            {
                ptB = PointMassList[0].position;
            }

            Vector2 toP = new Vector2
            {
                X = point.X - ptA.X, Y = point.Y - ptA.Y
            };

            Vector2 E = new Vector2
            {
                X = ptB.X - ptA.X, Y = ptB.Y - ptA.Y
            };

            float edgeLength = Mathf.Sqrt((E.X * E.X) + (E.Y * E.Y));
            if (edgeLength > 0.00001f)
            {
                E.X /= edgeLength;
                E.Y /= edgeLength;
            }

            Vector2 n = new Vector2();
            VectorHelper.Perpendicular(ref E, ref n);

            Vector2.Dot(ref toP, ref E, out float x);
            if (x <= 0.0f)
            {
                Vector2.Distance(ref point, ref ptA, out distance);
                hitPt = ptA;
                edgeD = 0f;
                normal = n;
            }
            else if (x >= edgeLength)
            {
                Vector2.Distance(ref point, ref ptB, out distance);
                hitPt = ptB;
                edgeD = 1f;
                normal = n;
            }
            else
            {
                Vector3 toP3 = new Vector3
                {
                    X = toP.X, Y = toP.Y
                };

                Vector3 E3 = new Vector3
                {
                    X = E.X, Y = E.Y
                };

                E3 = Vector3.Cross(toP3, E3);
                distance = Mathf.Abs(E3.Z);
                hitPt.X = ptA.X + (E.X * x);
                hitPt.Y = ptA.Y + (E.Y * x);
                edgeD = x / edgeLength;
                normal = n;
            }

            return distance;
        }

        public float GetClosestPointOnEdgeSquared(Vector2 point, int edgeNum, out Vector2 hitPt, out Vector2 normal, out float edgeD)
        {
            hitPt = new Vector2
            {
                X = 0f,
                Y = 0f
            };

            normal = new Vector2
            {
                X = 0f,
                Y = 0f
            };

            edgeD = 0f;
            float dist;

            Vector2 ptA = PointMassList[edgeNum].position;
            Vector2 ptB = edgeNum < (Count - 1) ? PointMassList[edgeNum + 1].position : PointMassList[0].position;

            Vector2 toP = new Vector2
            {
                X = point.X - ptA.X, Y = point.Y - ptA.Y
            };

            Vector2 E = new Vector2
            {
                X = ptB.X - ptA.X, Y = ptB.Y - ptA.Y
            };

            float edgeLength = Mathf.Sqrt((E.X * E.X) + (E.Y * E.Y));
            if (edgeLength > 0.00001f)
            {
                E.X /= edgeLength;
                E.Y /= edgeLength;
            }

            Vector2 n = new Vector2();
            VectorHelper.Perpendicular(ref E, ref n);

            Vector2.Dot(ref toP, ref E, out float x);
            if (x <= 0.0f)
            {
                Vector2.DistanceSquared(ref point, ref ptA, out dist);
                hitPt = ptA;
                edgeD = 0f;
                normal = n;
            }
            else if (x >= edgeLength)
            {
                Vector2.DistanceSquared(ref point, ref ptB, out dist);
                hitPt = ptB;
                edgeD = 1f;
                normal = n;
            }
            else
            {
                Vector3 toP3 = new Vector3
                {
                    X = toP.X, Y = toP.Y
                };

                Vector3 E3 = new Vector3
                {
                    X = E.X, Y = E.Y
                };

                E3 = Vector3.Cross(toP3, E3);
                dist = Mathf.Abs(E3.Z * E3.Z);
                hitPt.X = ptA.X + (E.X * x);
                hitPt.Y = ptA.Y + (E.Y * x);
                edgeD = x / edgeLength;
                normal = n;
            }

            return dist;
        }

        public PointMass GetClosestPointMass(Vector2 point, out float dist)
        {
            float closestSQD = 100000.0f;
            int closest = -1;

            for (int i = 0; i < Count; i++)
            {
                float thisD = (point - PointMassList[i].position).LengthSquared();
                if (thisD < closestSQD)
                {
                    closestSQD = thisD;
                    closest = i;
                }
            }

            dist = Mathf.Sqrt(closestSQD);
            return PointMassList[closest];
        }

        public void ApplyForce(ref Vector2 point, ref Vector2 force)
        {
            Vector2 R = (Position - point);

            float torqueF = Vector3.Cross(VectorHelper.Vector3FromVector2(R), VectorHelper.Vector3FromVector2(force)).Z;

            for (int i = 0; i < Count; i++)
            {
                Vector2 toPt = (PointMassList[i].position - Position);
                Vector2 torque = VectorHelper.Rotate(toPt, - Mathf.PI / 2f);

                PointMassList[i].force += torque * torqueF;
                PointMassList[i].force += force;
            }
        }
    }
}