using System;
using UnityEngine;


[Serializable]
public class AttributeFloat: ScriptableObject, IAttribute {
    public float value;

    public AttributeType Type {get => AttributeType.Float;}
    public int Int {get => (int)value;}
    public float Float {get => value;}
    public string String {get => null;}
}