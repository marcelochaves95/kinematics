#include <collision/collision.h>
#include <collision/aabb.h>
#include <Dynamics/Body.h>
#include <math/mathf.h>
#include <math/vector2.h>

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
            if (!boxB.contains(pt.x, pt.y))
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
            Vector2 fromPrev(pt.x - prev.x, pt.y - prev.y);
            Vector2 toNext(next.x - pt.x, next.y - pt.y);
            Vector2 ptNorm(fromPrev.x + toNext.x, fromPrev.y + toNext.y);
            ptNorm = ptNorm.perpendicular();

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
                float distToA = (pt1.x - pt.x) * (pt1.x - pt.x) + (pt1.y - pt.y) * (pt1.y - pt.y);
                float distToB = (pt2.x - pt.x) * (pt2.x - pt.x) + (pt2.y - pt.y) * (pt2.y - pt.y);
                if (distToA > closestAway && distToA > closestSame &&
                    distToB > closestAway && distToB > closestSame)
                {
                    continue;
                }

                Vector2 hitPt, normal;
                float edgeD;
                float dist = bodyB.GetClosestPointOnEdgeSquared(pt, j, hitPt, normal, edgeD);
                float dot = Vector2::dot(ptNorm, normal);
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
                infoSame.penetration = mathf::sqrt(infoSame.penetration);
                out.push_back(infoSame);
            }
            else if (found)
            {
                infoAway.penetration = mathf::sqrt(infoAway.penetration);
                out.push_back(infoAway);
            }
        }
    }
}
