#pragma once

// Port of src/Utils/Bitmask.cs (IDisposable dropped; Clear() does the reset).

namespace kinematics {

class Bitmask {
public:
    int Mask = 0;

    void Clear() { Mask = 0x00; }

    void SetOn(int bit) {
        Mask |= 0x01 << (bit > 0 ? bit - 1 : 0);
    }

    void SetOff(int bit) {
        Mask &= ~(0x01 << (bit > 0 ? bit - 1 : 0));
    }
};

} // namespace kinematics
