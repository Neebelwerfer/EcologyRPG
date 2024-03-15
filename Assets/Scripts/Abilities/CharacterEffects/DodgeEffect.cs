using Character;
using Character.Abilities;
using UnityEngine;

public class DodgeEffect : BuffEffect
{
    public DirectionMode directionMode = DirectionMode.Mouse;
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.3f;

    Vector3 direction;

    public override void OnApply(CastInfo caster, BaseCharacter target)
    {
        if (directionMode == DirectionMode.Mouse)
        {
            var lookAt = TargetUtility.GetMousePoint(Camera.main);
            lookAt.y = caster.owner.Position.y;
            direction = (lookAt - caster.owner.Position);
            Debug.DrawRay(caster.owner.Position, direction, Color.red, 1f);
        }
        else
        {
            direction = caster.owner.Forward.normalized;
        }
    }
    public override void OnReapply(BaseCharacter target)
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        if (target.state == CharacterStates.disabled)
        {
            remainingDuration = 0;
            return;
        }
        target.state = CharacterStates.dodging;
        target.Rigidbody.isKinematic = false;
        target.Rigidbody.velocity = dodgeSpeed * direction.normalized;
    }

    public override void OnRemoved(BaseCharacter target)
    {
        target.state = CharacterStates.active;
        target.Rigidbody.isKinematic = true;
    }


}