using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;

namespace EcologyRPG.GameSystems.Abilities.Components
{
    public class WeaponDamageComponent : CombatAbilityComponent
    {
        public float BaseDamage = 5;
        public float WeaponDamagePercent = 1;
        public DamageType DamageType;
        public bool AllowVariance = true;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            target.ApplyDamage(BaseAbility.CalculateDamage(cast.owner, DamageType, BaseDamage + cast.owner.Stats.GetStat("rawWeaponDamage").Value * WeaponDamagePercent, AllowVariance, true));
        }
    }
}
