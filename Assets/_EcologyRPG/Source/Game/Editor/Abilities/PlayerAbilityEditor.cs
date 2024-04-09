﻿using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.GameSystems.Abilities;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerAbilityDefinition))]
public class PlayerAbilityEditor : AttackAbilityDefinitionEditor
{
    bool showToxicAbility = false;
    SelectableAbilities selectedToxicAbility;
    public override void OnInspectorGUI()
    {
        PlayerAbilityDefinition ability = (PlayerAbilityDefinition)target;
        var icon = serializedObject.FindProperty("Icon");
        icon.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField("Icon", ability.Icon, typeof(Sprite), false);
        EditorGUILayout.LabelField("Resource Cost", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ResourceName"));
        if (ability.ResourceName != "")
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ResourceCost"));
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.Description)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.AnimationTrigger)));
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.BlockMovementOnWindup)));
        if (!ability.BlockMovementOnWindup)
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.ReducedSpeedOnWindup)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.BlockRotationOnWindup)));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.RotatePlayerTowardsMouse)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.UseMouseDirection)));
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        base.OnInspectorGUI();

        if(ability.Ability is not Dodge && ability.Ability is not Sprint)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (ability.ToxicAbility != null)
            {
                GUILayout.BeginHorizontal();
                showToxicAbility = EditorGUILayout.Foldout(showToxicAbility, "Toxic Ability");
                if (GUILayout.Button("X"))
                {
                    ability.ToxicAbility.Delete();
                    ability.ToxicAbility = null;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                GUILayout.EndHorizontal();
                if (showToxicAbility && ability.ToxicAbility != null)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.ToxicResoureCost)));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.ToxicSelfDamage)));
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    var editor = CreateEditor(ability.ToxicAbility);
                    editor.OnInspectorGUI();
                }
            }
            else if (ability.Ability != null)
            {
                if (GUILayout.Button("Copy current Ability"))
                {
                    ability.ToxicAbility = ability.Ability.GetCopy(ability);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                GUILayout.BeginHorizontal();
                selectedToxicAbility = (SelectableAbilities)EditorGUILayout.EnumPopup("Toxic Ability", selectedToxicAbility);
                if (GUILayout.Button("Create new ability"))
                {
                    var newAbility = CreateAbility(selectedToxicAbility);
                    if (newAbility != null)
                    {
                        ability.ToxicAbility = newAbility;
                        AssetDatabase.AddObjectToAsset(ability.ToxicAbility, ability);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
                GUILayout.EndHorizontal();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif