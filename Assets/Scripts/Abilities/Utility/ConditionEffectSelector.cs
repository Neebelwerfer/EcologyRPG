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
        StunCondition,
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
                    effect.DebuffEffect = ScriptableObject.CreateInstance<KnockCondition>();
                }
                else if(effectType == ConditionType.DotCondition)
                {
                    effect.DebuffEffect = ScriptableObject.CreateInstance<DamageOverTime>();
                }
                else if(effectType == ConditionType.SlowCondition)
                {
                    effect.DebuffEffect = ScriptableObject.CreateInstance<SlowCondition>();
                }
                else if (effectType == ConditionType.StunCondition)
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