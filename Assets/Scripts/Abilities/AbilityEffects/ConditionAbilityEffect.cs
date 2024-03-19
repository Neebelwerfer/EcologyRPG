using Character.Abilities.AbilityEffects;
using UnityEditor;
using UnityEngine;

namespace Character.Abilities.AbilityEffects
{
    public class ConditionAbilityEffect : CombatAbilityEffect
    {
        public DebuffCondition DebuffEffect;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            target.ApplyEffect(cast, Instantiate(DebuffEffect));
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ConditionAbilityEffect))]
public class ConditionAbilityEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ConditionAbilityEffect effect = (ConditionAbilityEffect)target;
        effect.DebuffEffect = (DebuffCondition)EditorGUILayout.ObjectField("Debuff Condition", effect.DebuffEffect, typeof(DebuffCondition), false);
        if (effect.DebuffEffect == null)
        {
            if(GUILayout.Button("Create Debuff Condition"))
            {
                var window = CreateInstance<ConditionEffectSelector>();
                window.effect = effect;
                window.Show();
            }
        }
    }
}
#endif