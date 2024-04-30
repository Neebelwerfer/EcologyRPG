using EcologyRPG.Core.Character;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConditionReferenceData))]
public class ConditionReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var id = serializedObject.FindProperty("ID");
        var conditionBehaviourID = serializedObject.FindProperty("ConditionBehaviourID");
        var useFixedUpdate = serializedObject.FindProperty("useFixedUpdate");
        var variableOverrides = serializedObject.FindProperty("variableOverrides");
        var duration = serializedObject.FindProperty("duration");

        EditorGUILayout.LabelField("Id", id.intValue.ToString());
        EditorGUILayout.PropertyField(conditionBehaviourID);
        EditorGUILayout.PropertyField(useFixedUpdate);
        EditorGUILayout.PropertyField(duration);
        EditorGUILayout.PropertyField(variableOverrides, true);

        if(GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}