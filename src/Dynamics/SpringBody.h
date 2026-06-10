#pragma once

#include <Collision/PointMass.h>
#include <Collision/Shape.h>
#include <Collision/Spring.h>
#include <Dynamics/Body.h>
#include <vector>

// Port of Dynamics/SpringBody.cs
//
// FAITHFUL PORT OF A LATENT C# BUG: Add() checks PointMassList.Contains(...) but
// pushes into _PointsMass. Because the springs are built from PointMassList
// instances, the check is always true, so _PointsMass stays empty and the final
// loop in ApplyInternalForces never runs (the masses are integrated by
// Body::UpdatePointMasses instead). Reproduced as-is to preserve behavioral
// parity — do NOT "fix" it here, that would change the simulation.

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
        std::vector<PointMassPtr> _PointsMass;
    };
}
