using Character.Abilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SimpleProjectileAttack")]
public class SimpleProjectileAttack : ProjectileAbility
{
    public float BaseDamage;
    public float Range;
    public float Speed;
    
    public GameObject ProjectilePrefab;

    Vector3 MousePoint;

    public override void CastEnded(CasterInfo caster)
    {
        var dir = (MousePoint - caster.castPos).normalized;
        dir.y = 0;
        ProjectileUtility.CreateProjectile(ProjectilePrefab, caster.castPos + (dir * Range), Speed, BaseDamage, destroyOnHit, targetMask, caster.owner);
    }

    public override void CastStarted(CasterInfo caster)
    {
        MousePoint = TargetUtility.GetMousePoint(Camera.main);
    }

    public override void OnHold(CasterInfo caster)
    {

    }
} 