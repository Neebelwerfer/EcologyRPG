namespace Character.Abilities.AbilityEffects
{
    public class WeaponDamageAbilityEffect : CombatAbilityEffect
    {
        public DamageType DamageType;
        public bool AllowVariance = true;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            target.ApplyDamage(BaseAbility.CalculateDamage(cast.owner, DamageType, cast.owner.Stats.GetStat("rawWeaponDamage").Value, AllowVariance, true));
        }
    }
}
