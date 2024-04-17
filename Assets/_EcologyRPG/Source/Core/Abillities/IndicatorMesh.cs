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

        Material material;

        private void Awake()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            material = GetComponent<MeshRenderer>().material;
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

        public void SetColor(Color color)
        {
            material.color = color;
        }


        public void TriangulateBox(Vector3 dir, float range, float width)
        {
            var right = Quaternion.AngleAxis(90, Vector3.up) * dir;
            Vector3 v1 = -right * (width / 2) + AbilityManager.IndicatorOffset;
            Vector3 v2 = right * (width / 2) + AbilityManager.IndicatorOffset;
            Vector3 v3 = -right * (width / 2) + dir * range + AbilityManager.IndicatorOffset;
            Vector3 v4 = right * (width / 2) + dir * range + AbilityManager.IndicatorOffset;
            AddQuad(v1, v2, v3, v4);
        }

        public void TriangulateCone(Vector3 dir, float range, float angle)
        {
            var angledDir = Quaternion.AngleAxis(angle / 2, Vector3.up) * dir;
            var oppositeAngledDir = Quaternion.AngleAxis(-angle / 2, Vector3.up) * dir;

            Vector3 v1 = Vector3.zero + AbilityManager.IndicatorOffset;
            Vector3 v2 = dir * range + AbilityManager.IndicatorOffset;
            Vector3 v3 = angledDir * Mathf.Sqrt(range * 2) + AbilityManager.IndicatorOffset;
            Vector3 v4 = oppositeAngledDir * Mathf.Sqrt(range * 2) + AbilityManager.IndicatorOffset;
            AddTriangle(v1, v2, v3);
            AddTriangle(v1, v4, v2);
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