using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    [RequireComponent(typeof(MeshFilter))]
    public class IndicatorMesh : MonoBehaviour
    {
        Mesh mesh;
        readonly List<Vector3> vertices = new List<Vector3>();
        readonly List<int> triangles = new List<int>();

        private void Awake()
        {
            mesh = GetComponent<MeshFilter>().mesh;
        }

        public void Clear()
        {
            mesh.Clear();
            vertices.Clear();
            triangles.Clear();
        }

        public void Apply()
        {
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
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