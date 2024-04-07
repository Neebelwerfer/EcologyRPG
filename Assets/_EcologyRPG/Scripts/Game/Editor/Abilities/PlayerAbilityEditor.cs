using EcologyRPG.Core.Abilities.AbilityData;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerAbilityDefinition))]
public class PlayerAbilityEditor : AttackAbilityDefinitionEditor
{
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
        serializedObject.ApplyModifiedProperties();
    }
}
#endif