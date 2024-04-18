using EcologyRPG.Core.Abilities;
using EcologyRPG.Utility;
using UnityEngine;

namespace EcologyRPG.Scripts
{
    [RequireComponent(typeof(MeshCollider))]
    public class DangerousTerrainMesh : CustomMesh
    {
        public float Radius = 2;
        public float Offset = 0.05f;

        MeshCollider meshCollider;

        protected override void Awake()
        {
            base.Awake();
            meshCollider = GetComponent<MeshCollider>();
        }

        private void Start()
        {
            if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1000, AbilityManager.WalkableGroundLayer))
            {
                transform.position = hit.point;
            }
            TriangulateDangerousTerrain(Vector3.zero);
        }

        public void TriangulateDangerousTerrain(Vector3 position)
        {
            Clear();
            Vector3 v1 = Vector3.zero + Vector3.up * Offset;
            for (int i = 0; i < 360; i += 10)
            {
                Vector3 v2 = Perturb(new Vector3(Mathf.Cos((i + 10) * Mathf.Deg2Rad) * Radius, 0, Mathf.Sin((i + 10) * Mathf.Deg2Rad) * Radius) + position + Vector3.up * Offset);
                Vector3 v3 = Perturb(new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * Radius, 0, Mathf.Sin(i * Mathf.Deg2Rad) * Radius) + position + Vector3.up * Offset);
                AddTriangle(v1, v2, v3);
                AddTriangleUV(new Vector2(0.5f, 0.5f), new Vector2(ValueToUV(v2.x), ValueToUV(v2.z)), new Vector2((ValueToUV(v3.x)), ValueToUV(v3.z)));
            }
            Apply();
            meshCollider.sharedMesh = mesh;
        }

        float ValueToUV(float value)
        {
            float uv;
            var diff = Mathf.Abs(value) / (Radius * 2);
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