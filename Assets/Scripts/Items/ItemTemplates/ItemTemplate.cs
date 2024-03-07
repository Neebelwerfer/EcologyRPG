using Character;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Items.ItemTemplates
{
    public enum GrowthType
    {
        Flat,
        Percentage
    }

    public enum ModType
    {
        Attribute,
        Stat
    }

    [System.Serializable]
    public class Ranges
    {
        [Header("Stat Modifiers")]
        public string name;
        public ModType type;
        public StatModType modType;
        public float minValue;
        public float maxValue;
        [Header("Growth Per Level")]
        public float GrowthPerLevel;
        public GrowthType growthType;

        public void ApplyMod(int level, EquipableItem item)
        {
            if (type == ModType.Stat)
            {
                GenerateStatMod(level, item);
            }
            else
            {
                GenerateAttributeMod(level, item);
            }
        }
        
        void GenerateStatMod(int level, EquipableItem item)
        {
            var value = GrowthPerLevel * level;
            float min;
            float max;
            if (growthType == GrowthType.Percentage)
            {
                value /= 100;
                min = minValue + minValue * value;
                max = maxValue + maxValue * value;
            }
            else
            {
                min = minValue + value;
                max = maxValue + value;
            }
            var mod = new StatModification(name, Random.Range(min, max), modType, item);
            item.statModifiers.Add(mod);
        }

        void GenerateAttributeMod(int level, EquipableItem item)
        {
            var value = GrowthPerLevel * level;
            float min;
            float max;
            if (growthType == GrowthType.Percentage)
            {
                value /= 100;
                min = minValue + minValue * value;
                max = maxValue + maxValue * value;
            }
            else
            {
                min = minValue + value;
                max = maxValue + value;
            }
            var attribute = new AttributeModification(name, Random.Range((int)min, (int)max), item);
            item.attributeModifiers.Add(attribute);
        }
    }

    public abstract class ItemTemplate : ScriptableObject
    {
        [Header("Item Properties")]
        public string Name;
        public string Description;
        public Sprite Icon;
        public float Weight;

        public abstract InventoryItem GenerateItem(int level);
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Ranges))]
    public class RangesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.indentLevel++;
            var type = property.FindPropertyRelative("type");
            var name = property.FindPropertyRelative("name");
            var minValue = property.FindPropertyRelative("minValue");
            var maxValue = property.FindPropertyRelative("maxValue");
            var modtype = property.FindPropertyRelative("modType");
            var growthPerLevel = property.FindPropertyRelative("GrowthPerLevel");
            var growthType = property.FindPropertyRelative("growthType");

            name.stringValue = EditorGUI.TextField(new Rect(position.x, position.y + 20, position.width, 20), "Name", name.stringValue);
            type.enumValueIndex = EditorGUI.Popup(new Rect(position.x, position.y + 40, position.width, 20), "Type", type.enumValueIndex, type.enumDisplayNames);

            if (type.enumValueIndex == 0)
            {
                minValue.floatValue = EditorGUI.IntField(new Rect(position.x, position.y + 60, position.width, 20), "Min Value", (int)minValue.floatValue);
                maxValue.floatValue = EditorGUI.IntField(new Rect(position.x, position.y + 80, position.width, 20), "Max Value", (int)maxValue.floatValue);
            } 
            else
            {
                modtype.enumValueIndex = EditorGUI.Popup(new Rect(position.x, position.y + 60, position.width, 20), "Mod Type", modtype.enumValueIndex, modtype.enumDisplayNames);
                minValue.floatValue = EditorGUI.FloatField(new Rect(position.x, position.y + 80, position.width, 20), "Min Value", minValue.floatValue);
                maxValue.floatValue = EditorGUI.FloatField(new Rect(position.x, position.y + 100, position.width, 20), "Max Value", maxValue.floatValue);
            }

            growthPerLevel.floatValue = EditorGUI.FloatField(new Rect(position.x, position.y + 100 + (type.enumValueIndex * 20), position.width, 20), "Growth Per Level", growthPerLevel.floatValue);
            growthType.enumValueIndex = EditorGUI.Popup(new Rect(position.x, position.y + 120 + (type.enumValueIndex * 20), position.width, 20), "Growth Type", growthType.enumValueIndex, growthType.enumDisplayNames);
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(property.FindPropertyRelative("type").enumValueIndex == 0)
                return 140;
            else
                return 160;
        }
    }
#endif
}