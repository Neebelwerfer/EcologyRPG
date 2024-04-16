using UnityEditor;
using UnityEngine;
using static EcologyRPG.Core.Items.Item;

namespace EcologyRPG.Core.Items
{
    [CustomEditor(typeof(EquipableItem), true)]
    public class EquipableItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var item = target as EquipableItem;

            if (item.generationRules == null)
            {
                if (GUILayout.Button("Allow Random Generation"))
                {
                    item.generationRules = CreateInstance<EquipmentGenerationRules>();
                    AssetDatabase.AddObjectToAsset(item.generationRules, item);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.SetDirty(item);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Generation Rules", EditorStyles.boldLabel);
                var generationRules = item.generationRules as EquipmentGenerationRules;
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

    [CustomPropertyDrawer(typeof(EquipmentModification))]
    public class EquipmentModifierEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type = property.FindPropertyRelative("type");
            var value = property.FindPropertyRelative("Value");

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, 20), type);

            if(type.enumValueIndex == 0)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + 20, position.width, 20), property.FindPropertyRelative("AttributeName"));
                value.floatValue = EditorGUI.IntField(new Rect(position.x, position.y + 40, position.width, 20), "Value", (int)value.floatValue);
            } 
            else
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + 20, position.width, 20), property.FindPropertyRelative("StatName"));
                EditorGUI.PropertyField(new Rect(position.x, position.y + 40, position.width, 20), property.FindPropertyRelative("modType"));
                value.floatValue = EditorGUI.FloatField(new Rect(position.x, position.y + 60, position.width, 20), "Value", value.floatValue);
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var type = property.FindPropertyRelative("type");
            if(type.enumValueIndex == 0)
            {
                return 60;
            }
            else
            {
                return 80;
            }
        }
    }
}