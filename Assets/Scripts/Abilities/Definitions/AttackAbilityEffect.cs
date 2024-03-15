using Character.Abilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class AttackAbilityEffect : AbilityEffect
{
    [Header("Attack Ability")]
    [Tooltip("The layermask that the ability will target")]
    public LayerMask targetMask;
    [Tooltip("The range of the ability")]
    public float Range;
    [Tooltip("Use the mouse direction as the direction of the ability instead of the cast position. This is useful for abilities that are casted in the direction of the mouse instead of the position of the caster.")]
    public bool useMouseDirection;

    protected Vector3 GetDir(CastInfo castInfo)
    {
        if (castInfo.dir != Vector3.zero)
        {
            return castInfo.dir;
        }
        
        if (useMouseDirection)
        {
            return (castInfo.mousePoint - castInfo.owner.CastPos).normalized;
        }
        else
        {
            return castInfo.owner.Forward;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AttackAbilityEffect), false)]
public class AttackAbilityEffectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        AttackAbilityEffect abilityEffect = (AttackAbilityEffect)target;
        if(EditorGUILayout.PropertyField(serializedObject.FindProperty("targetMask")))
        {
            EditorUtility.SetDirty(abilityEffect);
        }
        abilityEffect.Range = EditorGUILayout.FloatField("Range", abilityEffect.Range);
        abilityEffect.useMouseDirection = EditorGUILayout.Toggle("Use Mouse Direction", abilityEffect.useMouseDirection);
    }
}
#endif