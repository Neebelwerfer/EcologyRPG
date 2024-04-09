using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;
using System.Collections.Generic;
using UnityEditor;
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

        public virtual void CopyComponentsTo(BaseAbility ability)
        {
            ability.OnCastEffects = new List<AbilityComponent>();
            for (int i = 0; i < OnCastEffects.Count; i++)
            {
                var newEffect = OnCastEffects[i].GetCopy(ability);
                ability.OnCastEffects.Add(newEffect);
            }
        }

        public virtual BaseAbility GetCopy(Object owner)
        {
            var newAbility = Instantiate(this);
            AssetDatabase.AddObjectToAsset(newAbility, owner);
            CopyComponentsTo(newAbility);
            return newAbility;
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
        public virtual void Delete()
        {
            foreach (var effect in OnCastEffects)
            {
                effect.Delete();
            }
            DestroyImmediate(this, true);
        }
    }
}