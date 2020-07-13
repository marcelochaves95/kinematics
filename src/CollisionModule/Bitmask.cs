namespace Kinematics.CollisionModule
{
    public class Bitmask
    {
        public int Mask;

        public void clear() { Mask = 0x00; }
        public void setOn(int bit)
        {
            Mask |= (0x01 << ((bit > 0) ? (bit - 1) : 0));
        }
        public void setOff(int bit)
        {
            Mask &= ~(0x01 << ((bit > 0) ? (bit - 1) : 0));
        }
    }
}