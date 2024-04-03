using EcologyRPG._Core.Abilities;
using UnityEditor;

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