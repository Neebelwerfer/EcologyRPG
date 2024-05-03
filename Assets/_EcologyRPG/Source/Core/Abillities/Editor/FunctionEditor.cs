using MoonSharp.Interpreter;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class FunctionEditor : EditorWindow
{
    SerializedProperty property;
    public static void Open(SerializedProperty property)
    {
        var window = GetWindow<FunctionEditor>();
        window.property = property;
        window.Show();
    }

    private void OnGUI()
    {
        property.stringValue = EditorGUILayout.TextArea(property.stringValue, GUILayout.Height(400));
        if(GUILayout.Button("Test"))
        {
            TestFunction(property.stringValue);
        }
        property.serializedObject.ApplyModifiedProperties();
    }

    public void TestFunction(string value)
    {
        try
        {
            Script.RunString(value);
        }
        catch (SyntaxErrorException e)
        {
            Debug.LogError(e.Message);
            EditorCoroutineUtility.StartCoroutineOwnerless(ErrorMessage(e.Message));
        }
    }

    IEnumerator ErrorMessage(string message)
    {
        EditorGUILayout.HelpBox(message, MessageType.Error);
        yield return new WaitForSeconds(3);
    }
}