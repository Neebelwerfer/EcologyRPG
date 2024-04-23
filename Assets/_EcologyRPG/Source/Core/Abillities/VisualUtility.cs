using EcologyRPG.Core.Character;
using EcologyRPG.Core.Scripting;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UIElements;

namespace EcologyRPG.Core.Abilities
{
    public static class VisualUtility
    {
        public static void Register(Script script)
        {
            script.Globals["SpawnVFX"] = (System.Action<uint, Vector3Context, float>)SpawnVFX;
            script.Globals["SpawnVFXRotated"] = (System.Action<uint, Vector3Context, QuaternionContext, float>)SpawnVFX;
            script.Globals["SpawnVFXOnTarget"] = (System.Action<uint, BaseCharacter, float>)SpawnVFXOnTarget;

        }

        public static void SpawnVFX(uint id, Vector3Context position, float duration)
        {
            SpawnVFX(id, position, new QuaternionContext(Quaternion.identity), duration);
        }

        public static void SpawnVFX(uint id, Vector3Context position, QuaternionContext rotation, float duration)
        {
            var db = VFXDatabase.Instance;
            if (db == null)
            {
                VFXDatabase.Load();
                db = VFXDatabase.Instance;
            }
            if(id < db.vfxObjects.Length)
            {
                db.Spawn((int)id, position.Vector, rotation.Value, duration);
            }
        }

        public static void SpawnVFXOnTarget(uint id, BaseCharacter target, float duration)
        {
            var db = VFXDatabase.Instance;
            if (db == null)
            {
                VFXDatabase.Load();
                db = VFXDatabase.Instance;
            }
            if (id < db.vfxObjects.Length)
            {
                db.Spawn((int)id, target, duration);
            }
        }
    }
}