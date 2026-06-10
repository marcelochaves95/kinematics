#pragma once

// Port of Utils/Bitmask.cs (IDisposable dropped; Clear() does the reset).

namespace kinematics
{
    class Bitmask
    {
    public:
        int Mask = 0;

        void Clear();
        void SetOn(int bit);
        void SetOff(int bit);
    };
}
