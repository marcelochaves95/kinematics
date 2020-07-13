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
            for (i = 0; i < count - 1; i++)
            {
                Add(new Spring(pointmass_list[i], pointmass_list[i + 1], edgeSpringK, edgeSpringDamp));
            }

            Add(new Spring(pointmass_list[i], pointmass_list[0], edgeSpringK, edgeSpringDamp));
        }

        public void Add(Spring spring)
        {
            if (!pointmass_list.Contains(spring.pointmass_a))
            {
                spring_pointmass_list.Add(spring.pointmass_a);
            }

            if (!pointmass_list.Contains(spring.pointmass_b))
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
                for (int i = 0; i < count; i++)
                {
                    if (shape_k > 0)
                    {

                        Spring.SpringForce(ref pointmass_list[i].position, ref pointmass_list[i].velocity, ref curr_shape.points[i],
                                                        ref pointmass_list[i].velocity, 0.0f, shape_k, shape_damping, out force);

                        pointmass_list[i].force.X += force.X;
                        pointmass_list[i].force.Y += force.Y;
                    }
                }
            }

            for (int i = 0; i < spring_pointmass_list.Count; i++)
            {
                spring_pointmass_list[i].velocity.X *= damping;
                spring_pointmass_list[i].velocity.Y *= damping;
                spring_pointmass_list[i].Update(elapsed);
            }
        }
    }
}