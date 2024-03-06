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
    public DodgeEffect dodgeEffect;

    public override void CastEnded(CasterInfo caster)
    {
    }

    public override void CastStarted(CasterInfo caster)
    {
        
        caster.owner.ApplyEffect(caster, Instantiate(dodgeEffect));
    }

    public override void OnHold(CasterInfo caster)
    {
    }
}
