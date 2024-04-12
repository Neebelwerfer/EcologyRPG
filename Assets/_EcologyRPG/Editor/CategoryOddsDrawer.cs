using UnityEditor;
using UnityEngine;

namespace EcologyRPG.Utility
{
    [CustomPropertyDrawer(typeof(ReadOnlyString))]
    public class CategoryOddsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(position, label.text, property.stringValue);
        }
    }
}