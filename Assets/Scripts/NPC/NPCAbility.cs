using Character.Abilities;
using UnityEditor;
using UnityEngine;
using static AttackAbilityDefinitionEditor;

[CreateAssetMenu(menuName = "AbilityHolder/NPCAbility", fileName = "New NPC Ability")]
public class NPCAbility : AbilityDefintion
{
    public BaseAbility attackAbilityEffect;

    public override void CastEnded(CastInfo caster)
    {
        base.CastEnded(caster);
        caster.castPos = caster.owner.CastPos;
        caster.dir = caster.owner.transform.forward;
        attackAbilityEffect.Cast(caster);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(NPCAbility))]
public class NPCAbilityEditor : BaseAbilityEditor
{
    SelectableAbilities selectedAbility = SelectableAbilities.None;

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        NPCAbility ability = (NPCAbility)target;

        if (ability.attackAbilityEffect == null)
        {
            selectedAbility = (SelectableAbilities)EditorGUILayout.EnumPopup("Ability", selectedAbility);
            var res = (BaseAbility)EditorGUILayout.ObjectField(new GUIContent("Ability"), ability.attackAbilityEffect, typeof(BaseAbility), false);

            if (GUILayout.Button("Create New Ability"))
            {
                if (selectedAbility == SelectableAbilities.None)
                {
                    Debug.LogError("No ability selected");
                    return;
                }

                if (selectedAbility == SelectableAbilities.CenteredExplosion)
                {
                    res = CreateInstance<CenteredExplosion>();
                }
                else if (selectedAbility == SelectableAbilities.BasicProjectile)
                {
                    res = CreateInstance<BasicProjectile>();
                }
                else if (selectedAbility == SelectableAbilities.LoppedProjectile)
                {
                    res = CreateInstance<LoppedProjectile>();
                }
                else if (selectedAbility == SelectableAbilities.MultiProjectile)
                {
                    res = CreateInstance<MultipleProjectiles>();
                }
                else if (selectedAbility == SelectableAbilities.MeleeAttack)
                {
                    res = CreateInstance<MeleeAttack>();
                }
                res.name = res.GetType().Name;
            }
            if (res != null)
            {
                ability.attackAbilityEffect = Instantiate(res);
                AssetDatabase.AddObjectToAsset(ability.attackAbilityEffect, ability);
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


        if (ability.attackAbilityEffect != null)
        {

            if (GUILayout.Button("Remove Ability"))
            {
                DestroyImmediate(ability.attackAbilityEffect, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
                ability.attackAbilityEffect = null;
            }
        }
    }
}
#endif