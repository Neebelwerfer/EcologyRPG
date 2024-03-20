using Character.Abilities;
using Character.Abilities.AbilityEffects;
using Codice.Client.Commands;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LoppedProjectile : AttackAbility
{
    [Header("Lopped Projectile")]
    [Tooltip("The prefab of the projectile")]
    public GameObject ProjectilePrefab;
    [Tooltip("The layer mask of the colliders the projectile can travel through")]
    public LayerMask ignoreMask;
    [Tooltip("The angle of the projectile")]
    public float Angle;
    [Tooltip("The travel time of the projectile")]
    public float TravelTime;
    [Tooltip("The ability that will be cast when the projectile hits the ground")]
    public List<AbilityEffect> OnHitEffects = new();

    public override void Cast(CastInfo caster)
    {
        Debug.DrawRay(caster.mousePoint, Vector3.up * 1, Color.red, 5);
        ProjectileUtility.CreateCurvedProjectile(ProjectilePrefab, caster.mousePoint, TravelTime, -Angle, ignoreMask, caster.owner, (projectileObject) =>
        {
            var newInfo = new CastInfo { owner = caster.owner, castPos = projectileObject.transform.position, mousePoint = caster.mousePoint };
            Debug.DrawRay(projectileObject.transform.position, Vector3.up * 1, Color.green, 5);
            foreach (var effect in OnHitEffects)
            {
                effect.ApplyEffect(newInfo, null);
            }
        });
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(LoppedProjectile))]
public class LoppedProjectileEditor : AttackAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LoppedProjectile ability = (LoppedProjectile)target;
        ability.ProjectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.ProjectilePrefab, typeof(GameObject), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreMask"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Angle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TravelTime"));

        AbilityEffectEditor.Display("On hit effects", ability.OnHitEffects, ability, DisplayEffectType.All);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif