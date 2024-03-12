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
    public float ExplosionRadius;
    public float ExplosionDamage;
    public float TargetHitExtraDamage;
    public DamageType damageType;
    public List<DebuffEffect> effects;

    public GameObject ProjectilePrefab;

    Vector3 MousePoint;
    public override void CastEnded(CasterInfo caster)
    {
        var dir = (MousePoint - caster.castPos).normalized;
        dir.y = 0;
        ProjectileUtility.CreateProjectile(ProjectilePrefab, caster.castPos + (dir * Range), Speed, ExplosionDamage, damageType, destroyOnHit, targetMask, caster.owner, (target) =>
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
        MousePoint = TargetUtility.GetMousePoint(Camera.main);
    }

    public override void OnHold(CasterInfo caster)
    {
    }
}
