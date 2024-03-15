using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicProjectile")]
public class BasicProjectile : ProjectileAbility
{
    [Tooltip("The base damage of the projectile")]
    public float BaseDamage;
    [Tooltip("The type of damage the projectile will deal")]
    public DamageType damageType;
    [Tooltip("The travel speed of the projectile")]
    public float Speed;

    [Header("Projectile Settings")]
    [Tooltip("The prefab of the projectile")]
    public GameObject ProjectilePrefab;
    [Tooltip("Debuffs that will be applied to the target when the projectile hits")]    
    public List<DebuffEffect> Effects;
    Vector3 MousePoint;

    public override void CastEnded(CasterInfo caster)
    {
        base.CastEnded(caster);
        var dir = (MousePoint - caster.castPos).normalized;
        dir.y = 0;
        ProjectileUtility.CreateProjectile(ProjectilePrefab, caster.castPos + (dir * Range), Speed, destroyOnHit, targetMask, caster.owner, (target) =>
        {
            target.ApplyDamage(CalculateDamage(caster.owner, damageType, BaseDamage));
            foreach (var effect in Effects)
            {
                ApplyEffect(caster, target, effect);
            }
        });
    }

    public override void CastStarted(CasterInfo caster)
    {
        base.CastStarted(caster);
        MousePoint = TargetUtility.GetMousePoint(Camera.main);
    }

    public override void OnHold(CasterInfo caster)
    {

    }
} 