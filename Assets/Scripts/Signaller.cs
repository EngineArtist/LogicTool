using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;


[Serializable]
public struct SignalOutput {
    public string name;
    public GameObject target;
    public string input;
    [SerializeReference] public IAttribute[] args;

    public void SendSignal() {
        target.ReceiveSignal(name, args);
    }
}


[Serializable]
public struct SignalInput {
    public string name;
    public MethodBind methodBind;

    public void ReceiveSignal(GameObject target, IAttribute[] args) {
        methodBind.Call(target, name, args);
    }
}


[Serializable]
public class MethodBind {
    public string componentName;
    public string methodName;
    public string[] argTypeNames;

    private MethodInfo _methodInfo;
    private Component _component;

    public void Call(GameObject target, string signalName, IAttribute[] args) {
        if (_methodInfo != null) {
            _methodInfo.Invoke(_component, args);
            return;
        }
        _component = target.GetComponent(componentName);
        if (_component == null) {
            Debug.LogWarning("Cannot process signal '" + signalName + "': GameObject '" + target.name + "' doesn't have Component '" + componentName + "'");
            return;
        }
        var compType = _component.GetType();
        Type[] argTypes = new Type[argTypeNames.Length];
        for (int i = 0; i < argTypes.Length; ++i) {
            argTypes[i] = Type.GetType(argTypeNames[i], false, false);
        }
        _methodInfo = compType.GetMethod(methodName, argTypes);
        if (_methodInfo == null) {
            Debug.LogWarning("Cannot process signal '" + signalName + "': GameObject '" + target.name + "' with Component '" + componentName + "' doesn't have method '" + methodName + "' with the required arguments");
            return;
        }
        _methodInfo.Invoke(_component, args);
    }
}


public class Signaller: MonoBehaviour {
    public List<SignalOutput> outputs;
    public List<SignalInput> inputs;

    public void TestInput(IAttribute attr) {}
}


public static class SignallerExtension {
    public static void SendSignal(this GameObject gobj, string name) {
        var sign = gobj.GetComponent<Signaller>();
        if (sign != null) {
            for (int i = 0; i < sign.outputs.Count; ++i) {
                if (sign.outputs[i].name == name) {
                    sign.outputs[i].SendSignal();
                }
            }
        }
    }

    public static void ReceiveSignal(this GameObject gobj, string name, IAttribute[] args) {
        var sign = gobj.GetComponent<Signaller>();
        if (sign != null) {
            for (int i = 0; i < sign.inputs.Count; ++i) {
                if (sign.inputs[i].name == name) {
                    sign.inputs[i].methodBind.Call(gobj, name, args);
                }
            }
        }
    }
}


[CustomPropertyDrawer(typeof(SignalOutput))]
public class SignalOutputDrawer: PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return property.isExpanded ? 70: 16f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        var name = property.FindPropertyRelative("name");
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, 16f), property.isExpanded, name.stringValue, true, EditorStyles.foldout);
        if (property.isExpanded) {
            var target = property.FindPropertyRelative("target");
            var input = property.FindPropertyRelative("input");
            name.stringValue = EditorGUI.TextField(new Rect(position.x, position.y + 16f, position.width, 16f), name.stringValue);
            EditorGUI.PropertyField(new Rect(position.x, position.y + 32f, position.width, 16f), target);
            var targetGobj = (GameObject)target.objectReferenceValue;
            if (targetGobj != null) {
                var sign = targetGobj.GetComponent<Signaller>();
                if (sign != null && sign.inputs != null && sign.inputs.Count > 0) {
                    string[] inputNames = new string[sign.inputs.Count];
                    for (int i = 0; i < inputNames.Length; ++i) {
                        inputNames[i] = sign.inputs[i].name;
                    }
                    int sel = Array.FindIndex(inputNames, (string s) => s == input.stringValue);
                    if (sel < 0) sel = 0;
                    input.stringValue = inputNames[EditorGUI.Popup(new Rect(position.x, position.y + 48f, position.width, 16f), "Target input", sel, inputNames)];
                }
                else {
                    input.stringValue = "";
                }
            }
        }
        EditorGUI.EndProperty();
    }
}