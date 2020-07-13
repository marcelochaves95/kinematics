using System.Collections.Generic;
using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public struct CollisionInfo
    {
        public Body body_a;
        public Body body_b;
        public PointMass pointmass_a;
        public PointMass pointmass_b;
        public PointMass pointmass_c;
        public float edge_distance;
        public Vector2 normal;
        public Vector2 point;
        public float penetration;

        public void Clear()
        {
            body_a = body_b = null;
            pointmass_a = pointmass_b = pointmass_c = new PointMass();
            edge_distance = 0f;
            point = Vector2.Zero;
            normal = Vector2.Zero;
            penetration = 0f;
        }
    }


    public class Collision
    {
        public static List<CollisionInfo> Intersects(Body bodyA, Body bodyB)
        {
            List<CollisionInfo> data = new List<CollisionInfo>();

            int bApmCount = bodyA.Count;
            int bBpmCount = bodyB.Count;

            AxisAlignedBoundingBox boxB = bodyB.AABB;
            CollisionInfo infoAway = new CollisionInfo();
            CollisionInfo infoSame = new CollisionInfo();
            for (int i = 0; i < bApmCount; i++)
            {
                Vector2 pt = bodyA.PointMassList[i].position;
                if (!boxB.Contains(pt.X, pt.Y))
                {
                    continue;
                }

                if (!bodyB.Contains(ref pt))
                {
                    continue;
                }

                int prevPt = (i > 0) ? i - 1 : bApmCount - 1;
                int nextPt = (i < bApmCount - 1) ? i + 1 : 0;

                Vector2 prev = bodyA.PointMassList[prevPt].position;
                Vector2 next = bodyA.PointMassList[nextPt].position;

                Vector2 fromPrev = new Vector2
                {
                    X = pt.X - prev.X,
                    Y = pt.Y - prev.Y
                };

                Vector2 toNext = new Vector2
                {
                    X = next.X - pt.X,
                    Y = next.Y - pt.Y
                };

                Vector2 ptNorm = new Vector2
                {
                    X = fromPrev.X + toNext.X,
                    Y = fromPrev.Y + toNext.Y
                };

                VectorHelper.Perpendicular(ref ptNorm);

                float closestAway = 100000.0f;
                float closestSame = 100000.0f;

                infoAway.Clear();
                infoAway.body_a = bodyA;
                infoAway.pointmass_a = bodyA.PointMassList[i];
                infoAway.body_b = bodyB;

                infoSame.Clear();
                infoSame.body_a = bodyA;
                infoSame.pointmass_a = bodyA.PointMassList[i];
                infoSame.body_b = bodyB;

                bool found = false;

                for (int j = 0; j < bBpmCount; j++)
                {
                    int b1 = j;
                    int b2 = 1;

                    if (j < bBpmCount - 1)
                    {
                        b2 = j + 1;
                    }
                    else
                    {
                        b2 = 0;
                    }

                    Vector2 pt1 = bodyB.PointMassList[b1].position;
                    Vector2 pt2 = bodyB.PointMassList[b2].position;

                    float distToA = ((pt1.X - pt.X) * (pt1.X - pt.X)) + ((pt1.Y - pt.Y) * (pt1.Y - pt.Y));
                    float distToB = ((pt2.X - pt.X) * (pt2.X - pt.X)) + ((pt2.Y - pt.Y) * (pt2.Y - pt.Y));

                    if ((distToA > closestAway) && (distToA > closestSame) && (distToB > closestAway) && (distToB > closestSame))
                    {
                        continue;
                    }

                    float dist = bodyB.GetClosestPointOnEdgeSquared(pt, j, out Vector2 hitPt, out Vector2 norm, out float edgeD);

                    Vector2.Dot(ref ptNorm, ref norm, out float dot);
                    if (dot <= 0f)
                    {
                        if (dist < closestAway)
                        {
                            closestAway = dist;
                            infoAway.pointmass_b = bodyB.PointMassList[b1];
                            infoAway.pointmass_c = bodyB.PointMassList[b2];
                            infoAway.edge_distance = edgeD;
                            infoAway.point = hitPt;
                            infoAway.normal = norm;
                            infoAway.penetration = dist;
                            found = true;
                        }
                    }
                    else
                    {
                        if (dist < closestSame)
                        {
                            closestSame = dist;
                            infoSame.pointmass_b = bodyB.PointMassList[b1];
                            infoSame.pointmass_c = bodyB.PointMassList[b2];
                            infoSame.edge_distance = edgeD;
                            infoSame.point = hitPt;
                            infoSame.normal = norm;
                            infoSame.penetration = dist;
                        }
                    }
                }

                if ((found) && (closestAway > 0.3f) && (closestSame < closestAway))
                {
                    infoSame.penetration = Mathf.Sqrt(infoSame.penetration);
                    data.Add(infoSame);
                }
                else
                {
                    infoAway.penetration = Mathf.Sqrt(infoAway.penetration);
                    data.Add(infoAway);
                }
            }

            return data;
        }
    }
}