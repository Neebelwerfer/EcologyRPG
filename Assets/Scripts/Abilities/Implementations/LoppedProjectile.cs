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
    public List<AbilityEffect> OnHitEffects = new List<AbilityEffect>();

    public override void Cast(CastInfo caster)
    {
        Debug.DrawRay(caster.mousePoint, Vector3.up * 10, Color.red, 5);
        ProjectileUtility.CreateCurvedProjectile(ProjectilePrefab, caster.mousePoint, TravelTime, Angle, ignoreMask, caster.owner, (projectileObject) =>
        {
            var newInfo = new CastInfo { owner = caster.owner, castPos = projectileObject.transform.position, mousePoint = caster.mousePoint };
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
    private bool foldOut;
    private int index = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LoppedProjectile ability = (LoppedProjectile)target;
        ability.ProjectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", ability.ProjectilePrefab, typeof(GameObject), false);
        if (EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreMask")))
        {
            EditorUtility.SetDirty(ability);
        }
        ability.Angle = EditorGUILayout.FloatField("Angle", ability.Angle);
        ability.TravelTime = EditorGUILayout.FloatField("Travel Time", ability.TravelTime);

        foldOut = EditorGUILayout.BeginFoldoutHeaderGroup(foldOut, "On Hit Effects");
        if (foldOut)
        {
            foreach (var effect in ability.OnHitEffects)
            {
                EditorGUILayout.ObjectField(effect, typeof(AbilityEffect), false);
            }
            if (GUILayout.Button("Add Effect"))
            {
                var window = EditorWindow.GetWindow<AbilityEffectEditor>();
                window.editedEffects = new EditListEffect(ability, ability.OnHitEffects);
                window.Show();
            }

            if (ability.OnHitEffects.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Remove All"))
                {
                    foreach (var effect in ability.OnHitEffects)
                    {
                        DestroyImmediate(effect, true);
                    }
                    ability.OnHitEffects.Clear();
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }

                if (GUILayout.Button("Remove Last"))
                {
                    DestroyImmediate(ability.OnHitEffects[ability.OnHitEffects.Count - 1], true);
                    ability.OnHitEffects.RemoveAt(ability.OnHitEffects.Count - 1);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                var ind = EditorGUILayout.IntField("Index", index);
                if (ind > ability.OnHitEffects.Count)
                {
                    ind = ability.OnHitEffects.Count;
                }
                index = ind;
                if (GUILayout.Button("Remove Index"))
                {
                    DestroyImmediate(ability.OnHitEffects[index], true);
                    ability.OnHitEffects.RemoveAt(index);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

    }
}
#endif