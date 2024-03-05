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

public class KnockBackEffect : CharacterEffect
{
    Vector3 startPos;
    Vector3 targetPos;

    float timer;
    public KnockBackEffect(string name, Vector3 startPos, Vector3 targetPos, float duration, EffectType type) : base(name, duration, type)
    {
        this.startPos = startPos;
        this.targetPos = targetPos;
        timer = 0;
    }

    public override void OnApply(BaseCharacter target)
    {
        target.state = CharacterStates.disabled;
    }

    public override void OnRemoved(BaseCharacter target)
    {
        target.state = CharacterStates.active;
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        target.transform.position = Vector3.Lerp(startPos, targetPos, timer / duration);
        timer += deltaTime;
    }

    public static Vector3 CalculateTargetPos(BaseCharacter target, Vector3 direction, float distance)
    {
        var targetPos = target.transform.position + (direction * distance);
        targetPos.y = target.transform.position.y;
        return targetPos;
    }
}
