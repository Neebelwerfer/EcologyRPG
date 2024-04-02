using EcologyRPG._Core.Abilities;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(AbilityDefintion), false)]
public class AbilityDefinitionEditor : Editor
{
    protected bool showCooldownValue = true;
    public override void OnInspectorGUI()
    {
        AbilityDefintion abilityEffect = (AbilityDefintion)target;
        if (EditorGUILayout.PropertyField(serializedObject.FindProperty("DisplayName")))
        {
            abilityEffect.name = abilityEffect.DisplayName;
        }

        if(showCooldownValue) EditorGUILayout.PropertyField(serializedObject.FindProperty("Cooldown"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(abilityEffect.CastWindupTime)));

        AbilityComponentEditor.Display("Cast windup components", abilityEffect.CastWindUp, abilityEffect, DisplayComponentType.Visual);
    }
}
#endif