#pragma once

#include <collision/point_mass.h>
#include <collision/spring.h>
#include <vector>

namespace kinematics
{
    class Chain
    {
    public:
        float damping;
        std::vector<PointMassPtr> point_mass_list;
        std::vector<Spring> spring_list;

        Chain(PointMassPtr from, PointMassPtr to, int count, float k, float damping, float mass);

        void update(double elapsed);
    };
}
