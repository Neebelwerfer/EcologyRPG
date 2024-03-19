using Character.Abilities.AbilityEffects;
using UnityEditor;
using UnityEngine;

public class CharacterEffectSelector : EditorWindow
{
    enum EffectType
    {
        KnockEffect,
        DoTEffect,
        SlowEffect,
        StunEffect,
    }
    public CharacterAbilityEffect effect;
    EffectType effectType;

    private void OnGUI()
    {
        effectType = (EffectType)EditorGUILayout.EnumPopup("Effect Type", effectType);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Select"))
        {
            if(effect != null)
            {
                if(effectType == EffectType.KnockEffect)
                {
                    effect.DebuffEffect = ScriptableObject.CreateInstance<Knock>();
                }
                else if(effectType == EffectType.DoTEffect)
                {
                    effect.DebuffEffect = ScriptableObject.CreateInstance<DamageOverTime>();
                }
                else if(effectType == EffectType.SlowEffect)
                {
                    effect.DebuffEffect = ScriptableObject.CreateInstance<SlowEffect>();
                }
                else if (effectType == EffectType.StunEffect)
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