using EcologyRPG.Core.Abilities;
using EcologyRPG.GameSystems.Abilities;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(MultipleProjectiles))]
public class MultipleProjectilesEditor : ProjectileAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MultipleProjectiles ability = (MultipleProjectiles)target;
        EditorGUILayout.LabelField("Multiple Projectiles Settings", EditorStyles.boldLabel);

        var type = serializedObject.FindProperty("type");
        type.enumValueIndex = (int)(ProjectileType)EditorGUILayout.EnumPopup("Type", ability.type);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numberOfProjectiles"));
        if (ability.type == ProjectileType.Cone)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ConeAngle"));
        }
        else if (ability.type == ProjectileType.Line)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LineWidth"));
        }
        var prefab = serializedObject.FindProperty("projectilePrefab");
        prefab.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.projectilePrefab, typeof(GameObject), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Speed"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif