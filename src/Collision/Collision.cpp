#include <Collision/Collision.h>
#include <Collision/AABB.h>
#include <Dynamics/Body.h>
#include <Math/Mathf.h>
#include <Math/Vector2.h>

namespace kinematics
{
    std::vector<CollisionInfo> Collision::Intersects(Body& bodyA, Body& bodyB)
    {
        std::vector<CollisionInfo> data;
        int bApmCount = bodyA.Count;
        int bBpmCount = bodyB.Count;
        AABB boxB = bodyB.AABB;
        CollisionInfo infoAway;
        CollisionInfo infoSame;
        for (int i = 0; i < bApmCount; i++)
        {
            Vector2 pt = bodyA.PointMassList[i]->Position;
            if (!boxB.Contains(pt.X, pt.Y))
            {
                continue;
            }
            if (!bodyB.Contains(pt))
            {
                continue;
            }

            int prevPt = i > 0 ? i - 1 : bApmCount - 1;
            int nextPt = i < bApmCount - 1 ? i + 1 : 0;
            Vector2 prev = bodyA.PointMassList[prevPt]->Position;
            Vector2 next = bodyA.PointMassList[nextPt]->Position;
            Vector2 fromPrev(pt.X - prev.X, pt.Y - prev.Y);
            Vector2 toNext(next.X - pt.X, next.Y - pt.Y);
            Vector2 ptNorm(fromPrev.X + toNext.X, fromPrev.Y + toNext.Y);
            ptNorm = ptNorm.Perpendicular();

            float closestAway = 100000.0f;
            float closestSame = 100000.0f;
            infoAway.Clear();
            infoAway.BodyA = &bodyA;
            infoAway.PointMassA = bodyA.PointMassList[i];
            infoAway.BodyB = &bodyB;
            infoSame.Clear();
            infoSame.BodyA = &bodyA;
            infoSame.PointMassA = bodyA.PointMassList[i];
            infoSame.BodyB = &bodyB;
            bool found = false;
            for (int j = 0; j < bBpmCount; j++)
            {
                int b1 = j;
                int b2 = j < bBpmCount - 1 ? j + 1 : 0;

                Vector2 pt1 = bodyB.PointMassList[b1]->Position;
                Vector2 pt2 = bodyB.PointMassList[b2]->Position;
                float distToA = (pt1.X - pt.X) * (pt1.X - pt.X) + (pt1.Y - pt.Y) * (pt1.Y - pt.Y);
                float distToB = (pt2.X - pt.X) * (pt2.X - pt.X) + (pt2.Y - pt.Y) * (pt2.Y - pt.Y);
                if (distToA > closestAway && distToA > closestSame &&
                    distToB > closestAway && distToB > closestSame)
                {
                    continue;
                }

                Vector2 hitPt, normal;
                float edgeD;
                float dist = bodyB.GetClosestPointOnEdgeSquared(pt, j, hitPt, normal, edgeD);
                float dot = Vector2::Dot(ptNorm, normal);
                if (dot <= 0.0f)
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
                infoSame.Penetration = mathf::Sqrt(infoSame.Penetration);
                data.push_back(infoSame);
            }
            else
            {
                infoAway.Penetration = mathf::Sqrt(infoAway.Penetration);
                data.push_back(infoAway);
            }
        }

        return data;
    }
}
