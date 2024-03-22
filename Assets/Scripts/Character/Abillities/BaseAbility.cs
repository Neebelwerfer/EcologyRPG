using Character;
using Character.Abilities;
using Character.Abilities.AbilityEffects;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : ScriptableObject
{
    [Tooltip("The range of the ability")]
    public float Range;
    public List<AbilityEffect> OnCastEffects = new();

    public virtual void Cast(CastInfo castInfo)
    {
        foreach (var effect in OnCastEffects)
        {
            effect.ApplyEffect(castInfo, null);
        }
    }

    public static DamageInfo CalculateDamage(BaseCharacter caster, DamageType damageType, float BaseDamage, bool allowVariance = true, bool useWeaponDamage = false)
    {
        DamageInfo damageInfo = new()
        {
            type = damageType,
            source = caster
        };

        Stat ad;
        if(useWeaponDamage)
        {
            ad = caster.Stats.GetStat("weaponDamage");
        }
        else
        {
            ad = caster.Stats.GetStat("abilityDamage");
        }
        var damageVariance = allowVariance ? caster.Random.NextFloat(0.9f, 1.1f) : 1;
        damageInfo.damage = (BaseDamage * ad.Value) * damageVariance;

        return damageInfo;
    }

    protected virtual void OnDestroy()
    {
        foreach (var effect in OnCastEffects)
        {
            DestroyImmediate(effect, true);
        }
    }
}

#if UNITY_EDITOR
#endif