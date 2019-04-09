using System;
using System.Collections;

using BulletSharp;

using MathModule;

using UnityEngine;

namespace CollisionModule.Shape
{

    [AddComponentMenu("Physics Bullet/Shapes/Box")]
    public class BoxShape : CollisionShape
    {

        [SerializeField]
        protected Vector3D extents = Vector3D.One();
        public Vector3D Extents
        {
            get { return extents; }
            set
            {
                if (collisionShapePtr != null && value != extents)
                {
                    Debug.LogError("Cannot change the extents after the bullet shape has been created. Extents is only the initial value " +
                        "Use LocalScaling to change the shape of a bullet shape.");
                }
                else
                {
                    extents = value;
                }
            }
        }

        [SerializeField]
        protected Vector3D localScaling = Vector3D.One();
        public Vector3D LocalScaling
        {
            get { return localScaling; }
            set
            {
                localScaling = value;
                if (collisionShapePtr != null)
                {
                    ((BoxShape)collisionShapePtr).LocalScaling = value.ToBullet();
                }
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (drawGizmo == false)
            {
                return;
            }
            Vector3D position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3D scale = localScaling;
            BUtility.DebugDrawBox(position, rotation, scale, extents, Color.yellow);
        }

        public override CollisionShape CopyCollisionShape()
        {
            BoxShape bs = new BoxShape(extents.ToBullet());
            bs.LocalScaling = localScaling.ToBullet();
            return bs;
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = new BoxShape(extents.ToBullet());
                ((BoxShape)collisionShapePtr).LocalScaling = localScaling.ToBullet();
            }
            return collisionShapePtr;
        }
    }
}