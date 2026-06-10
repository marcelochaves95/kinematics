#pragma once

#include <Collision/Shape.h>
#include <Dynamics/SpringBody.h>
#include <Math/Vector2.h>
#include <vector>

// Port of Dynamics/PressureBody.cs — adds gas-pressure forces: each edge pushes
// outward along the vertex normals proportionally to pressure / volume (the
// volume is a shoelace-style estimate).

namespace kinematics
{
    class PressureBody : public SpringBody
    {
    public:
        PressureBody(const Shape& s, float mass, float gasPressure, float edgeSpringK,
            float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp);

        void ApplyInternalForces(double elapsed) override;

    private:
        float _volume = 0.0f;
        float _pressure;
        std::vector<float> _edgeLengthList;
        std::vector<Vector2> _normalList;
    };
}
