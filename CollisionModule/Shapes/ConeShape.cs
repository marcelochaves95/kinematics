using UnityEngine;

namespace CollisionModule.Shapes
{
    [AddComponentMenu("Physics Engine/Shapes/Cone")]
    public class ConeShape : BCollisionShape
    {
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

        [SerializeField] protected float height = 2f;
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
        protected Vector3 localScaling = Vector3.one;
        public Vector3 LocalScaling
        {
            get { return localScaling; }
            set
            {
                localScaling = value;
                if (collisionShapePtr != null)
                    ((ConeShape)collisionShapePtr).LocalScaling = value;
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (drawGizmo == false)
                return;

            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = localScaling;
            Utility.DebugDrawCone(position, rotation, scale, radius, height, 1, Color.yellow);
        }

        public override CollisionShape CopyCollisionShape()
        {
            ConeShape cs = new ConeShape(radius, height);
            cs.LocalScaling = localScaling;
            return cs;
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = new ConeShape(radius, height);
                ((ConeShape)collisionShapePtr).LocalScaling = localScaling;
            }

            return collisionShapePtr;
        }
    }
}