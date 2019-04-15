using UnityEngine;

namespace CollisionModule.Shapes
{
    [AddComponentMenu("Physics Engine/Shapes/Capsule")]
    public class CapsuleShape : CollisionShape
    {
        public enum CapsuleAxis { x, y, z }

        [SerializeField] protected float radius = 1f;
        public float Radius
        {
            get { return radius; }
            set
            {
                if (collisionShapePtr != null && value != radius)
                    Debug.LogError("Cannot change the radius after the bullet shape has been created. Radius is only the initial value " +
                        "Use LocalScaling to change the shape of a bullet shape.");
                else
                    radius = value;
            }
        }

        [SerializeField]
        protected float height = 2f;
        public float Height
        {
            get { return height; }
            set
            {
                if (collisionShapePtr != null && value != height)
                    Debug.LogError("Cannot change the height after the bullet shape has been created. Height is only the initial value " +
                        "Use LocalScaling to change the shape of a bullet shape.");
                else
                    height = value;
            }
        }

        [SerializeField]
        protected CapsuleAxis upAxis = CapsuleAxis.y;
        public CapsuleAxis UpAxis
        {
            get { return upAxis; }
            set
            {
                if (collisionShapePtr != null && value != upAxis)
                    Debug.LogError("Cannot change the upAxis after the bullet shape has been created. upAxis is only the initial value " +
                        "Use LocalScaling to change the shape of a bullet shape.");
                else
                    upAxis = value;
            }
        }

        [SerializeField]
        protected Vector3 localScaling = Vector3.one;
        public Vector3 LocalScaling
        {
            get { return localScaling; }
            set
            {
                localScaling = value;
                if (collisionShapePtr != null)
                    ((CapsuleShape)collisionShapePtr).LocalScaling = value;
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (drawGizmo == false)
                return;

            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = localScaling;

            switch (upAxis)
            {
                case CapsuleAxis.x:
                    rotation = Quaternion.AngleAxis(90, transform.forward) * rotation;
                    break;
                case CapsuleAxis.z:
                    rotation = Quaternion.AngleAxis(90, transform.right) * rotation;
                    break;
            }

            Utility.DebugDrawCapsule(position, rotation, scale, radius, height / 2f, 1, Gizmos.color);
        }

        CapsuleShape CreateCapsuleShape()
        {
            CapsuleShape capsuleShape = null;

            switch (upAxis)
            {
                case CapsuleAxis.x:
                    capsuleShape = new CapsuleShapeX(radius, height);
                    break;
                case CapsuleAxis.y:
                    capsuleShape = new CapsuleShape(radius, height);
                    break;
                case CapsuleAxis.z:
                    capsuleShape = new CapsuleShapeZ(radius, height);
                    break;
                default:
                    Debug.LogError("invalid axis value");
                    break;
            }

            capsuleShape.LocalScaling = localScaling;

            return capsuleShape;
        }

        public override CollisionShape CopyCollisionShape()
        {
            return CreateCapsuleShape();
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
                collisionShapePtr = CreateCapsuleShape();

            return collisionShapePtr;
        }
    }
}