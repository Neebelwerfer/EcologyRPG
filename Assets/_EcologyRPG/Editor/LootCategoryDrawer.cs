using UnityEditor;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    [CustomPropertyDrawer(typeof(LootCategory))]
    public class LootCategoryDrawer : PropertyDrawer
    {
        static LootDatabase lootDatabase;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(lootDatabase == null)
            {
                lootDatabase = AssetDatabase.LoadAssetAtPath<LootDatabase>(LootDatabase.Path);
            }

            if(lootDatabase == null)
            {
                EditorGUI.LabelField(position, "No Loot Database found!");
                return;
            }

            var category = property.stringValue;
            var index = 0;
            for (int i = 0; i < lootDatabase.CategoryOdds.Count; i++)
            {
                if (lootDatabase.CategoryOdds[i].category == category)
                {
                    index = i;
                    break;
                }
            }
            var options = new string[lootDatabase.CategoryOdds.Count];
            for (int i = 0; i < lootDatabase.CategoryOdds.Count; i++)
            {
                options[i] = lootDatabase.CategoryOdds[i].category;
            }
            index = EditorGUI.Popup(position, label.text, index, options);
            if (index >= 0)
            {
                property.stringValue = lootDatabase.CategoryOdds[index].category;
            } 
            else
            {
                property.stringValue = "";
            }

        }
    }
}