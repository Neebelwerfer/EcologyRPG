using EcologyRPG.Core.Abilities;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(AttackAbility), false)]
public class AttackAbilityEditor : BaseAbilityEditor
{
    protected bool isPlayer = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  
        AttackAbility abilityEffect = (AttackAbility)target;
        if(abilityEffect.displayFirstHitEffects) AbilityComponentEditor.Display("On First Hit Components", abilityEffect.OnFirstHit, abilityEffect, DisplayComponentType.All);
        AbilityComponentEditor.Display("On Hit Components", abilityEffect.OnHitEffects, abilityEffect, DisplayComponentType.All);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif