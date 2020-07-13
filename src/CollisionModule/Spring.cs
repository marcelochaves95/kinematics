using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class Spring
    {
        public float damping;
        public float k;
        public float d;
        public PointMass pointmass_a;
        public PointMass pointmass_b;

        public Spring(PointMass pointMassA, PointMass pointMassB, float k, float damping) : this(pointMassA, pointMassB, k, damping, 0)
        {
            Reset();
        }

        public Spring(PointMass pointMassA, PointMass pointMassB, float k, float damping, float length)
        {
            pointmass_a = pointMassA;
            pointmass_b = pointMassB;
            d = length;
            this.k = k;
            this.damping = damping;
        }

        public override string ToString()
        {
            return $"{{a:[{pointmass_a}] b:[{pointmass_b}] length:{d}}}";
        }

        public void Reset()
        {
            d = (pointmass_a.position - pointmass_b.position).Length();
        }


        public static void SpringForce(ref Spring spring, out Vector2 forceOut)
        {
            SpringForce(ref spring.pointmass_a.position, ref spring.pointmass_a.velocity,
                ref spring.pointmass_b.position, ref spring.pointmass_b.velocity, spring.d, spring.k, spring.damping, out forceOut);
        }

        public static void SpringForce(ref Vector2 posA, ref Vector2 velA, ref Vector2 posB, ref Vector2 velB, float springD, float springK, float damping, out Vector2 forceOut)
        {
            forceOut = default;
            float BtoAX = (posA.X - posB.X);
            float BtoAY = (posA.Y - posB.Y);

            float distance = Mathf.Sqrt((BtoAX * BtoAX) + (BtoAY * BtoAY));
            if (distance > 0.0001f)
            {
                BtoAX /= distance;
                BtoAY /= distance;
            }
            else
            {
                forceOut.X = 0;
                forceOut.Y = 0;
                return;
            }

            distance = springD - distance;

            float relVelX = velA.X - velB.X;
            float relVelY = velA.Y - velB.Y;

            float totalRelVel = (relVelX * BtoAX) + (relVelY * BtoAY);

            forceOut.X = BtoAX * ((distance * springK) - (totalRelVel * damping));
            forceOut.Y = BtoAY * ((distance * springK) - (totalRelVel * damping));
        }
    }
}