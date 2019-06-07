using UnityEngine;

namespace PhysicsEngine.CollisionModule
{
    public class SphereCollider : Collider
    {
        public float radius;
        float _radius;

        protected override void Start()
        {
            _radius = this.transform.localScale.x / 2 * radius;
            rb_Colliders.Add(this);
            type = 0;
        }

        public float GetRadius()
        {
            return _radius;
        }

        public override PhysicsEngine.MathModule.Vector3 GetSize()
        {
            return new PhysicsEngine.MathModule.Vector3(_radius, _radius, _radius);
        }
    }
}