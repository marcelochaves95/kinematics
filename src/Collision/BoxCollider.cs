using UnityEngine;
using Vector3 = Kinematics.Mathematics.Vector3;

namespace Kinematics.Collision
{
    [AddComponentMenu("Kinematics/Collision/BoxCollider")]
    public partial class BoxCollider : Collider
    {
        [Compact]
        public Vector3 vec3;
        [SerializeField]
        private bool drawShape = true;
        private Vector3 sizeBox;
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }
        public Vector3 Size { get; } = Vector3.One;

        protected override void Start()
        {
            base.Start();

            sizeBox = transform.localScale / 2 * Size.X;
            Colliders.Add(this);
            type = 1;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            Min = center - sizeBox;
            Max = center + sizeBox;
        }
    }
}