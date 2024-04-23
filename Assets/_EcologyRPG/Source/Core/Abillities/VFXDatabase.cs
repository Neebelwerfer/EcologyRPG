using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Character;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public class VFXDatabase : ScriptableObject
    {
        public static VFXDatabase Instance;
        public const string ResourcePath = "VFXDatabase";
        public const string ResourceFullPath = "Assets/_EcologyRPG/Resources/" + ResourcePath + ".asset";

        public VFXBinder[] vfxObjects = new VFXBinder[0];
        public VFXPool[] pools;

        public void Init()
        {
            pools = new VFXPool[vfxObjects.Length];
            for (int i = 0; i < vfxObjects.Length; i++)
            {
                pools[i] = new VFXPool(vfxObjects[i]);
            }
        }

        public void Spawn(int id, BaseCharacter target, float duration)
        {
            if (id < vfxObjects.Length)
            {
                var obj = vfxObjects[id];
                var go = pools[id].GetVFX();
                go.transform.SetPositionAndRotation(target.Transform.Position, target.Transform.Rotation);
                go.transform.SetParent(target.GameObject.transform);
                go.gameObject.SetActive(true);
                go.Start();
                TaskManager.Add(this, () =>
                {
                    pools[id].ReturnVFX(go);
                }, duration);
            }
        }

        public void Spawn(int id, Vector3 position, Quaternion rotation, float duration)
        {
            if (id < vfxObjects.Length)
            {
                var obj = vfxObjects[id];
                var go = pools[id].GetVFX();
                go.transform.SetPositionAndRotation(position, rotation);
                go.gameObject.SetActive(true);
                go.Start();
                TaskManager.Add(this, () =>
                {
                    pools[id].ReturnVFX(go);
                }, duration);
            }
        }


        public static void Load()
        {
            Instance = Resources.Load<VFXDatabase>(ResourcePath);
            Instance.Init();
        }
    }

    public class VFXPool
    {
        public VFXBinder prefab;
        public Stack<VFXBinder> pool;

        public VFXPool(VFXBinder prefab)
        {
            this.prefab = prefab;
            pool = new Stack<VFXBinder>();
        }

        public VFXBinder GetVFX()
        {
            if (pool.Count > 0)
            {
                return pool.Pop();
            }
            else
            {
                var obj = Object.Instantiate(prefab);
                obj.gameObject.SetActive(false);
                return obj;
            }
        }

        public void ReturnVFX(VFXBinder vfx)
        {
            vfx.End();
            vfx.transform.SetParent(null);
            vfx.gameObject.SetActive(false);
            pool.Push(vfx);
        }
    }
}