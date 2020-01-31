using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System;


public enum AttributeType {
    None,
    Int,
    Float,
    String,
}


[Serializable]
public struct Attribute: IAttribute {
    public AttributeType type;
    public ScriptableObject value;

    public AttributeType Type {get => type;}
    public int Int {get => ((IAttribute)(object)value).Int;}
    public float Float {get => ((IAttribute)(object)value).Float;}
    public string String {get => ((IAttribute)(object)value).String;}
}


public interface IAttribute {
    AttributeType Type {get;}
    int Int {get;}
    float Float {get;}
    string String {get;}
}


public class Attributes: MonoBehaviour {
    public List<Attribute> attributes;
}


[CustomPropertyDrawer(typeof(Attribute))]
public class AttributeDrawer: PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        switch (property.FindPropertyRelative("type").enumValueIndex) {
            case 0: return 16f;
            default: return 32f;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var type = property.FindPropertyRelative("type");
        var value = property.FindPropertyRelative("value");
        if (value.objectReferenceValue == null) {
            value.objectReferenceValue = ScriptableObject.CreateInstance(typeof(AttributeNone));
            type.enumValueIndex = 0;
        }

        EditorGUI.BeginProperty(position, label, property);
        AttributeType attrType = AttributeType.None;
        Enum.TryParse<AttributeType>(type.enumNames[type.enumValueIndex], out attrType);
        var selType = (AttributeType)EditorGUI.EnumPopup(new Rect(position.x, position.y, position.width, 16f), attrType);
        if (attrType != selType) {
            type.enumValueIndex = (int)selType;
            switch (selType) {
                case AttributeType.None: {
                    value.objectReferenceValue = ScriptableObject.CreateInstance(typeof(AttributeNone));
                    break;
                }
                case AttributeType.Int: {
                    value.objectReferenceValue = ScriptableObject.CreateInstance(typeof(AttributeInt));
                    break;
                }
                case AttributeType.Float: {
                    value.objectReferenceValue = ScriptableObject.CreateInstance(typeof(AttributeFloat));
                    break;
                }
                case AttributeType.String: {
                    value.objectReferenceValue = ScriptableObject.CreateInstance(typeof(AttributeString));
                    break;
                }
            }
        }
        var attrRef = (IAttribute)(object)value.objectReferenceValue;
        switch (attrRef.Type) {
            case AttributeType.None: {
                break;
            }
            case AttributeType.Int: {
                var attr = (AttributeInt)(object)value.objectReferenceValue;
                var i = EditorGUI.IntField(new Rect(position.x, position.y + 16f, position.width, 16f), attr.value);
                if (i != attr.value) {
                    attr.value = i;
                }
                break;
            }
            case AttributeType.Float: {
                var attr = (AttributeFloat)(object)value.objectReferenceValue;
                var f = EditorGUI.FloatField(new Rect(position.x, position.y + 16f, position.width, 16f), attr.value);
                if (f != attr.value) {
                    attr.value = f;
                }
                break;
            }
            case AttributeType.String: {
                var attr = (AttributeString)(object)value.objectReferenceValue;
                var s = EditorGUI.TextField(new Rect(position.x, position.y + 16f, position.width, 16f), attr.value);
                if (s != attr.value) {
                    attr.value = s;
                }
                break;
            }
        }
        EditorGUI.EndProperty();
    }
}