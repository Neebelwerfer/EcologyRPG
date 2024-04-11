using EcologyRPG.Core.Character;
using UnityEditor;

namespace EcologyRPG.GameSystems.Abilities.Conditions
{
#if UNITY_EDITOR
    [CustomEditor(typeof(KnockCondition))]
    public class KnockConditionEditor : ConditionEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("KnockBackDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("knockType"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}