using Character.Abilities;
using Character.Abilities.AbilityEffects;
using UnityEditor;
using UnityEngine;

namespace Character.Abilities.AbilityEffects
{
    public class CastAbilityEffect : CombatAbilityEffect
    {
        public AttackAbilityHolder _ability;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            cast.owner.StartCoroutine(_ability.HandleCast(cast));
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
                effect._ability = CreateInstance<AttackAbilityHolder>();
                effect._ability.name = "New Ability";
                AssetDatabase.AddObjectToAsset(effect._ability, effect);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
        else effect._ability = (AttackAbilityHolder)EditorGUILayout.ObjectField("Ability", effect._ability, typeof(AttackAbilityHolder), false);
    }
}
#endif