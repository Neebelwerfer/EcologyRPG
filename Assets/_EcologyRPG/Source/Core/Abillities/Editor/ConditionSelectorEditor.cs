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

    private void OnGUI()
    {
        if (ConditionEditor.conditionData == null)
        {
            ConditionEditor.Load();
        }
        var data = ConditionEditor.conditionData.data;

        if (data == null)
        {
            EditorGUILayout.HelpBox("No conditions found", MessageType.Error);
            return;
        }

        for (int i = 0; i < data.Length; i++)
        {
            if (GUILayout.Button(data[i].ConditionName))
            {
                property.intValue = data[i].ID;
                Close();
            }
        }
    }
}