using System.Collections.Generic;

using PhysicsEngine.MathModule;

namespace PhysicsEngine.CollisionModule
{
    public abstract class Collider : UnityEngine.MonoBehaviour
    {
        public static List<Collider> rb_Colliders = new List<Collider>();
        public Vector3 center;
        public PhysicsMaterial material;

        protected Vector3 _center;
        protected int type;

        Vector3 position;

        protected virtual void Start()
        {
            position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            _center = position + center;
        }

        protected virtual void FixedUpdate()
        {
            position.X = this.transform.position.x;
            position.Y = this.transform.position.y;
            position.Z = this.transform.position.z;
            _center = position + center;
        }

        public virtual int GetColType() => type;

        public static Collider CheckCollision(Collider actual, Collider other)
        {
            if (actual != other)
            {
                if (actual.GetColType() == 0 && other.GetColType() == 0)
                {
                    if (Vector3.Magnitude(actual._center - other._center) <= (actual.GetComponent<SphereCollider>().GetRadius() + other.GetComponent<SphereCollider>().GetRadius()))
                        return other;
                }
                else if (actual.GetColType() == 0 && other.GetColType() == 1)
                {
                    Vector3 boxHalf = other.GetComponent<BoxCollider>().GetSize() / 2.0f;
                    Vector3 dist = actual.GetComponent<SphereCollider>().GetCenter() - other.GetComponent<BoxCollider>().GetCenter();
                    Vector3 cut = Cut(dist, -boxHalf, boxHalf);
                    Vector3 next = other.GetComponent<BoxCollider>().GetCenter() + cut;
                    Vector3 dif = next - actual.GetComponent<SphereCollider>().GetCenter();

                    if (Vector3.Magnitude(dif) <= actual.GetComponent<SphereCollider>().GetRadius())
                        return other;
                }
                else if (actual.GetColType() == 1 && other.GetColType() == 0)
                {
                    Vector3 boxHalf = actual.GetComponent<BoxCollider>().GetSize() / 2.0f;
                    Vector3 dist = other.GetComponent<SphereCollider>().GetCenter() - actual.GetComponent<BoxCollider>().GetCenter();
                    Vector3 cut = Cut(dist, -boxHalf, boxHalf);
                    Vector3 next = actual.GetComponent<BoxCollider>().GetCenter() + cut;
                    Vector3 dif = next - other.GetComponent<SphereCollider>().GetCenter();

                    if (Vector3.Magnitude(dif) <= other.GetComponent<SphereCollider>().GetRadius())
                        return other;
                }
                else
                {
                    Vector3 actualMin = actual.GetComponent<BoxCollider>().GetMin();
                    Vector3 actualMax = actual.GetComponent<BoxCollider>().GetMax();
                    Vector3 otherMin = other.GetComponent<BoxCollider>().GetMin();
                    Vector3 otherMax = other.GetComponent<BoxCollider>().GetMax();

                    if (actualMin.X >= otherMin.X && actualMin.Y >= otherMin.Y && actualMin.Z >= otherMin.Z && actualMin.X <= otherMax.X && actualMin.Y <= otherMax.Y && actualMin.Z <= otherMax.Z)
                    {
                        print("Colidiu ! Caixa Caixa" + actual.gameObject.name + " " + other.gameObject.name);
                        return other;
                    }
                    else if (actualMax.X >= otherMin.X && actualMax.Y >= otherMin.Y && actualMax.Z >= otherMax.Z && actualMax.X <= otherMax.X && actualMax.Y <= otherMax.Y && actualMax.Z <= otherMax.Z)
                    {
                        print("Colidiu ! Caixa Caixa" + actual.gameObject.name + " " + other.gameObject.name);
                        return other;
                    }
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
            Vector3 localscale = Vector3.Zero;

            localscale.X = this.transform.localScale.x;
            localscale.Y = this.transform.localScale.y;
            localscale.Z = this.transform.localScale.z;

            return localscale;
        }

        public PhysicsMaterial GetPhysicsMaterial() => material;
    }
}