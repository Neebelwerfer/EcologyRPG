using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EcologyRPG.Core
{
    class PoolObject
    {
        public GameObject obj;
        public TimeSpan timeAdded;
    }
    public class GameObjectPool : IDisposable
    {
        public GameObject prefab;

        Stack<PoolObject> pool;
        bool taskRunning = false;

        public GameObjectPool(GameObject prefab)
        {
            this.prefab = prefab;
            pool = new Stack<PoolObject>();
        }

        GameObject Get()
        {
            if (pool.Count > 0)
            {
                var poolObj = pool.Pop();

                if (taskRunning && pool.Count == 0)
                {
                    taskRunning = false;
                    TaskManager.RemoveAllFromOwner(this);
                }
                return poolObj.obj;
            }
            else
            {
                var obj = Instantiate(prefab);
                return obj;
            }
        }

        public GameObject GetObject(Transform parent)
        {
            var obj = Get();
            obj.transform.SetParent(parent);
            obj.SetActive(true);
            return obj;
        }

        public GameObject GetObject(Vector3 pos, Quaternion rot, Transform parent = null)
        {
            var obj = Get();
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.transform.SetParent(parent);
            obj.SetActive(true);
            return obj;
        }

        public GameObject GetObject()
        {
            var obj = Get();
            obj.SetActive(true);
            return obj;
        }

        public GameObject Instantiate(GameObject prefab)
        {
            var obj = GameObject.Instantiate(prefab);
            obj.name = prefab.name;
            return obj;
        }

        public void ReturnObject(GameObject obj, bool resetParent = false)
        {
            if(resetParent)
            {
                obj.transform.SetParent(null);
            }
            obj.SetActive(false);
            pool.Push(new PoolObject()
            {
                obj = obj,
                timeAdded = DateTime.Now.TimeOfDay
            });

            if(!taskRunning)
            {
                taskRunning = true;
                TaskManager.Add(this, CleanUp, 60f, true);
            }
        }

        public void ClearPool()
        {
            foreach (var obj in pool)
            {
                GameObject.Destroy(obj.obj);
            }
            pool.Clear();
        }

        public void Preload(int count, Transform parent = null)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Push(new PoolObject()
                {
                    obj = obj,
                    timeAdded = DateTime.Now.TimeOfDay
                });
                if(parent != null)
                {
                    obj.transform.SetParent(parent);
                }
            }
        }

        public void CleanUp()
        {
            var list = pool.ToList();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                var obj = list[i];
                if (DateTime.Now.TimeOfDay - obj.timeAdded > TimeSpan.FromMinutes(5))
                {
                    GameObject.Destroy(obj.obj);
                    list.RemoveAt(i);
                }
            }

            pool = new Stack<PoolObject>(list);
        }

        public void Dispose()
        {
            foreach (var obj in pool)
            {
                GameObject.Destroy(obj.obj);
            }
            pool.Clear();
        }
    }
}