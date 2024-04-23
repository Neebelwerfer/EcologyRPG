using EcologyRPG.AbilityScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerAbilityReference))]
public class PlayerAbilityReferenceEditor : AbilityReferenceEditor
{
    public override void OnInspectorGUI()
    {
        DrawAbilityReferenceValues();
        DrawPlayerReferenceValues();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        DrawPlayerResourceCost();
        DrawCanActivateString();
        DrawGlobalOverrides();
        serializedObject.ApplyModifiedProperties();
    }

    public void DrawPlayerResourceCost()
    {
        var resourceName = serializedObject.FindProperty("ResourceName");
        var resourceCost = serializedObject.FindProperty("ResourceCost");
        var customResourceUsage = serializedObject.FindProperty("customResourceUsage");

        EditorGUILayout.PropertyField(resourceName);
        EditorGUILayout.PropertyField(resourceCost);
        EditorGUILayout.PropertyField(customResourceUsage);
        if (customResourceUsage.boolValue == false)
        {
            return;
        }
        var useResourceString = serializedObject.FindProperty("UseResourceString");
        EditorGUILayout.PropertyField(useResourceString);
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
