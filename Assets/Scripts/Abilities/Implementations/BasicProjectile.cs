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
    public List<CharacterEffect> Effects;
    Vector3 MousePoint;

    public override void CastEnded(CasterInfo caster)
    {
        var dir = (MousePoint - caster.castPos).normalized;
        dir.y = 0;
        ProjectileUtility.CreateProjectile(ProjectilePrefab, caster.castPos + (dir * Range), Speed, BaseDamage, damageType, destroyOnHit, targetMask, caster.owner, (target) =>
        {
            var info = new DamageInfo
            {
                damage = BaseDamage,
                type = damageType,
                source = caster.owner,
            };
            target.ApplyDamage(info);
            foreach (var effect in Effects)
            {
                target.ApplyEffect(caster, Instantiate(effect));
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