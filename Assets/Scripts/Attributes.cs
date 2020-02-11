using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;


public enum AttributeType {
    None,
    Int,
    Float,
    String,
}


public static class AttributeTypeExtension {
    public static Type ToType(this AttributeType t) {
        switch (t) {
            case AttributeType.Int: return typeof(int);
            case AttributeType.Float: return typeof(float);
            case AttributeType.String: return typeof(string);
            default: return null;
        }
    }

    public static AttributeType ToAttributeType(this Type t) {
        if      (t == null) return AttributeType.None;
        else if (t == typeof(int)) return AttributeType.Int;
        else if (t == typeof(float)) return AttributeType.Float;
        else if (t == typeof(string)) return AttributeType.String;
        else return AttributeType.None;
    }
}


[Serializable]
public struct Attribute {
    public string _name;
    public AttributeType _type;
    [SerializeReference] public IAttribute _value;

    public string Name {get => _name; set => _name = value;}
    public AttributeType Type {get => _value.Type;}
    public object Value {get => _value.Value; set => _value.Value = value;}
    public int Int {get => _value.Int; set => _value.Int = value;}
    public float Float {get => _value.Float; set => _value.Float = value;}
    public string String {get => _value.String; set => _value.String = value;}
}


public interface IAttribute {
    AttributeType Type {get;}
    object Value {get; set;}
    int Int {get; set;}
    float Float {get; set;}
    string String {get; set;}
}


public class Attributes: MonoBehaviour {
    public List<Attribute> attributes;

    public IAttribute Get(string name) {
        for (int i = 0; i < attributes.Count; ++i) {
            if (attributes[i]._name == name) {
                return attributes[i]._value;
            }
        }
        return null;
    }

    public T Get<T>(string name) {
        for (int i = 0; i < attributes.Count; ++i) {
            if (attributes[i]._name == name) {
                return (T)attributes[i].Value;
            }
        }
        return default(T);
    }

    public void Set(string name, object value) {
        for (int i = 0; i < attributes.Count; ++i) {
            if (attributes[i]._name == name) {
                attributes[i]._value.Value = value;
            }
        }
    }

    public void Set<T>(string name, T value) {
        Set(name, value);
    }
}

public static class AttributeExtensions {
    public static IAttribute Get(this GameObject gobj, string name) {
        var attr = gobj.GetComponent<Attributes>();
        if (attr == null) return null;
        return attr.Get(name);
    }

    public static T Get<T>(this GameObject gobj, string name) {
        var attr = gobj.GetComponent<Attributes>();
        if (attr == null) return default(T);
        return attr.Get<T>(name);
    }

    public static void Set(this GameObject gobj, string name, object value) {
        var attr = gobj.GetComponent<Attributes>();
        if (attr == null) return;
        attr.Set(name, value);
    }

    public static void Set<T>(this GameObject gobj, string name, T value) {
        var attr = gobj.GetComponent<Attributes>();
        if (attr == null) return;
        attr.Set<T>(name, value);
    }
}


[CustomPropertyDrawer(typeof(Attribute))]
public class AttributeDrawer: PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        var attrType = (AttributeType)property.FindPropertyRelative("_type").enumValueIndex;
        switch (attrType) {
            case AttributeType.Int: {return 56f;}
            case AttributeType.Float: {return 56f;}
            case AttributeType.String: {return 56f;}
            default: {return 40f;}
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        var _name = property.FindPropertyRelative("_name");
        var newName = EditorGUI.TextField(new Rect(position.x, position.y, position.width, 16f), _name.stringValue);
        if (newName != _name.stringValue) {
            _name.stringValue = newName;
        }
        var _type = property.FindPropertyRelative("_type");
        var attrType = (AttributeType)_type.enumValueIndex;
        var selType = (AttributeType)EditorGUI.EnumPopup(new Rect(position.x, position.y + 16f, position.width, 16f), "Type", attrType);
        var _value = property.FindPropertyRelative("_value");
        if (selType != attrType) {
            _type.enumValueIndex = (int)selType;
            switch (selType) {
                case AttributeType.None: {
                    _value.managedReferenceValue = null;
                    break;
                }
                case AttributeType.Int: {
                    _value.managedReferenceValue = new AttributeInt();
                    break;
                }
                case AttributeType.Float: {
                    _value.managedReferenceValue = new AttributeFloat();
                    break;
                }
                case AttributeType.String: {
                    _value.managedReferenceValue = new AttributeString();
                    break;
                }
            }
        }
        _value = property.FindPropertyRelative("_value");
        switch (selType) {
            case AttributeType.None: {
                break;
            }
            case AttributeType.Int: {
                EditorGUI.PropertyField(new Rect(position.x, position.y + 32f, position.width, 16f), _value.FindPropertyRelative("_value"));
                break;
            }
            case AttributeType.Float: {
                EditorGUI.PropertyField(new Rect(position.x, position.y + 32f, position.width, 16f), _value.FindPropertyRelative("_value"));
                break;
            }
            case AttributeType.String: {
                EditorGUI.PropertyField(new Rect(position.x, position.y + 32f, position.width, 16f), _value.FindPropertyRelative("_value"));
                break;
            }
        }
        EditorGUI.EndProperty();
    }
}