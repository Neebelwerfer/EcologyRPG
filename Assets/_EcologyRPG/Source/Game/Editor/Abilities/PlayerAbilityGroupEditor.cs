using EcologyRPG.GameSystems.PlayerSystems;
using UnityEditor;

[CustomEditor(typeof(PlayerAbilityGroup))]
public class PlayerAbilityGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var obj = (PlayerAbilityGroup)target;
        EditorGUILayout.LabelField("GUID: ", obj.GUID, EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilities"), true);
        serializedObject.ApplyModifiedProperties();
    }
}