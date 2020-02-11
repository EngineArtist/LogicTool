using System;


[Serializable]
public class AttributeInt: IAttribute {
    public int _value;

    public AttributeType Type {get => AttributeType.Int;}
    public object Value {get => (object)_value; set => _value = (int)value;}
    public int Int {get => _value; set => _value = value;}
    public float Float {get => (float)_value; set => _value = (int)value;}
    public string String {get => null; set {}}
}