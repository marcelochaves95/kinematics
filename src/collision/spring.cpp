#include <collision/spring.h>
#include <Math/Mathf.h>
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
        d = (point_mass_a->position - point_mass_b->position).Length();
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
        float BtoAX = posA.X - posB.X;
        float BtoAY = posA.Y - posB.Y;
        float distance = mathf::Sqrt(mathf::Pow(BtoAX, 2.0f) + mathf::Pow(BtoAY, 2.0f));
        Vector2 forceOut;
        if (distance > mathf::Epsilon)
        {
            BtoAX /= distance;
            BtoAY /= distance;
        }
        else
        {
            forceOut.X = 0.0f;
            forceOut.Y = 0.0f;
            return forceOut;
        }

        distance = springD - distance;
        float relVelX = velA.X - velB.X;
        float relVelY = velA.Y - velB.Y;
        float totalRelVel = relVelX * BtoAX + relVelY * BtoAY;
        forceOut.X = BtoAX * (distance * springK - totalRelVel * damping);
        forceOut.Y = BtoAY * (distance * springK - totalRelVel * damping);
        return forceOut;
    }
}
