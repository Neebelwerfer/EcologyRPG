using Character;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utility;

[Serializable]
public class StatManager : EditorWindow
{
    [SerializeField]
    public List<StatData> stats;

    [SerializeField]
    public List<AttributeData> attributes;

    const string path = "Assets/Resources/Stats.txt";

    SerializedObject so;
    Vector2 scrollPos = Vector2.zero;


    [MenuItem("Character/StatManager")]
    public static void ShowWindow()
    {
        GetWindow<StatManager>("StatManager");
    }

    public StatManager()
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
        var newList = JsonUtility.FromJson<SerializableList<StatData>>(json);
        stats = newList.list;
    }

    private void SaveFile()
    {
        StreamWriter writer = new StreamWriter(path, false);
        var json = JsonUtility.ToJson(stats.ToSerializable(), true);
        writer.Write(json);
        writer.Close();
        AssetDatabase.ImportAsset(path);
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
        return true;
    }
}

