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
            if (!PointMassList.Contains(spring.pointmass_a))
            {
                spring_pointmass_list.Add(spring.pointmass_a);
            }

            if (!PointMassList.Contains(spring.pointmass_b))
            {
                spring_pointmass_list.Add(spring.pointmass_b);
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

                s.pointmass_a.force.X += force.X;
                s.pointmass_a.force.Y += force.Y;
   
                s.pointmass_b.force.X -= force.X;
                s.pointmass_b.force.Y -= force.Y;
            }

            if (is_constrained)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (shape_k > 0)
                    {

                        Spring.SpringForce(ref PointMassList[i].position, ref PointMassList[i].velocity, ref CurrentShape.points[i],
                                                        ref PointMassList[i].velocity, 0.0f, shape_k, shape_damping, out force);

                        PointMassList[i].force.X += force.X;
                        PointMassList[i].force.Y += force.Y;
                    }
                }
            }

            for (int i = 0; i < spring_pointmass_list.Count; i++)
            {
                spring_pointmass_list[i].velocity.X *= Damping;
                spring_pointmass_list[i].velocity.Y *= Damping;
                spring_pointmass_list[i].Update(elapsed);
            }
        }
    }
}