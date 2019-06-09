using UnityEngine;

using Vector3 = PhysicsEngine.MathModule.Vector3;

namespace PhysicsEngine.CollisionModule
{
    [AddComponentMenu("PhysicsEngine/CollisionModule/SphereCollider")]
    public class SphereCollider : Collider
    {
        public float radius;
        private float _radius;

        protected override void Start()
        {
            _radius = this.transform.localScale.x / 2 * radius;
            colliders.Add(this);
            type = 0;
        }

        public float GetRadius() => _radius;

        public override Vector3 GetSize() => new Vector3(_radius, _radius, _radius);
    }
}