using Character;
using UnityEngine;

public class KnockBackEffect : CharacterEffect
{
    Vector3 startPos;
    Vector3 targetPos;

    float timer;
    public KnockBackEffect(string name, Vector3 startPos, Vector3 targetPos, float duration, EffectType type) : base(name, duration, type)
    {
        this.startPos = startPos;
        this.targetPos = targetPos;
        timer = 0;
    }

    public override void OnApply(BaseCharacter target)
    {
        target.state = CharacterStates.disabled;
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
