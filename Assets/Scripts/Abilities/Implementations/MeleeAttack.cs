using Character;
using Character.Abilities;
using Character.Abilities.AbilityEffects;
using Codice.Client.Commands;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : WeaponAttack
{
    public float width;
    public List<AbilityEffect> OnHitEffects;
    BaseCharacter[] targets;

    public override void Cast(CastInfo caster)
    {
        var dir = useMouseDirection ? TargetUtility.GetMouseDirection(caster.owner.Position, Camera.main) : caster.owner.Forward;

        if (targetType == TargetType.Cone)
            targets = TargetUtility.GetTargetsInCone(caster.owner.Position, dir, 45, Range, targetMask);
        else if (targetType == TargetType.Line)
            targets = TargetUtility.GetTargetsInLine(caster.owner.Position, dir, new Vector3(width / 2, 2, Range / 2), targetMask);
        else if (targetType == TargetType.Circular)
            targets = TargetUtility.GetTargetsInRadius(caster.owner.Position, Range, targetMask);

        foreach (var target in targets)
        {
            if (target != null && target.Faction != caster.owner.Faction)
            {
                foreach (var effect in OnHitEffects)
                {
                    effect.ApplyEffect(caster, target);
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MeleeAttack))]
public class MeleeAttackEditor : WeaponAttackEditor
{
    private bool foldOut;
    private int index = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeleeAttack ability = (MeleeAttack)target;
        ability.width = EditorGUILayout.FloatField("Width", ability.width);
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