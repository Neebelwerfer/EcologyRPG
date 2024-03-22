using Character;
using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;


public class SlowCondition : DebuffCondition
{
    enum SlowType
    {
        Flat,
        Decaying
    }

    static UniqueStatModificationHandler movementSpeedHandler = new UniqueStatModificationHandler("movementSpeed", StatModType.PercentMult, false);
    [SerializeField] SlowType slowType;

    [Header("Flat Slow Setting")]
    public float SlowAmount = 0.5f;

    [Header("Decaying Slow Setting")]
    [BoundedCurve(0, 0, 1, 1)]
    public AnimationCurve SlowCurve = new(new Keyframe(0, 0.5f), new Keyframe(1, 0.2f));

    public override void OnApply(CastInfo Caster, BaseCharacter target)
    {

        if(slowType == SlowType.Flat)
            movementSpeedHandler.AddValue(target, this, -SlowAmount);
        else if(slowType == SlowType.Decaying)
            movementSpeedHandler.AddValue(target, this, -SlowCurve.Evaluate(0));
    }

    public override void OnReapply(BaseCharacter target)
    {
        remainingDuration = duration;
    }

    public override void OnRemoved(BaseCharacter target)
    {
        movementSpeedHandler.RemoveValue(target, this);
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        if(slowType == SlowType.Decaying)
        {
            var timePercenage = 1 - (remainingDuration / duration);
            movementSpeedHandler.UpdateValue(target, this, -SlowCurve.Evaluate(timePercenage));
        }
    }
}
