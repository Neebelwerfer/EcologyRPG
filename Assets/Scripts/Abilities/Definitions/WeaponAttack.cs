using UnityEngine;

public abstract class WeaponAttack : AttackAbility
{
    public enum TargetType
    {
        Line,
        Cone,
        Circular
    }

    [Header("Weapon Attack")]
    [Tooltip("The base damage of the ability")]
    public float BaseDamage;
    [Tooltip("The type of targeting this ability will use")]
    public TargetType targetType;
}