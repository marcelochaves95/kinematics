using System.Collections.Generic;
using UnityEngine;
using Vector3 = Kinematics.MathModule.Vector3;

namespace Kinematics.Collision
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Collider : MonoBehaviour
    {
        protected int type;

        public Vector3 center;
        protected Vector3 _center;
        private Vector3 position;

        public PhysicsMaterial material;

        public static readonly List<Collider> colliders = new List<Collider>();

        protected virtual void Start()
        {
            UnityEngine.Vector3 positionRef = transform.position;
            position = new Vector3(positionRef.x, positionRef.y, positionRef.z);
            _center = position + center;
        }

        protected virtual void FixedUpdate()
        {
            position = transform.position;
            _center = position + center;
        }

        private int GetColliderType() => type;

        public static Collider CheckCollision(Collider actual, Collider other)
        {
            if (actual == other)
            {
                return null;
            }

            SphereCollider actualSphereComponent = actual.GetComponent<SphereCollider>();
            BoxCollider actualBoxComponent = actual.GetComponent<BoxCollider>();
            SphereCollider otherSphereComponent = other.GetComponent<SphereCollider>();
            BoxCollider otherBoxComponent = actual.GetComponent<BoxCollider>();

            Vector3 boxHalf = otherBoxComponent.GetSize() / 2.0f;
            Vector3 distance = actualSphereComponent.GetCenter() - otherBoxComponent.GetCenter();
            Vector3 cut = Cut(distance, -boxHalf, boxHalf);
            Vector3 next = otherBoxComponent.GetCenter() + cut;
            Vector3 difference = next - actualSphereComponent.GetCenter();

            int getColliderType = other.GetColliderType();

            if (actual.GetColliderType() == 0 && getColliderType== 0)
            {
                if (Vector3.Magnitude(actual._center - other._center) <= actualSphereComponent.GetRadius() + otherSphereComponent.GetRadius())
                {
                    return other;
                }
            }
            else if (actual.GetColliderType() == 0 && getColliderType == 1)
            {
                if (Vector3.Magnitude(difference) <= actualSphereComponent.GetRadius())
                {
                    return other;
                }
            }
            else if (actual.GetColliderType() == 1 && getColliderType == 0)
            {
                if (Vector3.Magnitude(difference) <= otherSphereComponent.GetRadius())
                {
                    return other;
                }
            }
            else
            {
                Vector3 actualMin = actualBoxComponent.GetMin();
                Vector3 actualMax = actualBoxComponent.GetMax();
                Vector3 otherMin = otherBoxComponent.GetMin();
                Vector3 otherMax = otherBoxComponent.GetMax();

                if (actualMin.X >= otherMin.X && actualMin.Y >= otherMin.Y && actualMin.Z >= otherMin.Z && actualMin.X <= otherMax.X && actualMin.Y <= otherMax.Y && actualMin.Z <= otherMax.Z)
                {
                    print("Collision of type Box with Box:" + actual.gameObject.name + " " + other.gameObject.name);
                    return other;
                }

                if (actualMax.X >= otherMin.X && actualMax.Y >= otherMin.Y && actualMax.Z >= otherMax.Z && actualMax.X <= otherMax.X && actualMax.Y <= otherMax.Y && actualMax.Z <= otherMax.Z)
                {
                    print("Collision of type Box with Box:" + actual.gameObject.name + " " + other.gameObject.name);
                    return other;
                }
            }

            return null;
        }

        public Vector3 GetCenter() => _center;

        private static Vector3 Cut(Vector3 distance, Vector3 min, Vector3 max)
        {
            float x = distance.X;
            float y = distance.Y;
            float z = distance.Z;

            if (distance.X > max.X)
            {
                x = max.X;
            }
            else if (distance.X < min.X)
            {
                x = min.X;
            }

            if (distance.Y > max.Y)
            {
                y = max.Y;
            }
            else if (distance.Y < min.Y)
            {
                y = min.Y;
            }

            if (distance.Z > max.Z)
            {
                z = max.Z;
            }
            else if (distance.Z < min.Z)
            {
                z = min.Z;
            }

            return new Vector3(x, y, z);
        }

        public virtual Vector3 GetSize()
        {
            return transform.localScale;
        }

        public virtual PhysicsMaterial GetPhysicsMaterial()
        {
            return material;
        }
    }
}