using Character;
using Character.Abilities;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowEffect", menuName = "Abilities/CharacterEffects/SlowEffect")]
public class SlowEffect : CharacterEffect
{
    public float SlowAmount = 0.5f;

    Stat movementSpeed;
    StatModification movementSpeedModifier;

    public override void OnApply(CasterInfo Caster, BaseCharacter target)
    {
        movementSpeed = target.Stats.GetStat("movementSpeed");
        movementSpeedModifier = new StatModification("movementSpeed", SlowAmount, StatModType.PercentMinus, this);
        movementSpeed.AddModifier(movementSpeedModifier);
    }

    public override void OnRemoved(BaseCharacter target)
    {
        movementSpeed.RemoveAllModifiersFromSource(this);
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
    }
}
