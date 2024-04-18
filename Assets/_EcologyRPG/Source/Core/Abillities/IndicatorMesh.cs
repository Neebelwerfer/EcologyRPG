using EcologyRPG.Core.Character;
using EcologyRPG.Utility;
using EcologyRPG.Utility.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    [RequireComponent(typeof(MeshFilter))]
    public class IndicatorMesh : CustomMesh
    {       
        BaseCharacter owner;
        Material material;

        protected override void Awake()
        {
            base.Awake();
            material = GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            if (owner != null)
            {
                if(Physics.Raycast(owner.Transform.Position, Vector3.down, out RaycastHit hit, 1000, AbilityManager.WalkableGroundLayer))
                {
                    transform.position = hit.point;
                }
            }
        }

        public void SetOwner(BaseCharacter owner)
        {
            this.owner = owner;
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
            Vector3 v1 = Vector3.zero + AbilityManager.IndicatorOffset;
            for (float i = -(angle/2); i < angle/2; i += 10)
            {
                Vector3 v2 = Quaternion.AngleAxis(i, Vector3.up) * dir * range + AbilityManager.IndicatorOffset;
                Vector3 v3 = Quaternion.AngleAxis(i + 10, Vector3.up) * dir * range + AbilityManager.IndicatorOffset;
                AddTriangle(v1, v2, v3);
            }
        }

        public void TriangulateCircle(Vector3 forward, float radius)
        {
            Vector3 prev = Quaternion.AngleAxis(0, Vector3.up) * forward * radius + AbilityManager.IndicatorOffset;
            Vector3 start = Vector3.zero;
            
            for (int i = 0; i < 370; i += 10)
            {
                Vector3 next = Quaternion.AngleAxis(i, Vector3.up) * forward * radius + AbilityManager.IndicatorOffset;
                AddTriangle(start, prev, next);
                prev = next;
            }
        }
    }
}