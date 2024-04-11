using UnityEditor;

namespace EcologyRPG.GameSystems.Abilities.Conditions
{
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
}
