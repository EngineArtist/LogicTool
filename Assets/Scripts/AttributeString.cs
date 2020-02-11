using System;


[Serializable]
public class AttributeString: IAttribute {
    public string _value;

    public AttributeType Type {get => AttributeType.String;}
    public object Value {get => (object)_value; set => _value = (string)value;}
    public int Int {get => int.MinValue; set {}}
    public float Float {get => float.NaN; set {}}
    public string String {get => _value; set => _value = value;}
}