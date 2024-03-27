using EcologyRPG.Core.Abilities;
using UnityEditor;

#if UNITY_EDITOR
public class BaseAbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BaseAbility abilityEffect = (BaseAbility)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Range"));

        AbilityComponentEditor.Display("On Cast Components", abilityEffect.OnCastEffects, abilityEffect, DisplayComponentType.All);
    }
}
#endif