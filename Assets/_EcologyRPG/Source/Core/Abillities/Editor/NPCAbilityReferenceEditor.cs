using EcologyRPG.GameSystems.NPC;
using UnityEditor;

[CustomEditor(typeof(NPCAbilityReference))]
public class NPCAbilityReferenceEditor : AbilityReferenceEditor
{
    public override void OnInspectorGUI()
    {
        NPCAbilityReference ability = (NPCAbilityReference)target;
        DrawAbilityReferenceValues();
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.Range)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minRange"));
        DrawCanActivateString();
        DrawGlobalOverrides();
        serializedObject.ApplyModifiedProperties();
    }
}