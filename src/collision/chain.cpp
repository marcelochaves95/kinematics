#include <collision/chain.h>
#include <math/vector2.h>
#include <memory>

namespace kinematics
{
    Chain::Chain(PointMassPtr from, PointMassPtr to, int count, float k, float damping, float mass)
    {
        this->damping = 0.99f;
        float length = Vector2::distance(from->position, to->position) / static_cast<float>(count);
        Vector2 direction = to->position - from->position;
        direction.normalize();

        point_mass_list.reserve(static_cast<size_t>(count) + 1);
        for (int i = 0; i < count + 1; i++)
        {
            point_mass_list.push_back(std::make_shared<PointMass>(
                Vector2(from->position.x + direction.x * length * i,
                    from->position.y + direction.y * length * i),
                mass));
        }

        point_mass_list[0] = from;
        point_mass_list[count] = to;

        spring_list.reserve(static_cast<size_t>(count));
        for (int i = 1; i < count + 1; i++)
        {
            spring_list.emplace_back(point_mass_list[i - 1], point_mass_list[i], k, damping);
        }
    }

    void Chain::update(double elapsed)
    {
        for (size_t i = 0; i < spring_list.size(); i++)
        {
            Spring& spring = spring_list[i];
            Vector2 force = Spring::spring_force(spring);
            spring.point_mass_a->force.x += force.x;
            spring.point_mass_a->force.y += force.y;
            spring.point_mass_b->force.x -= force.x;
            spring.point_mass_b->force.y -= force.y;
        }

        for (size_t i = 1; i + 1 < point_mass_list.size(); i++)
        {
            point_mass_list[i]->velocity.x *= damping;
            point_mass_list[i]->velocity.y *= damping;
            point_mass_list[i]->update(elapsed);
        }
    }
}
