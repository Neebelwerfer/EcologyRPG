using UnityEditor;
#if UNITY_EDITOR
public class BaseAbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BaseAbility abilityEffect = (BaseAbility)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Range"));

        AbilityEffectEditor.Display("On Cast Effects", abilityEffect.OnCastEffects, abilityEffect, DisplayEffectType.All);
    }
}
#endif