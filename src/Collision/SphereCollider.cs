using UnityEngine;
using Vector3 = Kinematics.MathModule.Vector3;

namespace Kinematics.Collision
{
    [AddComponentMenu("Kinematics/Collision/SphereCollider")]
    public class SphereCollider : Collider
    {
        public float radius;
        private float _radius;

        protected override void Start()
        {
            base.Start();

            _radius = transform.localScale.x / 2 * radius;
            colliders.Add(this);
            type = 0;
        }

        public float GetRadius() => _radius;

        public override Vector3 GetSize() => new Vector3(_radius);
    }
}
