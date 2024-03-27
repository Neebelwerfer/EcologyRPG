using EcologyRPG.Core.Character;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CharacterEditor : EditorWindow
{
    [SerializeField]
    public List<StatData> stats;

    [SerializeField]
    public List<AttributeData> attributes;

    [SerializeField]
    public List<ResourceData> resources;

    const string path = "Assets/_EcologyRPG/Resources/CharacterStats.txt";


    SerializedObject so;
    Vector2 scrollPos = Vector2.zero;


    [MenuItem("Character/Character Editor")]
    public static void ShowWindow()
    {
        GetWindow<CharacterEditor>("Character Editor");
    }

    public CharacterEditor()
    {
        stats = new List<StatData>();
        attributes = new List<AttributeData>();
        resources = new List<ResourceData>();
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
            SaveFile();
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
        resources = newList.Resources;
    }

    private void SaveFile()
    {
        StreamWriter writer = new StreamWriter(path, false);
        var json = JsonUtility.ToJson(new SerializableStats(stats, attributes, resources), true);
        writer.Write(json);
        writer.Close();
        AssetDatabase.ImportAsset(path);
        StatAttributeDrawer.IsDirty = true;
        Debug.Log("Successfully saved to " + path);
    }

    private void OnGUI()
    {
        so.Update();
        var serializedStatList = so.FindProperty("stats");
        var serializedAttributeList = so.FindProperty("attributes");
        var serializedResourceList = so.FindProperty("resources");
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.PropertyField(serializedStatList, true);
        EditorGUILayout.PropertyField(serializedAttributeList, true);
        EditorGUILayout.PropertyField(serializedResourceList, true);
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

        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].name == "")
            {
                Debug.LogError("Resource name cannot be empty");
                return false;
            }

            for (int j = i; j < resources.Count; j++)
            {
                if (i != j && resources[i].name == resources[j].name)
                {
                    Debug.LogError("Duplicate resource name: " + resources[i].name);
                    return false;
                }
            }

            if (resources[i].MaxValueStat == "")
            {
                Debug.LogError("Resource " + resources[i].name + " has no max value stat");
                return false;
            }

            for (int j = 0; j < stats.Count; j++)
            {
                if (stats[j].name == resources[i].MaxValueStat)
                {
                    break;
                }
                if (j == stats.Count - 1)
                {
                    Debug.LogError("Resource " + resources[i].name + " has a non-existent max value stat: " + resources[i].MaxValueStat);
                    return false;
                }
            }
        }
        return true;
    }
}