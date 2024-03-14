using Character.Abilities;
using Codice.Client.Commands;
using Codice.CM.Common;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ExplosiveProjectile", fileName = "New Explosive Projectile")]
public class ExplosiveProjectile : ProjectileAbility
{
    public float Speed;

    [Header("Explosive Projectile")]
    [Tooltip("The radius of the explosion")]
    public float ExplosionRadius;
    [Tooltip("The base damage of the explosion")]
    public float ExplosionDamage;
    [Tooltip("Extra damage that will be dealt to the target that was hit by the projectile")]
    public float TargetHitExtraDamage;
    [Tooltip("The type of damage the explosion will deal")]
    public DamageType damageType;
    [Tooltip("Debuffs that will be applied to the targets when the explosion hits")]
    public List<DebuffEffect> effects;
    [Tooltip("The prefab of the projectile")]
    public GameObject ProjectilePrefab;

    Vector3 MousePoint;
    public override void CastEnded(CasterInfo caster)
    {
        base.CastEnded(caster);
        var dir = (MousePoint - caster.castPos).normalized;
        dir.y = 0;
        ProjectileUtility.CreateProjectile(ProjectilePrefab, caster.castPos + (dir * Range), Speed, destroyOnHit, targetMask, caster.owner, (target) =>
        {
            var targets = TargetUtility.GetTargetsInRadius(target.transform.position, ExplosionRadius, targetMask);

            foreach (var t in targets)
            {

                if (t.Faction != caster.owner.Faction)
                {
                    t.ApplyDamage(CalculateDamage(caster.owner, damageType, t == target ? ExplosionDamage + TargetHitExtraDamage : ExplosionDamage));
                }

                foreach (var effect in effects)
                {
                    ApplyEffect(caster, t, effect);
                }
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
