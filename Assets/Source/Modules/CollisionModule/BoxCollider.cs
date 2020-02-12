using Modules.ExtensionModule;
using UnityEngine;

using Vector3 = Kinematics.MathModule.Vector3;

namespace Kinematics.CollisionModule
{
    [RequireComponent(typeof(BoxShape))]
    [AddComponentMenu("PhysicsEngine/CollisionModule/BoxCollider")]
    public class BoxCollider : Collider
    {
        private MathModule.Vector3 sizeBox;
        private MathModule.Vector3 min;
        private MathModule.Vector3 max;

        /* Unity Properties */
        public UnityEngine.Vector3 size = UnityEngine.Vector3.one;

        protected override void Start()
        {
            base.Start();

            sizeBox = transform.localScale.ToPhysics() / 2 * size.x;
            colliders.Add(this);
            type = 1;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            min = _center - sizeBox;
            max = _center + sizeBox;
        }

        public override MathModule.Vector3 GetSize() => sizeBox;

        public MathModule.Vector3 GetMin() => min;

        public MathModule.Vector3 GetMax() => max;
    }
}
