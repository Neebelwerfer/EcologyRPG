using EcologyRPG.Game.Abilities;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(CenteredExplosion))]
public class CenteredExplosionEditor : AbilityDefinitionEditor
{
    public override void OnInspectorGUI()
    {
        CenteredExplosion ability = (CenteredExplosion)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetMask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Radius"));

        AbilityComponentEditor.Display("On Cast Components", ability.OnCastEffects, ability, DisplayComponentType.Visual);
        AbilityComponentEditor.Display("On Hit Components", ability.OnHitEffects, ability, DisplayComponentType.All);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif