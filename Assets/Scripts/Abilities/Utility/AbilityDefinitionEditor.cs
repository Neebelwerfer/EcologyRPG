using Character.Abilities;
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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("CastTime"));

        AbilityEffectEditor.Display("Cast windup effects", abilityEffect.CastWindUp, abilityEffect, DisplayEffectType.Visual);
    }
}
#endif