using UnityEngine;
public sealed class GetSet : PropertyAttribute
{
    public readonly string Name;
    public bool Dirty;

    public GetSet(string name)
    {
        this.Name = name;
    }
}
