
using UnityEngine;

namespace Character.Abilities.AbilityEffects
{
    public abstract class AbilityEffect : ScriptableObject
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