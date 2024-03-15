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
public class Dodge : AbilityEffect
{
    [Header("Dodge Settings")]
    public DodgeEffect dodgeEffect;

    public override void Cast(CastInfo caster)
    {
        caster.owner.ApplyEffect(caster, Instantiate(dodgeEffect));
    }
}
