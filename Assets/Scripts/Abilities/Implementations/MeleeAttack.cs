using Character;
using Character.Abilities;
using Codice.Client.Commands;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MeleeAttack")]
public class MeleeAttack : AttackAbility
{
    public float BaseDamage;
    public float width;
    BaseCharacter[] targets;

    public override void CastStarted(CasterInfo caster)
    {

        targets = TargetUtility.GetTargetsInLine(caster.owner.Position, caster.owner.Forward, new Vector3(width/2, 2, attackRange/2), targetMask);
    }

    public override void CastEnded(CasterInfo caster)
    {
        foreach (var target in targets)
        {
            if (target != null && target.Faction != caster.owner.Faction)
            {
                target.ApplyDamage(new DamageInfo
                {
                    damage = BaseDamage,
                    source = caster.owner,
                    type = DamageType.Physical
                });
            }
        }
    }

    public override void OnHold(CasterInfo caster)
    {
    }
}