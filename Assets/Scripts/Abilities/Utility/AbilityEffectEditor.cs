using Character.Abilities.AbilityEffects;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EditAbility
{
    protected BaseAbility owner;
    public abstract void Add(AbilityEffect effect);
}

public class EditSingleEffect : EditAbility
{
    FieldInfo field;

    public EditSingleEffect(BaseAbility owner, string fieldName)
    {
        this.owner = owner;
        field = owner.GetType().GetField(fieldName);
    }

    public override void Add(AbilityEffect effect)
    {
        effect.name = effect.GetType().Name;
        field.SetValue(owner, effect);
        AssetDatabase.AddObjectToAsset(effect, owner);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(owner);
    }
}

public class EditListEffect : EditAbility
{
    List<AbilityEffect> Effects;

    public EditListEffect(BaseAbility owner, List<AbilityEffect> effects)
    {
        this.owner = owner;
        Effects = effects;
    }

    public override void Add(AbilityEffect effect)
    {
        effect.name = effect.GetType().Name;
        Effects.Add(effect);
        AssetDatabase.AddObjectToAsset(effect, owner);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(owner);
    }
}

public class AbilityEffectEditor : EditorWindow
{
    enum AbilityEffectType
    {
        AbilityCast,
        Damage,
        CharacterEffect,
        VFX,
    }
    public EditAbility editedEffects;

    AbilityEffectType type;

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Ability Effect Editor");
        type = (AbilityEffectType) EditorGUILayout.EnumPopup("Ability Effect Type:", type);
        if (GUILayout.Button("Add Ability Effect"))
        {
            editedEffects.Add(GetEffect(type));
        }
    }
    AbilityEffect GetEffect(AbilityEffectType type)
    {
        if(type == AbilityEffectType.Damage)
        {
            return CreateInstance<DamageAbilityEffect>();
        } 
        else if(type == AbilityEffectType.CharacterEffect)
        {
            return CreateInstance<CharacterAbilityEffect>();
        }
        else if(type == AbilityEffectType.AbilityCast)
        {
            return CreateInstance<CastAbilityEffect>();
        }
        else if (type == AbilityEffectType.VFX)
        {
            return CreateInstance<VFXAbilityEffect>();
        }
        return null;
    }
    
    public static void Display(ref bool foldOut, ref int index, List<AbilityEffect> effects, BaseAbility owner)
    {
        foldOut = EditorGUILayout.BeginFoldoutHeaderGroup(foldOut, "On Hit Effects");
        if (foldOut)
        {
            foreach (var effect in effects)
            {
                EditorGUILayout.ObjectField(effect, typeof(AbilityEffect), false);
            }
            if (GUILayout.Button("Add Effect"))
            {
                var window = EditorWindow.GetWindow<AbilityEffectEditor>();
                window.editedEffects = new EditListEffect(owner, effects);
                window.Show();
            }

            if (effects.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Remove All"))
                {
                    foreach (var effect in effects)
                    {
                        DestroyImmediate(effect, true);
                    }
                    effects.Clear();
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }

                if (GUILayout.Button("Remove Last"))
                {
                    DestroyImmediate(effects[effects.Count - 1], true);
                    effects.RemoveAt(effects.Count - 1);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                var ind = EditorGUILayout.IntField("Index", index);
                if (ind > effects.Count)
                {
                    ind = effects.Count;
                }
                index = ind;
                if (GUILayout.Button("Remove Index"))
                {
                    DestroyImmediate(effects[index], true);
                    effects.RemoveAt(index);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}

