using UnityEngine;

namespace CollisionModule.Shapes
{

    [AddComponentMenu("Physics Engine/Shapes/Box")]
    public class BoxShape : CollisionShape
    {
        [SerializeField] protected Vector3 extents = Vector3D.one;
        public Vector3 Extents
        {
            get { return extents; }
            set
            {
                if (collisionShapePtr != null && value != extents)
                    Debug.LogError("Cannot change the extents after the bullet shape has been created. Extents is only the initial value " +
                        "Use LocalScaling to change the shape of a bullet shape.");
                else
                    extents = value;
            }
        }

        [SerializeField] protected Vector3 localScaling = Vector3.one;
        public Vector3 LocalScaling
        {
            get { return localScaling; }
            set
            {
                localScaling = value;
                if (collisionShapePtr != null)
                    ((BoxShape)collisionShapePtr).LocalScaling = value;
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (!drawGizmo)
                return;

            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = localScaling;
            Utility.DebugDrawBox(position, rotation, scale, extents, Color.yellow);
        }

        public override CollisionShape CopyCollisionShape()
        {
            BoxShape boxShape = new BoxShape();
            boxShape.LocalScaling = localScaling;
            return boxShape;
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = new BoxShape(extents);
                ((BoxShape)collisionShapePtr).LocalScaling = localScaling;
            }
            return collisionShapePtr;
        }
    }
}