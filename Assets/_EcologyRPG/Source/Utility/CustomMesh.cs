using EcologyRPG.Utility.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace EcologyRPG.Utility
{
    [RequireComponent(typeof(MeshFilter))]
    public class CustomMesh : MonoBehaviour
    {
        public const float noiseScale = 0.003f;
        public const float PerturbStrength = 4f;
        protected static Texture2D noiseSource;

        public bool useUV = false;

        protected Mesh mesh;
        protected static ListPool<Vector3> verticesPool = new();
        protected static ListPool<int> trianglesPool = new();
        protected static ListPool<Vector2> uvPool = new();

        protected List<Vector3> vertices;
        protected List<int> triangles;
        protected List<Vector2> uvs;

        protected virtual void Awake()
        {
            if (noiseSource == null)
            {
                noiseSource = Resources.Load<Texture2D>("noise");
            }
            mesh = GetComponent<MeshFilter>().mesh;
        }

        public void Clear()
        {
            mesh.Clear(false);
            vertices = verticesPool.Get();
            triangles = trianglesPool.Get();
            if (useUV)
            {
                uvs = uvPool.Get();
            }
        }

        public void Apply()
        {
            mesh.SetVertices(vertices.ToArray());
            verticesPool.Add(vertices);
            mesh.SetTriangles(triangles.ToArray(), 0);
            trianglesPool.Add(triangles);
            if (useUV)
            {
                mesh.SetUVs(0, uvs);
                uvPool.Add(uvs);
            }
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
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

        public void AddTriangleUV(Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            if (!useUV) return;

            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
        }

        public void AddQuadUV(Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
        {
            if (!useUV) return;

            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
            uvs.Add(uv4);
        }

        public static Vector4 SampleNoise(Vector3 position)
        {
            return noiseSource.GetPixelBilinear(
                position.x * noiseScale,
                position.z * noiseScale
            );
        }

        public static Vector3 Perturb(Vector3 position)
        {
            Vector4 sample = SampleNoise(position);
            position.x += (sample.x * 2f - 1f) * PerturbStrength;
            position.z += (sample.z * 2f - 1f) * PerturbStrength;
            return position;
        }

        protected float ValueToUV(float value, float Range)
        {
            float uv;
            var diff = Mathf.Abs(value) / (Range * 2);
            if (value > 0)
            {
                uv = 0.5f + diff;
            }
            else
            {
                uv = 0.5f - diff;
            }

            return uv;
        }
    }
}