using Character;
using Character.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utility;



public abstract class Condition : ScriptableObject
{
    protected const string CharacterEffectPath = "Condition/";

    [ReadOnlyString]
    public string ID;
    public string displayName;
    [Min(0)] public float duration;
    [HideInInspector] public float remainingDuration;
    [HideInInspector] public BaseCharacter Owner;

    private void OnValidate()
    {       
        if(string.IsNullOrEmpty(ID))
            ID = Guid.NewGuid().ToString();

        if (string.IsNullOrEmpty(displayName))
        {
            displayName = this.GetType().Name;
        }
    }

    public abstract void OnApply(CastInfo Caster, BaseCharacter target);

    public abstract void OnReapply(BaseCharacter target);

    public abstract void OnUpdate(BaseCharacter target, float deltaTime);

    public abstract void OnRemoved(BaseCharacter target);

    protected static DamageInfo CalculateDamage(BaseCharacter Owner, DamageType type, float damage, bool allowVariance = false) => BaseAbility.CalculateDamage(Owner, type, damage, allowVariance);

    [ContextMenu("Delete")]
    protected virtual void Delete()
    {
        DestroyImmediate(this, true);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Condition), true)]
public class ConditionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var condition = (Condition)target;
        EditorGUILayout.LabelField("Condition Settings", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("ID", condition.ID);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("displayName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));
        serializedObject.ApplyModifiedProperties();
    }
}
#endif