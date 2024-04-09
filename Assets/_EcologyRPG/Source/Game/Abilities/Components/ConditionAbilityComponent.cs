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

        override public AbilityComponent GetCopy(Object owner)
        {
            var copy = CreateInstance<ConditionAbilityComponent>();
            AssetDatabase.AddObjectToAsset(copy, owner);
            var debuffConditionCopy = Instantiate(DebuffCondition);
            copy.DebuffCondition = debuffConditionCopy;
            AssetDatabase.AddObjectToAsset(debuffConditionCopy, copy);
            return copy;
        }
    }
}