using System.Collections.Generic;
using Kinematics.Collision;
using Kinematics.Math;
using Kinematics.Utils;

namespace Kinematics.Dynamics
{
    public class Body
    {
        private readonly Shape _baseShape;
        public Shape CurrentShape;
        public List<PointMass> PointMassList;
        internal AABB AABB;
        private Vector2 _scale = Vector2.One;
        public Vector2 Position;
        public Vector2 Velocity;
        private Vector2 _force;
        public int Count;
        private float _previousAngle;
        protected float Damping = 0.999f;
        public bool IsStatic = false;
        private bool _isDirty = true;
        private readonly bool _isMerging = false;
        internal readonly Bitmask BitmaskX;
        internal readonly Bitmask BitmaskY;

        public Body(Shape shape, float mass)
        {
            _baseShape = shape;
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
                PointMassList[i].Velocity.X *= Damping;
                PointMassList[i].Velocity.Y *= Damping;
                PointMassList[i].Update(elapsed);
            }
        }

        private void UpdataAABB(double elapsed)
        {
            AABB.Clear();

            for (int i = 0; i < Count; i++)
            {
                float x = PointMassList[i].Position.X;
                float y = PointMassList[i].Position.Y;

                AABB.Add(x, y);

                x += (float)(PointMassList[i].Velocity.X * elapsed);
                y += (float)(PointMassList[i].Velocity.Y * elapsed);

                AABB.Add(x, y);
            }
        }

        private void SetBodyPositionVelocityForce(Vector2 position, Vector2 velocity, Vector2 force)
        {
            GetBodyPositionVelocityForce(out Vector2 currentPosition, out Vector2 currentVelocity, out Vector2 currentForce);

            for (int i = 0; i < Count; i++)
            {
                PointMassList[i].Position -= currentPosition;
                PointMassList[i].Position += position;

                PointMassList[i].Velocity -= currentVelocity;
                PointMassList[i].Velocity += velocity;

                PointMassList[i].Force -= currentForce;
                PointMassList[i].Force += force;
            }
        }

        private void GetBodyPositionVelocityForce(out Vector2 position, out Vector2 velocity, out Vector2 force)
        {
            position = default;
            velocity = default;
            force = default;
            float inverse_count = 1f / Count;

            position.X = 0;
            position.Y = 0;

            velocity.X = 0;
            velocity.Y = 0;

            force.X = 0;
            force.Y = 0;

            for (int i = 0; i < Count; i++)
            {
                position.X += PointMassList[i].Position.X * inverse_count;
                position.Y += PointMassList[i].Position.Y * inverse_count;

                velocity.X += PointMassList[i].Velocity.X * inverse_count;
                velocity.Y += PointMassList[i].Velocity.Y * inverse_count;

                force.X += PointMassList[i].Force.X * inverse_count;
                force.Y += PointMassList[i].Force.Y * inverse_count;
            }
        }

        public void UpdateBodyPositionVelocityForce()
        {
            GetBodyPositionVelocityForce(out Position, out Velocity, out _force);
        }

