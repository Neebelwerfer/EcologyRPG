using EcologyRPG._Core.Abilities;
using EcologyRPG._Game.Abilities;
using EcologyRPG._Game.NPC;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using static AttackAbilityDefinitionEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(NPCAbility))]
public class NPCAbilityEditor : AbilityDefinitionEditor
{
    SelectableAbilities selectedAbility = SelectableAbilities.None;
    bool foldOut = true;
    public override void OnInspectorGUI()
    {
        NPCAbility ability = (NPCAbility)target;

        if (showCooldownValue) EditorGUILayout.PropertyField(serializedObject.FindProperty("Cooldown"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.CastWindupTime)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ability.AnimationTrigger)));
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        AbilityComponentEditor.Display("Cast windup components", ability.CastWindUp, ability, DisplayComponentType.Visual);

        if (ability.Ability == null)
        {
            selectedAbility = (SelectableAbilities)EditorGUILayout.EnumPopup("Ability", selectedAbility);
            var res = (BaseAbility)EditorGUILayout.ObjectField(new GUIContent("Ability"), ability.Ability, typeof(BaseAbility), false);

            if (GUILayout.Button("Create New Ability"))
            {
                res = CreateAbility(selectedAbility);

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

            EditorGUILayout.BeginHorizontal();
            foldOut = EditorGUILayout.Foldout(foldOut, "Ability");
            if (GUILayout.Button("Remove Ability"))
            {
                DestroyImmediate(ability.Ability, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
                ability.Ability = null;
            }
            EditorGUILayout.EndHorizontal(); 
            if(foldOut)
            {
                var a = CreateEditor(ability.Ability);
                a.OnInspectorGUI();
            }
            serializedObject.ApplyModifiedProperties();
        }


        

        //BaseAbility CreateAbility(SelectableAbilities ability)
        //{
        //    BaseAbility res = null;
        //    if (selectedAbility == SelectableAbilities.None)
        //    {
        //        Debug.LogError("No ability selected");
        //    }
        //    else if (selectedAbility == SelectableAbilities.CenteredExplosion)
        //    {
        //        res = CreateInstance<CenteredExplosion>();
        //    }
        //    else if (selectedAbility == SelectableAbilities.BasicProjectile)
        //    {
        //        res = CreateInstance<BasicProjectile>();
        //    }
        //    else if (selectedAbility == SelectableAbilities.LoppedProjectile)
        //    {
        //        res = CreateInstance<LoppedProjectile>();
        //    }
        //    else if (selectedAbility == SelectableAbilities.MultiProjectile)
        //    {
        //        res = CreateInstance<MultipleProjectiles>();
        //    }
        //    else if (selectedAbility == SelectableAbilities.MeleeAttack)
        //    {
        //        res = CreateInstance<MeleeAttack>();
        //    }
        //    return res;
        //}
    }
}
#endif