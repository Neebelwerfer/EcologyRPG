using EcologyRPG.Core.Abilities;
using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbilityAttribute))]
public class AbilityAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(AbilityEditor.abilityData == null)
        {
            AbilityEditor.Load();
        }
        var index = property.uintValue;

        var data = Array.Find(AbilityEditor.abilityData.data, x => x.ID == index);
        GUI.Label(new Rect(position.x, position.y, position.width / 2, position.height), "Ability:");

        if (data != null)
        {
            GUI.Label(new Rect(position.x + position.width/4, position.y, position.width/2, position.height), data.abilityName);
        }
        else
        {
            GUI.Label(new Rect(position.x + position.width / 4, position.y, position.width/2, position.height), "No Ability Selected");
        }

        if (GUI.Button(new Rect(position.x + position.width/2, position.y, position.width/2, position.height), "Select", EditorStyles.miniButton))
        {
            AbilitySelectorEditor.Open(property);
        }

    }
}


public class AbilitySelectorEditor : EditorWindow
{
    SerializedProperty property;
    bool[] foldouts;

    public static void Open(SerializedProperty property)
    {
        var editor = GetWindow<AbilitySelectorEditor>(true, "Ability Selector", true);
        editor.property = property;
        editor.Show();
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
                    property.uintValue = data[i].ID;
                    Close();
                }
                EditorGUI.indentLevel--;
            }
            
        }
    }
}