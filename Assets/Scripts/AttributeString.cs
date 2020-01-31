using System;
using UnityEngine;


[Serializable]
public class AttributeString: ScriptableObject, IAttribute {
    public string value;

    public AttributeType Type {get => AttributeType.String;}
    public int Int {get => int.MinValue;}
    public float Float {get => float.NaN;}
    public string String {get => value;}
}