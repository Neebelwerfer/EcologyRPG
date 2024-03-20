using Character.Abilities.AbilityEffects;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class EditListEffect
{
    ScriptableObject owner;
    List<AbilityEffect> Effects;

    public EditListEffect(ScriptableObject owner, List<AbilityEffect> effects)
    {
        this.owner = owner;
        Effects = effects;
    }

    public void Add(AbilityEffect effect)
    {
        effect.name = effect.GetType().Name;
        Effects.Add(effect);
        AssetDatabase.AddObjectToAsset(effect, owner);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(owner);
    }
}

public enum DisplayEffectType
{
    All,
    Combat,
    Visual
}

public struct DisplayEffect
{
    public string name;
    public Type ClassType;
    public DisplayEffectType DisplayType;
}

public class AbilityEffectEditor : EditorWindow
{
    
    static List<DisplayEffect> effects = new List<DisplayEffect>
    {
        new DisplayEffect { name = "Damage", ClassType = typeof(DamageAbilityEffect), DisplayType = DisplayEffectType.Combat },
        new DisplayEffect { name = "Condition", ClassType = typeof(ConditionAbilityEffect), DisplayType = DisplayEffectType.Combat },
        new DisplayEffect { name = "Ability Cast", ClassType = typeof(CastAbilityEffect), DisplayType = DisplayEffectType.Combat },
        new DisplayEffect { name = "VFX", ClassType = typeof(VFXAbilityEffect), DisplayType = DisplayEffectType.Visual}
    };

    DisplayEffect[] displayEffects;
    public EditListEffect editedEffects;
    public DisplayEffectType displayEffectType;


    int index = 0;

    public void Init(EditListEffect editEffects, DisplayEffectType displayEffectType)
    {
        editedEffects = editEffects;
        this.displayEffectType = displayEffectType;
        if (displayEffectType == DisplayEffectType.All)
        {
            displayEffects = effects.ToArray();
        }
        else
        {
            displayEffects = effects.FindAll(x => x.DisplayType == displayEffectType).ToArray();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Ability Effect Editor");
        index = EditorGUILayout.Popup(index, Array.ConvertAll(displayEffects, x => x.name));
        if (GUILayout.Button("Add Ability Effect"))
        {
            editedEffects.Add(GetEffect(displayEffects[index]));
        }
    }
    AbilityEffect GetEffect(DisplayEffect effect)
    {
        return (AbilityEffect)Activator.CreateInstance(effect.ClassType);
    }

    public static void Display(string Header, List<AbilityEffect> effects, ScriptableObject owner, DisplayEffectType displayEffectType)
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField(Header, EditorStyles.boldLabel);
        var extraName = displayEffectType == DisplayEffectType.All ? "Any" : displayEffectType.ToString();
        if (GUILayout.Button("Add " + extraName + " Effect"))
        {
            var window = EditorWindow.GetWindow<AbilityEffectEditor>();
            window.Init(new EditListEffect(owner, effects), displayEffectType);
            window.Show();
        }

        AbilityEffect effectToDelete = null;
        foreach (var effect in effects)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Effect: " + effect.name, EditorStyles.boldLabel);
            if (GUILayout.Button("Remove"))
            {
                effectToDelete = effect;
            }
            EditorGUILayout.EndHorizontal();
            var e = Editor.CreateEditor(effect);
            e.OnInspectorGUI();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        if(effectToDelete != null)
        {
            DestroyImmediate(effectToDelete, true);
            effects.Remove(effectToDelete);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }
}

