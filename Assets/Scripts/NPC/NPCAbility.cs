using Character.Abilities;
using UnityEditor;
using UnityEngine;
using static AttackAbilityDefinitionEditor;

[CreateAssetMenu(menuName = "Ability/NPC Ability Definition", fileName = "New NPC Ability Data")]
public class NPCAbility : AbilityDefintion
{
    public BaseAbility Ability;

    public override void CastEnded(CastInfo caster)
    {
        base.CastEnded(caster);
        caster.castPos = caster.owner.CastPos;
        caster.dir = caster.owner.transform.forward;
        Ability.Cast(caster);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(NPCAbility))]
public class NPCAbilityEditor : AbilityDefinitionEditor
{
    SelectableAbilities selectedAbility = SelectableAbilities.None;

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        NPCAbility ability = (NPCAbility)target;

        if (ability.Ability == null)
        {
            selectedAbility = (SelectableAbilities)EditorGUILayout.EnumPopup("Ability", selectedAbility);
            var res = (BaseAbility)EditorGUILayout.ObjectField(new GUIContent("Ability"), ability.Ability, typeof(BaseAbility), false);

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

            if (GUILayout.Button("Remove Ability"))
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