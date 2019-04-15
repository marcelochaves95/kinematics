using System;

using UnityEngine;

namespace CollisionModule.Shapes
{
    [System.Serializable]
    public abstract class CollisionShape : MonoBehaviour, IDisposable
    {
        public enum CollisionShapeType
        {
            // dynamic
            BoxShape = 0,
            SphereShape = 1,
            CapsuleShape = 2,
            CylinderShape = 3,
            ConeShape = 4,

            // static
            BvhTriangleMeshShape = 5,
            StaticPlaneShape = 6,
        };

        public CollisionShape collisionShapePtr = null;
        public bool drawGizmo = true;

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