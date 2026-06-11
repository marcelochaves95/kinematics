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
            force = Spring::SpringForce(spring);
            spring.PointMassA->Force.X += force.X;
            spring.PointMassA->Force.Y += force.Y;
            spring.PointMassB->Force.X -= force.X;
            spring.PointMassB->Force.Y -= force.Y;
        }

        if (_isConstrained)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_shapeK > 0.0f)
                {
                    // shape-matching spring: pull each PM toward its target shape
                    // point with rest length 0 (velA == velB, so no damping term).
                    force = Spring::SpringForce(
                        PointMassList[i]->Position, PointMassList[i]->Velocity,
                        CurrentShape.Points[i], PointMassList[i]->Velocity,
                        0.0f, _shapeK, _shapeDamping);
                    PointMassList[i]->Force.X += force.X;
                    PointMassList[i]->Force.Y += force.Y;
                }
            }
        }
    }
}
