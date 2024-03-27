using Character;
using Character.Abilities;
using Character.Abilities.AbilityComponents;
using System.Collections.Generic;
using UnityEngine;

public class DashCondition : BuffCondition
{
    public DirectionMode directionMode = DirectionMode.Mouse;
    public float dashRange = 10f;
    public bool StopOnHit = true;

    public List<AbilityComponent> OnFirstHitEffects;
    public List<AbilityComponent> OnHitEffects;

    Vector3 direction;

    float dodgeSpeed;
    bool firstHit = true;

    public override void OnApply(CastInfo caster, BaseCharacter target)
    {
        if (directionMode == DirectionMode.Mouse)
        {
            var lookAt = TargetUtility.GetMousePoint(Camera.main);
            lookAt.y = caster.owner.Position.y;
            direction = (lookAt - caster.owner.Position);
        }
        else
        {
            direction = caster.owner.Forward.normalized;
        }
        dodgeSpeed = dashRange / duration;
        target.OnCharacterCollision.AddListener(OnHit);
    }

    void OnHit(BaseCharacter target)
    {
        CastInfo info = new();
        info.owner = Owner;
        info.castPos = Owner.Position;
        if(StopOnHit) remainingDuration = 0;
        if(firstHit)
        {
            firstHit = false;
            foreach (var effect in OnFirstHitEffects)
            {
                effect.ApplyEffect(info, target);
            }
        }
        foreach (var effect in OnHitEffects)
        {
            effect.ApplyEffect(info, target);
        }
    }

    public override void OnReapply(BaseCharacter target)
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
        //target.Rigidbody.isKinematic = false;
        //target.Rigidbody.velocity = dodgeSpeed * direction.normalized;
        target.transform.position += deltaTime * dodgeSpeed * direction.normalized;
    }

    public override void OnRemoved(BaseCharacter target)
    {
        target.state = CharacterStates.active;
        target.OnCharacterCollision.RemoveListener(OnHit);
    }


}