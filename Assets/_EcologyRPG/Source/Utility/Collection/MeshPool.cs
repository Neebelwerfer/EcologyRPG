using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Utility.Collections
{
    public class MeshPool<T> where T : CustomMesh
    {
        readonly Stack<T> stack = new Stack<T>();
        readonly T prefab;

        public MeshPool(T prefab)
        {
            this.prefab = prefab;

        }

        ~MeshPool()
        {
            foreach (var mesh in stack)
            {
                Object.Destroy(mesh.gameObject);
            }
        }

        public T Get()
        {
            T mesh;
            if(stack.Count > 0)
            {
                mesh = stack.Pop();
            }
            else
            {
                mesh = Object.Instantiate(prefab);
                mesh.gameObject.SetActive(false);
            }
            return mesh;
        }

        public void Return(T mesh)
        {
            mesh.gameObject.SetActive(false);
            stack.Push(mesh);
        }
    }
}