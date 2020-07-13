using Kinematics.Math;

namespace Kinematics.Collision
{
    public class Body
    {
        public Shape base_shape;
        public Shape curr_shape;
        public PointMass[] pointmass_list;
        public AxisAlignedBoundingBox aabb;
        public Vector2 scale = Vector2.One;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 force;
        public int count;
        public float curr_angle;
        public float prev_angle;
        public float omega;
        public float damping = 0.999f;
        public bool is_static = false;
        public bool is_dirty = true;
        public bool is_merging = false;
        public Bitmask bitmaskx;
        public Bitmask bitmasky;

        public Body(Shape shape, float mass)
        {
            base_shape = shape;
            curr_shape = shape.Clone();
            count = shape.count;

            pointmass_list = new PointMass[shape.count];
            for (int i = 0; i < shape.count; i++)
            {
                pointmass_list[i] = new PointMass(shape.points[i], mass);
            }

            bitmaskx = new Bitmask();
            bitmasky = new Bitmask();
        }

        private void UpdatePointMasses(double elapsed)
        {
            for (int i = 0; i < count; i++)
            {
                pointmass_list[i].velocity.X *= damping;
                pointmass_list[i].velocity.Y *= damping;
                pointmass_list[i].Update(elapsed);
            }
        }

        private void UpdataAABB(double elapsed)
        {
            aabb.Clear();

            for (int i = 0; i < count; i++)
            {
                float x = pointmass_list[i].position.X;
                float y = pointmass_list[i].position.Y;

                aabb.Add(x, y);

                x += (float)(pointmass_list[i].velocity.X * elapsed);
                y += (float)(pointmass_list[i].velocity.Y * elapsed);

                aabb.Add(x, y);
            }
        }

        private void SetBodyPositionVelocityForce(Vector2 position, Vector2 velocity, Vector2 force)
        {
            GetBodyPositionVelocityForce(out Vector2 currentPosition, out Vector2 currentVelocity, out Vector2 currentForce);

            for (int i = 0; i < count; i++)
            {
                pointmass_list[i].position -= currentPosition;
                pointmass_list[i].position += position;

                pointmass_list[i].velocity -= currentVelocity;
                pointmass_list[i].velocity += velocity;

                pointmass_list[i].force -= currentForce;
                pointmass_list[i].force += force;
            }
        }

        private void GetBodyPositionVelocityForce(out Vector2 position, out Vector2 velocity, out Vector2 force)
        {
            position = default;
            velocity = default;
            force = default;
            float inverse_count = 1.0f / count;

            position.X = 0;
            position.Y = 0;

            velocity.X = 0;
            velocity.Y = 0;

            force.X = 0;
            force.Y = 0;

            for (int i = 0; i < count; i++)
            {
                position.X += pointmass_list[i].position.X * inverse_count;
                position.Y += pointmass_list[i].position.Y * inverse_count;

                velocity.X += pointmass_list[i].velocity.X * inverse_count;
                velocity.Y += pointmass_list[i].velocity.Y * inverse_count;

                force.X += pointmass_list[i].force.X * inverse_count;
                force.Y += pointmass_list[i].force.Y * inverse_count;
            }
        }

        public void UpdateBodyPositionVelocityForce(double elapsed)
        {
            GetBodyPositionVelocityForce(out position, out velocity, out force);
        }

