using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities.Components
{
    public class VFXAbilityComponent : VisualAbilityComponent
    {
        public GameObject vfxPrefab;
        public float duration = 1f;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            GameObject effect;
            if(target != null)
            {
                effect = Instantiate(vfxPrefab, target.Transform.Position, Quaternion.identity);
            }
            else
            {
                effect = Instantiate(vfxPrefab, cast.castPos, Quaternion.identity);
            }
            Destroy(effect, duration);
        }

        public override AbilityComponent GetCopy(Object owner)
        {
            var copy = CreateInstance<VFXAbilityComponent>();
            copy.name = name;
            copy.vfxPrefab = vfxPrefab;
            copy.duration = duration;
            AssetDatabase.AddObjectToAsset(copy, owner);
            return copy;
        }
    }
}