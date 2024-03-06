using Character;
using Character.Abilities;
using UnityEngine;

public class DodgeEffect : CharacterEffect
{
    float dodgeSpeed;
    Vector3 direction;

    public DodgeEffect(CasterInfo caster, DirectionMode directionMode, float dodgeSpeed, float duration) : base("Dodging", duration, EffectType.Buff)
    {
        this.dodgeSpeed = dodgeSpeed;

        if(directionMode == DirectionMode.Mouse)
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

    public override void OnApply(BaseCharacter target)
    {

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