using System.Collections.Generic;
using Kinematics.Collision;
using Kinematics.Math;

namespace Kinematics.Dynamics
{
    public class SpringBody : Body
    {
        private readonly float _shapeK;
        private readonly float _shapeDamping;
        private readonly bool _isConstrained;
        private readonly List<Spring> _springs;
        private readonly List<PointMass> _PointsMass;

        public SpringBody(Shape shape, float mass, float edgeSpringK, float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp) : base(shape, mass)
        {
            _isConstrained = true;
            _springs = new List<Spring>();
            _PointsMass = new List<PointMass>();
            _shapeK = shapeSpringK;
            _shapeDamping = shapeSpringDamp;
            int index;
            for (index = 0; index < Count - 1; index++)
            {
                Add(new Spring(PointMassList[index], PointMassList[index + 1], edgeSpringK, edgeSpringDamp));
            }

            Add(new Spring(PointMassList[index], PointMassList[0], edgeSpringK, edgeSpringDamp));
        }

        public void Add(Spring spring)
        {
            if (!PointMassList.Contains(spring.PointMassA))
            {
                _PointsMass.Add(spring.PointMassA);
            }

            if (!PointMassList.Contains(spring.PointMassB))
            {
                _PointsMass.Add(spring.PointMassB);
            }

            _springs.Add(spring);
        }

        public override void ApplyInternalForces(double elapsed)
        {
            Vector2 force;
            for (int i = 0; i < _springs.Count; i++)
            {
                Spring spring = _springs[i];
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
                        Spring.SpringForce(ref PointMassList[i].Position, ref PointMassList[i].Velocity, ref CurrentShape.Points[i], ref PointMassList[i].Velocity, 0f, _shapeK, _shapeDamping, out force);
                        PointMassList[i].Force.X += force.X;
                        PointMassList[i].Force.Y += force.Y;
                    }
                }
            }

            for (int i = 0; i < _PointsMass.Count; i++)
            {
                _PointsMass[i].Velocity.X *= Damping;
                _PointsMass[i].Velocity.Y *= Damping;
                _PointsMass[i].Update(elapsed);
            }
        }
    }
}