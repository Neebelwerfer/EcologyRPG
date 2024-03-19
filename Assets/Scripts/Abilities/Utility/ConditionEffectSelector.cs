using Character.Abilities.AbilityEffects;
using UnityEditor;
using UnityEngine;

public class ConditionEffectSelector : EditorWindow
{
    enum ConditionType
    {
        KnockCondition,
        DotCondition,
        SlowCondition,
        StunConditiont,
    }
    public ConditionAbilityEffect effect;
    ConditionType effectType;

    private void OnGUI()
    {
        effectType = (ConditionType)EditorGUILayout.EnumPopup("Condition Type", effectType);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Select"))
        {
            if(effect != null)
            {
                if(effectType == ConditionType.KnockCondition)
                {
                    effect.DebuffEffect = ScriptableObject.CreateInstance<Knock>();
                }
                else if(effectType == ConditionType.DotCondition)
                {
                    effect.DebuffEffect = ScriptableObject.CreateInstance<DamageOverTime>();
                }
                else if(effectType == ConditionType.SlowCondition)
                {
                    effect.DebuffEffect = ScriptableObject.CreateInstance<SlowEffect>();
                }
                else if (effectType == ConditionType.StunConditiont)
                {
                    effect.DebuffEffect = ScriptableObject.CreateInstance<Stun>();
                }
                effect.DebuffEffect.name = effectType.ToString();
                AssetDatabase.AddObjectToAsset(effect.DebuffEffect, effect);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            Close();
        }
        if(GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
}