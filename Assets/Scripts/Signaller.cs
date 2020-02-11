using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[Serializable]
public struct SignalOutput {
    public GameObject target;
    public string slot;
    public IAttribute param;
}


[Serializable]
public struct SignalInput {
    public string name;
    public MethodBind methodBind;
}


[Serializable]
public class MethodBind {
    public string componentName;
    public string methodName;
    public string[] paramTypeNames;
}


public class Signaller: MonoBehaviour {
    public List<SignalOutput> outputs;
    public List<SignalInput> inputs;

    public void TestInput(IAttribute attr) {}
}


public static class SignallerExtension {
}


[CustomPropertyDrawer(typeof(SignalOutput))]
public class SignalOutputDrawer: PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return 48f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.ObjectField(new Rect(position.x, position.y, position.width, 16f), property.FindPropertyRelative("target"), new GUIContent("Target"));
        EditorGUI.EndProperty();
    }
}