#pragma once

#include <Collision/Shape.h>
#include <Collision/Spring.h>
#include <Dynamics/Body.h>
#include <vector>

// Port of Dynamics/SpringBody.cs — a soft body with edge springs (between
// consecutive vertices) plus shape-matching springs (each vertex pulled toward
// its rotated rest position).
//
// NOTE: the C# kept a `_PointsMass` list whose populate-check was inverted, so it
// stayed empty and its per-frame update loop never ran (the point masses are
// integrated by Body::UpdatePointMasses). That dead machinery is dropped here —
// Add() simply registers the spring.

namespace kinematics
{
    class SpringBody : public Body
    {
    public:
        SpringBody(const Shape& shape, float mass, float edgeSpringK, float edgeSpringDamp,
            float shapeSpringK, float shapeSpringDamp);

        void Add(const Spring& spring);
        void ApplyInternalForces(double elapsed) override;

    private:
        float _shapeK;
        float _shapeDamping;
        bool _isConstrained;
        std::vector<Spring> _springs;
    };
}
