using System;
using System.Collections;

using MathModule;

using UnityEngine;

namespace CollisionModule.Shape
{
    [System.Serializable]
    public abstract class BCollisionShape : MonoBehaviour, IDisposable
    {
        public enum CollisionShapeType
        {
            // dynamic
            BoxShape = 0,
            SphereShape = 1,
            CapsuleShape = 2,
            CylinderShape = 3,
            ConeShape = 4,
            ConvexHull = 5,
            CompoundShape = 6,

            // static
            BvhTriangleMeshShape = 7,
            StaticPlaneShape = 8,
        };

        private CollisionShape collisionShapePtr = null;
        protected CollisionShape CollisionShapePtr { get => collisionShapePtr; set => collisionShapePtr = value; }
        private bool drawGizmo = true;
        public bool DrawGizmo { get => drawGizmo; set => drawGizmo = value; }

        private void OnDestroy()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isdisposing)
        {
            if (CollisionShapePtr != null)
            {
                CollisionShapePtr.Dispose();
                CollisionShapePtr = null;
            }
        }

        public abstract void OnDrawGizmosSelected();

        public abstract CollisionShape CopyCollisionShape();

        public abstract CollisionShape GetCollisionShape();
    }
}