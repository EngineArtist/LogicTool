using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(GameObject))]
public class GameObjectPropertyDrawer: PropertyDrawer {
    public static Dictionary<string, Texture> icons = new Dictionary<string, Texture>();
    public static bool objectPickerMode = false;
    public static GameObjectPropertyDrawer pickedDrawer = null;
    public static GameObject pickedObject = null;
    public static bool callbackRegistered = false;


    public static Texture GetIcon(string name) {
        Texture result = null;
        if (!icons.TryGetValue(name, out result)) {
            var guids = AssetDatabase.FindAssets(name);
            if (guids.Length > 0) {
                result = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(guids[0]));
                icons[name] = result;
            }
        }
        return result;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (!callbackRegistered) {
            callbackRegistered = true;
            SceneView.duringSceneGui -= UpdateCallback;
            SceneView.duringSceneGui += UpdateCallback;
        }
        if (!objectPickerMode && pickedDrawer == this) {
            property.objectReferenceValue = pickedObject;
            pickedObject = null;
            pickedDrawer = null;
        }
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var fieldRect = new Rect(position.x - 15f, position.y, position.width - 20f, position.height);
        EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
        var pickerRect = new Rect(position.x + position.width - 33f, position.y, position.height, position.height);
        if (GUI.Button(pickerRect, "")) {
            objectPickerMode = !objectPickerMode;
            if (objectPickerMode) pickedDrawer = this;
            else pickedDrawer = null;
        }
        GUI.DrawTexture(pickerRect, GetIcon("object_picker_16"), ScaleMode.ScaleAndCrop, true);
        var deleteRect = new Rect(position.x + position.width - 16f, position.y, position.height, position.height);
        var c = GUI.backgroundColor;
        GUI.backgroundColor = new Color(1f, .4f, .1f);
        if (GUI.Button(deleteRect, "")) {
            objectPickerMode = false;
            pickedDrawer = null;
            property.objectReferenceValue = null;
        }
        GUI.DrawTexture(deleteRect, GetIcon("object_delete_16"), ScaleMode.ScaleAndCrop, true);
        GUI.backgroundColor = c;
        EditorGUI.EndProperty();
    }

    public void UpdateCallback(SceneView view) {
        if (objectPickerMode) {
            var cur = Event.current;
            switch (cur.type) {
                case EventType.Layout: {
                    HandleUtility.AddDefaultControl(0);
                    break;
                }
                case EventType.MouseUp: {
                    pickedObject = HandleUtility.PickGameObject(Event.current.mousePosition, true);
                    objectPickerMode = false;
                    foreach (var c in Selection.activeGameObject.GetComponents<Component>()) {
                        EditorUtility.SetDirty(c);
                    }
                    break;
                }
            }
        }
    }
}

[CustomPropertyDrawer(typeof(Component), true)]
public class ComponentPropertyDrawer: PropertyDrawer{

    public static bool objectPickerMode = false;
    public static ComponentPropertyDrawer pickedDrawer = null;
    public static GameObject pickedObject = null;
    public static bool callbackRegistered = false;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!callbackRegistered) {
            callbackRegistered = true;
            SceneView.duringSceneGui -= UpdateCallback;
            SceneView.duringSceneGui += UpdateCallback;
        }
        if (!objectPickerMode && pickedDrawer == this) {
            property.objectReferenceValue = pickedObject;
            pickedObject = null;
            pickedDrawer = null;
        }
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var fieldRect = new Rect(position.x, position.y, position.width - 37f, position.height);
        EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
        var pickerRect = new Rect(position.x + position.width - 33f, position.y, position.height, position.height);
        if (GUI.Button(pickerRect, "")) {
            objectPickerMode = !objectPickerMode;
            if (objectPickerMode) pickedDrawer = this;
            else pickedDrawer = null;
        }
        GUI.DrawTexture(pickerRect, GameObjectPropertyDrawer.GetIcon("object_picker_16"), ScaleMode.ScaleAndCrop, true);
        var deleteRect = new Rect(position.x + position.width - 16f, position.y, position.height, position.height);
        var c = GUI.backgroundColor;
        GUI.backgroundColor = new Color(1f, .4f, .1f);
        if (GUI.Button(deleteRect, "")) property.objectReferenceValue = null;
        GUI.DrawTexture(deleteRect, GameObjectPropertyDrawer.GetIcon("object_delete_16"), ScaleMode.ScaleAndCrop, true);
        GUI.backgroundColor = c;
        EditorGUI.EndProperty();
    }

    public void UpdateCallback(SceneView view) {
        if (objectPickerMode) {
            var cur = Event.current;
            switch (cur.type) {
                case EventType.Layout: {
                    HandleUtility.AddDefaultControl(0);
                    break;
                }
                case EventType.MouseUp: {
                    pickedObject = HandleUtility.PickGameObject(Event.current.mousePosition, true);
                    objectPickerMode = false;
                    foreach (var c in Selection.activeGameObject.GetComponents<Component>()) {
                        EditorUtility.SetDirty(c);
                    }
                    break;
                }
            }
        }
    }
}