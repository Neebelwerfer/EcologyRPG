using EcologyRPG.Core.Character;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatAttribute))]
public class StatAttributeDrawer : PropertyDrawer
{
    public static List<string> Stats;
    public static List<string> Attributes;
    public static List<string> Resources;

    public static bool IsDirty = true;
    const string path = "Assets/Resources/CharacterStats.txt";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        StatAttribute statAttribute = (StatAttribute)attribute;
        if (statAttribute != null)
        {
            if (IsDirty || Stats == null)
            {
                var reader = new StreamReader(path);
                ParseFile(reader);
                reader.Dispose();
                IsDirty = false;
            }
        }
        var stringValue = property.stringValue;
        var index = 0;
        var type = statAttribute.StatType;

        if(type == StatType.Stat)
        {
            index = Stats.IndexOf(stringValue);
            index = index < 0 ? 0 : index;
            index = EditorGUI.Popup(position, label.text, index, Stats.ToArray());
            property.stringValue = Stats[index];
        }
        else if(type == StatType.Attribute)
        {
            index = Attributes.IndexOf(stringValue);
            index = index < 0 ? 0 : index;
            index = EditorGUI.Popup(position, label.text, index, Attributes.ToArray());
            property.stringValue = Attributes[index];
        }
        else if(type == StatType.Resource)
        {
            index = Resources.IndexOf(stringValue);
            index = index < 0 ? 0 : index;
            index = EditorGUI.Popup(position, label.text, index, Resources.ToArray());
            property.stringValue = Resources[index];
        }
        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }

    private void ParseFile(StreamReader reader)
    {
        var json = reader.ReadToEnd();
        var newList = JsonUtility.FromJson<SerializableStats>(json);
        Stats = newList.Stats.Select((s) => s.name).Prepend("").ToList();
        Attributes = newList.Attributes.Select((s) => s.name).Prepend("").ToList();
        Resources = newList.Resources.Select((s) => s.name).Prepend("").ToList();
    }
}
