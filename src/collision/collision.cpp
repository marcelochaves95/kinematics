#include <collision/collision.h>
#include <collision/aabb.h>
#include <Dynamics/Body.h>
#include <Math/Mathf.h>
#include <Math/Vector2.h>

namespace kinematics
{
    void Collision::intersects(Body& bodyA, Body& bodyB, std::vector<CollisionInfo>& out)
    {
        int bApmCount = bodyA.Count;
        int bBpmCount = bodyB.Count;
        AABB boxB = bodyB.Aabb;
        CollisionInfo infoAway;
        CollisionInfo infoSame;
        for (int i = 0; i < bApmCount; i++)
        {
            Vector2 pt = bodyA.PointMassList[i]->position;
            if (!boxB.contains(pt.X, pt.Y))
            {
                continue;
            }
            if (!bodyB.Contains(pt))
            {
                continue;
            }

            int prevPt = i > 0 ? i - 1 : bApmCount - 1;
            int nextPt = i < bApmCount - 1 ? i + 1 : 0;
            Vector2 prev = bodyA.PointMassList[prevPt]->position;
            Vector2 next = bodyA.PointMassList[nextPt]->position;
            Vector2 fromPrev(pt.X - prev.X, pt.Y - prev.Y);
            Vector2 toNext(next.X - pt.X, next.Y - pt.Y);
            Vector2 ptNorm(fromPrev.X + toNext.X, fromPrev.Y + toNext.Y);
            ptNorm = ptNorm.Perpendicular();

            float closestAway = 100000.0f;
            float closestSame = 100000.0f;
            infoAway.clear();
            infoAway.body_a = &bodyA;
            infoAway.point_mass_a = bodyA.PointMassList[i];
            infoAway.body_b = &bodyB;
            infoSame.clear();
            infoSame.body_a = &bodyA;
            infoSame.point_mass_a = bodyA.PointMassList[i];
            infoSame.body_b = &bodyB;
            bool found = false;
            for (int j = 0; j < bBpmCount; j++)
            {
                int b1 = j;
                int b2 = j < bBpmCount - 1 ? j + 1 : 0;

                Vector2 pt1 = bodyB.PointMassList[b1]->position;
                Vector2 pt2 = bodyB.PointMassList[b2]->position;
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
                        infoAway.point_mass_b = bodyB.PointMassList[b1];
                        infoAway.point_mass_c = bodyB.PointMassList[b2];
                        infoAway.edge_distance = edgeD;
                        infoAway.point = hitPt;
                        infoAway.normal = normal;
                        infoAway.penetration = dist;
                        found = true;
                    }
                }
                else
                {
                    if (dist < closestSame)
                    {
                        closestSame = dist;
                        infoSame.point_mass_b = bodyB.PointMassList[b1];
                        infoSame.point_mass_c = bodyB.PointMassList[b2];
                        infoSame.edge_distance = edgeD;
                        infoSame.point = hitPt;
                        infoSame.normal = normal;
                        infoSame.penetration = dist;
                    }
                }
            }

            if (found && closestAway > 0.3f && closestSame < closestAway)
            {
                infoSame.penetration = mathf::Sqrt(infoSame.penetration);
                out.push_back(infoSame);
            }
            else if (found)
            {
                infoAway.penetration = mathf::Sqrt(infoAway.penetration);
                out.push_back(infoAway);
            }
        }
    }
}
