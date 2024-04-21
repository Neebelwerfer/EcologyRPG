using EcologyRPG.Core.Abilities;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    public class IndicatorMeshContext
    {
        IndicatorMesh mesh;

        public IndicatorMeshContext(IndicatorMesh mesh)
        {
            this.mesh = mesh;
        }

        public void Destroy()
        {
            Object.Destroy(mesh.gameObject);
        }
    }
}