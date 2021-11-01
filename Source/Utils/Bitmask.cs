using System;

namespace Kinematics.Utils
{
    internal class Bitmask : IDisposable
    {
        public int Mask;

        public void Clear()
        {
            Dispose();
        }

        public void SetOn(int bit)
        {
            Mask |= 0x01 << (bit > 0 ? bit - 1 : 0);
        }

        public void SetOff(int bit)
        {
            Mask &= ~(0x01 << (bit > 0 ? bit - 1 : 0));
        }

        public void Dispose()
        {
            Mask = 0x00;
        }
    }
}