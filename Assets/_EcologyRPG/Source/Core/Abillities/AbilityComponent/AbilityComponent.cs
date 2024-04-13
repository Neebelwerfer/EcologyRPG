
using EcologyRPG.Core.Character;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.Core.Abilities.AbilityComponents
{
    public abstract class AbilityComponent : ScriptableObject
    {
        protected const string _path = "AbilityEffects/";

        public abstract void ApplyEffect(CastInfo cast, BaseCharacter target);

#if UNITY_EDITOR
        public virtual AbilityComponent GetCopy(Object owner)
        {
            var newEffect = Instantiate(this);
            AssetDatabase.AddObjectToAsset(newEffect, owner);
            return newEffect;
        }
#endif

        [ContextMenu("Delete")]
        public virtual void Delete()
        {
            DestroyImmediate(this, true);
        }
    }
}