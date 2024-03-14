using Character;
using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerExplosion", fileName = "New Player Explosion")]
public class CenteredExplosion : BaseAbility
{
    [Header("Centered Explosion")]
    [Tooltip("The layer mask of the targets that will be hit by the explosion")]
    public LayerMask targetMask;
    [Tooltip("The radius of the explosion")]
    public float Radius;
    [Tooltip("The base damage of the explosion")]
    public float BaseDamage;
    [Tooltip("The type of damage the explosion will deal")]
    public DamageType damageType;
    BaseCharacter[] targets;
    [Tooltip("Debuffs that will be applied to the targets when the explosion hits")]
    public List<DebuffEffect> effectsOnHit;

    public override void CastEnded(CasterInfo caster)
    {
        if(targets != null && targets.Length > 0)
        {
            foreach (var t in targets)
            {
                if (t.Faction == caster.owner.Faction) continue;  
                

                foreach (var effect in effectsOnHit)
                {
                    ApplyEffect(caster, t, effect);
                }

                t.ApplyDamage(CalculateDamage(caster.owner, damageType, BaseDamage));
            }
        }
    }

    public override void CastStarted(CasterInfo caster)
    {
        targets = TargetUtility.GetTargetsInRadius(caster.castPos, Radius, targetMask);
    }

    public override void OnHold(CasterInfo caster)
    {
    }
}
