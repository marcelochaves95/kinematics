using System;
using System.Collections;

using UnityEngine;

namespace CollisionModule.Shape
{
    [AddComponentMenu("Physics Bullet/Shapes/Sphere")]
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

        [SerializeField] protected Vector3D localScaling = Vector3D.one;
        public Vector3D LocalScaling
        {
            get { return localScaling; }
            set
            {
                localScaling = value;
                if (collisionShapePtr != null)
                    ((SphereShape)collisionShapePtr).LocalScaling = value.ToBullet();
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (drawGizmo == false)
                return;

            Vector3D position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3D scale = localScaling;
            BUtility.DebugDrawSphere(position, rotation, scale, Vector3D.one * radius, Color.yellow);
        }

        public override CollisionShape CopyCollisionShape()
        {
            SphereShape ss = new SphereShape(radius);
            ss.LocalScaling = localScaling.ToBullet();
            return ss;
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = new SphereShape(radius);
                ((SphereShape)collisionShapePtr).LocalScaling = localScaling.ToBullet();
            }
            return collisionShapePtr;
        }
    }
}