#pragma once

namespace kinematics
{
    class Bitmask
    {
    public:
        unsigned int mask = 0;

        void clear();
        void set_on(int bit);
        void set_off(int bit);
    };
}
