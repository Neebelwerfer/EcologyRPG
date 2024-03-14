public abstract class WeaponAttack : AttackAbility
{
    public enum TargetType
    {
        Line,
        Cone,
        Circular
    }

    public float BaseDamage;
    public TargetType targetType;
    public bool useMouseDirection;
}