#pragma once

// Port of Utils/Bitmask.cs (IDisposable dropped; Clear() does the reset).

namespace kinematics
{
    class Bitmask
    {
    public:
        // Unsigned so that SetOn(32) -> (1u << 31) is well-defined. With a signed
        // int that shift overflows into the sign bit (UB). The bit pattern matches
        // the C# `int` mask, and the broadphase only compares bits (`& == 0`).
        unsigned int Mask = 0;

        void Clear();
        void SetOn(int bit);
        void SetOff(int bit);
    };
}
