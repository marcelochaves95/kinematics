#pragma once

#include <collision/shape.h>
#include <collision/spring.h>
#include <dynamics/body.h>
#include <vector>

namespace kinematics
{
    class SpringBody : public Body
    {
    public:
        SpringBody(const Shape& shape, float mass, float edgeSpringK, float edgeSpringDamp,
            float shapeSpringK, float shapeSpringDamp);

        void add(const Spring& spring);
        void apply_internal_forces(double elapsed) override;

    private:
        float _shape_k;
        float _shape_damping;
        bool _is_constrained;
        std::vector<Spring> _springs;
    };
}
