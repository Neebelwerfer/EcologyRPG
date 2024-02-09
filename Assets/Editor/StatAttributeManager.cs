using Character;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utility;

[Serializable]
public class StatAttributeManager : EditorWindow
{
    [SerializeField]
    public List<StatData> stats;

    [SerializeField]
    public List<AttributeData> attributes;

    const string path = "Assets/Resources/CharacterStats.txt";


    SerializedObject so;
    Vector2 scrollPos = Vector2.zero;


    [MenuItem("Character/Manage Stats and Attributes")]
    public static void ShowWindow()
    {
        GetWindow<StatAttributeManager>("Stats and Attributes manager");
    }

    public StatAttributeManager()
    {
        stats = new List<StatData>();
        attributes = new List<AttributeData>();
    }

    private void OnEnable()
    {
        LoadFile();
        so = new SerializedObject(this);
    }

    private void LoadFile()
    {
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }

        StreamReader reader = new StreamReader(path);
        try
        {
            ParseFile(reader);
        } catch (Exception e)
        {
            Debug.LogError(e);
        } finally
        {
            reader.Dispose();
        }
    }

    private void ParseFile(StreamReader reader)
    {
        var json = reader.ReadToEnd();
        var newList = JsonUtility.FromJson<SerializableStats>(json);
        stats = newList.Stats;
        attributes = newList.Attributes;
    }

    private void SaveFile()
    {
        StreamWriter writer = new StreamWriter(path, false);
        var json = JsonUtility.ToJson(new SerializableStats(stats, attributes), true);
        writer.Write(json);
        writer.Close();
        AssetDatabase.ImportAsset(path);
        Debug.Log("Successfully saved to " + path);
    }

    private void OnGUI()
    {
        so.Update();
        var serializedStatList = so.FindProperty("stats");
        var serializedAttributeList = so.FindProperty("attributes");
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.PropertyField(serializedStatList, true);
        EditorGUILayout.PropertyField(serializedAttributeList, true);
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            if (CheckConstraints())
            {
                SaveFile();
            }
        }
        if (GUILayout.Button("Discard Changes"))
        {
            LoadFile();
        }
        EditorGUILayout.EndHorizontal();

        so.ApplyModifiedProperties();
    }

    private bool CheckConstraints()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].name == "")
            {
                Debug.LogError("Stat name cannot be empty");
                return false;
            }

            for (int j = i; j < stats.Count; j++)
            {
                if (i != j && stats[i].name == stats[j].name)
                {
                    Debug.LogError("Duplicate stat name: " + stats[i].name);
                    return false;
                }
            }
        }

        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].name == "")
            {
                Debug.LogError("Attribute name cannot be empty");
                return false;
            }

            for (int j = i; j < attributes.Count; j++)
            {
                if (i != j && attributes[i].name == attributes[j].name)
                {
                    Debug.LogError("Duplicate attribute name: " + attributes[i].name);
                    return false;
                }
            }

            if (attributes[i].statProgressions.Count == 0)
            {
                Debug.LogError("Attribute " + attributes[i].name + " has no stat progressions");
                return false;
            }

            for (int j = 0; j < attributes[i].statProgressions.Count; j++)
            {
                var statProgression = attributes[i].statProgressions[j];
                if (statProgression.statName == "")
                {
                    Debug.LogError("Attribute " + attributes[i].name + " has a stat progression with no stat name");
                    return false;
                }
                
                for (int s = 0; s < stats.Count; s++)
                {
                    if (stats[s].name == statProgression.statName)
                    {
                        break;
                    }
                    if (s == stats.Count - 1)
                    {
                        Debug.LogError("Attribute " + attributes[i].name + " has a stat progression with a non-existent stat name: " + statProgression.statName);
                        return false;
                    }
                }
            }
          
        }
        return true;
    }
}