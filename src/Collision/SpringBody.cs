using System.Collections.Generic;
using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class SpringBody : Body
    {
        public List<Spring> spring_list;
        public List<PointMass> spring_pointmass_list;
        public bool is_constrained = true;
        public float edge_k;
        public float edge_damping;
        public float shape_k;
        public float shape_damping;

        public SpringBody(Shape shape, float mass, float edgeSpringK, float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp) : base(shape, mass)
        {
            is_constrained = true;
            spring_list = new List<Spring>();
            spring_pointmass_list = new List<PointMass>();

            shape_k = shapeSpringK;
            shape_damping = shapeSpringDamp;
            edge_k = edgeSpringK;
            edge_damping = edgeSpringDamp;

            int i;
            for (i = 0; i < Count - 1; i++)
            {
                Add(new Spring(PointMassList[i], PointMassList[i + 1], edgeSpringK, edgeSpringDamp));
            }

            Add(new Spring(PointMassList[i], PointMassList[0], edgeSpringK, edgeSpringDamp));
        }

        public void Add(Spring spring)
        {
            if (!PointMassList.Contains(spring.PointMassA))
            {
                spring_pointmass_list.Add(spring.PointMassA);
            }

            if (!PointMassList.Contains(spring.PointMassB))
            {
                spring_pointmass_list.Add(spring.PointMassB);
            }

            spring_list.Add(spring);
        }

        public override void ApplyInternalForces(double elapsed)
        {
            Vector2 force = new Vector2();
            for (int i = 0; i < spring_list.Count; i++)
            {
                Spring s = spring_list[i];
                Spring.SpringForce(ref s, out force);

                s.PointMassA.Force.X += force.X;
                s.PointMassA.Force.Y += force.Y;
   
                s.PointMassB.Force.X -= force.X;
                s.PointMassB.Force.Y -= force.Y;
            }

            if (is_constrained)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (shape_k > 0)
                    {

                        Spring.SpringForce(ref PointMassList[i].Position, ref PointMassList[i].Velocity, ref CurrentShape.Points[i],
                                                        ref PointMassList[i].Velocity, 0.0f, shape_k, shape_damping, out force);

                        PointMassList[i].Force.X += force.X;
                        PointMassList[i].Force.Y += force.Y;
                    }
                }
            }

            for (int i = 0; i < spring_pointmass_list.Count; i++)
            {
                spring_pointmass_list[i].Velocity.X *= Damping;
                spring_pointmass_list[i].Velocity.Y *= Damping;
                spring_pointmass_list[i].Update(elapsed);
            }
        }
    }
}