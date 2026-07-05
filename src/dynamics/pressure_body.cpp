#include <dynamics/pressure_body.h>
#include <math/mathf.h>

namespace kinematics
{
    PressureBody::PressureBody(const Shape& s, float mass, float gasPressure,
        float edgeSpringK, float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp)
        : SpringBody(s, mass, edgeSpringK, edgeSpringDamp, shapeSpringK, shapeSpringDamp),
          _pressure(gasPressure)
    {
        _normal_list.resize(static_cast<size_t>(count));
        _edge_length_list.resize(static_cast<size_t>(count));
    }

    void PressureBody::apply_internal_forces(double elapsed)
    {
        SpringBody::apply_internal_forces(elapsed);

        _volume = 0.0f;

        for (int i = 0; i < count; i++)
        {
            int prev = i > 0 ? i - 1 : count - 1;
            int next = i < count - 1 ? i + 1 : 0;

            Vector2 edge1N(point_mass_list[i]->position.x - point_mass_list[prev]->position.x,
                point_mass_list[i]->position.y - point_mass_list[prev]->position.y);
            edge1N = edge1N.perpendicular();

            Vector2 edge2N(point_mass_list[next]->position.x - point_mass_list[i]->position.x,
                point_mass_list[next]->position.y - point_mass_list[i]->position.y);
            edge2N = edge2N.perpendicular();

            Vector2 norm(edge1N.x + edge2N.x, edge1N.y + edge2N.y);

            float nL = mathf::sqrt(norm.x * norm.x + norm.y * norm.y);
            if (nL > 0.001f)
            {
                norm.x /= nL;
                norm.y /= nL;
            }

            float edgeL = mathf::sqrt(edge2N.x * edge2N.x + edge2N.y * edge2N.y);

            _normal_list[i] = norm;
            _edge_length_list[i] = edgeL;

            float xDistance = mathf::abs(point_mass_list[i]->position.x -
                point_mass_list[next]->position.x);
            float volumeProduct = xDistance * mathf::abs(norm.x) * edgeL;
            _volume += 0.5f * volumeProduct;
        }

        if (_volume > mathf::EPSILON)
        {
            float invVolume = 1.0f / _volume;

            for (int i = 0; i < count; i++)
            {
                int j = i < count - 1 ? i + 1 : 0;

                float pressureV = invVolume * _edge_length_list[i] * _pressure;
                point_mass_list[i]->force.x += _normal_list[i].x * pressureV;
                point_mass_list[i]->force.y += _normal_list[i].y * pressureV;

                point_mass_list[j]->force.x += _normal_list[j].x * pressureV;
                point_mass_list[j]->force.y += _normal_list[j].y * pressureV;
            }
        }
    }
}
