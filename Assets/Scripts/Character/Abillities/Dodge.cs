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
        caster.owner.ApplyBuff(new Dodging(caster, directionMode, dodgeSpeed, dodgeDuration));
    }

    public override void OnHold(CasterInfo caster)
    {
    }
}

public class Dodging : Buff
{
    float dodgeSpeed;
    Vector3 direction;

    public Dodging(CasterInfo caster, DirectionMode directionMode, float dodgeSpeed, float duration) : base("Dodging", duration)
    {
        this.dodgeSpeed = dodgeSpeed;

        if(directionMode == DirectionMode.Mouse)
        {
            var mouseVector = Mouse.current.position.ReadValue();
            var mousePoint = Camera.main.ScreenPointToRay(mouseVector);

            if (Physics.Raycast(mousePoint, out RaycastHit hit, 100f, LayerMask.NameToLayer("Entity")))
            {
                var lookAt = hit.point;
                lookAt.y = caster.owner.transform.position.y;
                direction = (lookAt - caster.owner.transform.position);
                Debug.DrawRay(caster.owner.transform.position, direction, Color.red, 1f);
            }
        } 
        else
        {
            var rb = caster.owner.Rigidbody;
            if(rb.velocity == Vector3.zero)
            {
                direction = caster.owner.transform.forward.normalized;
            }
            else
            {
                direction = rb.velocity.normalized;
            }
        }
       

    }

    public override void OnApply(BaseCharacter target)
    {

    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        if(target.state == CharacterStates.disabled)
        {
            remainingDuration = 0;
            return;
        }
        target.state = CharacterStates.dodging;
        target.Rigidbody.velocity = direction.normalized * dodgeSpeed;

    }

    public override void OnRemoved(BaseCharacter target)
    {
        target.state = CharacterStates.active;
    }
}