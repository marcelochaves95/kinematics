using Kinematics.Common;
using Kinematics.Dynamics;

namespace Kinematics.Collision
{
    public struct CollisionInfo
    {
        public Body BodyA;
        public Body BodyB;
        public PointMass PointMassA;
        public PointMass PointMassB;
        public PointMass PointMassC;
        public float EdgeDistance;
        public Vector2 Normal;
        public Vector2 Point;
        public float Penetration;

        public void Clear()
        {
            BodyA = BodyB = null;
            PointMassA = PointMassB = PointMassC = new PointMass();
            EdgeDistance = 0f;
            Point = Vector2.Zero;
            Normal = Vector2.Zero;
            Penetration = 0f;
        }
    }
}