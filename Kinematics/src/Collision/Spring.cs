using Kinematics.Math;

namespace Kinematics.Collision
{
    public class Spring
    {
        public float Damping;
        public float K;
        public float D;
        public PointMass PointMassA;
        public PointMass PointMassB;

        public Spring(PointMass pointMassA, PointMass pointMassB, float k, float damping) : this(pointMassA, pointMassB, k, damping, 0)
        {
            Reset();
        }

        public Spring(PointMass pointMassA, PointMass pointMassB, float k, float damping, float length)
        {
            PointMassA = pointMassA;
            PointMassB = pointMassB;
            D = length;
            K = k;
            Damping = damping;
        }

        public override string ToString()
        {
            return $"{{A: [{PointMassA}] B: [{PointMassB}] Length: {D}}}";
        }

        public void Reset()
        {
            D = (PointMassA.Position - PointMassB.Position).Magnitude();
        }

        public static void SpringForce(ref Spring spring, out Vector2 forceOut)
        {
            SpringForce(ref spring.PointMassA.Position, ref spring.PointMassA.Velocity,
                ref spring.PointMassB.Position, ref spring.PointMassB.Velocity, spring.D, spring.K, spring.Damping, out forceOut);
        }

        public static void SpringForce(ref Vector2 posA, ref Vector2 velA, ref Vector2 posB, ref Vector2 velB, float springD, float springK, float damping, out Vector2 forceOut)
        {
            float BtoAX = posA.X - posB.X;
            float BtoAY = posA.Y - posB.Y;
            float distance = Mathf.Sqrt(Mathf.Pow(BtoAX, 2) + Mathf.Pow(BtoAY, 2));
            if (distance > Mathf.Epsilon)
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
            float totalRelVel = relVelX * BtoAX + relVelY * BtoAY;
            forceOut.X = BtoAX * (distance * springK - totalRelVel * damping);
            forceOut.Y = BtoAY * (distance * springK - totalRelVel * damping);
        }
    }
}