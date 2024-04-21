using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.AbilityTest.Editor
{
    [System.Serializable]
    class SerializedDataArray
    {
        public AbilityData[] data;

        public AbilityData this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public SerializedDataArray()
        {
            data = new AbilityData[0];
        }
    }

    public class AbilityEditor : EditorWindow
    {
        [MenuItem("Game/Ability Editor")]
        public static void ShowWindow()
        {
            GetWindow<AbilityEditor>("Ability Editor");
        }

        SerializedDataArray abilityData;
        uint idCounter = 0;
        bool[] foldouts;
        Vector2 scrollPos;

        private void OnEnable()
        {
           Load();
        }

        void Load()
        {
            var jsonObj = AssetDatabase.LoadAssetAtPath<TextAsset>(AbilityManager.AbilityDataPath);
            if (jsonObj != null)
            {
                abilityData = JsonUtility.FromJson<SerializedDataArray>(jsonObj.text);
                abilityData ??= new SerializedDataArray();
                foreach (var ability in abilityData.data)
                {
                    if (ability.ID > idCounter)
                    {
                        idCounter = ability.ID;
                    }
                }
            }
            else
            {
                if (!AssetDatabase.IsValidFolder(AbilityManager.AbilityFullpath))
                {
                    AssetDatabase.CreateFolder("Assets/_EcologyRPG/Resources", "Abilities");
                }
                
                File.WriteAllText(AbilityManager.AbilityDataPath, "[]");
                abilityData = new SerializedDataArray();
            }
            foldouts = new bool[abilityData.data.Length];
        }

        void Save()
        {
            var json = JsonUtility.ToJson(abilityData);
            StreamWriter writer = new StreamWriter(AbilityManager.AbilityDataPath);
            writer.Write(json);
            writer.Close();
            AssetDatabase.Refresh();
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            Save();
        }

        void CreateScript(int i)
        {
            var path = AbilityManager.AbilityFullpath + "Ability_" + abilityData[i].ID + AbilityManager.AbilityScriptExtension;
            File.WriteAllText(path, GenerateScript());
            abilityData[i].ScriptPath = path;
            Save();
        }

        private void OnGUI()
        {
            GUILayout.Label("Ability Editor", EditorStyles.boldLabel);

            if (GUILayout.Button("Create Ability"))
            {
                idCounter += 1;
                var newAbility = new AbilityData("New Ability", idCounter, "New Ability Description", 0);
                var newAbilityData = new AbilityData[abilityData.data.Length + 1];
                for (int i = 0; i < abilityData.data.Length; i++)
                {
                    newAbilityData[i] = abilityData[i];
                }
                newAbilityData[abilityData.data.Length] = newAbility;
                foldouts = new bool[newAbilityData.Length];
                abilityData.data = newAbilityData;
                Save();
            }
            GUILayout.Space(10);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < abilityData.data.Length; i++)
            {
                GUILayout.BeginHorizontal();
                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], abilityData[i].Name);
                if(GUILayout.Button("Edit Behaviour"))
                {
                    if (abilityData[i].ScriptPath == null || abilityData[i].ScriptPath == "")
                    {
                        CreateScript(i);
                        EditorUtility.OpenWithDefaultApp(abilityData[i].ScriptPath);
                    }
                    else
                    {
                        if (!File.Exists(abilityData[i].ScriptPath))
                        {
                            CreateScript(i);
                        }
                        EditorUtility.OpenWithDefaultApp(abilityData[i].ScriptPath);
                    }
                }

                GUILayout.EndHorizontal();
                if (foldouts[i])
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Name");
                    abilityData[i].Name = GUILayout.TextField(abilityData[i].Name);
                    GUILayout.EndHorizontal();

                    var icon = EditorGUILayout.ObjectField("Icon", AssetDatabase.LoadAssetAtPath<Texture2D>(abilityData[i].IconPath), typeof(Texture2D), false);
                    if (icon != null)
                    {
                        abilityData[i].IconPath = AssetDatabase.GetAssetPath(icon);
                    }
                    else
                    {
                        abilityData[i].IconPath = "";
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Description");
                    abilityData[i].Description = GUILayout.TextField(abilityData[i].Description);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Cooldown");
                    abilityData[i].Cooldown = EditorGUILayout.FloatField(abilityData[i].Cooldown);
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Delete"))
                    {
                        var newAbilityData = new AbilityData[abilityData.data.Length - 1];
                        for (int j = 0; j < i; j++)
                        {
                            newAbilityData[j] = abilityData[j];
                        }
                        for (int j = i + 1; j < abilityData.data.Length; j++)
                        {
                            newAbilityData[j - 1] = abilityData[j];
                        }
                        abilityData.data = newAbilityData;
                        Save();
                    }
                }
            }
            GUILayout.EndScrollView();
        }

        string GenerateScript()
        {
            return @"
function CanActivate(CastInfo)
    return true
end
            
function OnCast(CastInfo)
    Log(""Casting Ability"")
    Delay(1)
    Log(""Ability Casted"")
end
            ";
        }
    }
}
