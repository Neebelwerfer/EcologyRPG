using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Tags : ScriptableObject
{
    public List<string> tags = new List<string>();

}


public class CharacterTag : PropertyAttribute
{
    
}

[CustomPropertyDrawer(typeof(CharacterTag))]
public class CharacterTagDrawer : PropertyDrawer
{
    static Tags tags;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(tags == null)
        {
            tags = AssetDatabase.LoadAssetAtPath<Tags>("Assets/Resources/Config/CharacterTags.asset");
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

        if(GUI.changed)
        {
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}