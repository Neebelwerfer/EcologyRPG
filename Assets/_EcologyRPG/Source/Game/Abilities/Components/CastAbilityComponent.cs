using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Character;
using EcologyRPG.GameSystems.Abilities.Components;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities.Components
{
    public class CastAbilityComponent : CombatAbilityComponent
    {
        public AttackAbilityDefinition _ability;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            cast.owner.StartCoroutine(_ability.HandleCast(cast));
        }

        private void OnDestroy()
        {
            DestroyImmediate(_ability, true);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CastAbilityComponent))]
public class CastAbilityEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CastAbilityComponent component = (CastAbilityComponent)target;
        if(component._ability == null)
        {
          if(GUILayout.Button("Create Ability"))
            {
                component._ability = CreateInstance<AttackAbilityDefinition>();
                component._ability.name = "New Ability";
                AssetDatabase.AddObjectToAsset(component._ability, component);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
        else component._ability = (AttackAbilityDefinition)EditorGUILayout.ObjectField("Ability", component._ability, typeof(AttackAbilityDefinition), false);
    }
}
#endif