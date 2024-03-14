using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileAbility : AttackAbility
{
    [Header("Projectile Ability")]
    public bool destroyOnHit = true;
}