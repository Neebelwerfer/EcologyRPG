using EcologyRPG.Game.Abilities.Definitions;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(AttackAbility), false)]
public class AttackAbilityEditor : BaseAbilityEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  
        AttackAbility abilityEffect = (AttackAbility)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetMask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useMouseDirection"));
        if(abilityEffect.displayFirstHitEffects) AbilityComponentEditor.Display("On First Hit Components", abilityEffect.OnFirstHit, abilityEffect, DisplayComponentType.All);
        AbilityComponentEditor.Display("On Hit Components", abilityEffect.OnHitEffects, abilityEffect, DisplayComponentType.All);
    }
}
#endif