#include <dynamics/spring_body.h>
#include <math/vector2.h>

namespace kinematics
{
    SpringBody::SpringBody(const Shape& shape, float mass, float edgeSpringK,
        float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp)
        : Body(shape, mass), _shape_k(shapeSpringK), _shape_damping(shapeSpringDamp),
          _is_constrained(true)
    {
        int index;
        for (index = 0; index < count - 1; index++)
        {
            add(Spring(point_mass_list[index], point_mass_list[index + 1],
                edgeSpringK, edgeSpringDamp));
        }
        add(Spring(point_mass_list[index], point_mass_list[0], edgeSpringK, edgeSpringDamp));
    }

    void SpringBody::add(const Spring& spring)
    {
        _springs.push_back(spring);
    }

    void SpringBody::apply_internal_forces(double elapsed)
    {
        Body::apply_internal_forces(elapsed);
        Vector2 force;
        for (size_t i = 0; i < _springs.size(); i++)
        {
            Spring& spring = _springs[i];
            force = Spring::spring_force(spring);
            spring.point_mass_a->force.x += force.x;
            spring.point_mass_a->force.y += force.y;
            spring.point_mass_b->force.x -= force.x;
            spring.point_mass_b->force.y -= force.y;
        }

        if (_is_constrained)
        {
            for (int i = 0; i < count; i++)
            {
                if (_shape_k > 0.0f)
                {
                    force = Spring::spring_force(
                        point_mass_list[i]->position, point_mass_list[i]->velocity,
                        current_shape.points[i], point_mass_list[i]->velocity,
                        0.0f, _shape_k, _shape_damping);
                    point_mass_list[i]->force.x += force.x;
                    point_mass_list[i]->force.y += force.y;
                }
            }
        }
    }
}
