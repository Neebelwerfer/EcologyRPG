using EcologyRPG.Core.Abilities;
using UnityEngine;

namespace EcologyRPG.Game.Abilities
{
    public class BasicProjectile : ProjectileAbility
    {
        [Tooltip("The travel speed of the projectile")]
        public float Speed;

        [Header("Projectile Settings")]
        [Tooltip("The prefab of the projectile")]
        public GameObject ProjectilePrefab;

        public override void Cast(CastInfo castInfo)
        {
            var dir = GetDir(castInfo);
            firstHit = true;
            ProjectileUtility.CreateProjectile(ProjectilePrefab, castInfo.owner.CastPos + (dir * Range), Speed, destroyOnHit, targetMask, castInfo.owner, (target) =>
            {
                var newCastInfo = new CastInfo { owner = castInfo.owner, castPos = target.Transform.Position, dir = dir, mousePoint = Vector3.zero };
                DefaultOnHitAction()(newCastInfo, target);
            });
        }
    }
}
