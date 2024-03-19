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

[CreateAssetMenu(fileName = "Dodge", menuName = "Abilities/Dodge")]
public class Dodge : BaseAbility
{
    [Header("Dodge Settings")]
    public DodgeCondition dodgeEffect;

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
            ability.dodgeEffect = CreateInstance<DodgeCondition>();
            AssetDatabase.AddObjectToAsset(ability.dodgeEffect, ability);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
            ability.dodgeEffect = (DodgeCondition)EditorGUILayout.ObjectField("Dodge Effect", ability.dodgeEffect, typeof(DodgeCondition), false);
    }
}
#endif
