#pragma once

#include <vector>

#include "Collision/Shape.hpp"
#include "Dynamics/SpringBody.hpp"
#include "Math/Mathf.hpp"
#include "Math/Vector2.hpp"

// Port of src/Dynamics/PressureBody.cs
//
// Adds gas-pressure forces: each edge pushes outward along the vertex normals
// proportionally to pressure / volume. The volume is a shoelace-style estimate.

namespace kinematics {

class PressureBody : public SpringBody {
public:
    PressureBody(const Shape& s, float mass, float gasPressure, float edgeSpringK,
                 float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp)
        : SpringBody(s, mass, edgeSpringK, edgeSpringDamp, shapeSpringK, shapeSpringDamp),
          _pressure(gasPressure) {
        _normalList.resize(static_cast<size_t>(Count));
        _edgeLengthList.resize(static_cast<size_t>(Count));
    }

    void ApplyInternalForces(double elapsed) override {
        SpringBody::ApplyInternalForces(elapsed);

        _volume = 0.0f;

        for (int i = 0; i < Count; i++) {
            int prev = i > 0 ? i - 1 : Count - 1;
            int next = i < Count - 1 ? i + 1 : 0;

            Vector2 edge1N(PointMassList[i]->Position.X - PointMassList[prev]->Position.X,
                           PointMassList[i]->Position.Y - PointMassList[prev]->Position.Y);
            edge1N = edge1N.Perpendicular();

            Vector2 edge2N(PointMassList[next]->Position.X - PointMassList[i]->Position.X,
                           PointMassList[next]->Position.Y - PointMassList[i]->Position.Y);
            edge2N = edge2N.Perpendicular();

            Vector2 norm(edge1N.X + edge2N.X, edge1N.Y + edge2N.Y);

            float nL = mathf::Sqrt(norm.X * norm.X + norm.Y * norm.Y);
            if (nL > 0.001f) {
                norm.X /= nL;
                norm.Y /= nL;
            }

            float edgeL = mathf::Sqrt(edge2N.X * edge2N.X + edge2N.Y * edge2N.Y);

            _normalList[i] = norm;
            _edgeLengthList[i] = edgeL;

            float xDistance = mathf::Abs(PointMassList[i]->Position.X -
                                         PointMassList[next]->Position.X);
            float volumeProduct = xDistance * mathf::Abs(norm.X) * edgeL;
            _volume += 0.5f * volumeProduct;
        }

        float invVolume = 1.0f / _volume;

        for (int i = 0; i < Count; i++) {
            int j = i < Count - 1 ? i + 1 : 0;

            float pressureV = invVolume * _edgeLengthList[i] * _pressure;
            PointMassList[i]->Force.X += _normalList[i].X * pressureV;
            PointMassList[i]->Force.Y += _normalList[i].Y * pressureV;

            PointMassList[j]->Force.X += _normalList[j].X * pressureV;
            PointMassList[j]->Force.Y += _normalList[j].Y * pressureV;
        }
    }

private:
    float _volume = 0.0f;
    float _pressure;
    std::vector<float> _edgeLengthList;
    std::vector<Vector2> _normalList;
};

} // namespace kinematics
