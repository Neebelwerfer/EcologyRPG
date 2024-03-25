using Character;
using Character.Abilities;
using Character.Abilities.AbilityComponents;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class AttackAbility : BaseAbility
{
    [Header("Attack Ability")]
    [Tooltip("The layermask that the ability will target")]
    public LayerMask targetMask;
    [Tooltip("Use the mouse direction as the direction of the ability instead of the cast position. This is useful for abilities that are casted in the direction of the mouse instead of the position of the caster.")]
    public bool useMouseDirection = true;
    [Tooltip("Effects that will be applied to the first target when the ability hits")]
    public List<AbilityComponent> OnFirstHit = new();
    [Tooltip("Effects that will be applied to the target when the ability hits")]
    public List<AbilityComponent> OnHitEffects = new();

    public bool displayFirstHitEffects = true;
    protected bool firstHit = true;

    private void OnEnable()
    {
        if(targetMask.value == 0) LayerMask.NameToLayer("Entity");
    }

    protected virtual Action<CastInfo, BaseCharacter> DefaultOnHitAction()
    {
        return (castInfo, target) =>
        {
            if (firstHit)
            {
                firstHit = false;
                foreach (var effect in OnFirstHit)
                {
                    effect.ApplyEffect(castInfo, target);
                }
            }
            foreach (var effect in OnHitEffects)
            {
                effect.ApplyEffect(castInfo, target);
            }
        };
    }

    protected Vector3 GetDir(CastInfo castInfo)
    {
        if (castInfo.dir != Vector3.zero)
        {
            return castInfo.dir;
        }
        
        if (useMouseDirection)
        {
            var mp = castInfo.mousePoint;
            mp.y = castInfo.owner.CastPos.y;
            return (mp - castInfo.owner.CastPos).normalized;
        }
        else
        {
            return castInfo.owner.Forward;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AttackAbility), false)]
public class AttackAbilityEditor : BaseAbilityEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  
        AttackAbility abilityEffect = (AttackAbility)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetMask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useMouseDirection"));
        if(abilityEffect.displayFirstHitEffects) AbilityComponentEditor.Display("On First Hit Components", abilityEffect.OnFirstHit, abilityEffect, DisplayComponentType.All);
        AbilityComponentEditor.Display("On Hit Components", abilityEffect.OnHitEffects, abilityEffect, DisplayComponentType.All);
    }
}
#endif