using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class StatData
{
    public string name;
    public string displayName;
    public float baseValue;
    [TextArea(3, 10)]
    public string description;
}

public class StatManager : EditorWindow
{
    [SerializeField]
    public List<StatData> stats;

    const string path = "Assets/Editor/StatManager.txt";
    const string codePath = "Assets/Scripts/Character/Stats.cs";

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

        stats.Clear();
        StreamReader reader = new StreamReader(path);
        try
        {
            ParseFile(reader);
        } catch (System.Exception e)
        {
            Debug.LogError(e);
        } finally
        {
            reader.Dispose();
        }
    }

    private void ParseFile(StreamReader reader)
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] data = line.Split(',');
            StatData stat = new StatData();
            stat.name = data[0];
            stat.baseValue = float.Parse(data[1]);
            stat.description = parseDescription(data[2]);
            stat.displayName = data[3];
            stats.Add(stat);
        }
    }

    string parseDescription(string description)
    {
        return description.Replace(";", "\n");
    }

    private void SaveFile()
    {
        StreamWriter writer = new StreamWriter(path, false);

        for (int i = 0; i < stats.Count; i++)
        {
            var description = stats[i].description.Replace("\n", ";");
            writer.WriteLine(stats[i].name + "," + stats[i].baseValue + "," + description);
        }
        writer.Close();
        AssetDatabase.ImportAsset(path);
    }

    private void OnGUI()
    {
        so.Update();
        var serializedList = so.FindProperty("stats");
        EditorGUILayout.PropertyField(serializedList, true);


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            if (CheckConstraints())
            {
                SaveFile();
                GenerateCode();
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

    private void GenerateCode()
    {
        StringBuilder code = new StringBuilder();
        code.AppendLine("using System.Collections;");
        code.AppendLine("using System.Collections.Generic;");
        code.AppendLine("using UnityEngine;");
        code.AppendLine();
        code.AppendLine("public class Stats : MonoBehaviour");
        code.AppendLine("{");
        code.AppendLine("   List<Stat> StatList;");
        code.AppendLine("   public Stats()");
        code.AppendLine("   {");
        code.AppendLine("       StatList = new List<Stat>()");
        code.AppendLine("       {");
        for (int i = 0; i < stats.Count; i++)
        {
            code.AppendLine("           new Stat(\"" + stats[i].name + "\", " + stats[i].baseValue + "f, \"" + stats[i].description.Replace("\n",";") + "\", \"" + stats[i].displayName + "\")"+ ", ");
        }
        code.AppendLine("       };");
        code.AppendLine("   }");
        code.AppendLine();
        code.AppendLine("   public Stat GetStat(string name)");
        code.AppendLine("   {");
        code.AppendLine("       foreach (Stat stat in StatList)");
        code.AppendLine("       {");
        code.AppendLine("           if (stat.Name == name)");
        code.AppendLine("           {");
        code.AppendLine("               return stat;");
        code.AppendLine("           }");
        code.AppendLine("       }");
        code.AppendLine("       return null;");
        code.AppendLine("   }");
        code.AppendLine("}");
        File.WriteAllText(codePath, code.ToString());
        AssetDatabase.ImportAsset(codePath);
    }
}

