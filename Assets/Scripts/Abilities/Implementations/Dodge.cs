using Character.Abilities;
using Codice.Client.Commands;
using log4net.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

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
        caster.owner.ApplyEffect(caster, Instantiate(dodgeEffect));
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
            ability.dodgeEffect = (DashCondition)EditorGUILayout.ObjectField("Dodge Effect", ability.dodgeEffect, typeof(DashCondition), false);
    }
}
#endif
