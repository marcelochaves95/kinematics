#include <Dynamics/SpringBody.h>
#include <Math/Vector2.h>

namespace kinematics
{
    SpringBody::SpringBody(const Shape& shape, float mass, float edgeSpringK,
        float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp)
        : Body(shape, mass), _shapeK(shapeSpringK), _shapeDamping(shapeSpringDamp),
          _isConstrained(true)
    {
        int index;
        for (index = 0; index < Count - 1; index++)
        {
            Add(Spring(PointMassList[index], PointMassList[index + 1],
                edgeSpringK, edgeSpringDamp));
        }
        Add(Spring(PointMassList[index], PointMassList[0], edgeSpringK, edgeSpringDamp));
    }

    void SpringBody::Add(const Spring& spring)
    {
        _springs.push_back(spring);
    }

    void SpringBody::ApplyInternalForces(double elapsed)
    {
        Body::ApplyInternalForces(elapsed);
        Vector2 force;
        for (size_t i = 0; i < _springs.size(); i++)
        {
            Spring& spring = _springs[i];
            force = Spring::spring_force(spring);
            spring.point_mass_a->force.X += force.X;
            spring.point_mass_a->force.Y += force.Y;
            spring.point_mass_b->force.X -= force.X;
            spring.point_mass_b->force.Y -= force.Y;
        }

        if (_isConstrained)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_shapeK > 0.0f)
                {
                    // shape-matching spring: pull each PM toward its target shape
                    // point with rest length 0 (velA == velB, so no damping term).
                    force = Spring::spring_force(
                        PointMassList[i]->position, PointMassList[i]->velocity,
                        CurrentShape.points[i], PointMassList[i]->velocity,
                        0.0f, _shapeK, _shapeDamping);
                    PointMassList[i]->force.X += force.X;
                    PointMassList[i]->force.Y += force.Y;
                }
            }
        }
    }
}
