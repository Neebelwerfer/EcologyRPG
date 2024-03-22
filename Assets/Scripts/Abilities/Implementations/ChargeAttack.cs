using Character.Abilities;
using UnityEditor;

public class ChargeAttack : AttackAbility
{
    public DashCondition dash;

    public override void Cast(CastInfo castInfo)
    {
        base.Cast(castInfo);
        if(useMouseDirection)
        {
            dash.directionMode = DirectionMode.Mouse;
        }
        else
        {
            dash.directionMode = DirectionMode.Movement;
        }
        dash.dashRange = Range;
        dash.OnFirstHitEffects = OnFirstHit;
        dash.OnHitEffects = OnHitEffects;

        castInfo.owner.ApplyCondition(castInfo, Instantiate(dash));
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(ChargeAttack))]
public class ChargeAttackEditor : AttackAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ChargeAttack ability = (ChargeAttack)target;

        if(ability.dash == null)
        {
            ability.dash = CreateInstance<DashCondition>();
            AssetDatabase.AddObjectToAsset(ability.dash, ability);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        EditorGUILayout.LabelField("Dash Settings", EditorStyles.boldLabel);
        ability.dash.StopOnHit = EditorGUILayout.Toggle("Stop On Hit", ability.dash.StopOnHit);
        ability.dash.duration = EditorGUILayout.FloatField("Dash Time", ability.dash.duration);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif