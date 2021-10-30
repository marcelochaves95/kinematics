using System;
using Kinematics.Dynamics;
using Microsoft.Xna.Framework;

namespace Kinematics.Collision
{
    internal struct CollisionInfo : IDisposable
    {
        public float EdgeDistance;
        public float Penetration;
        public Vector2 Normal;
        public Vector2 Point;
        public Body BodyA;
        public Body BodyB;
        public PointMass PointMassA;
        public PointMass PointMassB;
        public PointMass PointMassC;

        public void Clear()
        {
            Dispose();
        }

        public void Dispose()
        {
            BodyA = null;
            BodyB = null;
            PointMassA = new PointMass();
            PointMassB = new PointMass();
            PointMassC = new PointMass();
            Point = Vector2.Zero;
            Normal = Vector2.Zero;
            EdgeDistance = 0f;
            Penetration = 0f;
        }
    }
}
