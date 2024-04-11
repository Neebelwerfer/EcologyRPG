using EcologyRPG.GameSystems.NPC;
using UnityEditor;

[CustomEditor(typeof(NPCAbilityGroup))]
public class NPCAbilityGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("changeMode"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilities"), true);
        serializedObject.ApplyModifiedProperties();
    }
}