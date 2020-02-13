using System.Collections.Generic;
using UnityEngine;
using Vector3 = Kinematics.MathModule.Vector3;

namespace Kinematics.CollisionModule
{
    [RequireComponent(typeof(RigidBody))]
    public abstract class Collider : MonoBehaviour
    {
        protected int type;

        public Vector3 center;
        protected Vector3 _center;
        private Vector3 position;

        public PhysicsMaterial material;

        public static List<Collider> colliders = new List<Collider>();

        protected virtual void Start()
        {
            position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            _center = position + center;
        }

        protected virtual void FixedUpdate()
        {
            position = transform.position;
            _center = position + center;
        }

        public virtual int GetColliderType() => type;

        public static Collider CheckCollision(Collider actual, Collider other)
        {
            if (actual == other)
                return null;

            if (actual.GetColliderType() == 0 && other.GetColliderType() == 0)
            {
                if (Vector3.Magnitude(actual._center - other._center) <= (actual.GetComponent<SphereCollider>().GetRadius() + other.GetComponent<SphereCollider>().GetRadius()))
                    return other;
            }
            else if (actual.GetColliderType() == 0 && other.GetColliderType() == 1)
            {
                var boxHalf = other.GetComponent<BoxCollider>().GetSize() / 2.0f;
                var distance = actual.GetComponent<SphereCollider>().GetCenter() - other.GetComponent<BoxCollider>().GetCenter();
                var cut = Cut(distance, -boxHalf, boxHalf);
                var next = other.GetComponent<BoxCollider>().GetCenter() + cut;
                var difference = next - actual.GetComponent<SphereCollider>().GetCenter();

                if (Vector3.Magnitude(difference) <= actual.GetComponent<SphereCollider>().GetRadius())
                    return other;
            }
            else if (actual.GetColliderType() == 1 && other.GetColliderType() == 0)
            {
                var boxHalf = actual.GetComponent<BoxCollider>().GetSize() / 2.0f;
                var distance = other.GetComponent<SphereCollider>().GetCenter() - actual.GetComponent<BoxCollider>().GetCenter();
                var cut = Cut(distance, -boxHalf, boxHalf);
                var next = actual.GetComponent<BoxCollider>().GetCenter() + cut;
                var difference = next - other.GetComponent<SphereCollider>().GetCenter();

                if (Vector3.Magnitude(difference) <= other.GetComponent<SphereCollider>().GetRadius())
                    return other;
            }
            else
            {
                var actualMin = actual.GetComponent<BoxCollider>().GetMin();
                var actualMax = actual.GetComponent<BoxCollider>().GetMax();
                var otherMin = other.GetComponent<BoxCollider>().GetMin();
                var otherMax = other.GetComponent<BoxCollider>().GetMax();

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

        public virtual Vector3 GetCenter() => _center;

        public static Vector3 Cut(Vector3 distance, Vector3 min, Vector3 max)
        {
            float x, y, z;
            x = distance.X;
            y = distance.Y;
            z = distance.Z;

            if (distance.X > max.X)
                x = max.X;
            else if (distance.X < min.X)
                x = min.X;
            if (distance.Y > max.Y)
                y = max.Y;
            else if (distance.Y < min.Y)
                y = min.Y;
            if (distance.Z > max.Z)
                z = max.Z;
            else if (distance.Z < min.Z)
                z = min.Z;

            return new Vector3(x, y, z);
        }

        public virtual Vector3 GetSize() => transform.localScale;

        public PhysicsMaterial GetPhysicsMaterial() => material;
    }
}
