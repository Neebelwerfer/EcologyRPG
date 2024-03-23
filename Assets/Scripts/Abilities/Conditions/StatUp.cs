using Character;
using Character.Abilities;
using Character.Attributes;
using UnityEditor;
using UnityEngine;

public class StatUp : BuffCondition
{
    [StatAttribute(StatType.Stat)]
    public string StatName;
    public StatModType ModType;
    public float Value;

    static UniqueStatModificationHandler UniqueStatModHandler;

    public override void OnApply(CastInfo Caster, BaseCharacter target)
    {
        UniqueStatModHandler = new UniqueStatModificationHandler(StatName, ModType, true);
        UniqueStatModHandler.AddValue(target, this, Value);
    }

    public override void OnReapply(BaseCharacter target)
    {
        remainingDuration = duration;
    }

    public override void OnRemoved(BaseCharacter target)
    {
        UniqueStatModHandler.RemoveValue(target, this);
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(StatUp))]
public class StatUpEditor : ConditionEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        StatUp ability = (StatUp)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("StatName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ModType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Value"));
        serializedObject.ApplyModifiedProperties();
    }
}
#endif