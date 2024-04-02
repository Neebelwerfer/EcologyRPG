using EcologyRPG.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Game.NPC
{
    public class NPCGameObjectPool : IDisposable
    {

        readonly Dictionary<string, GameObjectPool> pool;

        public NPCGameObjectPool()
        {
            pool = new();
        }

        public void Dispose()
        {
            foreach (var gameObjectPool in pool)
            {
                gameObjectPool.Value.Dispose();
            }
        }

        public GameObject GetGameObject(GameObject prefab)
        {
            if(pool.TryGetValue(prefab.name, out var gameObjectPool))
            {
                return gameObjectPool.GetObject();
            }
            else
            {
                var newPool = new GameObjectPool(prefab);
                pool.Add(prefab.name, newPool);
                return newPool.GetObject();
            }
        }

        public GameObject GetGameObject(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            if (pool.TryGetValue(prefab.name, out var gameObjectPool))
            {
                return gameObjectPool.GetObject(pos, rot);
            }
            else
            {
                var newPool = new GameObjectPool(prefab);
                pool.Add(prefab.name, newPool);
                return newPool.GetObject(pos, rot);
            }
        }

        public GameObject GetGameObject(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            if (pool.TryGetValue(prefab.name, out var gameObjectPool))
            {
                return gameObjectPool.GetObject(pos, rot, parent);
            }
            else
            {
                var newPool = new GameObjectPool(prefab);
                pool.Add(prefab.name, newPool);
                return newPool.GetObject(pos, rot, parent);
            }
        }
        
        public void ReturnGameObject(GameObject go)
        {
            if (pool.TryGetValue(go.name, out var gameObjectPool))
            {
                gameObjectPool.ReturnObject(go);
            }
            else
            {
                Debug.LogError("No pool for this object");
            }
        }

    }
}