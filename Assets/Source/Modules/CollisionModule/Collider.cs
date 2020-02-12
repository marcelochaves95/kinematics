using System.Collections.Generic;
using Modules.ExtensionModule;
using UnityEngine;

using Vector3 = Kinematics.MathModule.Vector3;
using Collider = Kinematics.CollisionModule.Collider;

using BoxCollider = Kinematics.CollisionModule.BoxCollider;
using SphereCollider = Kinematics.CollisionModule.SphereCollider;

namespace Kinematics.CollisionModule
{
    [RequireComponent(typeof(RigidBody))]
    public abstract class Collider : MonoBehaviour
    {
        protected int type;

        public MathModule.Vector3 center;
        protected MathModule.Vector3 _center;
        private MathModule.Vector3 position;

        public PhysicsMaterial material;

        public static List<Collider> colliders = new List<Collider>();

        protected virtual void Start()
        {
            position = new MathModule.Vector3(transform.position.x, transform.position.y, transform.position.z);
            _center = position + center;
        }

        protected virtual void FixedUpdate()
        {
            position = transform.position.ToPhysics();
            _center = position + center;
        }

        public virtual int GetColliderType() => type;

        public static Collider CheckCollision(Collider actual, Collider other)
        {
            if (actual == other)
                return null;

            if (actual.GetColliderType() == 0 && other.GetColliderType() == 0)
            {
                if (MathModule.Vector3.Magnitude(actual._center - other._center) <= (actual.GetComponent<SphereCollider>().GetRadius() + other.GetComponent<SphereCollider>().GetRadius()))
                    return other;
            }
            else if (actual.GetColliderType() == 0 && other.GetColliderType() == 1)
            {
                var boxHalf = other.GetComponent<BoxCollider>().GetSize() / 2.0f;
                var distance = actual.GetComponent<SphereCollider>().GetCenter() - other.GetComponent<BoxCollider>().GetCenter();
                var cut = Cut(distance, -boxHalf, boxHalf);
                var next = other.GetComponent<BoxCollider>().GetCenter() + cut;
                var difference = next - actual.GetComponent<SphereCollider>().GetCenter();

                if (MathModule.Vector3.Magnitude(difference) <= actual.GetComponent<SphereCollider>().GetRadius())
                    return other;
            }
            else if (actual.GetColliderType() == 1 && other.GetColliderType() == 0)
            {
                var boxHalf = actual.GetComponent<BoxCollider>().GetSize() / 2.0f;
                var distance = other.GetComponent<SphereCollider>().GetCenter() - actual.GetComponent<BoxCollider>().GetCenter();
                var cut = Cut(distance, -boxHalf, boxHalf);
                var next = actual.GetComponent<BoxCollider>().GetCenter() + cut;
                var difference = next - other.GetComponent<SphereCollider>().GetCenter();

                if (MathModule.Vector3.Magnitude(difference) <= other.GetComponent<SphereCollider>().GetRadius())
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
                else if (actualMax.X >= otherMin.X && actualMax.Y >= otherMin.Y && actualMax.Z >= otherMax.Z && actualMax.X <= otherMax.X && actualMax.Y <= otherMax.Y && actualMax.Z <= otherMax.Z)
                {
                    print("Collision of type Box with Box:" + actual.gameObject.name + " " + other.gameObject.name);
                    return other;
                }
            }

            return null;
        }

        public virtual MathModule.Vector3 GetCenter() => _center;

        public static MathModule.Vector3 Cut(MathModule.Vector3 distance, MathModule.Vector3 min, MathModule.Vector3 max)
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

            return new MathModule.Vector3(x, y, z);
        }

        public virtual MathModule.Vector3 GetSize() => transform.localScale.ToPhysics();

        public PhysicsMaterial GetPhysicsMaterial() => material;
    }
}