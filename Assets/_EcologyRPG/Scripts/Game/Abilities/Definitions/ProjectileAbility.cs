using UnityEngine;

namespace EcologyRPG.Game.Abilities.Definitions
{
    public enum ProjectileType
    {
        Line,
        Cone,
        Circular
    }

    public abstract class ProjectileAbility : AttackAbility
    {
        [Header("Projectile Ability")]
        [Tooltip("The projectile will be destroyed when it hits the first target")]
        public bool destroyOnHit = true;
    }
}
