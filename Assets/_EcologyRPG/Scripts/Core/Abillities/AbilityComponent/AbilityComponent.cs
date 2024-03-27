
using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Abilities.AbilityComponents
{
    public abstract class AbilityComponent : ScriptableObject
    {
        protected const string _path = "AbilityEffects/";

        public abstract void ApplyEffect(CastInfo cast, BaseCharacter target);

        [ContextMenu("Delete")]
        protected void Delete()
        {
            DestroyImmediate(this, true);
        }
    }
}