using EcologyRPG.Core.Abilities;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities
{
    public class LoppedProjectile : AttackAbility
    {
        [Header("Lopped Projectile")]
        [Tooltip("The prefab of the projectile")]
        public GameObject ProjectilePrefab;
        [Tooltip("The layer mask of the colliders the projectile can travel through")]
        public LayerMask ignoreMask;
        [Tooltip("The angle of the projectile")]
        public float Angle;
        [Tooltip("The travel time of the projectile")]
        public float TravelTime;

        public override void Cast(CastInfo caster)
        {
            Debug.DrawRay(caster.mousePoint, Vector3.up * 1, Color.red, 5);
            ProjectileUtility.CreateCurvedProjectile(ProjectilePrefab, caster.castPos, caster.mousePoint, TravelTime, -Angle, ignoreMask, caster.owner, (projectileObject) =>
            {
                var newInfo = new CastInfo { owner = caster.owner, castPos = projectileObject.transform.position, mousePoint = caster.mousePoint };
                Debug.DrawRay(projectileObject.transform.position, Vector3.up * 1, Color.green, 5);
                foreach (var effect in OnHitEffects)
                {
                    effect.ApplyEffect(newInfo, null);
                }
            });
        }
    }
}