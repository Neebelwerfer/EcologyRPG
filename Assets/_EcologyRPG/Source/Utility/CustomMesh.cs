using EcologyRPG.Utility.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Utility
{
    [RequireComponent(typeof(MeshFilter))]
    public class CustomMesh : MonoBehaviour
    {
        public Mesh mesh;
        protected static ListPool<Vector3> verticesPool = new();
        protected static ListPool<int> trianglesPool = new();

        protected List<Vector3> vertices;
        protected List<int> triangles;

        protected virtual void Awake()
        {
            mesh = GetComponent<MeshFilter>().mesh;
        }

        public void Clear()
        {
            mesh.Clear();
            vertices = verticesPool.Get();
            triangles = trianglesPool.Get();
        }

        public void Apply()
        {
            mesh.vertices = vertices.ToArray();
            verticesPool.Add(vertices);
            mesh.triangles = triangles.ToArray();
            trianglesPool.Add(triangles);

            mesh.RecalculateNormals();
        }

        public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = vertices.Count;
            vertices.Add((v1));
            vertices.Add((v2));
            vertices.Add((v3));
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
        }

        public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }
    }
}