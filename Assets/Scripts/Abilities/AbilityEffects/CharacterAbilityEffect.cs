using Character.Abilities.AbilityEffects;
using UnityEditor;
using UnityEngine;

namespace Character.Abilities.AbilityEffects
{
    public class CharacterAbilityEffect : CombatAbilityEffect
    {
        public DebuffCondition DebuffEffect;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            target.ApplyEffect(cast, Instantiate(DebuffEffect));
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterAbilityEffect))]
public class CharacterAbilityEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CharacterAbilityEffect effect = (CharacterAbilityEffect)target;
        if(effect.DebuffEffect == null)
        {
          if(GUILayout.Button("Create Debuff Effect"))
            {
                var window = CreateInstance<CharacterEffectSelector>();
                window.effect = effect;
                window.Show();
            }
        }
        else
        effect.DebuffEffect = (DebuffCondition)EditorGUILayout.ObjectField("Debuff Effect", effect.DebuffEffect, typeof(DebuffCondition), false);
    }
}
#endif