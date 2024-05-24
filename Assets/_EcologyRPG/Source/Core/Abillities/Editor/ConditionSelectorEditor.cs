using EcologyRPG.Core.Abilities;
using UnityEditor;
using UnityEngine;

public class ConditionSelectorEditor : EditorWindow
{
    SerializedProperty property;

    public static void Open(SerializedProperty property)
    {
        var editor = GetWindow<ConditionSelectorEditor>(true, "Condition Selector", true);
        editor.property = property;
        editor.Show();
    }

    ConditionReferenceDatabase db;

    private void OnEnable()
    {
        db = ConditionReferenceDatabase.LoadConditions();
    }

    private void OnGUI()
    {
        var data = db.conditions;

        if (data == null)
        {
            EditorGUILayout.HelpBox("No conditions found", MessageType.Error);
            return;
        }

        for (int i = 0; i < data.Length; i++)
        {
            if (GUILayout.Button(data[i].name))
            {
                property.stringValue = data[i].ID.ToString();
                property.serializedObject.ApplyModifiedProperties();
                Close();
            }
        }
    }
}