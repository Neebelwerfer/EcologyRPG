using EcologyRPG.Core.Abilities;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(GlobalVariable))]
public class GlobalVariableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var name = property.FindPropertyRelative("Name");
        var type = property.FindPropertyRelative("Type");
        var value = property.FindPropertyRelative("Value");

        GUI.Label(new Rect(position.x, position.y, 40, 20), "Name: ");
        name.stringValue = GUI.TextField(new Rect(position.x + 50, position.y, 200, 20), name.stringValue);
        GUI.Label(new Rect(position.x, position.y + 30, 40, 20), "Type: ");
        EditorGUI.LabelField(new Rect(position.x + 50, position.y + 30, 200, 20), type.enumNames[type.enumValueIndex]);
        
        GUI.Label(new Rect(position.x, position.y + 60, 40, 20), "Value: ");
        DrawValueEditor(new Rect(position.x + 50, position.y + 60, 200, 20), type, value);

        if(GUI.changed)
        {
            property.serializedObject.ApplyModifiedProperties();
        }
        EditorGUI.EndProperty();
    }

    public static void DrawValueEditor(Rect position, SerializedProperty type, SerializedProperty value)
    {
        if (type.enumValueIndex == -1)
        {
            return;
        }

        var enumVal = (GlobalVariableType)type.enumValueIndex;

        if (enumVal == GlobalVariableType.AbilityID)
        {
            DrawIDSelections(enumVal, value);
            return;
        }
        value.stringValue = enumVal switch
        {
            GlobalVariableType.String => GUI.TextField(position, value.stringValue),
            GlobalVariableType.Int => EditorGUI.IntField(position, int.Parse(value.stringValue)).ToString(),
            GlobalVariableType.Float => EditorGUI.FloatField(position, float.Parse(value.stringValue)).ToString(),
            GlobalVariableType.Bool => EditorGUI.Toggle(position, bool.Parse(value.stringValue)).ToString(),
            GlobalVariableType.DamageType => EditorGUI.EnumPopup(position, (Enum)Enum.Parse(typeof(DamageType), value.stringValue)).ToString(),
            _ => GUI.TextField(position, value.stringValue),
        };
    }

    public static void DrawValueEditor(SerializedProperty type, SerializedProperty value)
    {
        if(type.enumValueIndex == -1)
        {
            return;
        }

        var enumVal = (GlobalVariableType)type.enumValueIndex;

        if(enumVal == GlobalVariableType.AbilityID)
        {
            DrawIDSelections(enumVal, value);
            return;
        }

        value.stringValue = enumVal switch
        {
            GlobalVariableType.String => EditorGUILayout.TextField(value.stringValue),
            GlobalVariableType.Int => EditorGUILayout.IntField(int.Parse(value.stringValue)).ToString(),
            GlobalVariableType.Float => EditorGUILayout.FloatField(float.Parse(value.stringValue)).ToString(),
            GlobalVariableType.Bool => EditorGUILayout.Toggle(bool.Parse(value.stringValue)).ToString(),
            GlobalVariableType.DamageType => EditorGUILayout.EnumPopup((Enum)Enum.Parse(typeof(DamageType), value.stringValue)).ToString(),
            _ => EditorGUILayout.TextField(value.stringValue),
        };
    }

    public static void DrawIDSelections(Rect position, GlobalVariableType type, SerializedProperty value)
    {
        if(type == GlobalVariableType.AbilityID)
        {
            var abilityData = AbilityEditor.abilityData;
            if(abilityData == null)
            {
                AbilityEditor.Load();
                abilityData = AbilityEditor.abilityData;
            }

            var index = int.Parse(value.stringValue);
            var ability = Array.Find(abilityData.data, element => element.ID == index);
            var name = ability == null ? "Ability not found" : ability.abilityName;

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width / 2, position.height), name);
            if(GUI.Button(new Rect(position.x + position.width / 2 + 10, position.y, position.width / 2, 20), "Select"))
            {
                AbilitySelectorEditor.Open(value);
            }
        }

    }

    public static void DrawIDSelections(GlobalVariableType type, SerializedProperty value)
    {
        if(type == GlobalVariableType.AbilityID)
        {
            var abilityData = AbilityEditor.abilityData;
            if(abilityData == null)
            {
                AbilityEditor.Load();
                abilityData = AbilityEditor.abilityData;
            }

            var index = int.Parse(value.stringValue);
            var ability = Array.Find(abilityData.data, element => element.ID == index);
            var name = ability == null ? "Ability not found" : ability.abilityName;

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name);
            if(GUILayout.Button("Select"))
            {
                AbilitySelectorEditor.Open(value);
            }
            GUILayout.EndHorizontal();
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 80;
    }
}