using EcologyRPG.Core.Abilities;
using EcologyRPG.Utility;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities
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

        private void OnEnable()
        {
            if (Physics.Raycast(transform.position + (Vector3.up * 10), Vector3.down, out RaycastHit hit, 1000, AbilityManager.WalkableGroundLayer))
            {
                transform.position = hit.point;
            }
            TriangulateDangerousTerrain(Radius, Offset);
        }

        public void SetRadiusAndOffset(float radius, float offset)
        {
            Radius = radius;
            Offset = offset;
        }

        public void TriangulateDangerousTerrain(float Radius, float Offset)
        {
           
            Clear();
            Vector3 position = Vector3.zero;
            Vector3 center = Vector3.zero + Vector3.up * Offset;
            for (int i = 0; i < 360; i += 10)
            {
                Vector3 v2 = Perturb(new Vector3(Mathf.Cos((i + 10) * Mathf.Deg2Rad) * Radius, 0, Mathf.Sin((i + 10) * Mathf.Deg2Rad) * Radius) + position + Vector3.up * Offset);
                Vector3 v3 = Perturb(new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * Radius, 0, Mathf.Sin(i * Mathf.Deg2Rad) * Radius) + position + Vector3.up * Offset);

                if(!Fit(ref v2)) continue;
                if(!Fit(ref v3)) continue;

                AddTriangle(center, v2, v3);
                AddTriangleUV(new Vector2(0.5f, 0.5f), new Vector2(ValueToUV(v2.x), ValueToUV(v2.z)), new Vector2((ValueToUV(v3.x)), ValueToUV(v3.z)));
            }
            Apply();
            meshCollider.sharedMesh = mesh;
        }

        bool Fit(ref Vector3 v)
        {
            bool onGround = true;
            int counter = 0;
            var sizeDelta = v * 0.1f;
            while (!Physics.Raycast(v + transform.position, Vector3.down, 1000, AbilityManager.WalkableGroundLayer))
            {
                v -= sizeDelta;
                counter++;
                if (counter > 10)
                {
                    onGround = false;
                    break;
                }
            }
            
            if (onGround)
            {
                if(Physics.Raycast(v + transform.position, Vector3.down, out RaycastHit hit, 1000, AbilityManager.WalkableGroundLayer))
                {
                    v = hit.point - transform.position + Vector3.up * Offset;
                }
            }
            return onGround;
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