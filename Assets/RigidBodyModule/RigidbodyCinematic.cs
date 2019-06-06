using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathModule;

public class RigidbodyCinematic : MonoBehaviour
{
    rb_Collider col;

    [SerializeField]
    float mass = 0.1f;
    [SerializeField]
    float drag = 0.0f;
    [SerializeField]
    bool useGravity = false;
    [SerializeField]
    MathModule.Vector3 gravity = -MathModule.Vector3.Up * 9.81f;

    [SerializeField]
    bool isKinematic = false;

    [Header("Freeze Position")]
    [SerializeField]
    bool pX = false;
    [SerializeField]
    bool pY = false;
    [SerializeField]
    bool pZ = false;

    MathModule.Vector3 velocity = MathModule.Vector3.Zero;
    MathModule.Vector3 position = MathModule.Vector3.Zero;
    UnityEngine.Vector3 pos = UnityEngine.Vector3.zero;

    private void Start()
    {
        col = this.GetComponent<rb_Collider>();
        position.X = this.transform.position.x;
    }
    private void Update()
    {
        velocity += gravity * Time.fixedDeltaTime * (useGravity ? 1 : 0);
        
        if (!isKinematic)
        {
            velocity *= (1 - (drag * Time.fixedDeltaTime));
        }
        velocity = (MathModule.Vector3.Right * velocity.X * (pX ? 0 : 1)) + (MathModule.Vector3.Up * velocity.Y * (pY ? 0 : 1)) + (MathModule.Vector3.Forward * velocity.Z * (pZ ? 0 : 1));

        if (CheckCollision(MathModule.Vector3.Up * velocity.Y * Time.deltaTime) != null )
        {
            if (col.GetPhysicsMaterial() != null)
            {
                velocity = col.GetPhysicsMaterial().CalculateFriction(velocity, MathModule.Vector3.Zero, MathModule.Vector3.Magnitude(gravity) * mass, mass);
            }
            if (CheckCollision(MathModule.Vector3.Up * velocity.Y * Time.deltaTime).GetCenter().Y + CheckCollision(MathModule.Vector3.Up * velocity.Y * Time.deltaTime).GetSize().Y <= col.GetCenter().Y)
            {
                velocity.Y = 0;
            }
        }
        position += velocity * Time.deltaTime;
        pos.x = position.X;
        pos.y = position.Y;
        pos.z = position.Z;
        this.transform.position = pos;
    }

    public void AddForce(MathModule.Vector3 force)
    {
        if (!isKinematic)
        {
            velocity += ((force / mass) + gravity*(useGravity?1:0)) * Time.fixedDeltaTime;
        }
    }

    public void SetVelocity(MathModule.Vector3 vel)
    {
        velocity = vel;
    }

    public rb_Collider CheckCollision(MathModule.Vector3 pos)
    {
        if(col != null)
        {
            MathModule.Vector3 aux = col.center;
            col.center = pos;
            for (int i = 0; i < rb_Collider.rb_Colliders.Count; i++)
            {
                if (rb_Collider.CheckCollision(col, rb_Collider.rb_Colliders[i]) != null)
                {
                    return rb_Collider.CheckCollision(col, rb_Collider.rb_Colliders[i]);
                }
            }
            col.center = aux;
        }

        return null;
    }

}
