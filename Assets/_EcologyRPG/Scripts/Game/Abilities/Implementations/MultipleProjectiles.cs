using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities
{
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

        Vector3 dir;
        float angleBetweenProjectiles;

        public override void Cast(CastInfo caster)
        {
            var dir = GetDir(caster);
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

            if (type == ProjectileType.Line)
            {
                var center = caster.castPos;
                var left = Quaternion.Euler(0, -90, 0) * dir;
                var start = center + left * LineWidth / 2;
                for (int i = 0; i < numberOfProjectiles; i++)
                {
                    var pos = start + i * LineWidth * -left / (numberOfProjectiles - 1);
                    ProjectileUtility.CreateBasicProjectile(projectilePrefab, pos, dir, Range, Speed, destroyOnHit, targetMask, caster.owner, (target) =>
                    {
                        OnHit(caster, target, dir);
                    });
                }
            }
            else if (type == ProjectileType.Cone)
            {
                var start = Quaternion.Euler(0, -ConeAngle / 2, 0) * dir;
                for (int i = 0; i < numberOfProjectiles; i++)
                {
                    var newDir = Quaternion.Euler(0, angleBetweenProjectiles * i, 0) * start;
                    ProjectileUtility.CreateBasicProjectile(projectilePrefab, caster.castPos, newDir, Range, Speed, destroyOnHit, targetMask, caster.owner, (target) =>
                    {
                        OnHit(caster, target, newDir);
                    });
                }
            }
            else if (type == ProjectileType.Circular)
            {
                for (int i = 0; i < numberOfProjectiles; i++)
                {
                    var newDir = Quaternion.Euler(0, angleBetweenProjectiles * i, 0) * dir;
                    ProjectileUtility.CreateBasicProjectile(projectilePrefab, caster.castPos, newDir, Range, Speed, destroyOnHit, targetMask, caster.owner, (target) =>
                    {
                        OnHit(caster, target, newDir);
                    });
                }
            }
        }

        void OnHit(CastInfo caster, BaseCharacter target, Vector3 direction)
        {
            var newCastInfo = new CastInfo { owner = caster.owner, castPos = target.Transform.Position, dir = direction, mousePoint = caster.mousePoint };
            DefaultOnHitAction()(newCastInfo, target);
        }
    }
}
