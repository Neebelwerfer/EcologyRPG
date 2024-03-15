using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerAbility))]
public class PlayerAbilityEditor : AttackAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlayerAbility ability = (PlayerAbility)target;
        EditorGUILayout.LabelField("Resource Cost", EditorStyles.boldLabel);
        ability.ResourceName = EditorGUILayout.TextField("Resource Name", ability.ResourceName);
        if (ability.ResourceName != "")
           ability.ResourceCost = EditorGUILayout.FloatField(ability.ResourceCost);    
    }
}
#endif