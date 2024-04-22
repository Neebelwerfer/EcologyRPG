using EcologyRPG.AbilityScripting;
using UnityEditor;

[CustomEditor(typeof(PlayerAbilityReference))]
public class PlayerAbilityReferenceEditor : AbilityReferenceEditor
{
    public override void OnInspectorGUI()
    {
        var resourceName = serializedObject.FindProperty("ResourceName");
        var resourceCost = serializedObject.FindProperty("ResourceCost");
        EditorGUILayout.PropertyField(resourceName);
        EditorGUILayout.PropertyField(resourceCost);

        DrawAbilityReferenceValues();
        DrawPlayerReferenceValues();
        DrawGlobalOverrides();
        serializedObject.ApplyModifiedProperties();
    }

    public void DrawPlayerReferenceValues()
    {
        var name = serializedObject.FindProperty("Name");
        var description = serializedObject.FindProperty("Description");
        var icon = serializedObject.FindProperty("Icon");
        var useMouseDirection = serializedObject.FindProperty("useMouseDirection");

        EditorGUILayout.PropertyField(name);
        EditorGUILayout.PropertyField(description);
        EditorGUILayout.PropertyField(icon);
        EditorGUILayout.PropertyField(useMouseDirection);
    }
}
