#include <collision/spring.h>
#include <math/mathf.h>
#include <utility>

namespace kinematics
{
    Spring::Spring(PointMassPtr a, PointMassPtr b, float k, float damping)
        : Spring(std::move(a), std::move(b), k, damping, 0.0f)
    {
        reset();
    }

    Spring::Spring(PointMassPtr a, PointMassPtr b, float k, float damping, float length)
        : damping(damping), k(k), d(length), point_mass_a(std::move(a)), point_mass_b(std::move(b))
    {
    }

    void Spring::reset()
    {
        d = (point_mass_a->position - point_mass_b->position).length();
    }

    Vector2 Spring::spring_force(const Spring& spring)
    {
        return spring_force(spring.point_mass_a->position, spring.point_mass_a->velocity,
            spring.point_mass_b->position, spring.point_mass_b->velocity,
            spring.d, spring.k, spring.damping);
    }

    Vector2 Spring::spring_force(const Vector2& posA, const Vector2& velA,
        const Vector2& posB, const Vector2& velB,
        float springD, float springK, float damping)
    {
        float BtoAX = posA.x - posB.x;
        float BtoAY = posA.y - posB.y;
        float distance = mathf::sqrt(mathf::pow(BtoAX, 2.0f) + mathf::pow(BtoAY, 2.0f));
        Vector2 forceOut;
        if (distance > mathf::EPSILON)
        {
            BtoAX /= distance;
            BtoAY /= distance;
        }
        else
        {
            forceOut.x = 0.0f;
            forceOut.y = 0.0f;
            return forceOut;
        }

        distance = springD - distance;
        float relVelX = velA.x - velB.x;
        float relVelY = velA.y - velB.y;
        float totalRelVel = relVelX * BtoAX + relVelY * BtoAY;
        forceOut.x = BtoAX * (distance * springK - totalRelVel * damping);
        forceOut.y = BtoAY * (distance * springK - totalRelVel * damping);
        return forceOut;
    }
}
