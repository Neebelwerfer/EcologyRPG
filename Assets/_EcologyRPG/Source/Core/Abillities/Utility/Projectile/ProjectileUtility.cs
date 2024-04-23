using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Scripting;
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

            script.Globals["CreateCurvedProjectile"] = (Func<int, CastContext, float, float, CurvedProjectileBehaviour>)((int prefabID, CastContext context, float time, float angle) =>
            {
                return CreateCurvedProjectile(prefabID, context, time, angle);
            });
        }

        public static BasicProjectileBehaviour CreateBasicProjectile(int prefabID, CastContext context, float range, float speed, bool destroyOnhit)
        {
            return new BasicProjectileBehaviour(prefabID, range, context.castPos.Vector, context.dir.Vector, speed, destroyOnhit, Core.Abilities.AbilityManager.TargetMask, context.GetOwner());
        }

        public static CurvedProjectileBehaviour CreateCurvedProjectile(int prefabID, CastContext castContext, float time, float angle)
        {
            return new CurvedProjectileBehaviour(prefabID, castContext.castPos.Vector, castContext.targetPoint.Vector, castContext.GetOwner(), time, -angle);
        }
    }
}