using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackAbility : BaseAbility
{
    [Header("Attack Ability")]
    public LayerMask targetMask;
    public float attackRange;
}