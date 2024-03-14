using Character;
using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Multiple Projectiles")]
public class MultipleProjectiles : ProjectileAbility
{
    public ProjectileType type;
    [Min(2)]
    public int numberOfProjectiles = 2;
    [Header("Cone Settings")]
    public float ConeAngle;
    [Header("Line Settings")]
    public float LineWidth;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float Speed;
    public float BaseDamage;
    public DamageType damageType;
    public List<DebuffEffect> effects;

    Vector3 dir;
    float angleBetweenProjectiles;

    public MultipleProjectiles()
    {

    }

    public override void CastStarted(CasterInfo caster)
    {
        base.CastStarted(caster);
        dir = TargetUtility.GetMouseDirection(caster.castPos, Camera.main);
        dir.y = 0;
        dir.Normalize();

        if (type == ProjectileType.Cone)
        {
            angleBetweenProjectiles = ConeAngle / (numberOfProjectiles - 1);
        }
        else if (type == ProjectileType.Line)
        {
            angleBetweenProjectiles = LineWidth / numberOfProjectiles;
        }
        else if (type == ProjectileType.Circular)
        {
            angleBetweenProjectiles = 360 / numberOfProjectiles;
        }

    }

    public override void CastEnded(CasterInfo caster)
    {
        base.CastEnded(caster);


        if (type == ProjectileType.Line)
        {
            var center = caster.castPos;
            var left = Quaternion.Euler(0, -90, 0) * dir;
            var start = center + left * LineWidth / 2;
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var pos = start + i * LineWidth * -left / (numberOfProjectiles - 1);
                ProjectileUtility.CreateProjectile(projectilePrefab, pos, pos + dir * Range, Speed, destroyOnHit, targetMask, caster.owner, (target) =>
                {
                    OnHit(caster, target);
                });
            }
        }
        else if (type == ProjectileType.Cone)
        {
            var start = Quaternion.Euler(0, -ConeAngle / 2, 0) * dir;
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var newDir = Quaternion.Euler(0, angleBetweenProjectiles * i, 0) * start;
                ProjectileUtility.CreateProjectile(projectilePrefab, caster.castPos + newDir * Range, Speed, destroyOnHit, targetMask, caster.owner, (target) =>
                {
                    OnHit(caster, target);
                });
            }
        }
        else if (type == ProjectileType.Circular)
        {
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var newDir = Quaternion.Euler(0, angleBetweenProjectiles * i, 0) * dir;
                ProjectileUtility.CreateProjectile(projectilePrefab, caster.castPos + newDir * Range, Speed, destroyOnHit, targetMask, caster.owner, (target) =>
                {
                    OnHit(caster, target);
                });
            }
        }
        
    }

    public override void OnHold(CasterInfo caster)
    {

    }

    void OnHit (CasterInfo caster, BaseCharacter target)
    {
        target.ApplyDamage(CalculateDamage(target, damageType, BaseDamage));
        foreach (var effect in effects)
        {
            ApplyEffect(caster, target, effect);
        }
    }
}