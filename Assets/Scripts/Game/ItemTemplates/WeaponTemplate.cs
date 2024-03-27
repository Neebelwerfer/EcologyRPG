using Character;
using Character.Abilities;
using Character.Abilities.AbilityComponents;
using Items.ItemTemplates;
using UnityEditor;
using UnityEngine;

namespace Items.ItemTemplates
{
    [CreateAssetMenu(fileName = "New Weapon template", menuName = "Items/Templates/Weapon")]
    public class WeaponTemplate : EquipableItemTemplate
    {
        [Header("Weapon Properties")]
        public float minDamage;
        public float maxDamage;
        [Header("Growth")]
        public float GrowthPerLevel;
        public GrowthType growthType;

        [HideInInspector] public PlayerAbilityDefinition WeaponAttackAbility;

        public WeaponTemplate()
        {
            equipmentType = EquipmentType.Weapon;

        }

        public override InventoryItem GenerateItem(int level)
        {
            var item = CreateInstance<Weapon>();
            item.Name = Name;
            item.Description = Description;
            item.Icon = Icon;
            item.Weight = Weight;

            var ability = Instantiate(WeaponAttackAbility);
            var range = new Ranges { minValue = minDamage, maxValue = maxDamage, GrowthPerLevel = GrowthPerLevel, growthType = growthType, modType = StatModType.Flat, StatName = "rawWeaponDamage", type = ModType.Stat };
            range.ApplyMod(level, item);
            item.WeaponAbility = ability;

            foreach (var mod in Modifiers)
            {
                mod.ApplyMod(level, item);
            }

            return new InventoryItem(item, 1);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponTemplate))]
public class WeaponTemplateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WeaponTemplate template = (WeaponTemplate)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("WeaponAttackAbility"));
        if (template.WeaponAttackAbility == null)
        {
            if(GUILayout.Button("Generate Weapon Attack Ability"))
            {
                template.WeaponAttackAbility = ScriptableObject.CreateInstance<PlayerAbilityDefinition>();
                template.WeaponAttackAbility.name = "Weapon Attack Ability";
                template.WeaponAttackAbility.Ability = ScriptableObject.CreateInstance<MeleeAttack>();
                template.WeaponAttackAbility.Ability.name = "Melee Attack";
                var effect = ScriptableObject.CreateInstance<WeaponDamageComponent>();
                effect.name = "Weapon Damage Effect";
                effect.DamageType = DamageType.Physical;
                ((MeleeAttack)template.WeaponAttackAbility.Ability).OnHitEffects.Add(effect);

                AssetDatabase.AddObjectToAsset(template.WeaponAttackAbility, template);
                AssetDatabase.AddObjectToAsset(template.WeaponAttackAbility.Ability, template.WeaponAttackAbility);
                AssetDatabase.AddObjectToAsset(effect, template.WeaponAttackAbility.Ability);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

        }
        else
        {
            if(GUILayout.Button("Remove Weapon Attack Ability"))
            {
                DestroyImmediate(template.WeaponAttackAbility.Ability, true);
                DestroyImmediate(template.WeaponAttackAbility, true);
                template.WeaponAttackAbility = null;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif