using EcologyRPG.Core.Abilities;
using MoonSharp.Interpreter;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(GlobalVariable))]
public class GlobalVariableDrawer : PropertyDrawer
{
    const float valueWidth = 400;
    float height = 0;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        height = 0;
        EditorGUI.BeginProperty(position, label, property);
        var name = property.FindPropertyRelative("Name");
        var type = property.FindPropertyRelative("Type");
        var value = property.FindPropertyRelative("Value");

        GUI.Label(new Rect(position.x, position.y, 40, 20), "Name: ");
        name.stringValue = GUI.TextField(new Rect(position.x + 50, position.y, 200, 20), name.stringValue);
        GUI.Label(new Rect(position.x, position.y + 30, 40, 20), "Type: ");
        EditorGUI.LabelField(new Rect(position.x + 50, position.y + 30, 200, 20), type.enumNames[type.enumValueIndex]);
        
        GUI.Label(new Rect(position.x, position.y + 60, 40, 20), "Value: ");
        DrawValueEditor(new Rect(position.x + 50, position.y + 60, 200, 20), type, value, ref height);
        height += 80;

        if(GUI.changed)
        {
            property.serializedObject.ApplyModifiedProperties();
        }
        EditorGUI.EndProperty();
    }

    public static void DrawValueEditor(Rect position, SerializedProperty type, SerializedProperty value, ref float height)
    {
        if (type.enumValueIndex == -1)
        {
            return;
        }

        var enumVal = (GlobalVariableType)type.enumValueIndex;

        if (enumVal is GlobalVariableType.AbilityID or GlobalVariableType.ConditionID)
        {
            DrawIDSelections(position, enumVal, value, ref height);
            return;
        }

        if(enumVal == GlobalVariableType.Function)
        {
            DrawFunction(position, value, ref height);
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

    public static void DrawFunction(Rect position, SerializedProperty value, ref float height)
    {
        height += 20;
        if(GUI.Button(position, "Edit"))
        {
            FunctionEditor.Open(value);
        }
    }



    public static void DrawValueEditor(SerializedProperty type, SerializedProperty value)
    {
        if(type.enumValueIndex == -1)
        {
            return;
        }

        var enumVal = (GlobalVariableType)type.enumValueIndex;

        if (enumVal is GlobalVariableType.AbilityID or GlobalVariableType.ConditionID)
        {
            DrawIDSelections(enumVal, value);
            return;
        }

        if (enumVal == GlobalVariableType.Function)
        {
            if(GUILayout.Button("Edit", GUILayout.Width(valueWidth)))
            {
                FunctionEditor.Open(value);
            }
            return;
        }

        value.stringValue = enumVal switch
        {
            GlobalVariableType.String => EditorGUILayout.TextField(value.stringValue, GUILayout.Width(valueWidth)),
            GlobalVariableType.Int => EditorGUILayout.IntField(int.Parse(value.stringValue), GUILayout.Width(valueWidth)).ToString(),
            GlobalVariableType.Float => EditorGUILayout.FloatField(float.Parse(value.stringValue), GUILayout.Width(valueWidth)).ToString(),
            GlobalVariableType.Bool => EditorGUILayout.Toggle(bool.Parse(value.stringValue), GUILayout.Width(valueWidth)).ToString(),
            GlobalVariableType.DamageType => EditorGUILayout.EnumPopup((Enum)Enum.Parse(typeof(DamageType), value.stringValue), GUILayout.Width(valueWidth)).ToString(),
            _ => EditorGUILayout.TextField(value.stringValue, GUILayout.Width(valueWidth)),
        };
    }

    public static void DrawIDSelections(Rect position, GlobalVariableType type, SerializedProperty value, ref float height)
    {
        if(type is GlobalVariableType.AbilityID)
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

        if(type is GlobalVariableType.ConditionID)
        {
            var db = ConditionReferenceDatabase.LoadConditions();
            var conditionData = db.conditions;

            if(conditionData == null)
            {
                Debug.LogError("Condition data is null");
                return;
            }

            var index = int.Parse(value.stringValue);
            var condition = Array.Find(conditionData, element => element.ID == index);
            var name = condition == null ? "Condition not found" : condition.name;

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width / 2, position.height), name);
            if(GUI.Button(new Rect(position.x + position.width / 2 + 10, position.y, position.width / 2, 20), "Select"))
            {
                ConditionSelectorEditor.Open(value);
            }
        }

        height += position.height;

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
            EditorGUILayout.LabelField(name, GUILayout.Width(valueWidth));
            if(GUILayout.Button("Select"))
            {
                AbilitySelectorEditor.Open(value);
            }
            GUILayout.EndHorizontal();
        }

        if(type is GlobalVariableType.ConditionID)
        {
            var db = ConditionReferenceDatabase.LoadConditions();
            var conditionData = db.conditions;

            if (conditionData == null)
            {
                Debug.LogError("Condition data is null");
                return;
            }

            var index = int.Parse(value.stringValue);
            var condition = Array.Find(conditionData, element => element.ID == index);
            var name = condition == null ? "Condition not found" : condition.name;

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name, GUILayout.MaxWidth(valueWidth));
            if(GUILayout.Button("Select"))
            {
                ConditionSelectorEditor.Open(value);
            }
            GUILayout.EndHorizontal();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return height;
    }
}