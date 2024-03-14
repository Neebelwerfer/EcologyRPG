using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackAbility : BaseAbility
{
    [Header("Buffs")]
    [Tooltip("Buffs that will be applied when the cast starts")]
    public List<BuffEffect> buffsOnCastStart;
    [Tooltip("Buffs that will be applied when the cast ends")]
    public List<BuffEffect> buffsOnCastEnded;

    [Header("Attack Ability")]
    [Tooltip("The layermask that the ability will target")]
    public LayerMask targetMask;
    [Tooltip("The range of the ability")]
    public float Range;

    public override void CastStarted(CasterInfo caster)
    {
        foreach (BuffEffect buff in buffsOnCastStart)
        {
            caster.owner.ApplyEffect(caster, Instantiate(buff));
        }
    }

    public override void CastEnded(CasterInfo caster)
    {
        foreach (BuffEffect buff in buffsOnCastEnded)
        {
            caster.owner.ApplyEffect(caster, Instantiate(buff));
        }
    }
}