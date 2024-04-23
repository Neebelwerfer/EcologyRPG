#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class AbilitySelectorEditor : EditorWindow
{
    SerializedProperty property;
    bool[] foldouts;

    bool reflectionMode = false;

    FieldInfo field;
    object target;

    public static void Open(SerializedProperty property)
    {
        var editor = GetWindow<AbilitySelectorEditor>(true, "Ability Selector", true);
        editor.property = property;
        editor.reflectionMode = false;
        editor.Show();
    }

    public static void Open(FieldInfo field, object target)
    {
        var editor = GetWindow<AbilitySelectorEditor>(true, "Ability Selector", true);
        editor.field = field;
        editor.target = target;
        editor.reflectionMode = true;
    }

    private void OnGUI()
    {
        if(AbilityEditor.abilityData == null)
        {
            AbilityEditor.Load();
        }
        var data = AbilityEditor.abilityData.data;

        if (data == null) return;
        if(foldouts == null || foldouts.Length != data.Length)
        {
            foldouts = new bool[data.Length];
        }

        for(int i = 0; i < data.Length; i++)
        {
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], data[i].abilityName);
            if (foldouts[i])
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Description", data[i].Description);
                EditorGUILayout.LabelField("Category", data[i].Category.ToString());
                if (GUILayout.Button("Select"))
                {
                    if (reflectionMode)
                    {
#pragma warning disable UNT0018 // System.Reflection features in performance critical messages
                        field.SetValue(target, data[i].ID);
#pragma warning restore UNT0018 // System.Reflection features in performance critical messages
                    }
                    else
                    {
                        property.intValue = data[i].ID;
                    }
                    Close();
                }
                EditorGUI.indentLevel--;
            }
            
        }
    }
}
#endif