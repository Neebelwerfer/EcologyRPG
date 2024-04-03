using EcologyRPG._Core;
using EcologyRPG._Core.Character;
using EcologyRPG.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.DefaultControls;

[Serializable]
class SerializableFlags
{
    public Dictionary<string, int> flags;
}

public class FlagsEditor : EditorWindow
{

    [MenuItem("Game/Flag Editor")]
    public static void ShowWindow()
    {
        GetWindow<FlagsEditor>("Flags Editor");
    }

    Dictionary<string, int> flags;

    FlagsEditor()
    {
        LoadFile();
        flags ??= new Dictionary<string, int>();
    }

    string flagName = "";
    string SearchString = "";

    private void OnGUI()
    {
        EditorGUILayout.Space();
        SearchString = EditorGUILayout.TextField("Search", SearchString);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        for (int i = flags.Count - 1; i >= 0; i--)
        {
            var key = flags.Keys.ElementAt(i);
            if (!key.Contains(SearchString)) continue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(key);            
            flags[key] = EditorGUILayout.IntField(flags[key]);
            if (GUILayout.Button("Remove"))
            {
                flags.Remove(key);
            }   
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginHorizontal();
        flagName = EditorGUILayout.TextField(flagName);
        if (GUILayout.Button("Add new flag"))
        {
            if(!string.IsNullOrEmpty(flagName) && !flags.ContainsKey(flagName))
            {
                Debug.Log("Adding new flag: " + flagName);
                flags.Add(flagName, 0);
                flagName = "";
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {
            SaveFile();
        }
    }

    private void LoadFile()
    {
        if (!File.Exists(Flags.path))
        {
            File.Create(Flags.path).Dispose();
            SaveFile();
        }

        StreamReader reader = new StreamReader(Flags.path);
        try
        {
            ParseFile(reader);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            reader.Dispose();
        }
    }

    private void ParseFile(StreamReader reader)
    {
        var json = reader.ReadToEnd();
        var newList = JsonUtility.FromJson<SerializableDictionary<string, int>>(json);
        flags = newList.ToDictionary();
    }

    private void SaveFile()
    {
        StreamWriter writer = new StreamWriter(Flags.path, false);
        var json = JsonUtility.ToJson(flags.ToSerializable(), true);
        writer.Write(json);
        writer.Close();
        AssetDatabase.ImportAsset(Flags.path);
        Debug.Log("Successfully saved to " + Flags.path);
    }
}