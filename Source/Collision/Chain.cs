using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Kinematics.Collision
{
    public class Chain
    {
        public float Damping;
        public List<PointMass> PointMassList;
        public List<Spring> SpringList;

        public Chain(PointMass from, PointMass to, int count, float k, float damping, float mass)
        {
            Damping = 0.99f;
            PointMassList = new List<PointMass>();
            SpringList = new List<Spring>();
            float length = Vector2.Distance(from.Position, to.Position) / count;
            Vector2 direction = to.Position - from.Position;
            direction.Normalize();
            for (int i = 0; i < count + 1; i++)
            {
                PointMassList.Add(new PointMass(new Vector2(from.Position.X + direction.X * length * i, from.Position.Y + direction.Y * length* i), mass));
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
                spring.PointMassA.Force.X += force.X;
                spring.PointMassA.Force.Y += force.Y;
                spring.PointMassB.Force.X -= force.X;
                spring.PointMassB.Force.Y -= force.Y;
            }

            for (int i = 1; i < PointMassList.Count-1; i++)
            {
                PointMassList[i].Velocity.X *= Damping;
                PointMassList[i].Velocity.Y *= Damping;
                PointMassList[i].Update(elapsed);
            }
        }
    }
}