using Character.Abilities;
using Character.Abilities.AbilityEffects;
using UnityEditor;
using UnityEngine;

namespace Character.Abilities.AbilityEffects
{
    public class CastAbilityEffect : CombatAbilityEffect
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
[CustomEditor(typeof(CastAbilityEffect))]
public class CastAbilityEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CastAbilityEffect effect = (CastAbilityEffect)target;
        if(effect._ability == null)
        {
          if(GUILayout.Button("Create Ability"))
            {
                effect._ability = CreateInstance<AttackAbilityDefinition>();
                effect._ability.name = "New Ability";
                AssetDatabase.AddObjectToAsset(effect._ability, effect);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
        else effect._ability = (AttackAbilityDefinition)EditorGUILayout.ObjectField("Ability", effect._ability, typeof(AttackAbilityDefinition), false);
    }
}
#endif