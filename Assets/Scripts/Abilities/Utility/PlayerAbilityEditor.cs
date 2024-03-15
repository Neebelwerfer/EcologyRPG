using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerAbility))]
public class PlayerAbilityEditor : AttackAbilityEditor
{
    public override void OnInspectorGUI()
    {
        PlayerAbility ability = (PlayerAbility)target;
        ability.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", ability.Icon, typeof(Sprite), false);
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Resource Cost", EditorStyles.boldLabel);
        ability.ResourceName = EditorGUILayout.TextField("Resource Name", ability.ResourceName);
        if (ability.ResourceName != "")
           ability.ResourceCost = EditorGUILayout.FloatField(ability.ResourceCost);    
    }
}
#endif