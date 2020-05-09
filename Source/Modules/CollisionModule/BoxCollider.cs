using UnityEngine;
using Vector3 = Kinematics.MathModule.Vector3;

namespace Kinematics.CollisionModule
{
    [RequireComponent(typeof(BoxShape))]
    [AddComponentMenu("Kinematics/CollisionModule/BoxCollider")]
    public class BoxCollider : Collider
    {
        private Vector3 sizeBox;
        private Vector3 min;
        private Vector3 max;

        /* Unity Properties */
        public Vector3 size = Vector3.One;

        protected override void Start()
        {
            base.Start();

            sizeBox = transform.localScale / 2 * size.X;
            colliders.Add(this);
            type = 1;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            min = _center - sizeBox;
            max = _center + sizeBox;
        }

        public override Vector3 GetSize() => sizeBox;

        public Vector3 GetMin() => min;

        public Vector3 GetMax() => max;
    }
}
