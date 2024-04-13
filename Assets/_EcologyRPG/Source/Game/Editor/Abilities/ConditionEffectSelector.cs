using EcologyRPG.GameSystems.Abilities.Components;
using EcologyRPG.GameSystems.Abilities.Conditions;
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
    public ConditionAbilityComponent effect;
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
                    effect.DebuffCondition = ScriptableObject.CreateInstance<KnockCondition>();
                }
                else if(effectType == ConditionType.DotCondition)
                {
                    effect.DebuffCondition = ScriptableObject.CreateInstance<DamageOverTime>();
                }
                else if(effectType == ConditionType.SlowCondition)
                {
                    effect.DebuffCondition = ScriptableObject.CreateInstance<SlowCondition>();
                }
                else if (effectType == ConditionType.StunCondition)
                {
                    effect.DebuffCondition = ScriptableObject.CreateInstance<Stun>();
                }
                effect.DebuffCondition.name = effectType.ToString();
                AssetDatabase.AddObjectToAsset(effect.DebuffCondition, effect);
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