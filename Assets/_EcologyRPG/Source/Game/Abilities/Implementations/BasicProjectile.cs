using EcologyRPG.Core.Abilities;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities
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
            base.Cast(castInfo);
            var dir = GetDir(castInfo);
            firstHit = true;
            ProjectileUtility.CreateBasicProjectile(ProjectilePrefab, castInfo.castPos, dir, Range, Speed, destroyOnHit, AbilityManager.TargetMask, castInfo.owner, clip, (target) =>
            {
                var newCastInfo = new CastInfo { owner = castInfo.owner, castPos = target.Transform.Position, dir = dir, targetPoint = Vector3.zero };
                DefaultOnHitAction()(newCastInfo, target);
            });
        }
    }
}
