#pragma once

#include <collision/shape.h>
#include <dynamics/spring_body.h>
#include <math/vector2.h>
#include <vector>

namespace kinematics
{
    class PressureBody : public SpringBody
    {
    public:
        PressureBody(const Shape& s, float mass, float gasPressure, float edgeSpringK,
            float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp);

        void apply_internal_forces(double elapsed) override;

    private:
        float _volume = 0.0f;
        float _pressure;
        std::vector<float> _edge_length_list;
        std::vector<Vector2> _normal_list;
    };
}
