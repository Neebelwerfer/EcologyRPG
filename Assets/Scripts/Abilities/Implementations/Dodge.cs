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
        caster.owner.ApplyCharacterModification(new DodgeEffect(caster, directionMode, dodgeSpeed, dodgeDuration));
    }

    public override void OnHold(CasterInfo caster)
    {
    }
}
