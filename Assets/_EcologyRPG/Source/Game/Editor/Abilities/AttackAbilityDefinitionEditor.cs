using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.GameSystems.Abilities;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AttackAbilityDefinition))]
public class AttackAbilityDefinitionEditor : AbilityDefinitionEditor
{
    SelectableAbilities selectedAbility = SelectableAbilities.None;
    public enum SelectableAbilities
    {
        None,
        CenteredExplosion,
        BasicProjectile,
        LoppedProjectile,
        MultiProjectile,
        ExpandingProjectile,
        MeleeAttack,
        ChargeAttack,
    }
    bool foldOut = true;

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

        if (ability.Ability == null)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            selectedAbility = (SelectableAbilities)EditorGUILayout.EnumPopup("Ability", selectedAbility);
            var res = (BaseAbility) EditorGUILayout.ObjectField(new GUIContent("Ability"), ability.Ability, typeof(BaseAbility), false);

            if(GUILayout.Button("Create New Ability"))
            {
                if(selectedAbility == SelectableAbilities.None)
                {
                    Debug.LogError("No ability selected");
                    return;
                }
                res = CreateAbility(selectedAbility);
                if (res != null)
                {
                    res.name = res.GetType().Name;
                    ability.DisplayName = res.name;
                }
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
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.BeginHorizontal();
            foldOut = EditorGUILayout.Foldout(foldOut, "Ability: " + ability.Ability.name.Replace("(Clone)", ""));
            if (ability.Ability != null)
            {

                if (GUILayout.Button("Remove Ability"))
                {
                    DestroyImmediate(ability.Ability, true);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.SetDirty(ability);
                    ability.Ability = null;
                }
            }
            EditorGUILayout.EndHorizontal();
            if(foldOut && ability != null)
            {
                var a = CreateEditor(ability.Ability);
                a.OnInspectorGUI();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        serializedObject.ApplyModifiedProperties();
    }

    public static BaseAbility CreateAbility(SelectableAbilities ability)
    {
        if (ability == SelectableAbilities.CenteredExplosion)
        {
            return CreateInstance<CenteredExplosion>();
        }
        else if (ability == SelectableAbilities.BasicProjectile)
        {
            return CreateInstance<BasicProjectile>();
        }
        else if (ability == SelectableAbilities.LoppedProjectile)
        {
            return CreateInstance<LoppedProjectile>();
        }
        else if (ability == SelectableAbilities.MultiProjectile)
        {
            return CreateInstance<MultipleProjectiles>();
        }
        else if (ability == SelectableAbilities.ExpandingProjectile)
        {
            return CreateInstance<ExpandingProjectile>();
        }
        else if (ability == SelectableAbilities.MeleeAttack)
        {
            return CreateInstance<MeleeAttack>();
        }
        else if (ability == SelectableAbilities.ChargeAttack)
        {
            return CreateInstance<ChargeAttack>();
        }
        else
        {
            return null;
        }
    }
}
#endif