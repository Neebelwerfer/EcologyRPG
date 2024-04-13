using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities.Components
{
    public class ConditionAbilityComponent : CombatAbilityComponent
    {
        public DebuffCondition DebuffCondition;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            target.ApplyCondition(cast, Instantiate(DebuffCondition));
        }

        private void OnDestroy()
        {
            DestroyImmediate(DebuffCondition, true);
        }

        public override void Delete()
        {
            DestroyImmediate(DebuffCondition, true);
            base.Delete();
        }

#if UNITY_EDITOR
        override public AbilityComponent GetCopy(Object owner)
        {
            var copy = CreateInstance<ConditionAbilityComponent>();
            copy.name = name;
            AssetDatabase.AddObjectToAsset(copy, owner);
            var debuffConditionCopy = Instantiate(DebuffCondition);
            copy.DebuffCondition = debuffConditionCopy;
            AssetDatabase.AddObjectToAsset(debuffConditionCopy, copy);
            return copy;
        }
#endif

    }
}