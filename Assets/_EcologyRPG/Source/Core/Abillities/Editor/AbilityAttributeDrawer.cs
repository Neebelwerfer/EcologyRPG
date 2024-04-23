using EcologyRPG.Core.Abilities;
using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbilityAttribute))]
public class AbilityAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(AbilityEditor.abilityData == null)
        {
            AbilityEditor.Load();
        }
        var index = property.uintValue;

        var data = Array.Find(AbilityEditor.abilityData.data, x => x.ID == index);
        GUI.Label(new Rect(position.x, position.y, position.width / 2, position.height), "Ability:");

        if (data != null)
        {
            GUI.Label(new Rect(position.x + position.width/4, position.y, position.width/2, position.height), data.abilityName);
        }
        else
        {
            GUI.Label(new Rect(position.x + position.width / 4, position.y, position.width/2, position.height), "No Ability Selected");
        }

        if (GUI.Button(new Rect(position.x + position.width/2, position.y, position.width/2, position.height), "Select", EditorStyles.miniButton))
        {
            AbilitySelectorEditor.Open(property);
        }

    }
}