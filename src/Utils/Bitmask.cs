namespace Kinematics.Utils
{
    public class Bitmask
    {
        public int Mask;

        public void Clear()
        {
            Mask = 0x00;
        }

        public void setOn(int bit)
        {
            Mask |= (0x01 << ((bit > 0) ? (bit - 1) : 0));
        }

        public void SetOff(int bit)
        {
            Mask &= ~(0x01 << ((bit > 0) ? (bit - 1) : 0));
        }
    }
}