using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicProjectile")]
public class BasicProjectile : ProjectileAbility
{
    public float BaseDamage;
    public DamageType damageType;
    public float Speed;
    
    public GameObject ProjectilePrefab;
    public List<DebuffEffect> Effects;
    Vector3 MousePoint;

    public override void CastEnded(CasterInfo caster)
    {
        var dir = (MousePoint - caster.castPos).normalized;
        dir.y = 0;
        ProjectileUtility.CreateProjectile(ProjectilePrefab, caster.castPos + (dir * Range), Speed, BaseDamage, damageType, destroyOnHit, targetMask, caster.owner, (target) =>
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
        MousePoint = TargetUtility.GetMousePoint(Camera.main);
    }

    public override void OnHold(CasterInfo caster)
    {

    }
} 