using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;
using System;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    public static class ProjectileUtility
    {
        public static void AddToGlobal(Script script)
        {
            script.Globals["CreateBasicProjectile"] = (Func<int, CastContext, float, float, bool, BasicProjectileBehaviour>)((int prefabID, CastContext context, float range, float speed, bool destroyOnhit) =>
            {
                return CreateBasicProjectile(prefabID, context, range, speed, destroyOnhit);
            });
        }

        public static BasicProjectileBehaviour CreateBasicProjectile(int prefabID, CastContext context, float range, float speed, bool destroyOnhit)
        {
            return new BasicProjectileBehaviour(prefabID, range, context.castPos.Vector, context.dir.Vector, speed, destroyOnhit, Core.Abilities.AbilityManager.TargetMask, context.GetOwner());
        }

        public static void CreateCurvedProjectile(int prefabID, Vector3 Origin, Vector3 targetPos, float time, float angle, LayerMask IgnoreMask, BaseCharacter owner, Action<GameObject> OnGroundHit)
        {
            new CurvedProjectileBehaviour(prefabID, Origin, targetPos, time, angle, IgnoreMask, owner, OnGroundHit);
        }
    }
}