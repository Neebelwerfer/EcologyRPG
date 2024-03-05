using Character.Abilities;
using Codice.Client.Commands;
using Codice.CM.Common;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PiercingProjectile", fileName = "New Piercing Projectile")]
public class PiercingProjectile : ProjectileAbility
{
    public float BaseDamage;
    public DamageType damageType;
    public float Range;
    public float Speed;

    public GameObject ProjectilePrefab;

    Vector3 MousePoint;

    public override void CastEnded(CasterInfo caster)
    {
        var dir = (MousePoint - caster.castPos).normalized;
        dir.y = 0;
        ProjectileUtility.CreateProjectile(ProjectilePrefab, caster.castPos + (dir * Range), Speed, BaseDamage, damageType, false, targetMask, caster.owner);
    }

    public override void CastStarted(CasterInfo caster)
    {
        MousePoint = TargetUtility.GetMousePoint(Camera.main);
    }

    public override void OnHold(CasterInfo caster)
    {

    }
}
