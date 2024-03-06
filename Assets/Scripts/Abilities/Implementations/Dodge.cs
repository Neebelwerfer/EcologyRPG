using Character;
using Character.Abilities;
using Codice.Client.Commands;
using log4net.Util;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

public enum DirectionMode
{
    Mouse,
    Movement
}

[CreateAssetMenu(fileName = "Dodge", menuName = "Abilities/Dodge")]
public class Dodge : BaseAbility
{
    [Header("Dodge Settings")]
    public DirectionMode directionMode = DirectionMode.Mouse;
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.3f;

    public override void CastEnded(CasterInfo caster)
    {
    }

    public override void CastStarted(CasterInfo caster)
    {
        caster.owner.ApplyCharacterModification(new Dodging(caster, directionMode, dodgeSpeed, dodgeDuration));
    }

    public override void OnHold(CasterInfo caster)
    {
    }
}

public class Dodging : CharacterEffect
{
    float dodgeSpeed;
    Vector3 direction;

    public Dodging(CasterInfo caster, DirectionMode directionMode, float dodgeSpeed, float duration) : base("Dodging", duration, EffectType.Buff)
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