        public void RotateShape(double elapsed)
        {
            float angle = 0;
            int originalSign = 1;
            float originalAngle = 0;
            for (int i = 0; i < count; i++)
            {
                Vector2 baseNorm = new Vector2
                {
                    X = base_shape.points[i].X,
                    Y = base_shape.points[i].Y
                };
                Vector2.Normalize(ref baseNorm, out baseNorm);

                Vector2 curNorm = new Vector2
                {
                    X = pointmass_list[i].position.X - position.X,
                    Y = pointmass_list[i].position.Y - position.Y
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

            angle /= count;

            float angleChange = (angle - prev_angle);
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

            omega = angleChange / (float)elapsed;
            prev_angle = angle;

            for (int i = 0; i < count; i++)
            {
                float x = base_shape.points[i].X * scale.X;
                float y = base_shape.points[i].Y * scale.Y;
                float c = Mathf.Cos(angle);
                float s = Mathf.Sin(angle);
                curr_shape.points[i].X = (c * x) - (s * y) + position.X;
                curr_shape.points[i].Y = (c * y) + (s * x) + position.Y;
            }
        }

        public virtual void ApplyInternalForces(double elapsed) { }

        public void Update(double elapsed)
        {
            if (!is_dirty)
            {
                return;
            }

            if (is_merging)
            {
                return;
            }

            SetBodyPositionVelocityForce(position, velocity, force);

            RotateShape(elapsed);
            
            ApplyInternalForces(elapsed);

            UpdatePointMasses(elapsed);
            UpdataAABB(elapsed);

            UpdateBodyPositionVelocityForce(elapsed);

            if (is_static)
            {
                is_dirty = false;
            }
        }

        public override string ToString()
        {
            return $"{{position:[{position}] velocity:[{velocity}] force[{force}]}}";
        }

        public string ToStringSimple()
        {
            return $"{{position:[{{{position.X:0.0}, {position.Y:0.0}}}] velocity:[{{{velocity.X:0.0}, {velocity.Y:0.0}}}]}}";
        }

        public bool Contains(ref Vector2 point)
        {
            Vector2 endPt = new Vector2();
            endPt.X = aabb.max.X + 0.1f;
            endPt.Y = point.Y;

            bool inside = false;
            Vector2 edgeSt = pointmass_list[0].position;
            Vector2 edgeEnd = new Vector2();
            for (int i = 0; i < count; i++)
            {
                if (i < (count - 1))
                {
                    edgeEnd = pointmass_list[i + 1].position;
                }
                else
                {
                    edgeEnd = pointmass_list[0].position;
                }

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

            for (int i = 0; i < count; i++)
            {
                float dist = GetClosestPointOnEdge(point, i, out Vector2 tempHit, out Vector2 tempNorm, out float tempEdgeD);
                if (dist < closestD)
                {
                    closestD = dist;
                    pointA = i;
                    if (i < (count - 1))
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

            Vector2 ptA = pointmass_list[edgeNum].position;
            Vector2 ptB = new Vector2();

            if (edgeNum < (count - 1))
            {
                ptB = pointmass_list[edgeNum + 1].position;
            }
            else
            {
                ptB = pointmass_list[0].position;
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
            hitPt = new Vector2();
            hitPt.X = 0f;
            hitPt.Y = 0f;

            normal = new Vector2();
            normal.X = 0f;
            normal.Y = 0f;

            edgeD = 0f;
            float dist;

            Vector2 ptA = pointmass_list[edgeNum].position;
            Vector2 ptB;

            if (edgeNum < (count - 1))
            {
                ptB = pointmass_list[edgeNum + 1].position;
            }
            else
            {
                ptB = pointmass_list[0].position;
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

            // normal
            Vector2 n = new Vector2();
            VectorHelper.Perpendicular(ref E, ref n);

            // calculate the distance!
            float x;
            Vector2.Dot(ref toP, ref E, out x);
            if (x <= 0.0f)
            {
                // x is outside the line segment, distance is from pt to ptA.
                //dist = (pt - ptA).Length();
                Vector2.DistanceSquared(ref point, ref ptA, out dist);
                hitPt = ptA;
                edgeD = 0f;
                normal = n;
            }
            else if (x >= edgeLength)
            {
                // x is outside of the line segment, distance is from pt to ptB.
                //dist = (pt - ptB).Length();
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

            for (int i = 0; i < count; i++)
            {
                float thisD = (point - pointmass_list[i].position).LengthSquared();
                if (thisD < closestSQD)
                {
                    closestSQD = thisD;
                    closest = i;
                }
            }

            dist = Mathf.Sqrt(closestSQD);
            return pointmass_list[closest];
        }

        public void ApplyForce(ref Vector2 point, ref Vector2 force)
        {
            Vector2 R = (position - point);

            float torqueF = Vector3.Cross(VectorHelper.Vector3FromVector2(R), VectorHelper.Vector3FromVector2(force)).Z;

            for (int i = 0; i < count; i++)
            {
                Vector2 toPt = (pointmass_list[i].position - position);
                Vector2 torque = VectorHelper.Rotate(toPt, - Mathf.PI / 2f);

                pointmass_list[i].force += torque * torqueF;
                pointmass_list[i].force += force;
            }
        }
    }
}