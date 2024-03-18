using UnityEngine;

namespace Character.Abilities.AbilityEffects
{
    public class DamageAbilityEffect : CombatAbilityEffect
    {
        public float BaseDamage;
        public DamageType DamageType;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
           target.ApplyDamage(BaseAbility.CalculateDamage(cast.owner, DamageType, BaseDamage));
        }
    }
}
