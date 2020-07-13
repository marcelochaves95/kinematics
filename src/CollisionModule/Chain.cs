using System.Collections.Generic;
using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class Chain
    {
        public List<PointMass> PointMassList;
        public List<Spring> SpringList;

        public float Damping;

        public Chain(PointMass from, PointMass to, int count, float k, float damping, float mass)
        {
            Damping = 0.99f;
            PointMassList = new List<PointMass>();
            SpringList = new List<Spring>();

            float length = Vector2.Distance(from.position, to.position) / count;
            Vector2 direction = to.position - from.position;
            direction.Normalize();

            for (int i = 0; i < count + 1; i++)
            {
                PointMassList.Add(new PointMass(new Vector2(from.position.X + direction.X * length * i, from.position.Y + direction.Y * length* i), mass));
            }

            PointMassList[0] = from;
            PointMassList[count] = to;

            for (int i = 1; i < count + 1; i++)
            {
                SpringList.Add(new Spring(PointMassList[i - 1], PointMassList[i - 0], k, damping));
            }
        }

        public void Update(double elapsed)
        {
            for (int i = 0; i < SpringList.Count; i++)
            {
                Spring spring = SpringList[i];
                Spring.SpringForce(ref spring, out Vector2 force);

                spring.PointMassA.force.X += force.X;
                spring.PointMassA.force.Y += force.Y;

                spring.PointMassB.force.X -= force.X;
                spring.PointMassB.force.Y -= force.Y;
            }

            for (int i = 1; i < PointMassList.Count-1; i++)
            {
                PointMassList[i].velocity.X *= Damping;
                PointMassList[i].velocity.Y *= Damping;
                PointMassList[i].Update(elapsed);
            }
        }
    }
}