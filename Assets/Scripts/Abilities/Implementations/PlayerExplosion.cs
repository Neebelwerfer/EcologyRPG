using Character;
using Character.Abilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerExplosion", fileName = "New Player Explosion")]
public class PlayerExplosion : BaseAbility
{
    public LayerMask targetMask;
    public float Radius;
    public float BaseDamage;
    public DamageType damageType;
    public float KnockBackDistance;
    BaseCharacter[] targets;

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

                var targetPos = KnockBackEffect.CalculateTargetPos(t, (t.transform.position - caster.castPos).normalized, KnockBackDistance);
                t.ApplyCharacterModification(new KnockBackEffect("KnockBack", t.transform.position, targetPos, 0.5f, EffectType.Debuff));
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
