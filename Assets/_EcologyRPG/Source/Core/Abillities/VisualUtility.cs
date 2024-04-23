using EcologyRPG.Core.Scripting;
using MoonSharp.Interpreter;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public static class VisualUtility
    {
        public static void Register(Script script)
        {
            script.Globals["SpawnObject"] = (System.Action<uint, Vector3Context, float>)SpawnObject;
            script.Globals["SpawnObjectRotated"] = (System.Action<uint, Vector3Context, QuaternionContext, float>)SpawnObject;
        }

        public static void SpawnObject(uint id, Vector3Context position, float duration)
        {
            SpawnObject(id, position, new QuaternionContext(Quaternion.identity), duration);
        }

        public static void SpawnObject(uint id, Vector3Context position, QuaternionContext rotation, float duration)
        {
            var db = VFXObjectDatabase.Instance;
            if (db == null)
            {
                VFXObjectDatabase.Load();
                db = VFXObjectDatabase.Instance;
            }
            if(id < db.vfxObjects.Length)
            {
                var obj = db.vfxObjects[id];
                var go = Object.Instantiate(obj, position.Vector, rotation.Value);
                Object.Destroy(go, duration);
            }
        }
    }
}