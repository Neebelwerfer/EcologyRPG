using UnityEngine;

namespace EcologyRPG.Game.Abilities.Definitions
{
    public abstract class WeaponAttack : AttackAbility
    {
        public enum TargetType
        {
            Line,
            Cone,
            Circular
        }

        [Header("Weapon Attack")]
        [Tooltip("The type of targeting this ability will use")]
        public TargetType targetType;
    }
}

