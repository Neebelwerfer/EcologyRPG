using EcologyRPG.Core.Character;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.Core.Items
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
        [Tooltip("The name of the stat"), StatAttribute(StatType.Stat)]
        public string StatName;
        [Tooltip("The name of the attribute"), StatAttribute(StatType.Attribute)]
        public string AttributeName;
        [Tooltip("The type of the modifier")]
        public ModType type = ModType.Stat;
        [Tooltip("The type of stat modifier")]
        public StatModType modType = StatModType.Flat;
        [Header("Value Ranges")]
        [Tooltip("The minimum value of the modifier")]
        public float minValue;
        [Tooltip("The maximum value of the modifier")]
        public float maxValue;
        [Header("Growth Per Level")]
        [Tooltip("The growth per level of the item")]
        public float GrowthPerLevel;
        [Tooltip("The growth type of the modifier")]
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
                min = minValue + minValue * value;
                max = maxValue + maxValue * value;
            }
            else
            {
                min = minValue + value;
                max = maxValue + value;
            }
            var mod = new StatModification(StatName, Random.Range(min, max), modType, item);
            if(item.statModifiers.Find(x => x.StatName == mod.StatName) == null)
                item.statModifiers.Add(mod);
            else
            {
                var stat = item.statModifiers.Find(x => x.StatName == mod.StatName);
                stat.Value = mod.Value;
            }
        }

        void GenerateAttributeMod(int level, EquipableItem item)
        {
            var value = GrowthPerLevel * level;
            float min;
            float max;
            if (growthType == GrowthType.Percentage)
            {
                min = minValue + minValue * value;
                max = maxValue + maxValue * value;
            }
            else
            {
                min = minValue + value;
                max = maxValue + value;
            }
            var attribute = new AttributeModification(AttributeName, Random.Range((int)min, (int)max), item);
            if(item.attributeModifiers.Find(x => x.AttributeName == attribute.AttributeName) == null)
                item.attributeModifiers.Add(attribute);
            else
            {
                var attr = item.attributeModifiers.Find(x => x.AttributeName == attribute.AttributeName);
                attr.Value = attribute.Value;
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Ranges))]
    public class RangesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.indentLevel++;
            var type = property.FindPropertyRelative("type");
            var statName = property.FindPropertyRelative("StatName");
            var attributeName = property.FindPropertyRelative("AttributeName");
            var minValue = property.FindPropertyRelative("minValue");
            var maxValue = property.FindPropertyRelative("maxValue");
            var modtype = property.FindPropertyRelative("modType");
            var growthPerLevel = property.FindPropertyRelative("GrowthPerLevel");
            var growthType = property.FindPropertyRelative("growthType");

            if(modtype.enumValueIndex == -1)
                modtype.enumValueIndex = 0;

            type.enumValueIndex = EditorGUI.Popup(new Rect(position.x, position.y, position.width, 20), "Type", type.enumValueIndex, type.enumDisplayNames);

            if (type.enumValueIndex == 0)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + 20, position.width, 20), attributeName);
                minValue.floatValue = EditorGUI.IntField(new Rect(position.x, position.y + 40, position.width, 20), "Min Value", (int)minValue.floatValue);
                maxValue.floatValue = EditorGUI.IntField(new Rect(position.x, position.y + 60, position.width, 20), "Max Value", (int)maxValue.floatValue);
            } 
            else
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + 20, position.width, 20), statName);
                modtype.enumValueIndex = EditorGUI.Popup(new Rect(position.x, position.y + 40, position.width, 20), "Mod Type", modtype.enumValueIndex, modtype.enumDisplayNames);
                minValue.floatValue = EditorGUI.FloatField(new Rect(position.x, position.y + 60, position.width, 20), "Min Value", minValue.floatValue);
                maxValue.floatValue = EditorGUI.FloatField(new Rect(position.x, position.y + 80, position.width, 20), "Max Value", maxValue.floatValue);
            }

            growthPerLevel.floatValue = EditorGUI.FloatField(new Rect(position.x, position.y + 80 + (type.enumValueIndex * 20), position.width, 20), "Growth Per Level", growthPerLevel.floatValue);
            growthType.enumValueIndex = EditorGUI.Popup(new Rect(position.x, position.y + 100 + (type.enumValueIndex * 20), position.width, 20), "Growth Type", growthType.enumValueIndex, growthType.enumDisplayNames);
            EditorGUI.indentLevel--;
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(property.FindPropertyRelative("type").enumValueIndex == 0)
                return 120;
            else
                return 140;
        }
    }
#endif
}