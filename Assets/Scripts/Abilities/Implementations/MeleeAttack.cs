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

        var dir = GetDir(caster);

        if (targetType == TargetType.Cone)
            targets = TargetUtility.GetTargetsInCone(caster.castPos, dir, 45, Range, targetMask);
        else if (targetType == TargetType.Line)
            targets = TargetUtility.GetTargetsInLine(caster.castPos, dir, new Vector3(width / 2, 2, Range / 2), targetMask);
        else if (targetType == TargetType.Circular)
            targets = TargetUtility.GetTargetsInRadius(caster.castPos, Range, targetMask);

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

        AbilityEffectEditor.Display(ability.OnHitEffects, ability);

    }
}
#endif