using UnityEngine;

namespace CollisionModule.Shapes
{
    [AddComponentMenu("Physics Engine/Shapes/Cylinder")]
    public class CylinderShape : CollisionShape
    {
        [SerializeField] protected Vector3 halfExtent = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 HalfExtent
        {
            get { return halfExtent; }
            set
            {
                if (collisionShapePtr != null && value != halfExtent)
                    Debug.LogError("Cannot change the extents after the bullet shape has been created. Extents is only the initial value " +
                        "Use LocalScaling to change the shape of a bullet shape.");
                else
                    halfExtent = value;
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
                    ((CylinderShape)collisionShapePtr).LocalScaling = value;
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (drawGizmo == false)
                return;

            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = localScaling;
            Utility.DebugDrawCylinder(position, rotation, scale, halfExtent.x, halfExtent.y, 1, Color.yellow);
        }

        public override CollisionShape CopyCollisionShape()
        {
            CylinderShape cylinderShape = new CylinderShape(halfExtent);
            cylinderShape.LocalScaling = localScaling;

            return cylinderShape;
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = new CylinderShape(halfExtent);
                ((CylinderShape)collisionShapePtr).LocalScaling = localScaling;
            }

            return collisionShapePtr;
        }
    }
}