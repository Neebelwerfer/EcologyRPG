using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackAbility : BaseAbility
{

    public List<BuffEffect> buffsOnCastStart;
    public List<BuffEffect> buffsOnCastEnded;

    [Header("Attack Ability")]
    public LayerMask targetMask;
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