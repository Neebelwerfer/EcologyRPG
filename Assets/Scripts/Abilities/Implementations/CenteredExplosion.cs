using Character;
using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerExplosion", fileName = "New Player Explosion")]
public class CenteredExplosion : BaseAbility
{
    public LayerMask targetMask;
    public float Radius;
    public float BaseDamage;
    public DamageType damageType;
    BaseCharacter[] targets;
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
