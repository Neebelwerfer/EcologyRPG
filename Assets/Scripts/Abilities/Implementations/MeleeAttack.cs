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

        targets = TargetUtility.GetTargetsInLine(caster.owner.Position, caster.owner.Forward, new Vector3(width/2, 2, Range/2), targetMask);
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