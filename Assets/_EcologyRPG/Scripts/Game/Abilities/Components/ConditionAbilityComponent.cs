using EcologyRPG._Core.Abilities;
using EcologyRPG._Core.Abilities.AbilityComponents;
using EcologyRPG._Core.Character;

namespace EcologyRPG._Game.Abilities.Components
{
    public class ConditionAbilityComponent : CombatAbilityComponent
    {
        public DebuffCondition DebuffCondition;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            target.ApplyCondition(cast, Instantiate(DebuffCondition));
        }
    }
}