using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;

namespace EcologyRPG.Game.Abilities.Components
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