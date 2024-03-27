using Character.Abilities;
using UnityEngine;

public class ExpandingProjectile : ProjectileAbility
{
    [Tooltip("The travel speed of the projectile")]
    public float Speed;

    public Vector3 GrowthRate = new Vector3(0.2f, 0.2f, 0.2f);

    [Header("Projectile Settings")]
    [Tooltip("The prefab of the projectile")]
    public GameObject ProjectilePrefab;

    public override void Cast(CastInfo castInfo)
    {
        base.Cast(castInfo);
        var dir = GetDir(castInfo);
        firstHit = true;
        ProjectileUtility.CreateProjectile(ProjectilePrefab, castInfo.owner.CastPos + (dir * Range), Speed, destroyOnHit, targetMask, castInfo.owner, (target) =>
        {
            var newCastInfo = new CastInfo { owner = castInfo.owner, castPos = target.transform.position, dir = dir, mousePoint = Vector3.zero };
            DefaultOnHitAction()(newCastInfo, target);
        }, (projectile) =>
        {
            var scale = projectile.transform.localScale;
            var growth = GrowthRate * Time.deltaTime;
            projectile.transform.localScale = new Vector3(scale.x * (1 + growth.x), scale.y * (1 + growth.y), scale.z * (1 + growth.z));
        });
    }
}