using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities.Conditions
{
    public class StatDown : DebuffCondition, IUpdateCondition
    {
        [StatAttribute(StatType.Stat)]
        public string StatName;
        [SerializeField] StatModType ModType;
        public float Value;

        static UniqueStatModificationHandler UniqueStatModHandler;

        public override void OnApply(CastInfo Caster, BaseCharacter target)
        {
            UniqueStatModHandler = new UniqueStatModificationHandler(StatName, ModType, false);
            UniqueStatModHandler.AddValue(target, this, Value);
        }

        public override void OnReapply(BaseCharacter target)
        {

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
    [CustomEditor(typeof(StatDown))]
    public class StatDownEditor : ConditionEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            StatDown ability = (StatDown)target;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("StatName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ModType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Value"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}