using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;

namespace EcologyRPG.GameSystems.Abilities.Conditions
{
    public class SprintCondition : BuffCondition
    {
        public float Value;
        Stat stat;
        public override void OnApply(CastInfo Caster, BaseCharacter target)
        {
            stat = target.Stats.GetStat("movementSpeed");
            stat.AddModifier(new StatModification("movementSpeed", Value, StatModType.PercentMult, this));
            target.Animator.SetBool("Is_Running", true);
        }

        public override void OnReapply(BaseCharacter target)
        {
            remainingDuration = duration;
        }

        public override void OnRemoved(BaseCharacter target)
        {
            stat.RemoveAllModifiersFromSource(this);
            target.Animator.SetBool("Is_Running", false);
        }

        public override void OnUpdate(BaseCharacter target, float deltaTime)
        {
        }
    }
}