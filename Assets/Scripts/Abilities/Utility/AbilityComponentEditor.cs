using Character.Abilities.AbilityComponents;
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
    List<AbilityComponent> Effects;

    public EditListEffect(ScriptableObject owner, List<AbilityComponent> effects)
    {
        this.owner = owner;
        Effects = effects;
    }

    public void Add(AbilityComponent effect)
    {
        effect.name = effect.GetType().Name;
        Effects.Add(effect);
        AssetDatabase.AddObjectToAsset(effect, owner);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(owner);
    }
}

public enum DisplayComponentType
{
    All,
    Combat,
    Visual
}

public struct DisplayComponent
{
    public string name;
    public Type ClassType;
    public DisplayComponentType DisplayType;
}

public class AbilityComponentEditor : EditorWindow
{
    
    static List<DisplayComponent> effects = new List<DisplayComponent>
    {
        new DisplayComponent { name = "Damage", ClassType = typeof(AbilityDamageComponent), DisplayType = DisplayComponentType.Combat },
        new DisplayComponent { name = "Condition", ClassType = typeof(ConditionAbilityComponent), DisplayType = DisplayComponentType.Combat },
        new DisplayComponent { name = "Ability Cast", ClassType = typeof(CastAbilityComponent), DisplayType = DisplayComponentType.Combat },
        new DisplayComponent { name = "VFX", ClassType = typeof(VFXAbilityComponent), DisplayType = DisplayComponentType.Visual}
    };

    DisplayComponent[] displayEffects;
    public EditListEffect editedEffects;
    public DisplayComponentType displayEffectType;


    int index = 0;

    public void Init(EditListEffect editEffects, DisplayComponentType displayEffectType)
    {
        editedEffects = editEffects;
        this.displayEffectType = displayEffectType;
        if (displayEffectType == DisplayComponentType.All)
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
        EditorGUILayout.LabelField("Ability Component Editor");
        index = EditorGUILayout.Popup(index, Array.ConvertAll(displayEffects, x => x.name));
        if (GUILayout.Button("Add Ability Component"))
        {
            editedEffects.Add(GetEffect(displayEffects[index]));
        }
    }
    AbilityComponent GetEffect(DisplayComponent effect)
    {
        return (AbilityComponent)Activator.CreateInstance(effect.ClassType);
    }

    public static void Display(string Header, List<AbilityComponent> components, ScriptableObject owner, DisplayComponentType displayComponentType)
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField(Header, EditorStyles.boldLabel);
        var extraName = displayComponentType == DisplayComponentType.All ? "Any" : displayComponentType.ToString();
        if (GUILayout.Button("Add " + extraName + " Component"))
        {
            var window = EditorWindow.GetWindow<AbilityComponentEditor>();
            window.Init(new EditListEffect(owner, components), displayComponentType);
            window.Show();
        }

        AbilityComponent effectToDelete = null;
        components.RemoveAll(x => x == null);
        foreach (var effect in components)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Component: " + effect.name, EditorStyles.boldLabel);
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
            components.Remove(effectToDelete);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }
}

