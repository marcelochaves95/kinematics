using System.Collections.Generic;

using Vector3 = PhysicsEngine.MathModule.Vector3;
using Collider = PhysicsEngine.CollisionModule.Collider;

using BoxCollider = PhysicsEngine.CollisionModule.BoxCollider;
using SphereCollider = PhysicsEngine.CollisionModule.SphereCollider;

namespace PhysicsEngine.CollisionModule
{
    public abstract class Collider : UnityEngine.MonoBehaviour
    {
        public static List<Collider> colliders = new List<Collider>();
        public Vector3 center;
        public PhysicsMaterial material;

        protected Vector3 _center;
        protected int type;

        private Vector3 position;

        protected virtual void Start()
        {
            position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            _center = position + center;
        }

        protected virtual void FixedUpdate()
        {
            position.X = transform.position.x;
            position.Y = transform.position.y;
            position.Z = transform.position.z;
            _center = position + center;
        }

        public virtual int GetColType() => type;

        public static Collider CheckCollision(Collider actual, Collider other)
        {
            if (actual == other)
                return null;
            
            if (actual.GetColType() == 0 && other.GetColType() == 0)
            {
                if (Vector3.Magnitude(actual._center - other._center) <= (actual.GetComponent<SphereCollider>().GetRadius() + other.GetComponent<SphereCollider>().GetRadius()))
                    return other;
            }
            else if (actual.GetColType() == 0 && other.GetColType() == 1)
            {
                var boxHalf = other.GetComponent<BoxCollider>().GetSize() / 2.0f;
                var distance = actual.GetComponent<SphereCollider>().GetCenter() - other.GetComponent<BoxCollider>().GetCenter();
                var cut = Cut(distance, -boxHalf, boxHalf);
                var next = other.GetComponent<BoxCollider>().GetCenter() + cut;
                var difference = next - actual.GetComponent<SphereCollider>().GetCenter();

                if (Vector3.Magnitude(difference) <= actual.GetComponent<SphereCollider>().GetRadius())
                    return other;
            }
            else if (actual.GetColType() == 1 && other.GetColType() == 0)
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
                else if (actualMax.X >= otherMin.X && actualMax.Y >= otherMin.Y && actualMax.Z >= otherMax.Z && actualMax.X <= otherMax.X && actualMax.Y <= otherMax.Y && actualMax.Z <= otherMax.Z)
                {
                    print("Collision of type Box with Box:" + actual.gameObject.name + " " + other.gameObject.name);
                    return other;
                }
            }

            return null;
        }

        public virtual Vector3 GetCenter() => _center;

        public static Vector3 Cut(Vector3 d, Vector3 min, Vector3 max)
        {
            float x, y, z;
            x = d.X;
            y = d.Y;
            z = d.Z;

            if (d.X > max.X)
                x = max.X;
            else if (d.X < min.X)
                x = min.X;
            if (d.Y > max.Y)
                y = max.Y;
            else if (d.Y < min.Y)
                y = min.Y;
            if (d.Z > max.Z)
                z = max.Z;
            else if (d.Z < min.Z)
                z = min.Z;

            return new Vector3(x, y, z);
        }

        public virtual Vector3 GetSize()
        {
            var localscale = Vector3.Zero;

            localscale.X = this.transform.localScale.x;
            localscale.Y = this.transform.localScale.y;
            localscale.Z = this.transform.localScale.z;

            return localscale;
        }

        public PhysicsMaterial GetPhysicsMaterial() => material;
    }
}
