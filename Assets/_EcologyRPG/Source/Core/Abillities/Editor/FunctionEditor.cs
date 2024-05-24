using MoonSharp.Interpreter;
using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class FunctionEditor : EditorWindow
{
    SerializedProperty property;
    Vector2 scrollPos;

    public static void Open(SerializedProperty property)
    {
        var window = GetWindow<FunctionEditor>();
        window.property = property;
        window.Show();
    }

    private void OnGUI()
    {
        if (property == null)
        {
            Close();
            return;
        }


        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        property.stringValue =  WithoutSelectAll(() => EditorGUILayout.TextArea(property.stringValue, new GUILayoutOption[] { GUILayout.Height(600), GUILayout.ExpandHeight(true) }));
        EditorGUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Save"))
        {
            property.serializedObject.ApplyModifiedProperties();
        }
        if(GUILayout.Button("Close"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }

    private T WithoutSelectAll<T>(Func<T> guiCall)
    {
        bool preventSelection = (Event.current.type == EventType.MouseDown);

        Color oldCursorColor = GUI.skin.settings.cursorColor;

        if (preventSelection)
            GUI.skin.settings.cursorColor = new Color(0, 0, 0, 0);

        T value = guiCall();

        if (preventSelection)
            GUI.skin.settings.cursorColor = oldCursorColor;

        return value;
    }
}