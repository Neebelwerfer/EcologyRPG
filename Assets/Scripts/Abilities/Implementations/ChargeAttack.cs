using UnityEditor;

public class ChargeAttack : AttackAbility
{
    public bool StopOnHit = true;
    public float ChargeRange = 5;
    public float ChargeSpeed = 10;
}

#if UNITY_EDITOR
public class ChargeAttackEditor : AttackAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ChargeAttack ability = (ChargeAttack)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("StopOnHit"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ChargeRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ChargeSpeed"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif