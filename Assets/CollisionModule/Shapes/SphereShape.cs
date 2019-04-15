using UnityEngine;

namespace CollisionModule.Shapes
{
    [AddComponentMenu("Physics Engine/Shapes/Sphere")]
    public class SphereShape : CollisionShape
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

        [SerializeField] protected Vector3 localScaling = Vector3.one;
        public Vector3 LocalScaling
        {
            get { return localScaling; }
            set
            {
                localScaling = value;
                if (collisionShapePtr != null)
                    ((SphereShape)collisionShapePtr).LocalScaling = value;
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (drawGizmo == false)
                return;

            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = localScaling;
            Utility.DebugDrawSphere(position, rotation, scale, Vector3.one * radius, Color.yellow);
        }

        public override CollisionShape CopyCollisionShape()
        {
            SphereShape sphereShape = new SphereShape(radius);
            sphereShape.LocalScaling = localScaling;
            return sphereShape;
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = new SphereShape(radius);
                ((SphereShape)collisionShapePtr).LocalScaling = localScaling;
            }
            
            return collisionShapePtr;
        }
    }
}