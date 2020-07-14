using System.Collections.Generic;
using Kinematics.Math;
using Kinematics.Dynamics;

namespace Kinematics.Collision
{
    internal class Collision
    {
        public static List<CollisionInfo> Intersects(Body bodyA, Body bodyB)
        {
            List<CollisionInfo> data = new List<CollisionInfo>();

            int bApmCount = bodyA.Count;
            int bBpmCount = bodyB.Count;

            AABB boxB = bodyB.AABB;
            CollisionInfo infoAway = new CollisionInfo();
            CollisionInfo infoSame = new CollisionInfo();
            for (int i = 0; i < bApmCount; i++)
            {
                Vector2 pt = bodyA.PointMassList[i].Position;
                if (!boxB.Contains(pt.X, pt.Y))
                {
                    continue;
                }

                if (!bodyB.Contains(ref pt))
                {
                    continue;
                }

                int prevPt = i > 0 ? i - 1 : bApmCount - 1;
                int nextPt = i < bApmCount - 1 ? i + 1 : 0;

                Vector2 prev = bodyA.PointMassList[prevPt].Position;
                Vector2 next = bodyA.PointMassList[nextPt].Position;

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

                ptNorm = Vector2.Perpendicular(ptNorm);

                float closestAway = 100000.0f;
                float closestSame = 100000.0f;

                infoAway.Clear();
                infoAway.BodyA = bodyA;
                infoAway.PointMassA = bodyA.PointMassList[i];
                infoAway.BodyB = bodyB;

                infoSame.Clear();
                infoSame.BodyA = bodyA;
                infoSame.PointMassA = bodyA.PointMassList[i];
                infoSame.BodyB = bodyB;

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

                    Vector2 pt1 = bodyB.PointMassList[b1].Position;
                    Vector2 pt2 = bodyB.PointMassList[b2].Position;

                    float distToA = (pt1.X - pt.X) * (pt1.X - pt.X) + (pt1.Y - pt.Y) * (pt1.Y - pt.Y);
                    float distToB = (pt2.X - pt.X) * (pt2.X - pt.X) + (pt2.Y - pt.Y) * (pt2.Y - pt.Y);

                    if (distToA > closestAway && distToA > closestSame && distToB > closestAway && distToB > closestSame)
                    {
                        continue;
                    }

                    float dist = bodyB.GetClosestPointOnEdgeSquared(pt, j, out Vector2 hitPt, out Vector2 normal, out float edgeD);
                    float dot = Vector2.Dot(ptNorm, normal);
                    if (dot <= 0f)
                    {
                        if (dist < closestAway)
                        {
                            closestAway = dist;
                            infoAway.PointMassB = bodyB.PointMassList[b1];
                            infoAway.PointMassC = bodyB.PointMassList[b2];
                            infoAway.EdgeDistance = edgeD;
                            infoAway.Point = hitPt;
                            infoAway.Normal = normal;
                            infoAway.Penetration = dist;
                            found = true;
                        }
                    }
                    else
                    {
                        if (dist < closestSame)
                        {
                            closestSame = dist;
                            infoSame.PointMassB = bodyB.PointMassList[b1];
                            infoSame.PointMassC = bodyB.PointMassList[b2];
                            infoSame.EdgeDistance = edgeD;
                            infoSame.Point = hitPt;
                            infoSame.Normal = normal;
                            infoSame.Penetration = dist;
                        }
                    }
                }

                if (found && closestAway > 0.3f && closestSame < closestAway)
                {
                    infoSame.Penetration = Mathf.Sqrt(infoSame.Penetration);
                    data.Add(infoSame);
                }
                else
                {
                    infoAway.Penetration = Mathf.Sqrt(infoAway.Penetration);
                    data.Add(infoAway);
                }
            }

            return data;
        }
    }
}