using Character;
using Character.Abilities;
using UnityEngine;

[CreateAssetMenu(fileName = "KnockEffect", menuName = "Abilities/CharacterEffects/KnockEffect")]
public class KnockEffect : CharacterEffect
{
    enum KnockType
    {
        Away,
        Towards
    }

    public float KnockBackDistance;
    [SerializeField] KnockType knockType;

    Vector3 startPos;
    Vector3 targetPos;

    float timer;


    public override void OnApply(CasterInfo caster, BaseCharacter target)
    {
        startPos = target.Position;
        Vector3 dir;
        if(knockType == KnockType.Away)
            dir = (target.Position - caster.castPos).normalized;
        else
            dir = (caster.castPos - target.Position).normalized;

        targetPos = KnockEffect.CalculateTargetPos(target, dir, KnockBackDistance);
        timer = 0;
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
