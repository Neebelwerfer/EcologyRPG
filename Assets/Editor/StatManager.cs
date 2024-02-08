using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class StatManager : EditorWindow
{
    [SerializeField]
    public List<StatData> stats;

    const string path = "Assets/Resources/Stats.txt";

    SerializedObject so;


    [MenuItem("Character/StatManager")]
    public static void ShowWindow()
    {
        GetWindow<StatManager>("StatManager");
    }

    public StatManager()
    {
        stats = new List<StatData>();
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
        var serializedList = so.FindProperty("stats");
        EditorGUILayout.PropertyField(serializedList, true);
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            if (CheckConstraints())
            {
                SaveFile();
            }
            else
            {
                Debug.LogError("Invalid data");
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
                return false;
            }

            for (int j = i; j < stats.Count; j++)
            {
                if (i != j && stats[i].name == stats[j].name)
                {
                    return false;
                }
            }
        }
        return true;
    }
}

