using System;
using UnityEngine;
using Object = UnityEngine.Object;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public sealed class UnityAttribute : PropertyAttribute
{
    public string label;
    public GUIStyle labelStyle;
    public float width;
 
    public UnityAttribute(string label)
    {
        this.label = label;
        labelStyle = GUI.skin.GetStyle("miniLabel");
        width = labelStyle.CalcSize(new GUIContent(label)).x;
    }
}