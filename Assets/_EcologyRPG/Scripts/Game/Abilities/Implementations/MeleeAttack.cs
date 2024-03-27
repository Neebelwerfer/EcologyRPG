using Character;
using Character.Abilities;
using Character.Abilities.AbilityComponents;
using Codice.Client.Commands;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : WeaponAttack
{
    public float width;
    public float angle = 45;
    BaseCharacter[] targets;

    public override void Cast(CastInfo caster)
    {

        var dir = GetDir(caster);

        if (targetType == TargetType.Cone)
            targets = TargetUtility.GetTargetsInCone(caster.castPos, dir, angle, Range, targetMask);
        else if (targetType == TargetType.Line)
            targets = TargetUtility.GetTargetsInLine(caster.castPos, dir, new Vector3(width / 2, 2, Range / 2), targetMask);
        else if (targetType == TargetType.Circular)
            targets = TargetUtility.GetTargetsInRadius(caster.castPos, Range, targetMask);

        foreach (var target in targets)
        {
            if (target != null && target.Faction != caster.owner.Faction)
            {
                DefaultOnHitAction()(caster, target);
            }
        }
    }
}