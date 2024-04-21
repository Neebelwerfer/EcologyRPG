using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.AbilityScripting.Editor
{
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
        AbilityCategory selectedCategory;

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
            File.WriteAllText(path, GenerateScript(abilityData[i].Category));
            abilityData[i].ScriptPath = path;
            Save();
        }

        private void OnGUI()
        {
            GUILayout.Label("Ability Editor", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            selectedCategory = (AbilityCategory)EditorGUILayout.EnumPopup("Category", selectedCategory);
            if (GUILayout.Button("Create Ability"))
            {
                idCounter += 1;
                var newAbility = new AbilityData("New Ability", selectedCategory, idCounter, "New Ability Description", 0);
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
            GUILayout.EndHorizontal();

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

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Category");
                    abilityData[i].Category = (AbilityCategory)EditorGUILayout.EnumPopup(abilityData[i].Category);
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
                        File.Delete(abilityData[i].ScriptPath);
                        Save();
                    }

                    if(GUILayout.Button("Save"))
                    {
                        Save();
                    }
                }
            }
            GUILayout.EndScrollView();
        }

        string PlayerCost()
        {
            return @"
function PayCost()
    local resource = Context.GetOwner().GetResource(""stamina"")
    resource:consume(10)
end
";
        }

        string GenerateScript(AbilityCategory abilityCategory)
        {
            return @"
function CanActivate(CastInfo)
    local resource = Context.GetOwner().GetResource(""stamina"")
    return resource:has(10)
end" + (abilityCategory == AbilityCategory.Player ? PlayerCost() : "") +

            @"          
function OnCast(CastInfo)
    Log(""Casting Ability"")
    Delay(1)
    Log(""Ability Casted"")
end
";
        }
    }
}
