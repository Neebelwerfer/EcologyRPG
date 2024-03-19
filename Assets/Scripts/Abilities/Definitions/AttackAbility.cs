using Character.Abilities;
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

    private void OnEnable()
    {
        if(targetMask.value == 0) LayerMask.NameToLayer("Entity");
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
public class AttackAbilityEditor : Editor
{

    public override void OnInspectorGUI()
    {
        AttackAbility abilityEffect = (AttackAbility)target;
        if(EditorGUILayout.PropertyField(serializedObject.FindProperty("targetMask")))
        {
            EditorUtility.SetDirty(abilityEffect);
        }
        abilityEffect.Range = EditorGUILayout.FloatField("Range", abilityEffect.Range);
        abilityEffect.useMouseDirection = EditorGUILayout.Toggle("Use Mouse Direction", abilityEffect.useMouseDirection);
    }
}
#endif