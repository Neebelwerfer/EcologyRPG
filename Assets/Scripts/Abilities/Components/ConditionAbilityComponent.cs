using Character.Abilities.AbilityComponents;
using UnityEditor;
using UnityEngine;

namespace Character.Abilities.AbilityComponents
{
    public class ConditionAbilityComponent : CombatAbilityComponent
    {
        public DebuffCondition DebuffCondition;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            target.ApplyEffect(cast, Instantiate(DebuffCondition));
        }
    }
}

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