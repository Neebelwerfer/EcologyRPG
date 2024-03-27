using EcologyRPG.Game.Abilities.Implementations;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(LoppedProjectile))]
public class LoppedProjectileEditor : AttackAbilityEditor
{
    public override void OnInspectorGUI()
    {
        LoppedProjectile ability = (LoppedProjectile)target;
        ability.displayFirstHitEffects = false;
        base.OnInspectorGUI();

        ability.ProjectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.ProjectilePrefab, typeof(GameObject), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreMask"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Angle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TravelTime"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif