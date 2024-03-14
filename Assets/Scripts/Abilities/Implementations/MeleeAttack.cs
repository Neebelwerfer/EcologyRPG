using Character;
using Character.Abilities;
using Codice.Client.Commands;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MeleeAttack")]
public class MeleeAttack : WeaponAttack
{
    public float width;
    BaseCharacter[] targets;

    public override void CastStarted(CasterInfo caster)
    {
        var dir = useMouseDirection ? TargetUtility.GetMouseDirection(caster.owner.Position, Camera.main) : caster.owner.Forward;

        if(targetType == TargetType.Cone)
            targets = TargetUtility.GetTargetsInCone(caster.owner.Position, dir, 45, Range, targetMask);
        else if(targetType == TargetType.Line)
            targets = TargetUtility.GetTargetsInLine(caster.owner.Position, dir, new Vector3(width/2, 2, Range/2), targetMask);
        else if(targetType == TargetType.Circular)
            targets = TargetUtility.GetTargetsInRadius(caster.owner.Position, Range, targetMask);
    }

    public override void CastEnded(CasterInfo caster)
    {
        foreach (var target in targets)
        {
            if (target != null && target.Faction != caster.owner.Faction)
            {
                target.ApplyDamage(CalculateDamage(caster.owner, DamageType.Physical, BaseDamage));
            }
        }
    }

    public override void OnHold(CasterInfo caster)
    {
    }
}