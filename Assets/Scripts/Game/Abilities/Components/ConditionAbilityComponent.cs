namespace Character.Abilities.AbilityComponents
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