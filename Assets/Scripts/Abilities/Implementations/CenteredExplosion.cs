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
    public List<CharacterEffect> effectsOnHit;

    public override void CastEnded(CasterInfo caster)
    {
        if(targets != null && targets.Length > 0)
        {
            foreach (var t in targets)
            {
                if (t.Faction == caster.owner.Faction) continue;  
                
                var info = new DamageInfo()
                {
                    damage = BaseDamage,
                    source = caster.owner,
                    type = damageType
                };

                foreach (var effect in effectsOnHit)
                {
                    ApplyEffect(caster, t, effect);
                }

                t.ApplyDamage(info);
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
