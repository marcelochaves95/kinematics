using System.Collections.Generic;
using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class Chain
    {
        public List<PointMass> pointmass_list;
        public List<Spring> spring_list;

        public float damping;

        public Chain(PointMass from, PointMass to, int count, float k, float damping, float mass)
        {
            this.damping = 0.99f;
            pointmass_list = new List<PointMass>();
            spring_list = new List<Spring>();

            float length = Vector2.Distance(from.position, to.position) / count;
            Vector2 direction = to.position - from.position;
            direction.Normalize();

            for (int i = 0; i < count + 1; i++)
            {
                pointmass_list.Add(new PointMass(new Vector2(from.position.X + direction.X * length * i, from.position.Y + direction.Y * length* i), mass));
            }

            pointmass_list[0] = from;
            pointmass_list[count] = to;

            for (int i = 1; i < count+1; i++)
                spring_list.Add(new Spring(pointmass_list[i - 1], pointmass_list[i - 0], k, damping));
        }

        public void Update(double elapsed)
        {
            for (int i = 0; i < spring_list.Count; i++)
            {
                Spring s = spring_list[i];
                Spring.SpringForce(ref s, out Vector2 force);

                s.pointmass_a.force.X += force.X;
                s.pointmass_a.force.Y += force.Y;

                s.pointmass_b.force.X -= force.X;
                s.pointmass_b.force.Y -= force.Y;
            }

            for (int i = 1; i < pointmass_list.Count-1; i++)
            {
                pointmass_list[i].velocity.X *= damping;
                pointmass_list[i].velocity.Y *= damping;
                pointmass_list[i].Update(elapsed);
            }
        }
    }
}