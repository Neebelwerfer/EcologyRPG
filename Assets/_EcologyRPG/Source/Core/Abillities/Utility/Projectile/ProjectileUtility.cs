using EcologyRPG.Core.Character;
using System;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public static class ProjectileUtility
    {
        public static void CreateBasicProjectile(GameObject prefab, Vector3 Origin, Vector3 dir, float range, float speed, bool destroyOnhit, LayerMask mask, BaseCharacter owner, AudioClip audioClip, Action<BaseCharacter> onHit = null, Action<GameObject> onUpdate = null)
        {
            new BasicProjectileBehaviour(prefab, range, Origin, dir, speed, destroyOnhit, mask, owner, audioClip, onHit, onUpdate);
        }

        public static void CreateCurvedProjectile(GameObject prefab, Vector3 Origin, Vector3 targetPos, float time, float angle, LayerMask IgnoreMask, BaseCharacter owner, Action<GameObject> OnGroundHit, AudioClip audioClip)
        {
            new CurvedProjectileBehaviour(prefab, Origin, targetPos, time, angle, IgnoreMask, owner, OnGroundHit, audioClip);
        }
    }
}