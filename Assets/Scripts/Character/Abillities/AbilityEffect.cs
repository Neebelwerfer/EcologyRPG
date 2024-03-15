using Character;
using Character.Abilities;
using UnityEngine;

public abstract class AbilityEffect : ScriptableObject
{
    [Tooltip("The range of the ability")]
    public float Range;

    public abstract void Cast(CastInfo castInfo);

    protected static void ApplyEffect(CastInfo caster, BaseCharacter target, CharacterEffect effect)
    {
        var instancedEffect = Instantiate(effect);
        instancedEffect.Owner = caster.owner;
        target.ApplyEffect(caster, instancedEffect);
    }

    public static DamageInfo CalculateDamage(BaseCharacter caster, DamageType damageType, float BaseDamage, bool allowVariance = true)
    {
        DamageInfo damageInfo = new()
        {
            type = damageType,
            source = caster
        };

        var ad = caster.Stats.GetStat("abilityDamage");
        var damageVariance = allowVariance ? caster.Random.NextFloat(0.9f, 1.1f) : 1;
        damageInfo.damage = (BaseDamage * ad.Value) * damageVariance;

        return damageInfo;
    }
}