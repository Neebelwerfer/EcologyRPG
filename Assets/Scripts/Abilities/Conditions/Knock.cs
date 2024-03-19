﻿using Character;
using Character.Abilities;
using UnityEngine;

public class Knock : DebuffCondition
{
    enum KnockType
    {
        Away,
        Towards
    }

    public float KnockBackDistance = 2;
    [SerializeField] KnockType knockType;

    Vector3 startPos;
    Vector3 targetPos;

    float timer;


    public override void OnApply(CastInfo caster, BaseCharacter target)
    {
        startPos = target.Position;
        Vector3 dir;
        if(knockType == KnockType.Away)
            dir = (target.Position - caster.castPos).normalized;
        else
            dir = (caster.castPos - target.Position).normalized;

        targetPos = Knock.CalculateTargetPos(target, dir, KnockBackDistance);
        timer = 0;
        target.state = CharacterStates.disabled;
    }
    public override void OnReapply(BaseCharacter target)
    {

    }

    public override void OnRemoved(BaseCharacter target)
    {
        target.state = CharacterStates.active;
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        target.transform.position = Vector3.Lerp(startPos, targetPos, timer / duration);
        timer += deltaTime;
    }

    public static Vector3 CalculateTargetPos(BaseCharacter target, Vector3 direction, float distance)
    {
        var targetPos = target.transform.position + (direction * distance);
        targetPos.y = target.transform.position.y;
        return targetPos;
    }
}
