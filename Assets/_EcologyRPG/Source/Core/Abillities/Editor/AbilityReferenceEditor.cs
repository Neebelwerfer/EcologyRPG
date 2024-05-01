using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityReference), false)]
public class AbilityReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawAbilityReferenceValues();  
        DrawCanActivateString();
        serializedObject.ApplyModifiedProperties();
    }

    public void DrawCanActivateString()
    {
        var activateString = serializedObject.FindProperty("customActivationTest");
        EditorGUILayout.PropertyField(activateString);
        if(activateString.boolValue == false)
        {
            return;
        }
        var canActivateString = serializedObject.FindProperty("CanActivateString");
        EditorGUILayout.PropertyField(canActivateString);
    }

    public void DrawAbilityReferenceValues()
    {
        var abilityID = serializedObject.FindProperty("AbilityID");
        var cooldown = serializedObject.FindProperty("Cooldown");

        EditorGUILayout.PropertyField(abilityID);
        EditorGUILayout.PropertyField(cooldown);
    }

    public void DrawGlobalOverrides()
    {
        var abilityReference = (AbilityReference)target;
        serializedObject.Update();

        if (AbilityEditor.abilityData == null)
        {
            AbilityEditor.Load();
        }

        var abilityData = Array.Find(AbilityEditor.abilityData.data, element => element.ID == abilityReference.AbilityID);

        if(abilityData == null)
        {
            EditorGUILayout.HelpBox("AbilityData not found for ID: " + abilityReference.AbilityID, MessageType.Error);
            return;
        }

        if (abilityReference.globalVariablesOverride == null || abilityReference.globalVariablesOverride.Length == 0)
        {
            if(GUILayout.Button("Load Default Globals"))
            {
                if (AbilityEditor.abilityData == null)
                {
                    AbilityEditor.Load();
                }
                abilityReference.globalVariablesOverride =  new GlobalVariable[abilityData._DefaultGlobalVariables.Length];
                for (int i = 0; i < abilityData._DefaultGlobalVariables.Length; i++)
                {
                    abilityReference.globalVariablesOverride[i] = abilityData._DefaultGlobalVariables[i].Clone();
                }
                EditorUtility.SetDirty(abilityReference);
            }
            return;
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Ability Settings overrides", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name", GUILayout.Width(200));
        EditorGUILayout.LabelField("Type", GUILayout.Width(200));
        EditorGUILayout.LabelField("Value", GUILayout.Width(200));
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        var array = serializedObject.FindProperty("globalVariablesOverride");
        for (int i = 0; i < array.arraySize; i++)
        { 
            var global = array.GetArrayElementAtIndex(i);
            var name = global.FindPropertyRelative("Name");
            var type = global.FindPropertyRelative("Type");
            var value = global.FindPropertyRelative("Value");
            var enumNames = type.enumNames;
            var enumName = enumNames[type.enumValueIndex];
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name.stringValue, GUILayout.Width(200));
            EditorGUILayout.LabelField(enumName, GUILayout.Width(200));
            GlobalVariableDrawer.DrawValueEditor(type, value);
            GUILayout.EndHorizontal();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(abilityReference);
                serializedObject.ApplyModifiedProperties();
            }
        }

        if (GUILayout.Button("Regenerate Settings"))
        {
            foreach (var global in abilityData._DefaultGlobalVariables)
            {
                if(!Array.Exists(abilityReference.globalVariablesOverride, element => element.Name == global.Name))
                {
                    Array.Resize(ref abilityReference.globalVariablesOverride, abilityReference.globalVariablesOverride.Length + 1);
                    abilityReference.globalVariablesOverride[^1] = global.Clone();
                    EditorUtility.SetDirty(abilityReference);
                }
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Changes"))
        {
            EditorUtility.SetDirty(abilityReference);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        if(GUILayout.Button("Clear Setting overrides"))
        {
            abilityReference.globalVariablesOverride = new GlobalVariable[0];
            EditorUtility.SetDirty(abilityReference);
        }
        GUILayout.EndHorizontal();
    }
}