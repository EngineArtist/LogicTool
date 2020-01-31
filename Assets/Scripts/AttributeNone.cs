using System;
using UnityEngine;


[Serializable]
public class AttributeNone: ScriptableObject, IAttribute {
    public AttributeType Type {get => AttributeType.None;}
    public int Int {get => int.MinValue;}
    public float Float {get => float.NaN;}
    public string String {get => null;}
}