using EcologyRPG.Game.Abilities.Implementations;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ExpandingProjectile))]
public class ExpandingProjectileEditor : ProjectileAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ExpandingProjectile ability = (ExpandingProjectile)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("GrowthRate"));
        var prefab = serializedObject.FindProperty("ProjectilePrefab");
        prefab.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.ProjectilePrefab, typeof(GameObject), false);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif