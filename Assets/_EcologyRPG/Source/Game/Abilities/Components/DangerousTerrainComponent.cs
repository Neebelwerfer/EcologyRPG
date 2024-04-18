using EcologyRPG.Core;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;
using EcologyRPG.Utility.Collections;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities.Components
{
    public class DangerousTerrainComponent : VisualAbilityComponent
    {
        public float Radius = 2;
        public float Offset = 0.05f;
        public float Duration = 5;
        public bool OnTarget = true;

        static MeshPool<DangerousTerrainMesh> pool;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Setup()
        {
            pool = new MeshPool<DangerousTerrainMesh>(Game.Settings.dangerousTerrainMesh);
        }

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            DangerousTerrainMesh mesh = pool.Get();
            mesh.transform.position = GetPoint(cast, target);
            mesh.SetRadiusAndOffset(Radius, Offset);
            mesh.gameObject.SetActive(true);
            TaskManager.Add(this, () => pool.Return(mesh), Duration);
        }

        Vector3 GetPoint(CastInfo cast, BaseCharacter target)
        {
            if(target != null && OnTarget)
            {
                return target.Transform.Position;
            }
            else if(!OnTarget)
            {
                return cast.owner.Transform.Position;
            }
            else
            {
                return cast.castPos;
            }
        }
    }
}