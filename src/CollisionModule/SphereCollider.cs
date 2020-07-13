using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class SphereCollider : Collider
    {
        public float Radius;
        private float radius;

        protected override void Start()
        {
            base.Start();

            //radius = transform.localScale.x / 2 * Radius;
            Colliders.Add(this);
            type = 0;
        }

        public float GetRadius()
        {
            return radius;
        }

        public override Vector3 GetSize()
        {
            return new Vector3(radius);
        }
    }
}