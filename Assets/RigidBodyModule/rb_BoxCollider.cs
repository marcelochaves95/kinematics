using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rb_BoxCollider : rb_Collider {

    public Vector3 size = Vector3.one;
    MathModule.Vector3 _size;
    MathModule.Vector3 min;
    MathModule.Vector3 max;

    protected override void Start()
    {
        base.Start();
        _size.X = this.transform.localScale.x / 2 * size.x;
        _size.Y = this.transform.localScale.y / 2 * size.y;
        _size.Z = this.transform.localScale.z / 2 * size.z;
        rb_Colliders.Add(this);
        type = 1;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        min = _center - _size;
        max = _center + _size;
    }

    public override MathModule.Vector3 GetSize()
    {
        return _size;
    }

    public MathModule.Vector3 GetMin()
    {
        return min;
    }
    public MathModule.Vector3 GetMax()
    {
        return max;
    }
    public float ScaleMagnitude()
    {
        return (_size.X + _size.Y + _size.Z) /3;
    }
}