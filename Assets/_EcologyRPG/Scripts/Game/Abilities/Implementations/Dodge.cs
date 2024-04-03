using EcologyRPG.Core.Abilities;
using EcologyRPG.GameSystems.Abilities.Conditions;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities
{
    public enum DirectionMode
    {
        Mouse,
        Movement
    }

    public class Dodge : BaseAbility
    {
        [Header("Dodge Settings")]
        public DashCondition dodgeEffect;

        public override void Cast(CastInfo caster)
        {
            caster.owner.ApplyCondition(caster, Instantiate(dodgeEffect));
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Dodge))]
    public class DodgeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Dodge ability = (Dodge)target;
            if (ability.dodgeEffect == null)
            {
                ability.dodgeEffect = CreateInstance<DashCondition>();
                AssetDatabase.AddObjectToAsset(ability.dodgeEffect, ability);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                Editor editor = Editor.CreateEditor(ability.dodgeEffect);
                editor.OnInspectorGUI();
            }
        }
    }
#endif
}

