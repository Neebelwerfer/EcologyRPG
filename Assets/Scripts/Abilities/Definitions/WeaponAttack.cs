using UnityEditor;
using UnityEngine;

public abstract class WeaponAttack : AttackAbility
{
    public enum TargetType
    {
        Line,
        Cone,
        Circular
    }

    [Header("Weapon Attack")]
    [Tooltip("The type of targeting this ability will use")]
    public TargetType targetType;
}

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponAttack))]
public class WeaponAttackEditor : AttackAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WeaponAttack ability = (WeaponAttack)target;
        ability.targetType = (WeaponAttack.TargetType)EditorGUILayout.EnumPopup("Target Type", ability.targetType);
    }
}
#endif