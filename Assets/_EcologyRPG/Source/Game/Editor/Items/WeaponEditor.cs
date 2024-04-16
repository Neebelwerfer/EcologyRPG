using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Abilities;
using EcologyRPG.GameSystems.Abilities.Components;
using EcologyRPG.GameSystems.Abilities;
using UnityEditor;
using UnityEngine;
using static EcologyRPG.Core.Items.Item;

namespace EcologyRPG.Core.Items
{
    [CustomEditor(typeof(Weapon), true)]
    public class WeaponEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var item = target as Weapon;

            if (item.WeaponAbility == null)
            {
                if (GUILayout.Button("Generate Weapon Attack Ability"))
                {
                    item.WeaponAbility = ScriptableObject.CreateInstance<PlayerAbilityDefinition>();
                    item.WeaponAbility.name = "Weapon Attack Ability";
                    item.WeaponAbility.Ability = ScriptableObject.CreateInstance<MeleeAttack>();
                    item.WeaponAbility.Ability.name = "Melee Attack";
                    var effect = ScriptableObject.CreateInstance<WeaponDamageComponent>();
                    effect.name = "Weapon Damage Effect";
                    effect.DamageType = DamageType.Physical;
                    ((MeleeAttack)item.WeaponAbility.Ability).OnHitEffects.Add(effect);

                    AssetDatabase.AddObjectToAsset(item.WeaponAbility, item);
                    AssetDatabase.AddObjectToAsset(item.WeaponAbility.Ability, item.WeaponAbility);
                    AssetDatabase.AddObjectToAsset(effect, item.WeaponAbility.Ability);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

            }
            else
            {
                if (GUILayout.Button("Remove Weapon Attack Ability"))
                {
                    DestroyImmediate(item.WeaponAbility.Ability, true);
                    DestroyImmediate(item.WeaponAbility, true);
                    item.WeaponAbility = null;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            if (item.generationRules == null)
            {
                if (GUILayout.Button("Allow Random Generation"))
                {
                    item.generationRules = CreateInstance<WeaponGenerationRules>();
                    AssetDatabase.AddObjectToAsset(item.generationRules, item);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.SetDirty(item);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Generation Rules", EditorStyles.boldLabel);
                var generationRules = item.generationRules as WeaponGenerationRules;
                if (generationRules.Modifiers == null)
                {
                    generationRules.Modifiers = new System.Collections.Generic.List<Ranges>();
                }
                var editor = CreateEditor(generationRules);
                editor.OnInspectorGUI();
                if (GUILayout.Button("Disallow Random Generation"))
                {
                    DestroyImmediate(item.generationRules, true);
                    item.generationRules = null;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.SetDirty(item);
                }

                if (GUI.changed)
                    EditorUtility.SetDirty(item);
            }
        }
    }
}