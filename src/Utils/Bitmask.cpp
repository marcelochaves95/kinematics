#include <Utils/Bitmask.h>

namespace kinematics
{
    void Bitmask::Clear()
    {
        Mask = 0x00;
    }

    void Bitmask::SetOn(int bit)
    {
        Mask |= 0x01u << (bit > 0 ? bit - 1 : 0);
    }

    void Bitmask::SetOff(int bit)
    {
        Mask &= ~(0x01u << (bit > 0 ? bit - 1 : 0));
    }
}
