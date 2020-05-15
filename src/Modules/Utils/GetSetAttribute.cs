using UnityEngine;

public sealed class GetSetAttribute : PropertyAttribute
{
    public readonly string Name;
    public bool Dirty;

    public GetSetAttribute(string name)
    {
        Name = name;
    }
}
