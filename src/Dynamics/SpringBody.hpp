#pragma once

#include <algorithm>
#include <cmath>
#include <vector>

#include "Collision/PointMass.hpp"
#include "Collision/Shape.hpp"
#include "Collision/Spring.hpp"
#include "Dynamics/Body.hpp"
#include "Math/Vector2.hpp"

// Port of src/Dynamics/SpringBody.cs
//
// FAITHFUL PORT OF A LATENT C# BUG: Add() checks PointMassList.Contains(...) but
// pushes into _PointsMass. Because the springs are built from PointMassList
// instances, the check is always true, so _PointsMass stays empty and the final
// loop in ApplyInternalForces never runs. The point masses are instead
// integrated by Body::UpdatePointMasses. We reproduce this exactly to preserve
// behavioral parity (do NOT "fix" it here — that would change the simulation).

namespace kinematics {

class SpringBody : public Body {
public:
    SpringBody(const Shape& shape, float mass, float edgeSpringK, float edgeSpringDamp,
               float shapeSpringK, float shapeSpringDamp)
        : Body(shape, mass),
          _shapeK(shapeSpringK), _shapeDamping(shapeSpringDamp), _isConstrained(true) {
        int index;
        for (index = 0; index < Count - 1; index++) {
            Add(Spring(PointMassList[index], PointMassList[index + 1],
                       edgeSpringK, edgeSpringDamp));
        }
        Add(Spring(PointMassList[index], PointMassList[0], edgeSpringK, edgeSpringDamp));
    }

    void Add(const Spring& spring) {
        if (std::find(PointMassList.begin(), PointMassList.end(), spring.PointMassA)
            == PointMassList.end()) {
            _PointsMass.push_back(spring.PointMassA);
        }
        if (std::find(PointMassList.begin(), PointMassList.end(), spring.PointMassB)
            == PointMassList.end()) {
            _PointsMass.push_back(spring.PointMassB);
        }
        _springs.push_back(spring);
    }

    void ApplyInternalForces(double elapsed) override {
        Body::ApplyInternalForces(elapsed);
        Vector2 force;
        for (size_t i = 0; i < _springs.size(); i++) {
            Spring& spring = _springs[i];
            force = Spring::SpringForce(spring);
            spring.PointMassA->Force.X += force.X;
            spring.PointMassA->Force.Y += force.Y;
            spring.PointMassB->Force.X -= force.X;
            spring.PointMassB->Force.Y -= force.Y;
        }

        if (_isConstrained) {
            for (int i = 0; i < Count; i++) {
                if (_shapeK > 0.0f) {
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

        // Dead loop (_PointsMass is always empty — see header note).
        for (size_t i = 0; i < _PointsMass.size(); i++) {
            float mass = _PointsMass[i]->Mass;
            if (!std::isinf(mass)) {
                _PointsMass[i]->Force += Gravity * mass;
            }
            _PointsMass[i]->Velocity.X *= DAMPING;
            _PointsMass[i]->Velocity.Y *= DAMPING;
            _PointsMass[i]->Update(elapsed);
        }
    }

private:
    float _shapeK;
    float _shapeDamping;
    bool _isConstrained;
    std::vector<Spring> _springs;
    std::vector<PointMassPtr> _PointsMass;
};

} // namespace kinematics
