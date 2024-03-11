using Character;
using Character.Abilities;
using UnityEngine;


[CreateAssetMenu(fileName = "new Damage over Time Effect", menuName = CharacterEffectPath + "Damage over Time Effect")]
public class DamageOverTimeEffect : CharacterEffect
{
    public float damagePerTick = 10;
    public DamageType DamageType;
    public float tickRate = 1;

    private float timeSinceLastTick = 0;
    private DamageInfo damageInfo;

    public override void OnApply(CasterInfo caster, BaseCharacter target)
    {
        timeSinceLastTick = 0;
        damageInfo = new DamageInfo()
        {
            damage = damagePerTick,
            type = DamageType,
            source = caster.owner
        };

    }

    public override void OnReapply(BaseCharacter target)
    {
        remainingDuration = duration;
    }

    public override void OnRemoved(BaseCharacter target)
    {
        var mult = 1 - (timeSinceLastTick / tickRate);

        var damageInfo = new DamageInfo()
        {
            damage = damagePerTick * mult,
            type = DamageType,
            source = target
        };
        target.ApplyDamage(damageInfo);
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        timeSinceLastTick += deltaTime;
        if (timeSinceLastTick >= tickRate)
        {
            timeSinceLastTick = 0;
            target.ApplyDamage(damageInfo);
        }
    }
}