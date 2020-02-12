using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;


[Serializable]
public struct SignalOutput {
    public GameObject target;
    public string slot;
    [SerializeReference] public IAttribute[] args;
}


[Serializable]
public struct SignalInput {
    public string name;
    public MethodBind methodBind;

    public void ReceiveSignal(GameObject target, IAttribute[] args) {
        methodBind.Call(target, args, name);
    }
}


[Serializable]
public class MethodBind {
    public string componentName;
    public string methodName;
    public string[] argTypeNames;

    private MethodInfo _methodInfo;
    private Component _component;

    public void Call(GameObject target, IAttribute[] args, string signalName) {
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
}


[CustomPropertyDrawer(typeof(SignalOutput))]
public class SignalOutputDrawer: PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return 48f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, 16f), property.FindPropertyRelative("target"), new GUIContent("Target"));
        EditorGUI.EndProperty();
    }
}