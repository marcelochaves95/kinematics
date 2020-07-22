using System.Collections.Generic;
using Kinematics.Collision;
using Kinematics.Math;

namespace Kinematics.Dynamics
{
    public class SpringBody : Body
    {
        private readonly List<Spring> _springList;
        private readonly List<PointMass> _springPointMassList;
        private readonly bool _isConstrained = true;
        private float _edgeK;
        private float _edgeDamping;
        private readonly float _shapeK;
        private readonly float _shapeDamping;

        public SpringBody(Shape shape, float mass, float edgeSpringK, float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp) : base(shape, mass)
        {
            _isConstrained = true;
            _springList = new List<Spring>();
            _springPointMassList = new List<PointMass>();

            _shapeK = shapeSpringK;
            _shapeDamping = shapeSpringDamp;
            _edgeK = edgeSpringK;
            _edgeDamping = edgeSpringDamp;

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
                _springPointMassList.Add(spring.PointMassA);
            }

            if (!PointMassList.Contains(spring.PointMassB))
            {
                _springPointMassList.Add(spring.PointMassB);
            }

            _springList.Add(spring);
        }

        public override void ApplyInternalForces(double elapsed)
        {
            Vector2 force;
            for (int i = 0; i < _springList.Count; i++)
            {
                Spring spring = _springList[i];
                Spring.SpringForce(ref spring, out force);

                spring.PointMassA.Force.X += force.X;
                spring.PointMassA.Force.Y += force.Y;
   
                spring.PointMassB.Force.X -= force.X;
                spring.PointMassB.Force.Y -= force.Y;
            }

            if (_isConstrained)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (_shapeK > 0)
                    {
                        Spring.SpringForce(ref PointMassList[i].Position, ref PointMassList[i].Velocity, ref CurrentShape.Points[i],
                                                        ref PointMassList[i].Velocity, 0f, _shapeK, _shapeDamping, out force);

                        PointMassList[i].Force.X += force.X;
                        PointMassList[i].Force.Y += force.Y;
                    }
                }
            }

            for (int i = 0; i < _springPointMassList.Count; i++)
            {
                _springPointMassList[i].Velocity.X *= Damping;
                _springPointMassList[i].Velocity.Y *= Damping;
                _springPointMassList[i].Update(elapsed);
            }
        }
    }
}