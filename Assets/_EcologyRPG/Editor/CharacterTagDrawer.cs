﻿using UnityEditor;
using UnityEngine;

namespace EcologyRPG.Utility
{
    [CustomPropertyDrawer(typeof(CharacterTag))]
    public class CharacterTagDrawer : PropertyDrawer
    {
        static Tags tags;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (tags == null)
            {
                tags = AssetDatabase.LoadAssetAtPath<Tags>("Assets/_EcologyRPG/Resources/Config/CharacterTags.asset");
            }

            var tagArray = tags.tags.ToArray();


            var currentTag = property.stringValue;

            int index = 0;
            for (int i = 0; i < tagArray.Length; i++)
            {
                if (tagArray[i] == currentTag)
                {
                    index = i;
                    break;
                }
            }

            index = EditorGUI.Popup(position, label.text, index, tagArray);
            property.stringValue = tagArray[index];

            if (GUI.changed)
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}