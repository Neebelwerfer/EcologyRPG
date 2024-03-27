using EcologyRPG.Game.Abilities.Implementations;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(BasicProjectile))]
public class BasicProjectileEditor : ProjectileAbilityEditor
{
    public override void OnInspectorGUI()
    {
        BasicProjectile ability = (BasicProjectile)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Speed"));
        var prefab = serializedObject.FindProperty("ProjectilePrefab");
        prefab.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.ProjectilePrefab, typeof(GameObject), false);
        base.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif