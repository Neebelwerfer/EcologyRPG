using Character;
using Character.Abilities;
using UnityEngine;

[CreateAssetMenu(fileName = "DecayingSlowEffect", menuName = "Abilities/CharacterEffects/DecayingSlowEffect")]
public class DecayingSlowEffect : CharacterEffect
{
    [BoundedCurve(0, 0, 1, 1)]
    public AnimationCurve SlowCurve = new AnimationCurve(new Keyframe(0, 0.5f), new Keyframe(1, 0.2f));

    Stat movementSpeed;
    StatModification movementSpeedModifier;

    public override void OnApply(CasterInfo Caster, BaseCharacter target)
    {
        movementSpeed = target.Stats.GetStat("movementSpeed");
        movementSpeedModifier = new StatModification("movementSpeed", SlowCurve.Evaluate(0), StatModType.PercentMinus, this);
        movementSpeed.AddModifier(movementSpeedModifier);
    }

    public override void OnRemoved(BaseCharacter target)
    {
        movementSpeed.RemoveAllModifiersFromSource(this);
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        var timePercenage = 1 - (remainingDuration / duration);
        movementSpeedModifier.Value = SlowCurve.Evaluate(timePercenage);
    }
}