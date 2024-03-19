using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerAbilityHolder))]
public class PlayerAbilityEditor : AttackAbilityDefinitionEditor
{
    public override void OnInspectorGUI()
    {
        PlayerAbilityHolder ability = (PlayerAbilityHolder)target;
        ability.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", ability.Icon, typeof(Sprite), false);
        EditorGUILayout.LabelField("Resource Cost", EditorStyles.boldLabel);
        ability.ResourceName = EditorGUILayout.TextField("Resource Name", ability.ResourceName);
        if (ability.ResourceName != "")
        {
            ability.ResourceCost = EditorGUILayout.FloatField(ability.ResourceCost);
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        base.OnInspectorGUI();
    }
}
#endif