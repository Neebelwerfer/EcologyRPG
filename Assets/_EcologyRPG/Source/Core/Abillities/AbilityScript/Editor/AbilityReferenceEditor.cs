using EcologyRPG.AbilityScripting;
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityReference), false)]
public class AbilityReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawAbilityReferenceValues();  
        serializedObject.ApplyModifiedProperties();
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
                abilityReference.globalVariablesOverride = abilityData._DefaultGlobalVariables;
                EditorUtility.SetDirty(abilityReference);
            }
            return;
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Global Variable Overrides", EditorStyles.boldLabel);
        for (int i = 0; i < abilityReference.globalVariablesOverride.Length; i++)
        {
            GUILayout.BeginHorizontal();
            var global = abilityReference.globalVariablesOverride[i];
            EditorGUILayout.LabelField(global.Name);
            if(global.Type == GlobalVariableType.Float)
            {
                global.Value = EditorGUILayout.FloatField(global.GetFloat()).ToString();
            }
            else if (global.Type == GlobalVariableType.Int)
            {
                global.Value = EditorGUILayout.IntField(global.GetInt()).ToString();
            }
            else if (global.Type == GlobalVariableType.String)
            {
                global.Value = EditorGUILayout.TextField(global.Value);
            }
            else if (global.Type == GlobalVariableType.Bool)
            {
                global.Value = EditorGUILayout.Toggle(global.GetBool()).ToString();
            }
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Regenerate global variables"))
        {
            foreach (var global in abilityData._DefaultGlobalVariables)
            {
                if(!Array.Exists(abilityReference.globalVariablesOverride, element => element.Name == global.Name))
                {
                    Array.Resize(ref abilityReference.globalVariablesOverride, abilityReference.globalVariablesOverride.Length + 1);
                    abilityReference.globalVariablesOverride[^1] = global;
                }
            }
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(abilityReference);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        if(GUILayout.Button("Clear global variable overrides"))
        {
            abilityReference.globalVariablesOverride = new GlobalVariable[0];
            EditorUtility.SetDirty(abilityReference);
        }
        GUILayout.EndHorizontal();
    }
}