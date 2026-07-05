#include <Dynamics/PressureBody.h>
#include <math/mathf.h>

namespace kinematics
{
    PressureBody::PressureBody(const Shape& s, float mass, float gasPressure,
        float edgeSpringK, float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp)
        : SpringBody(s, mass, edgeSpringK, edgeSpringDamp, shapeSpringK, shapeSpringDamp),
          _pressure(gasPressure)
    {
        _normalList.resize(static_cast<size_t>(Count));
        _edgeLengthList.resize(static_cast<size_t>(Count));
    }

    void PressureBody::ApplyInternalForces(double elapsed)
    {
        SpringBody::ApplyInternalForces(elapsed);

        _volume = 0.0f;

        for (int i = 0; i < Count; i++)
        {
            int prev = i > 0 ? i - 1 : Count - 1;
            int next = i < Count - 1 ? i + 1 : 0;

            Vector2 edge1N(PointMassList[i]->position.x - PointMassList[prev]->position.x,
                PointMassList[i]->position.y - PointMassList[prev]->position.y);
            edge1N = edge1N.perpendicular();

            Vector2 edge2N(PointMassList[next]->position.x - PointMassList[i]->position.x,
                PointMassList[next]->position.y - PointMassList[i]->position.y);
            edge2N = edge2N.perpendicular();

            Vector2 norm(edge1N.x + edge2N.x, edge1N.y + edge2N.y);

            float nL = mathf::sqrt(norm.x * norm.x + norm.y * norm.y);
            if (nL > 0.001f)
            {
                norm.x /= nL;
                norm.y /= nL;
            }

            float edgeL = mathf::sqrt(edge2N.x * edge2N.x + edge2N.y * edge2N.y);

            _normalList[i] = norm;
            _edgeLengthList[i] = edgeL;

            float xDistance = mathf::abs(PointMassList[i]->position.x -
                PointMassList[next]->position.x);
            float volumeProduct = xDistance * mathf::abs(norm.x) * edgeL;
            _volume += 0.5f * volumeProduct;
        }

        // Skip the pressure term if the body has collapsed to ~zero volume:
        // 1/_volume would be Inf and spread NaN through every point mass,
        // corrupting the body permanently. A valid body's volume is order 1,
        // far above this threshold, so this never trips in normal use.
        if (_volume > mathf::EPSILON)
        {
            float invVolume = 1.0f / _volume;

            for (int i = 0; i < Count; i++)
            {
                int j = i < Count - 1 ? i + 1 : 0;

                float pressureV = invVolume * _edgeLengthList[i] * _pressure;
                PointMassList[i]->force.x += _normalList[i].x * pressureV;
                PointMassList[i]->force.y += _normalList[i].y * pressureV;

                PointMassList[j]->force.x += _normalList[j].x * pressureV;
                PointMassList[j]->force.y += _normalList[j].y * pressureV;
            }
        }
    }
}