        public void RotateShape(double elapsed)
        {
            float angle = 0;
            int originalSign = 1;
            float originalAngle = 0;
            for (int i = 0; i < Count; i++)
            {
                Vector2 baseNormal = new Vector2
                {
                    X = _baseShape.Points[i].X,
                    Y = _baseShape.Points[i].Y
                };
                Vector2.Normalize(baseNormal);

                Vector2 currentNormal = new Vector2
                {
                    X = PointMassList[i].Position.X - Position.X,
                    Y = PointMassList[i].Position.Y - Position.Y
                };
                Vector2.Normalize(currentNormal);

                float dot = Vector2.Dot(baseNormal, currentNormal);
                if (dot > 1f)
                {
                    dot = 1f;
                }

                if (dot < -1f)
                {
                    dot = -1f;
                }

                float thisAngle = Mathf.Acos(dot);
                if (!Vector2.IsCCW(baseNormal, currentNormal))
                {
                    thisAngle = -thisAngle;
                }

                if (i == 0)
                {
                    originalSign = thisAngle >= 0f ? 1 : -1;
                    originalAngle = thisAngle;
                }
                else
                {
                    float diff = thisAngle - originalAngle;
                    int thisSign = thisAngle >= 0f ? 1 : -1;

                    if (Mathf.Abs(diff) > Mathf.PI && thisSign != originalSign)
                    {
                        thisAngle = thisSign == -1 ? Mathf.PI + (Mathf.PI + thisAngle) : Mathf.PI - thisAngle - Mathf.PI;
                    }
                }

                angle += thisAngle;
            }

            angle /= Count;

            float angleChange = (angle - _previousAngle);
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

            _previousAngle = angle;

            for (int i = 0; i < Count; i++)
            {
                float x = _baseShape.Points[i].X * _scale.X;
                float y = _baseShape.Points[i].Y * _scale.Y;
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);
                CurrentShape.Points[i].X = (cos * x) - (sin * y) + Position.X;
                CurrentShape.Points[i].Y = (cos * y) + (sin * x) + Position.Y;
            }
        }

        public virtual void ApplyInternalForces(double elapsed) { }

        public void Update(double elapsed)
        {
            if (!_isDirty)
            {
                return;
            }

            if (_isMerging)
            {
                return;
            }

            SetBodyPositionVelocityForce(Position, Velocity, _force);

            RotateShape(elapsed);
            
            ApplyInternalForces(elapsed);

            UpdatePointMasses(elapsed);
            UpdataAABB(elapsed);

            UpdateBodyPositionVelocityForce();

            if (IsStatic)
            {
                _isDirty = false;
            }
        }

        public override string ToString()
        {
            return $"{{position:[{Position}] velocity:[{Velocity}] force[{_force}]}}";
        }

        public bool Contains(ref Vector2 point)
        {
            Vector2 endPt = new Vector2
            {
                X = AABB.Max.X + 0.1f,
                Y = point.Y
            };

            bool inside = false;
            Vector2 edgeSt = PointMassList[0].Position;
            for (int i = 0; i < Count; i++)
            {
                Vector2 edgeEnd = i < Count - 1 ? PointMassList[i + 1].Position : PointMassList[0].Position;
                if (edgeSt.Y <= point.Y && edgeEnd.Y > point.Y || edgeSt.Y > point.Y && edgeEnd.Y <= point.Y)
                {
                    float slope = (edgeEnd.X - edgeSt.X) / (edgeEnd.Y - edgeSt.Y);
                    float hitX = edgeSt.X + ((point.Y - edgeSt.Y) * slope);

                    if (hitX >= point.X && hitX <= endPt.X)
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

            float closestD = 1000f;

            for (int i = 0; i < Count; i++)
            {
                float dist = GetClosestPointOnEdge(point, i, out Vector2 tempHit, out Vector2 tempNorm, out float tempEdgeD);
                if (dist < closestD)
                {
                    closestD = dist;
                    pointA = i;
                    if (i < Count - 1)
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

            Vector2 ptA = PointMassList[edgeNum].Position;
            Vector2 ptB = edgeNum < Count - 1 ? PointMassList[edgeNum + 1].Position : PointMassList[0].Position;

            Vector2 toP = new Vector2
            {
                X = point.X - ptA.X, Y = point.Y - ptA.Y
            };

            Vector2 e = new Vector2
            {
                X = ptB.X - ptA.X, Y = ptB.Y - ptA.Y
            };

            float edgeLength = Mathf.Sqrt(e.X * e.X + e.Y * e.Y);
            if (edgeLength > Mathf.Epsilon)
            {
                e.X /= edgeLength;
                e.Y /= edgeLength;
            }

            Vector2 n = Vector2.Perpendicular(e);
            float x = Vector2.Dot(toP, e);
            if (x <= 0f)
            {
                distance = Vector2.Distance(point, ptA);
                hitPt = ptA;
                edgeD = 0f;
                normal = n;
            }
            else if (x >= edgeLength)
            {
                distance = Vector2.Distance(point, ptB);
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

                Vector3 e3 = new Vector3
                {
                    X = e.X, Y = e.Y
                };

                e3 = Vector3.Cross(toP3, e3);
                distance = Mathf.Abs(e3.Z);
                hitPt.X = ptA.X + e.X * x;
                hitPt.Y = ptA.Y + e.Y * x;
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

            Vector2 ptA = PointMassList[edgeNum].Position;
            Vector2 ptB = edgeNum < Count - 1 ? PointMassList[edgeNum + 1].Position : PointMassList[0].Position;

            Vector2 toP = new Vector2
            {
                X = point.X - ptA.X,
                Y = point.Y - ptA.Y
            };

            Vector2 e = new Vector2
            {
                X = ptB.X - ptA.X,
                Y = ptB.Y - ptA.Y
            };

            float edgeLength = Mathf.Sqrt(e.X * e.X + e.Y * e.Y);
            if (edgeLength > Mathf.Epsilon)
            {
                e.X /= edgeLength;
                e.Y /= edgeLength;
            }

            Vector2 n = Vector2.Perpendicular(e);
            float x = Vector2.Dot(toP, e);
            if (x <= 0f)
            {
                dist = Vector2.DistanceSquared(point, ptA);
                hitPt = ptA;
                edgeD = 0f;
                normal = n;
            }
            else if (x >= edgeLength)
            {
                dist = Vector2.DistanceSquared(point, ptB);
                hitPt = ptB;
                edgeD = 1f;
                normal = n;
            }
            else
            {
                Vector3 toP3 = new Vector3
                {
                    X = toP.X,
                    Y = toP.Y
                };

                Vector3 e3 = new Vector3
                {
                    X = e.X,
                    Y = e.Y
                };

                e3 = Vector3.Cross(toP3, e3);
                dist = Mathf.Abs(e3.Z * e3.Z);
                hitPt.X = ptA.X + e.X * x;
                hitPt.Y = ptA.Y + e.Y * x;
                edgeD = x / edgeLength;
                normal = n;
            }

            return dist;
        }

        public PointMass GetClosestPointMass(Vector2 point, out float dist)
        {
            float closestSQD = 100000f;
            int closest = -1;

            for (int i = 0; i < Count; i++)
            {
                float thisD = (point - PointMassList[i].Position).LengthSquared();
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
            Vector2 r = Position - point;

            float torqueF = Vector3.Cross(new Vector3(r), new Vector3(force)).Z;

            for (int i = 0; i < Count; i++)
            {
                Vector2 toPt = (PointMassList[i].Position - Position);
                Vector2 torque = Vector2.Rotate(toPt, -Mathf.PI / 2f);

                PointMassList[i].Force += torque * torqueF;
                PointMassList[i].Force += force;
            }
        }
    }
}