using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(AttackAbilityDefinition))]
public class AttackAbilityDefinitionEditor : BaseAbilityEditor
{
    SelectableAbilities selectedAbility = SelectableAbilities.None;
    public enum SelectableAbilities
    {
        None,
        CenteredExplosion,
        BasicProjectile,
        LoppedProjectile,
        MultiProjectile,
        MeleeAttack,
        Dodge
    }
    public override void OnInspectorGUI()
    {
        AttackAbilityDefinition ability = (AttackAbilityDefinition)target;
        if (ability.GetType().IsSubclassOf(typeof(AttackAbilityDefinition)))
        {
            showCooldownValue = true;
        }
        else
        {
            showCooldownValue = false;
        }

        if(ability.Ability == null)
        {
            selectedAbility = (SelectableAbilities)EditorGUILayout.EnumPopup("Ability", selectedAbility);
            var res = (BaseAbility) EditorGUILayout.ObjectField(new GUIContent("Ability"), ability.Ability, typeof(BaseAbility), false);

            if(GUILayout.Button("Create New Ability"))
            {
                if(selectedAbility == SelectableAbilities.None)
                {
                    Debug.LogError("No ability selected");
                    return;
                }

                if(selectedAbility == SelectableAbilities.CenteredExplosion)
                {
                    res = CreateInstance<CenteredExplosion>();
                }
                else if(selectedAbility == SelectableAbilities.BasicProjectile)
                {
                    res = CreateInstance<BasicProjectile>();
                }
                else if(selectedAbility == SelectableAbilities.LoppedProjectile)
                {
                    res = CreateInstance<LoppedProjectile>();
                }
                else if(selectedAbility == SelectableAbilities.MultiProjectile)
                {
                    res = CreateInstance<MultipleProjectiles>();
                }
                else if(selectedAbility == SelectableAbilities.MeleeAttack)
                {
                    res = CreateInstance<MeleeAttack>();
                }
                else if(selectedAbility == SelectableAbilities.Dodge)
                {
                    res = CreateInstance<Dodge>();
                }
                else
                {
                    Debug.LogError("No ability selected");
                    return;
                }
                res.name = res.GetType().Name;
            }
            if (res != null)
            {
                ability.Ability = Instantiate(res);
                AssetDatabase.AddObjectToAsset(ability.Ability, ability);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
            }
        }
        else
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Ability"));
        }


        if (ability.Ability != null)
        {
               
            if(GUILayout.Button("Remove Ability"))
            {
                DestroyImmediate(ability.Ability, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
                ability.Ability = null;
            }
        }
    }
}
#endif