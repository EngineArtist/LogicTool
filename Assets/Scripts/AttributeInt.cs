using System;
using UnityEngine;


[Serializable]
public class AttributeInt: ScriptableObject, IAttribute {
    public int value;

    public AttributeType Type {get => AttributeType.Int;}
    public int Int {get => value;}
    public float Float {get => (float)value;}
    public string String {get => null;}
}