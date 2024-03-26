using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class GameObjectPool
    {
        public GameObject prefab;

        readonly Stack<GameObject> pool;

        public GameObjectPool(GameObject prefab)
        {
            this.prefab = prefab;
            pool = new Stack<GameObject>();
        }

        public GameObject GetObject(Transform parent)
        {
            var obj = GetObject();
            obj.transform.SetParent(parent);
            return obj;
        }

        public GameObject GetObject(Vector3 pos, Quaternion rot, Transform parent = null)
        {
            var obj = GetObject();
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.transform.SetParent(parent);
            return obj;
        }

        public GameObject GetObject()
        {
            if(pool.Count > 0)
            {
                var obj = pool.Pop();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                var obj = Instantiate(prefab);
                return obj;
            }
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
            pool.Push(obj);
        }

        public void ClearPool()
        {
            foreach (var obj in pool)
            {
                GameObject.Destroy(obj);
            }
            pool.Clear();
        }

        public void Preload(int count, Transform parent = null)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Push(obj);
                if(parent != null)
                {
                    obj.transform.SetParent(parent);
                }
            }
        }
    }
}