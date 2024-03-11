using Character;
using Character.Abilities;
using UnityEngine;



[CreateAssetMenu(fileName = "SlowEffect", menuName = "Abilities/CharacterEffects/SlowEffect")]
public class SlowEffect : CharacterEffect
{
    enum SlowType
    {
        Flat,
        Decaying
    }

    [SerializeField] SlowType slowType;

    [Header("Flat Slow Setting")]
    public float SlowAmount = 0.5f;

    [Header("Decaying Slow Setting")]
    [BoundedCurve(0, 0, 1, 1)]
    public AnimationCurve SlowCurve = new(new Keyframe(0, 0.5f), new Keyframe(1, 0.2f));

    Stat movementSpeed;
    StatModification movementSpeedModifier;

    public override void OnApply(CasterInfo Caster, BaseCharacter target)
    {
        movementSpeed = target.Stats.GetStat("movementSpeed");

        if(slowType == SlowType.Flat)
            movementSpeedModifier = new StatModification("movementSpeed", SlowAmount, StatModType.PercentMinus, this);
        else if(slowType == SlowType.Decaying)
            movementSpeedModifier = new StatModification("movementSpeed", SlowCurve.Evaluate(0), StatModType.PercentMinus, this);

        movementSpeed.AddModifier(movementSpeedModifier);
    }

    public override void OnReapply(BaseCharacter target)
    {
        remainingDuration = duration;
    }

    public override void OnRemoved(BaseCharacter target)
    {
        movementSpeed.RemoveAllModifiersFromSource(this);
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        if(slowType == SlowType.Decaying)
        {
            var timePercenage = 1 - (remainingDuration / duration);
            movementSpeedModifier.Value = SlowCurve.Evaluate(timePercenage);
        }
    }
}
