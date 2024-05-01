using EcologyRPG.Core.Abilities;
using System.IO;
using UnityEditor;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.UIElements;
using UnityEngine;

public class ConditionEditor : EditorWindow
{
    

    [MenuItem("Game/Condition Editor")]
    public static void ShowWindow()
    {
        GetWindow<ConditionEditor>("Condition Editor");
    }

    public static SerializedConditionArray conditionData;
    public static bool[] foldouts;

    Vector2 scrollPos;
    string searchName = "";



    private void OnEnable()
    {
        Load();
    }



    public static void Load()
    {
        var jsonObj = AssetDatabase.LoadAssetAtPath<TextAsset>(AbilityManager.ConditionDataPath);
        if (jsonObj != null)
        {


            conditionData = JsonUtility.FromJson<SerializedConditionArray>(jsonObj.text);
            conditionData ??= new SerializedConditionArray();
        }
        else
        {
            if (!AssetDatabase.IsValidFolder(AbilityManager.ConditionFullpath))
            {
                AssetDatabase.CreateFolder("Assets/_EcologyRPG/Resources", "Conditions");
            }

            File.WriteAllText(AbilityManager.ConditionDataPath, "{}");
            conditionData = new SerializedConditionArray();
        }
        foldouts = new bool[conditionData.data.Length];
    }

    public static void Save()
    {
        var json = JsonUtility.ToJson(conditionData, true);
        StreamWriter writer = new StreamWriter(AbilityManager.ConditionDataPath);
        writer.Write(json);
        writer.Close();
        AssetDatabase.Refresh();
    }

    private void OnGUI()
    {
        int DeleteIndex = -1;
        GUILayout.Label("Condition Editor", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Condition"))
        {
            conditionData.IDCounter += 1;
            var newCondition = new ConditionData("new Condition", conditionData.IDCounter, UpdateType.Update);
            var newConditionData = new ConditionData[conditionData.data.Length + 1];
            for (int i = 0; i < conditionData.data.Length; i++)
            {
                newConditionData[i] = conditionData.data[i];
            }
            newConditionData[conditionData.data.Length] = newCondition;
            foldouts = new bool[newConditionData.Length];
            conditionData.data = newConditionData;
            Save();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.Space(10);
        searchName = EditorGUILayout.TextField("Search ", searchName).ToLower();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(10);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < conditionData.data.Length; i++)
        {
            if (!string.IsNullOrEmpty(searchName) && !conditionData.data[i].ConditionName.ToLower().Contains(searchName)) continue;
            GUILayout.BeginHorizontal();
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], conditionData.data[i].ConditionName);
            if (GUILayout.Button("Edit"))
            {
                SingleConditionEditor.Open(i);
            }
            GUILayout.EndHorizontal();
            if (foldouts[i])
            {
                GUILayout.Label("ID: " + conditionData.data[i].ID);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Name");
                conditionData.data[i].ConditionName = GUILayout.TextField(conditionData.data[i].ConditionName);
                GUILayout.EndHorizontal();

                if (conditionData.data[i]._DefaultGlobalVariables == null)
                {
                    conditionData.data[i]._DefaultGlobalVariables = new GlobalVariable[0];
                }

                foreach (var variable in conditionData.data[i]._DefaultGlobalVariables)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.BeginHorizontal();
                    
                    //var obj = new SerializedObject(variable);
                    //var name = obj.FindProperty("Name");
                    //var type = obj.FindProperty("Type");
                    //var value = obj.FindProperty("Value");
                    //EditorGUILayout.LabelField("Name", name.name);
                    //var enumNames = type.enumNames;
                    //var enumName = enumNames[type.enumValueIndex];
                    //EditorGUILayout.LabelField("Type", enumName);
                    //EditorGUILayout.LabelField("Value", value.stringValue);
                    GUILayout.EndHorizontal();
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                if (GUILayout.Button("Delete Ability"))
                {
                    DeleteIndex = i;

                }
            }

        }
        if (GUILayout.Button("Save"))
        {
            Save();
        }
        GUILayout.EndScrollView();

        if (DeleteIndex > -1)
        {
            var ability = conditionData.data[DeleteIndex];
            var newConditionData = new ConditionData[conditionData.data.Length - 1];
            for (int i = 0; i < conditionData.data.Length; i++)
            {
                if (i < DeleteIndex)
                {
                    newConditionData[i] = conditionData.data[i];
                }
                else if (i > DeleteIndex)
                {
                    newConditionData[i - 1] = conditionData.data[i];
                }
            }
            conditionData.data = newConditionData;
            if (!string.IsNullOrEmpty(ability.ScriptPath)) File.Delete(ability.ScriptPath);
            Save();
        }
    }

    public static void CreateScript(int i)
    {
        var path = AbilityManager.ConditionFullpath + "Condition_" + conditionData.data[i].ID + AbilityManager.ScriptExtension;
        File.WriteAllText(path, scriptTemplate);
        conditionData.data[i].ScriptPath = path;
        Save();
    }

    static readonly string scriptTemplate = $@"
function OnApply()

end

function OnUpdate()

end

function OnRemove()

end
";
}
