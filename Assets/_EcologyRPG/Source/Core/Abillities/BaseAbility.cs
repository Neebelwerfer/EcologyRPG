using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public abstract class BaseAbility : ScriptableObject
    {
        [Tooltip("The range of the ability")]
        public float Range;
        public List<AbilityComponent> OnCastEffects = new();

        public virtual void Cast(CastInfo castInfo)
        {
            foreach (var effect in OnCastEffects)
            {
                effect.ApplyEffect(castInfo, null);
            }
        }

        public virtual bool CanCast(BaseCharacter caster)
        {
            return true;
        }

        public static DamageInfo CalculateDamage(BaseCharacter caster, DamageType damageType, float BaseDamage, bool allowVariance = true, bool useWeaponDamage = false)
        {
            DamageInfo damageInfo = new()
            {
                type = damageType,
                source = caster
            };

            Stat ad;
            if (useWeaponDamage)
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

        [ContextMenu("Delete")]
        protected virtual void Delete()
        {
            DestroyImmediate(this, true);
        }
    }
}