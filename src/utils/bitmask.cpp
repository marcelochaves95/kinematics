#include <utils/bitmask.h>

namespace kinematics
{
    void Bitmask::clear()
    {
        mask = 0x00;
    }

    void Bitmask::set_on(int bit)
    {
        mask |= 0x01u << (bit > 0 ? bit - 1 : 0);
    }

    void Bitmask::set_off(int bit)
    {
        mask &= ~(0x01u << (bit > 0 ? bit - 1 : 0));
    }
}
