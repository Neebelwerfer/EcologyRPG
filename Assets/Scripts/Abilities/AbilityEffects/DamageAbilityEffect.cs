using UnityEngine;

namespace Character.Abilities.AbilityEffects
{
    public class DamageAbilityEffect : CombatAbilityEffect
    {
        public float BaseDamage = 5;
        public DamageType DamageType;
        public bool AllowVariance = true;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
           target.ApplyDamage(BaseAbility.CalculateDamage(cast.owner, DamageType, BaseDamage, AllowVariance));
        }
    }
}
