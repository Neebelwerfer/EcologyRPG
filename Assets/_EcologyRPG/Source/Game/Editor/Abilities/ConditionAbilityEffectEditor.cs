using EcologyRPG.GameSystems.Abilities.Components;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ConditionAbilityComponent))]
public class ConditionAbilityEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ConditionAbilityComponent component = (ConditionAbilityComponent)target;
        component.DebuffCondition = (DebuffCondition)EditorGUILayout.ObjectField("Debuff Condition", component.DebuffCondition, typeof(DebuffCondition), false);
        if (component.DebuffCondition == null)
        {
            if(GUILayout.Button("Create Debuff Condition"))
            {
                var window = CreateInstance<ConditionEffectSelector>();
                window.effect = component;
                window.Show();
            }
        } else
        {
            Editor editor = Editor.CreateEditor(component.DebuffCondition);
            editor.OnInspectorGUI();
        }
    }
}
#endif