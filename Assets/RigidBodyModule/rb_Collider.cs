using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class rb_Collider : MonoBehaviour
{
    public static List<rb_Collider> rb_Colliders = new List<rb_Collider>();
    public MathModule.Vector3 center;
    public rb_PhysicsMaterial material;

    protected MathModule.Vector3 _center;
    protected int type;

    MathModule.Vector3 position;

    protected virtual void Start()
    {
        position = new MathModule.Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        _center = position + center;
    }
    protected virtual void FixedUpdate()
    {
        position.X = this.transform.position.x;
        position.Y = this.transform.position.y;
        position.Z = this.transform.position.z;
        _center = position + center;
    }

    public virtual int GetColType()
    {
        return type;
    }

    public static rb_Collider CheckCollision(rb_Collider actual, rb_Collider other)
    {
        if(actual != other)
        {
            if (actual.GetColType() == 0 && other.GetColType() == 0)
            {
                if (    MathModule.Vector3.Magnitude(actual._center - other._center) <= (actual.GetComponent<rb_SphereCollider>().GetRadius() + other.GetComponent<rb_SphereCollider>().GetRadius()))
                {
                    return other;
                }
            } else if(actual.GetColType() == 0 && other.GetColType() == 1)
            {
                MathModule.Vector3 boxHalf = other.GetComponent<rb_BoxCollider>().GetSize() / 2.0f;
                MathModule.Vector3 dist = actual.GetComponent<rb_SphereCollider>().GetCenter() - other.GetComponent<rb_BoxCollider>().GetCenter();
                MathModule.Vector3 cut = Cut(dist, -boxHalf, boxHalf);
                MathModule.Vector3 next = other.GetComponent<rb_BoxCollider>().GetCenter() + cut;
                MathModule.Vector3 dif = next - actual.GetComponent<rb_SphereCollider>().GetCenter();
                if (MathModule.Vector3.Magnitude(dif) <= actual.GetComponent<rb_SphereCollider>().GetRadius())
                {
                    return other;
                }

            } else if(actual.GetColType() == 1 && other.GetColType() == 0)
            {
                MathModule.Vector3 boxHalf = actual.GetComponent<rb_BoxCollider>().GetSize() / 2.0f;
                MathModule.Vector3 dist = other.GetComponent<rb_SphereCollider>().GetCenter() - actual.GetComponent<rb_BoxCollider>().GetCenter();
                MathModule.Vector3 cut = Cut(dist, -boxHalf, boxHalf);
                MathModule.Vector3 next = actual.GetComponent<rb_BoxCollider>().GetCenter() + cut;
                MathModule.Vector3 dif = next - other.GetComponent<rb_SphereCollider>().GetCenter();
                if (MathModule.Vector3.Magnitude(dif) <= other.GetComponent<rb_SphereCollider>().GetRadius())
                {
                    return other;
                }
            }
            else
            {
                MathModule.Vector3 actualMin = actual.GetComponent<rb_BoxCollider>().GetMin();
                MathModule.Vector3 actualMax = actual.GetComponent<rb_BoxCollider>().GetMax();
                MathModule.Vector3 otherMin = other.GetComponent<rb_BoxCollider>().GetMin();
                MathModule.Vector3 otherMax = other.GetComponent<rb_BoxCollider>().GetMax();

                if (actualMin.X >= otherMin.X && actualMin.Y >= otherMin.Y && actualMin.Z >= otherMin.Z && actualMin.X <= otherMax.X && actualMin.Y <= otherMax.Y && actualMin.Z <= otherMax.Z)
                {
                    Debug.Log("Colidiu ! Caixa Caixa" + actual.gameObject.name + " " + other.gameObject.name);
                    return other;
                }
                else if ( actualMax.X >= otherMin.X && actualMax.Y >= otherMin.Y && actualMax.Z >= otherMax.Z && actualMax.X <= otherMax.X && actualMax.Y <= otherMax.Y && actualMax.Z <= otherMax.Z)
                {
                    Debug.Log("Colidiu ! Caixa Caixa" + actual.gameObject.name + " " + other.gameObject.name);
                    return other;
                }
            }
        }
        return null;
    }

    public virtual MathModule.Vector3 GetCenter()
    {
        return _center;
    }

    public static MathModule.Vector3 Cut(MathModule.Vector3 d, MathModule.Vector3 min, MathModule.Vector3 max) {

        float x,y,z;
        x = d.X;
        y = d.Y;
        z = d.Z;
        if (d.X > max.X)
            x = max.X;
        else if(d.X < min.X)
            x = min.X;
        if (d.Y > max.Y)
            y = max.Y;
        else if(d.Y < min.Y)
            y = min.Y;
        if (d.Z > max.Z)
            z = max.Z;
        else if(d.Z < min.Z)
            z = min.Z;

        return new MathModule.Vector3(x, y, z);
    }

    public virtual MathModule.Vector3 GetSize()
    {
        MathModule.Vector3 localscale = MathModule.Vector3.Zero;
        localscale.X = this.transform.localScale.x;
        localscale.Y = this.transform.localScale.y;
        localscale.Z = this.transform.localScale.z;

        return localscale;
    }

    public rb_PhysicsMaterial GetPhysicsMaterial()
    {
        return material;
    }
}
