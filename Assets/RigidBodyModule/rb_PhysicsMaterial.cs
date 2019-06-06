using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rb_PhysicsMaterial:MonoBehaviour
{
    [SerializeField]
    float ue = 0.3f;
    [SerializeField]
    float uc = 0.3f;
    MathModule.Vector3 fe;
    MathModule.Vector3 fc;

    public MathModule.Vector3 CalculateFriction(MathModule.Vector3 velocity, MathModule.Vector3 force, float normal, float mass)
    {
        float time = Time.fixedDeltaTime;
        MathModule.Vector3 acceleration;
        fe = ue * -MathModule.Vector3.Normalize(velocity) * normal;
        fc = uc * -MathModule.Vector3.Normalize(velocity) * normal;
        if(MathModule.Vector3.Magnitude(velocity) > 0.0f)
        {
            acceleration = (force + fc / mass);
        }
        else
        {
            acceleration = (force + fe / mass);
        }

        if(((MathModule.Vector3.Magnitude(force) - MathModule.Vector3.Magnitude(fe)) > 0.0f) || ( MathModule.Vector3.Magnitude(velocity) > 0.0f))
        {
            velocity += acceleration * time;
        }
        else
        {
            velocity = MathModule.Vector3.Zero;
        }

        return velocity;
    }
}
