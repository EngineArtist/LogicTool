using System;


[Serializable]
public class AttributeFloat: IAttribute {
    public float _value;

    public AttributeType Type {get => AttributeType.Float;}
    public object Value {get => (object)_value; set => _value = (float)value;}
    public int Int {get => (int)_value; set => _value = (float)value;}
    public float Float {get => _value; set => _value = value;}
    public string String {get => null; set {}}
}