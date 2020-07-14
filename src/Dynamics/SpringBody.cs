using System.Collections.Generic;
using Kinematics.Collision;
using Kinematics.Math;

namespace Kinematics.Dynamics
{
    public class SpringBody : Body
    {
        public List<Spring> SpringList;
        public List<PointMass> SpringPointMassList;
        public bool IsConstrained = true;
        public float EdgeK;
        public float EdgeDamping;
        public float ShapeK;
        public float ShapeDamping;

        public SpringBody(Shape shape, float mass, float edgeSpringK, float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp) : base(shape, mass)
        {
            IsConstrained = true;
            SpringList = new List<Spring>();
            SpringPointMassList = new List<PointMass>();

            ShapeK = shapeSpringK;
            ShapeDamping = shapeSpringDamp;
            EdgeK = edgeSpringK;
            EdgeDamping = edgeSpringDamp;

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
                SpringPointMassList.Add(spring.PointMassA);
            }

            if (!PointMassList.Contains(spring.PointMassB))
            {
                SpringPointMassList.Add(spring.PointMassB);
            }

            SpringList.Add(spring);
        }

        public override void ApplyInternalForces(double elapsed)
        {
            Vector2 force;
            for (int i = 0; i < SpringList.Count; i++)
            {
                Spring spring = SpringList[i];
                Spring.SpringForce(ref spring, out force);

                spring.PointMassA.Force.X += force.X;
                spring.PointMassA.Force.Y += force.Y;
   
                spring.PointMassB.Force.X -= force.X;
                spring.PointMassB.Force.Y -= force.Y;
            }

            if (IsConstrained)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (ShapeK > 0)
                    {
                        Spring.SpringForce(ref PointMassList[i].Position, ref PointMassList[i].Velocity, ref CurrentShape.Points[i],
                                                        ref PointMassList[i].Velocity, 0.0f, ShapeK, ShapeDamping, out force);

                        PointMassList[i].Force.X += force.X;
                        PointMassList[i].Force.Y += force.Y;
                    }
                }
            }

            for (int i = 0; i < SpringPointMassList.Count; i++)
            {
                SpringPointMassList[i].Velocity.X *= Damping;
                SpringPointMassList[i].Velocity.Y *= Damping;
                SpringPointMassList[i].Update(elapsed);
            }
        }
    }
}